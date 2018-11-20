# [WithoutHaste.DataFiles.DotNet](TableOfContents.WithoutHaste.DataFiles.DotNet.md).DotNetCommentParameterLink

**Inheritance:** [object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [DotNetComment](WithoutHaste.DataFiles.DotNet.DotNetComment.md)  
**Implements:** [IDotNetCommentLink](WithoutHaste.DataFiles.DotNet.IDotNetCommentLink.md)  

Represents a link in the comments to an internal parameter name.  

# Examples

## Example A:

`<paramref name="paramName" />`  

# Properties

## [string](https://docs.microsoft.com/en-us/dotnet/api/system.string) FullName { get; }

Return the fully qualified name of the referenced assembly element.  

## [string](https://docs.microsoft.com/en-us/dotnet/api/system.string) Name { get; protected set; }

Name of the parameter in local method.  

# Constructors

## DotNetCommentParameterLink([string](https://docs.microsoft.com/en-us/dotnet/api/system.string) name)

## DotNetCommentParameterLink([string](https://docs.microsoft.com/en-us/dotnet/api/system.string) name, [WithoutHaste.DataFiles.DotNet.CommentTag](WithoutHaste.DataFiles.DotNet.CommentTag.md) tag)

# Static Methods

## static DotNetCommentParameterLink FromVisualStudioXml([System.Xml.Linq.XElement](https://docs.microsoft.com/en-us/dotnet/api/system.xml.linq.xelement) element)

Parses .Net XML documentation for paramref.  
