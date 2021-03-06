﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WithoutHaste.DataFiles.Markdown
{
	/// <summary>
	/// Represents a header and all contents until the next header of the same depth.
	/// </summary>
	public class MarkdownSection : IMarkdownInSection, IMarkdownIsBlock
	{
		/// <summary>
		/// 0-indexed nesting depth of section.
		/// </summary>
		/// <example><c># Header</c> is depth 1</example>
		/// <example><c>## Header</c> is depth 2</example>
		public int Depth {
			get {
				return depth;
			}
			protected set {
				if(value < 0) throw new Exception("Depth cannot be less than 0.");

				depth = value;
				foreach(MarkdownSection section in elements.OfType<MarkdownSection>())
				{
					section.Depth = depth + 1;
				}
			}
		}

		/// <summary>
		/// Displayed header text.
		/// </summary>
		public string Header { get; set; }

		/// <summary>
		/// True if the section contains no elements.
		/// </summary>
		public bool IsEmpty { get { return (elements.Count == 0); } }

		/// <summary>All markdown elements within section.</summary>
		public IMarkdownInSection[] Elements { get { return elements.ToArray(); } }

		private int depth;
		private List<IMarkdownInSection> elements = new List<IMarkdownInSection>();

		#region Constructors

		/// <summary></summary>
		public MarkdownSection(string header, int depth=1)
		{
			this.depth = depth;
			Header = header;
		}

		#endregion

		/// <summary>
		/// Creates new section and adds it to the end of this section. Defaults to depth of parent + 1.
		/// </summary>
		/// <param name="header">Section header</param>
		/// <returns>The new section</returns>
		public MarkdownSection AddSection(string header)
		{
			MarkdownSection section = new MarkdownSection(header, Depth + 1);
			elements.Add(section);
			return section;
		}

		/// <summary>
		/// Adds existing section to the end of this section. Depths are updated.
		/// </summary>
		/// <param name="section">Existing section.</param>
		public void AddSection(MarkdownSection section)
		{
			elements.Add(section);
			section.Depth = this.Depth + 1;
		}

		/// <summary>
		/// Adds the element to the end of this section.
		/// </summary>
		public void Add(IMarkdownInSection element)
		{
			elements.Add(element);
		}

		/// <summary>
		/// Adds all the elements to the end of this section.
		/// </summary>
		public void Add(params IMarkdownInSection[] elements)
		{
			this.elements.AddRange(elements);
		}

		/// <summary>
		/// Adds all the elements to the end of this section.
		/// </summary>
		public void Add(List<IMarkdownInSection> elements)
		{
			this.elements.AddRange(elements);
		}

		/// <summary>
		/// Adds the element in a new MarkdownLine at the end of this section.
		/// </summary>
		public void AddInLine(IMarkdownInLine element)
		{
			elements.Add(new MarkdownLine(element));
		}

		/// <summary>
		/// Adds the text in a new MarkdownLine at the end of this section.
		/// </summary>
		public void AddInLine(string text)
		{
			elements.Add(new MarkdownLine(text));
		}

		/// <summary>
		/// Adds the elements in a new MarkdownLine at the end of this section.
		/// </summary>
		public void AddInLine(params IMarkdownInLine[] elements)
		{
			this.elements.Add(new MarkdownLine(elements));
		}

		/// <summary>
		/// Adds the elements in a new MarkdownLine at the end of this section.
		/// </summary>
		public void AddInLine(List<IMarkdownInLine> elements)
		{
			this.elements.Add(new MarkdownLine(elements));
		}

		/// <summary>
		/// Adds the element in a new MarkdownParagraph at the end of this section.
		/// </summary>
		public void AddInParagraph(IMarkdownInSection element)
		{
			elements.Add(new MarkdownParagraph(element));
		}

		/// <summary>
		/// Adds the elements in a new MarkdownParagraph at the end of this section.
		/// </summary>
		public void AddInParagraph(params IMarkdownInSection[] elements)
		{
			this.elements.Add(new MarkdownParagraph(elements));
		}

		/// <summary>
		/// Adds the elements in a new MarkdownParagraph at the end of this section.
		/// </summary>
		public void AddInParagraph(List<IMarkdownInSection> elements)
		{
			this.elements.Add(new MarkdownParagraph(elements));
		}

		/// <summary>
		/// Adds the text in a new MarkdownParagraph at the end of this section.
		/// </summary>
		public void AddInParagraph(string text)
		{
			elements.Add(new MarkdownParagraph(text));
		}

		/// <summary>
		/// Returns true if the last element in the section has the specified type.
		/// </summary>
		public bool EndsWith(Type type)
		{
			if(elements.Count == 0) return false;
			return (elements.Last().GetType() == type);
		}

		/// <duplicate cref='ToMarkdownString(string)'/>
		public string ToMarkdownString()
		{
			return ToMarkdownString(null);
		}

		/// <summary>
		/// Return markdown-formatted text, taking the previous text of the file into account.
		/// </summary>
		public string ToMarkdownString(string previousText)
		{
			StringBuilder builder = new StringBuilder();

			if(!String.IsNullOrEmpty(previousText))
			{
				if(previousText.EndsWith("\n\n"))
				{
					//no action
				}
				else if(previousText.EndsWith("\n"))
				{
					if(previousText.Length > 1)
					{
						builder.Append("\n");
					}
				}
				else
				{
					builder.Append("\n\n");
				}
			}

			builder.Append(new String('#', Depth) + " " + Header + "\n\n");
			string thisPreviousText = null;
			foreach(IMarkdownInSection element in elements)
			{
				if(element is MarkdownSection)
				{
					thisPreviousText = (element as MarkdownSection).ToMarkdownString(thisPreviousText);
				}
				else
				{
					thisPreviousText = element.ToMarkdownString(thisPreviousText);
				}
				builder.Append(thisPreviousText);
			}

			return Utilities.EnsureTwoEndlines(builder.ToString());
		}
	}
}
