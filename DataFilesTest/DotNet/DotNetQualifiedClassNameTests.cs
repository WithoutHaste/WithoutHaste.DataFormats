﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WithoutHaste.DataFiles.DotNet;

namespace DataFilesTest
{
	[TestClass]
	public class DotNetQualifiedClassNameTests
	{
		protected class OneGenericClass<CustomT>
		{
		}

		protected class TwoGenericClass<CustomT,CustomU>
		{
			public class NestedClass { }
		}

		[TestMethod]
		public void DotNetQualifiedClassName_FromXml_ClassGenericParameters_OneGeneric()
		{
			//arrange
			string expectedFullName = "DataFilesTest.DotNetQualifiedClassNameTests.OneGenericClass<T>";
			Type type = typeof(OneGenericClass<>);
			//act
			DotNetQualifiedClassName result = DotNetQualifiedName.FromAssemblyInfo(type) as DotNetQualifiedClassName;
			//assert
			Assert.AreEqual(expectedFullName, result.FullName);
		}

		[TestMethod]
		public void DotNetQualifiedClassName_FromAssembly_ClassGenericParameters_OneGeneric()
		{
			//arrange
			string expectedFullName = "DataFilesTest.DotNetQualifiedClassNameTests.OneGenericClass<CustomT>";
			Type type = typeof(OneGenericClass<>);
			//act
			DotNetQualifiedClassName result = DotNetQualifiedName.FromAssemblyInfo(type) as DotNetQualifiedClassName;
			result.AddAssemblyInfo(type);
			//assert
			Assert.AreEqual(expectedFullName, result.FullName);
		}

		[TestMethod]
		public void DotNetQualifiedClassName_FromXml_ClassGenericParameters_TwoGeneric()
		{
			//arrange
			string expectedFullName = "DataFilesTest.DotNetQualifiedClassNameTests.TwoGenericClass<T,U>";
			Type type = typeof(TwoGenericClass<,>);
			//act
			DotNetQualifiedClassName result = DotNetQualifiedName.FromAssemblyInfo(type) as DotNetQualifiedClassName;
			//assert
			Assert.AreEqual(expectedFullName, result.FullName);
		}

		[TestMethod]
		public void DotNetQualifiedClassName_FromAssembly_ClassGenericParameters_TwoGeneric()
		{
			//arrange
			string expectedFullName = "DataFilesTest.DotNetQualifiedClassNameTests.TwoGenericClass<CustomT,CustomU>";
			Type type = typeof(TwoGenericClass<,>);
			//act
			DotNetQualifiedClassName result = DotNetQualifiedName.FromAssemblyInfo(type) as DotNetQualifiedClassName;
			result.AddAssemblyInfo(type);
			//assert
			Assert.AreEqual(expectedFullName, result.FullName);
		}

		[TestMethod]
		public void DotNetQualifiedClassName_FromXml_ClassGenericParameters_Nested()
		{
			//arrange
			string expectedFullName = "DataFilesTest.DotNetQualifiedClassNameTests.TwoGenericClass<T,U>.NestedClass";
			Type type = typeof(TwoGenericClass<,>.NestedClass);
			//act
			DotNetQualifiedClassName result = DotNetQualifiedName.FromAssemblyInfo(type) as DotNetQualifiedClassName;
			//assert
			Assert.AreEqual(expectedFullName, result.FullName);
		}

		[TestMethod]
		public void DotNetQualifiedClassName_FromAssembly_ClassGenericParameters_NestedClass()
		{
			//arrange
			string expectedFullName = "DataFilesTest.DotNetQualifiedClassNameTests.TwoGenericClass<CustomT,CustomU>.NestedClass";
			Type type = typeof(TwoGenericClass<,>.NestedClass);
			//act
			DotNetQualifiedClassName result = DotNetQualifiedName.FromAssemblyInfo(type) as DotNetQualifiedClassName;
			result.AddAssemblyInfo(type);
			//assert
			Assert.AreEqual(expectedFullName, result.FullName);
		}

		[TestMethod]
		public void DotNetQualifiedClassName_Clone_NullFullNamespace()
		{
			//arrange
			DotNetQualifiedClassName a = DotNetQualifiedClassName.FromVisualStudioXml("System");
			//act
			DotNetQualifiedClassName result = a.Clone();
			//assert
			Assert.AreEqual(a, result);
		}

		[TestMethod]
		public void DotNetQualifiedClassName_Clone_ManySegments()
		{
			//arrange
			DotNetQualifiedClassName a = DotNetQualifiedClassName.FromVisualStudioXml("System.Collections.Generic.List");
			//act
			DotNetQualifiedClassName result = a.Clone();
			//assert
			Assert.AreEqual(a, result);
		}

		[TestMethod]
		public void DotNetQualifiedClassName_Clone_GenericType()
		{
			//arrange
			DotNetQualifiedClassName a = DotNetQualifiedClassName.FromVisualStudioXml("System.Collections.Generic.List`1");
			//act
			DotNetQualifiedClassName result = a.Clone();
			//assert
			Assert.AreEqual(a, result);
			Assert.AreEqual(1, result.GenericTypeCount);
		}

		[TestMethod]
		public void DotNetQualifiedClassName_Clone_NestedGenericType()
		{
			//arrange
			DotNetQualifiedClassName a = DotNetQualifiedClassName.FromVisualStudioXml("System.Collections.Generic`2.List`1");
			//act
			DotNetQualifiedClassName result = a.Clone();
			//assert
			Assert.AreEqual(a, result);
			Assert.AreEqual(1, result.GenericTypeCount);
			Assert.AreEqual(2, result.FullClassNamespace.GenericTypeCount);
		}
	}
}
