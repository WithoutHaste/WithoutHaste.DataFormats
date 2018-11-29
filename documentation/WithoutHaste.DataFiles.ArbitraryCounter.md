# [WithoutHaste.DataFiles](TableOfContents.WithoutHaste.DataFiles.md).ArbitraryCounter

**Abstract**  
**Inheritance:** [object](https://docs.microsoft.com/en-us/dotnet/api/system.object)  

Counts using an arbitrary set of digits.  

**Remarks:**  
Using "integers" as an analogy, Counter values cannot be negative.  

# Fields

## protected readonly [Char[]](https://docs.microsoft.com/en-us/dotnet/api/system.char[]) VALID_CHARACTERS

All valid characters, in order from smallest to largest.  

## protected [List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1) value

Current internal value.  

# Properties

## [string](https://docs.microsoft.com/en-us/dotnet/api/system.string) MINIMUM_VALUE { get; }

Counter can never decrement below the minimum value.  

## [string](https://docs.microsoft.com/en-us/dotnet/api/system.string) Value { get; }

Current display value.  

# Constructors

## ArbitraryCounter([Char[]](https://docs.microsoft.com/en-us/dotnet/api/system.char[]) validCharacters)

Initialize at MINIMUM_VALUE.  

# Methods

## protected [List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1) CopyValue()

Returns an independent copy of the value.  

## protected [void](https://docs.microsoft.com/en-us/dotnet/api/system.void) Decrement([List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1) value, [int](https://docs.microsoft.com/en-us/dotnet/api/system.int32) delta)

Increment a value.  

## protected [void](https://docs.microsoft.com/en-us/dotnet/api/system.void) Increment([List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1) value, [int](https://docs.microsoft.com/en-us/dotnet/api/system.int32) delta)

Increment a value.  

## [void](https://docs.microsoft.com/en-us/dotnet/api/system.void) SetValue([string](https://docs.microsoft.com/en-us/dotnet/api/system.string) start)

## protected [void](https://docs.microsoft.com/en-us/dotnet/api/system.void) SetValue([List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1) start)
