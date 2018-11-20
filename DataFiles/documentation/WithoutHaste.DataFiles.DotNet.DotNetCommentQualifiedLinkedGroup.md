# [WithoutHaste.DataFiles.DotNet](TableOfContents.WithoutHaste.DataFiles.DotNet.md).DotNetCommentQualifiedLinkedGroup

**Inheritance:** [object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [DotNetComment](WithoutHaste.DataFiles.DotNet.DotNetComment.md) → [DotNetCommentGroup](WithoutHaste.DataFiles.DotNet.DotNetCommentGroup.md) → [DotNetCommentLinkedGroup](WithoutHaste.DataFiles.DotNet.DotNetCommentLinkedGroup.md)  

Represents a section of comments that is linked to a fully qualified type or member.  

# Examples

## Example A:

`<permission cref="Namespace.Type.Member">nested comments</permission>`  

## Example B:

`<exception cref="Namespace.ExceptionType">nested comments</exception>`  

# Properties

## [WithoutHaste.DataFiles.DotNet.DotNetCommentQualifiedLink](WithoutHaste.DataFiles.DotNet.DotNetCommentQualifiedLink.md) QualifiedLink { get; }

Strongly-typed link.  

# Constructors

## DotNetCommentQualifiedLinkedGroup([WithoutHaste.DataFiles.DotNet.DotNetCommentQualifiedLink](WithoutHaste.DataFiles.DotNet.DotNetCommentQualifiedLink.md) link, [WithoutHaste.DataFiles.DotNet.CommentTag](WithoutHaste.DataFiles.DotNet.CommentTag.md) tag, [WithoutHaste.DataFiles.DotNet.DotNetComment](WithoutHaste.DataFiles.DotNet.DotNetComment.md) comment)

## DotNetCommentQualifiedLinkedGroup([WithoutHaste.DataFiles.DotNet.DotNetCommentQualifiedLink](WithoutHaste.DataFiles.DotNet.DotNetCommentQualifiedLink.md) link, [WithoutHaste.DataFiles.DotNet.CommentTag](WithoutHaste.DataFiles.DotNet.CommentTag.md) tag, WithoutHaste.DataFiles.DotNet.DotNetComment[] comments)

## DotNetCommentQualifiedLinkedGroup([WithoutHaste.DataFiles.DotNet.DotNetCommentQualifiedLink](WithoutHaste.DataFiles.DotNet.DotNetCommentQualifiedLink.md) link, [WithoutHaste.DataFiles.DotNet.CommentTag](WithoutHaste.DataFiles.DotNet.CommentTag.md) tag, [List&lt;WithoutHaste.DataFiles.DotNet.DotNetComment&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1) comments)

# Methods

## [bool](https://docs.microsoft.com/en-us/dotnet/api/system.boolean) Matches([WithoutHaste.DataFiles.DotNet.DotNetMember](WithoutHaste.DataFiles.DotNet.DotNetMember.md) member)

Returns true if link name matches the member name.  

# Static Methods

## static DotNetCommentQualifiedLinkedGroup FromVisualStudioXml([System.Xml.Linq.XElement](https://docs.microsoft.com/en-us/dotnet/api/system.xml.linq.xelement) element)

Parses .Net XML documentation for permission or exception.  
