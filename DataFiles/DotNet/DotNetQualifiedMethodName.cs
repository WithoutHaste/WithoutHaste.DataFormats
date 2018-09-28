﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WithoutHaste.DataFiles.DotNet
{
	/// <summary>
	/// Represents a fully qualified method name.
	/// </summary>
	/// <remarks>
	/// Cannot handle methods that declare more than 12 generic types,
	/// such as <![CDATA[MyMethod<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>]]>.
	/// </remarks>
	public class DotNetQualifiedMethodName : DotNetQualifiedName
	{
		/// <summary>Default names that will be given to generic-method-types, in order.</summary>
		public static string[] GenericTypeNames = new string[] { "A", "B", "C", "A2", "B2", "C2", "A3", "B3", "C3", "A4", "B4", "C4" };

		/// <summary>Local method name with generic type parameters (if applicable).</summary>
		public override string LocalName {
			get {
				if(genericTypeCount == 0)
					return localName;
				if(genericTypeAliases != null)
					return String.Format("{0}<{1}>", localName, String.Join(",", genericTypeAliases));
				else
					return String.Format("{0}<{1}>", localName, String.Join(",", GenericTypeNames.Take(genericTypeCount).ToArray()));
			}
		}

		/// <summary>The number of generic-types required by the method declaration.</summary>
		protected int genericTypeCount = 0;

		/// <summary>Specific generic type aliases for this method. If null, the shared <see cref="GenericTypeNames"/> will be used.</summary>
		protected string[] genericTypeAliases;

		#region Constructors

		/// <summary>Empty constructor</summary>
		public DotNetQualifiedMethodName() : base()
		{
		}

		/// <summary></summary>
		public DotNetQualifiedMethodName(string localName, int genericTypeCount = 0) : base(localName)
		{
			this.genericTypeCount = genericTypeCount;
		}

		/// <summary></summary>
		public DotNetQualifiedMethodName(string localName, DotNetQualifiedName fullNamespace, int genericTypeCount = 0) : base(localName, fullNamespace)
		{
			this.genericTypeCount = genericTypeCount;
		}

		#endregion

		/// <summary>
		/// Load additional documentation information from the assembly itself.
		/// </summary>
		public void AddAssemblyInfo(MethodInfo methodInfo)
		{
			string fullName = methodInfo.ToString();
			if(fullName.Contains("["))
			{
				string aliases = fullName.Substring(fullName.IndexOf("[") + 1);
				aliases = aliases.Substring(0, aliases.IndexOf("]"));
				this.genericTypeAliases = aliases.Split(',');
			}
			if(methodInfo.DeclaringType != null && FullNamespace != null && FullNamespace is DotNetQualifiedClassName)
			{
				(FullNamespace as DotNetQualifiedClassName).AddAssemblyInfo(methodInfo.DeclaringType.GetTypeInfo());
			}
		}
	}
}
