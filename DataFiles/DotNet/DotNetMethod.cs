﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WithoutHaste.DataFiles.DotNet
{
	/// <summary></summary>
	public enum MethodCategory
	{
		/// <summary>Not enough information is available to determine method category.</summary>
		Unknown = 0,
		/// <summary>No special category.</summary>
		Normal,
		/// <summary>Static method.</summary>
		Static,
		/// <summary>Abstract method.</summary>
		Abstract,
		/// <summary>Virtual method.</summary>
		Virtual,
		/// <summary>Delegate type.</summary>
		Delegate
	};

	/// <summary>
	/// Represents a method.
	/// </summary>
	public class DotNetMethod : DotNetMember
	{
		/// <summary></summary>
		public MethodCategory Category { get; protected set; }

		/// <summary>Fully qualified name of return data type, if known. Null if not known.</summary>
		public DotNetQualifiedTypeName ReturnTypeName { get; protected set; }

		/// <summary></summary>
		public List<DotNetParameter> Parameters = new List<DotNetParameter>();

		#region Constructors

		/// <summary>Empty constructor</summary>
		public DotNetMethod() : base(new DotNetQualifiedName())
		{
		}

		/// <summary></summary>
		public DotNetMethod(DotNetQualifiedName name) : base(name)
		{
		}

		/// <summary></summary>
		public DotNetMethod(DotNetQualifiedName name, List<DotNetParameter> parameters) : base(name)
		{
			this.Parameters.AddRange(parameters);
		}

		/// <summary>
		/// Parse .Net XML documentation for method signature data.
		/// </summary>
		/// <param name="memberElement">Expects tag "member".</param>
		public static DotNetMethod FromVisualStudioXml(XElement memberElement)
		{
			string signature = memberElement.Attribute("name")?.Value;
			if(signature == null)
				return new DotNetMethod();

			//parameterless methods don't have a () at all
			int divider = signature.IndexOf('(');
			string name = null;
			string parameters = null;
			string returns = null;
			if(divider == -1)
			{
				name = signature;
			}
			else
			{
				name = signature.Substring(0, divider);
				parameters = signature.Substring(divider);

				//implicit and explicit operators will have the return type at the end after a ~
				if(parameters.IndexOf('~') > -1)
				{
					returns = parameters.Substring(parameters.IndexOf('~') + 1);
					parameters = parameters.Substring(0, parameters.IndexOf('~'));
				}
			}

			DotNetQualifiedMethodName qualifiedName = DotNetQualifiedMethodName.FromVisualStudioXml(name);

			//for constructors
			bool isConstructor = qualifiedName.LocalName.EndsWith("#ctor");
			if(isConstructor)
			{
				qualifiedName.SetLocalName(qualifiedName.FullNamespace.LocalName);
			}
			//todo: check for #cctor for static constructors

			//for operators
			bool isOperator = qualifiedName.LocalName.StartsWith("op_");

			//parse parameters
			List<DotNetParameter> qualifiedParameters = ParametersFromVisualStudioXml(parameters);

			DotNetMethod method = null;
			if(isConstructor)
				method = new DotNetMethodConstructor(qualifiedName, qualifiedParameters);
			else if(isOperator)
				method = new DotNetMethodOperator(qualifiedName, qualifiedParameters);
			else
				method = new DotNetMethod(qualifiedName, qualifiedParameters);
			method.ParseVisualStudioXmlDocumentation(memberElement);

			if(!String.IsNullOrEmpty(returns))
				method.ReturnTypeName = DotNetQualifiedTypeName.FromVisualStudioXml(returns);

			return method;
		}

		#endregion

		/// <summary>
		/// Parse .Net XML documentation parameter lists.
		/// </summary>
		/// <param name="text">
		/// Expects: null
		/// Expects: empty string
		/// Expects: "(type, type, type)"
		/// </param>
		public static List<DotNetParameter> ParametersFromVisualStudioXml(string text)
		{
			List<DotNetParameter> parameters = new List<DotNetParameter>();
			if(!string.IsNullOrEmpty(text))
			{
				//remove possible { } and possible ( )
				text = text.RemoveOuterBraces();
				text = text.RemoveOuterBraces();

				string[] fields = text.SplitIgnoreNested(',');
				for(int i = 0; i < fields.Length; i++)
				{
					string f = fields[i];
					if(!String.IsNullOrEmpty(f))
					{
						parameters.Add(DotNetParameter.FromVisualStudioXml(f));
					}
				}
			}
			return parameters;
		}

		/// <summary>
		/// Returns true if this method's signature matches the reflected MethodInfo.
		/// </summary>
		public bool MatchesSignature(MethodInfo methodInfo)
		{
			if((Name as DotNetQualifiedMethodName).IsGeneric)
			{
				if(methodInfo.Name + "``" + methodInfo.GetGenericArguments().Length != this.Name.LocalXmlName)
					return false;
			}
			else
			{
				if(methodInfo.Name != this.Name.LocalName)
					return false;
			}
			if(this is DotNetMethodOperator && this.ReturnTypeName != null) //for implicit/explicit operators
			{
				if(this.ReturnTypeName != DotNetQualifiedTypeName.FromAssemblyInfo(methodInfo.ReturnType))
					return false;
			}
			return MatchesArguments(methodInfo.GetParameters());
		}

		/// <summary>
		/// Returns true if this method's parameter list matches the reflected ParameterInfo.
		/// </summary>
		public bool MatchesArguments(ParameterInfo[] otherParameters)
		{
			if(Parameters.Count != otherParameters.Length)
				return false;

			for(int i = 0; i < Parameters.Count; i++)
			{
				string otherName = DotNetQualifiedTypeName.FromAssemblyInfo(otherParameters[i].ParameterType).FullName;
				if(Parameters[i].FullTypeName != otherName)
					return false;
			}
			return true;
		}

		/// <summary>
		/// Returns true if this method's parameter list matches the provided parameter list. Only parameter types matter, not the names.
		/// </summary>
		public bool MatchesArguments(List<DotNetParameter> otherParameters)
		{
			if(Parameters.Count != otherParameters.Count)
				return false;

			for(int i = 0; i < Parameters.Count; i++)
			{
				if(Parameters[i].TypeName.FullName != otherParameters[i].TypeName.FullName)
					return false;
			}
			return true;
		}

		/// <summary>
		/// Load additional documentation information from the assembly itself.
		/// </summary>
		public virtual void AddAssemblyInfo(MethodInfo methodInfo)
		{
			if(Category == MethodCategory.Unknown || Category == MethodCategory.Normal)
			{
				if(methodInfo.IsStatic)
					Category = MethodCategory.Static;
				else if(methodInfo.IsAbstract)
					Category = MethodCategory.Abstract;
				else if(methodInfo.IsVirtual && !methodInfo.IsFinal)
					Category = MethodCategory.Virtual;
				else
					Category = MethodCategory.Normal;
			}

			if(methodInfo.ReturnType != null)
				ReturnTypeName = DotNetQualifiedTypeName.FromAssemblyInfo(methodInfo.ReturnType);

			if(Category == MethodCategory.Delegate)
			{
				Parameters.Clear(); //expected to be empty already
				foreach(ParameterInfo parameterInfo in methodInfo.GetParameters())
				{
					DotNetParameter parameter = new DotNetParameter(DotNetQualifiedTypeName.FromAssemblyInfo(parameterInfo.ParameterType));
					parameter.AddAssemblyInfo(parameterInfo);
					Parameters.Add(parameter);
				}
			}
			else
			{
				int index = 0;
				foreach(ParameterInfo parameterInfo in methodInfo.GetParameters())
				{
					Parameters[index].AddAssemblyInfo(parameterInfo);
					index++;
				}
			}

			if(Name != null && Name is DotNetQualifiedMethodName)
			{
				(Name as DotNetQualifiedMethodName).AddAssemblyInfo(methodInfo);
			}
		}

		/// <summary>
		/// Returns true if this method and the method link have matching signatures, based on the fully qualified name and the list of parameter types.
		/// </summary>
		public bool MatchesSignature(DotNetCommentMethodLink link)
		{
			return link.MatchesSignature(this);
		}

		#region Low Level

		/// <duplicate cref='Equals(Object)' />
		public static bool operator ==(DotNetMethod a, DotNetMethod b)
		{
			if(object.ReferenceEquals(a, null) && object.ReferenceEquals(b, null))
				return true;
			if(object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null))
				return false;
			return a.Equals(b);
		}

		/// <duplicate cref='Equals(Object)' />
		public static bool operator !=(DotNetMethod a, DotNetMethod b)
		{
			if(object.ReferenceEquals(a, null) && object.ReferenceEquals(b, null))
				return false;
			if(object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null))
				return true;
			return !a.Equals(b);
		}

		/// <summary>Equality is based on the full namespace/name/generic-type-parameters of the method, and on parameter-types.</summary>
		public override bool Equals(Object b)
		{
			if(!(b is DotNetMethod))
				return false;
			if(object.ReferenceEquals(this, null) && object.ReferenceEquals(b, null))
				return true;
			if(object.ReferenceEquals(this, null) || object.ReferenceEquals(b, null))
				return false;

			DotNetMethod other = (b as DotNetMethod);
			if(this.Name != other.Name)
				return false;
			if(this.Parameters.Count != other.Parameters.Count)
				return false;
			for(int i = 0; i < this.Parameters.Count; i++)
			{
				if(this.Parameters[i].TypeName != other.Parameters[i].TypeName)
					return false;
			}
			return true;
		}

		/// <summary></summary>
		public override int GetHashCode()
		{
			return Name.GetHashCode() & Parameters.GetHashCode();
		}

		#endregion
	}
}