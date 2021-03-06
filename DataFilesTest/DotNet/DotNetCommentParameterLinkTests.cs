﻿using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WithoutHaste.DataFiles.DotNet;

namespace DataFilesTest
{
	[TestClass]
	public class DotNetCommentParameterLinkTests
	{
		[TestMethod]
		public void DotNetCommentParameterLink_FromXml()
		{
			//arrange
			string name = "test";
			XElement element = XElement.Parse("<paramref name='" + name + "' />", LoadOptions.PreserveWhitespace);
			//act
			DotNetCommentParameterLink result = DotNetCommentParameterLink.FromVisualStudioXml(element);
			//assert
			Assert.AreEqual(name, result.Name);
		}
	}
}
