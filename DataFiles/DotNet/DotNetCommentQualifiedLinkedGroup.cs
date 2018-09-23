﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WithoutHaste.DataFiles.DotNet
{
	/// <summary>
	/// Represents a section of comments that is linked to a fully qualified type or member.
	/// </summary>
	/// <example><![CDATA[<permission cref="Namespace.Type.Member">nested comments</permission>]]></example>
	/// <example><![CDATA[<exception cref="Namespace.ExceptionType">nested comments</exception>]]></example>
	public class DotNetCommentQualifiedLinkedGroup : DotNetCommentLinkedGroup<DotNetCommentQualifiedLink>
	{
		#region Constructors

		/// <summary></summary>
		public DotNetCommentQualifiedLinkedGroup(DotNetCommentQualifiedLink link, DotNetComment comment) : base(link, comment)
		{
		}

		/// <summary></summary>
		public DotNetCommentQualifiedLinkedGroup(DotNetCommentQualifiedLink link, params DotNetComment[] comments) : base(link, comments)
		{
		}

		/// <summary></summary>
		public DotNetCommentQualifiedLinkedGroup(DotNetCommentQualifiedLink link, List<DotNetComment> comments) : base(link, comments)
		{
		}

		/// <summary>Parses .Net XML documentation for permission or exception.</summary>
		public static new DotNetCommentQualifiedLinkedGroup FromVisualStudioXml(XElement element)
		{
			ValidateXmlTag(element, new string[] { "permission", "exception" });
			return new DotNetCommentQualifiedLinkedGroup(
				DotNetCommentQualifiedLink.FromVisualStudioXml(element.Attribute("cref")?.Value),
				ParseSection(element)
			);
		}

		#endregion
	}
}
