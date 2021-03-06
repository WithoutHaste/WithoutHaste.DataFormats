# [WithoutHaste.DataFiles.DotNet](TableOfContents.WithoutHaste.DataFiles.DotNet.md).DotNetCommentMethodLink

**Inheritance:** object → [DotNetComment](WithoutHaste.DataFiles.DotNet.DotNetComment.md) → [DotNetCommentQualifiedLink](WithoutHaste.DataFiles.DotNet.DotNetCommentQualifiedLink.md)  
**Implements:** [IDotNetCommentLink](WithoutHaste.DataFiles.DotNet.IDotNetCommentLink.md)  

Represents a link in the comments to an internal or extenal method.  

# Properties

## FullSignature

**string { public get; }**  

Fully qualified method name with parameters.  

**Example A:**  
Namespace.Type.Method()  

**Example B:**  
Namespace.Type.Method(int,string)  

**Example C:**  
`Namespace.Type.Method(System.Collections.Generic.List<int>)`  

## MethodName

**[DotNetQualifiedMethodName](WithoutHaste.DataFiles.DotNet.DotNetQualifiedMethodName.md) { public get; }**  

Strongly typed name.  

# Constructors

## DotNetCommentMethodLink([DotNetQualifiedMethodName](WithoutHaste.DataFiles.DotNet.DotNetQualifiedMethodName.md) name)

# Methods

## MatchesSignature([DotNetMethod](WithoutHaste.DataFiles.DotNet.DotNetMethod.md) method)

**bool**  

Returns true if this method link and the method have matching signatures, based on the fully qualified name and the list of parameter types.  

# Static Methods

## FromVisualStudioXml(string cref)

**static DotNetCommentMethodLink**  

Parses .Net XML documentation cref for methods.  

**Example A:**  
`<permission cref="Namespace.Type.Method(Type1, Type2)">nested comments and/or plain text</permission>`  

