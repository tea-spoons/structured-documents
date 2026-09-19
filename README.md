# Structured Documents
Unified document parsing for fundamentally different file formats.

Currently supported file formats:
* Markdown
* XML
* Json

## Usage
- Create an instance of any of the available subtypes of `StructuredDocument`.
- Call `Load` and pass the file's string contents.
- Use getter methods (like `GetText`) and supply them with a `DataPath` to extract information.

### Data Paths
A **data path** is a slash-seperated string that defines where to look for data.

Example (markdown):
```md
# One
## Two
Something
## Three
Information
```
would translate to
```csharp
document.GetText("one/two"); // "Something"
document.GetText("one/three"); // "Information"
```

#### Data Path Index
In some formats (like markdown), Informations can be ordered within an otherwise equal data path:
```md
# Foo
First paragraph

Second paragraph

| A table |
| ------- |

Third paragraph
```
Methods where a data path is passed accept an optional **index** for defining the `n`th block *of the same type* (!).
```csharp
document.GetText("Foo"); // "First paragraph"
document.GetText("Foo", 1); // "Second paragraph"
document.GetTable("Foo"); // Gets the table (first table under that heading)
document.GetText("Foo", 2); // "Third paragraph"
```

## Installation

In Unity: **Window > Package Manager > + > Add package from git URL**, then enter:

```
https://github.com/tea-spoons/structured-documents.git
```

Pin a release by appending a tag, for example `#v0.6.1`.

### Dependencies

Unity cannot resolve git dependencies automatically, so add these to your project first:

- `com.tea-spoons.collections` 0.6.0

## Notes

Markdown documents (which needed the Markdig library) are not included in this release. JSON support is unchanged.

## License

Copyright (c) 2026 Bigpoint. Authored by Muhammad Tarek Abdou.

Available for research, education and other noncommercial use under the [PolyForm Noncommercial 1.0.0](LICENSE.md)
license. Commercial use is not permitted.
