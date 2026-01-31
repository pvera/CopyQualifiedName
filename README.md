# Copy Fully Qualified Name - Visual Studio Extension

A Visual Studio 2022 extension that allows you to right-click on any method, property, class, or member and copy its fully qualified name to the clipboard. This was built out of frustration from not being able to easily get the fully qualified names of members in large projects. 

It gets old after the 100th in a row that you are trying to update a wiki article or a long commit message. 

## Features

- **Copy Fully Qualified Name**: Copies `Namespace.Class.Member` format
- **Copy Qualified Name (without namespace)**: Copies `Class.Member` format
- Works with C# and VB.NET
- Keyboard shortcut: `Ctrl+Shift+Alt+Q`

## Installation

1. Build the solution in Visual Studio 2022
2. Double-click the generated `.vsix` file in `bin\Debug` or `bin\Release`
3. Restart Visual Studio

## Usage

1. Place your cursor on any method, property, class, or member
2. Right-click to open the context menu
3. Select **"Copy Fully Qualified Name"** or **"Copy Qualified Name (without namespace)"**
4. The name is now in your clipboard!

### Example

If your cursor is on `HelloWorld` in:

```vb
Namespace foo
    Public Class bar
        Public Sub HelloWorld()
        End Sub
    End Class
End Namespace
```

- **Copy Fully Qualified Name** ? `foo.bar.HelloWorld`
- **Copy Qualified Name (without namespace)** ? `bar.HelloWorld`

## Building from Source

### Prerequisites

- Visual Studio 2022 (17.0 or later)
- Visual Studio extension development workload
- .NET Framework 4.8 SDK

### Build Steps

```bash
git clone https://github.com/yourusername/CopyQualifiedName.git
cd CopyQualifiedName
dotnet restore
dotnet build
```

## License

MIT License - See [LICENSE.txt](LICENSE.txt)
