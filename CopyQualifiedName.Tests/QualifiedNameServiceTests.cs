using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CopyQualifiedName.Tests
{
    [TestClass]
    public class QualifiedNameServiceTests
    {
        #region C# Tests

        [TestMethod]
        public void GetFormattedSymbolName_CSharpClass_WithNamespace_ReturnsFullyQualifiedName()
        {
            // Arrange
            var code = @"
namespace MyNamespace
{
    public class MyClass { }
}";
            var symbol = GetSymbolFromCSharpCode(code, "MyClass");

            // Act
            var result = QualifiedNameService.GetFormattedSymbolName(symbol!, includeNamespace: true);

            // Assert
            Assert.AreEqual("MyNamespace.MyClass", result);
        }

        [TestMethod]
        public void GetFormattedSymbolName_CSharpClass_WithoutNamespace_ReturnsClassName()
        {
            // Arrange
            var code = @"
namespace MyNamespace
{
    public class MyClass { }
}";
            var symbol = GetSymbolFromCSharpCode(code, "MyClass");

            // Act
            var result = QualifiedNameService.GetFormattedSymbolName(symbol!, includeNamespace: false);

            // Assert
            Assert.AreEqual("MyClass", result);
        }

        [TestMethod]
        public void GetFormattedSymbolName_CSharpMethod_WithNamespace_ReturnsFullyQualifiedName()
        {
            // Arrange
            var code = @"
namespace MyNamespace
{
    public class MyClass
    {
        public void MyMethod() { }
    }
}";
            var symbol = GetSymbolFromCSharpCode(code, "MyMethod");

            // Act
            var result = QualifiedNameService.GetFormattedSymbolName(symbol!, includeNamespace: true);

            // Assert
            Assert.AreEqual("MyNamespace.MyClass.MyMethod", result);
        }

        [TestMethod]
        public void GetFormattedSymbolName_CSharpMethod_WithoutNamespace_ReturnsClassAndMethod()
        {
            // Arrange
            var code = @"
namespace MyNamespace
{
    public class MyClass
    {
        public void MyMethod() { }
    }
}";
            var symbol = GetSymbolFromCSharpCode(code, "MyMethod");

            // Act
            var result = QualifiedNameService.GetFormattedSymbolName(symbol!, includeNamespace: false);

            // Assert
            Assert.AreEqual("MyClass.MyMethod", result);
        }

        [TestMethod]
        public void GetFormattedSymbolName_CSharpProperty_WithNamespace_ReturnsFullyQualifiedName()
        {
            // Arrange
            var code = @"
namespace MyNamespace
{
    public class MyClass
    {
        public string MyProperty { get; set; }
    }
}";
            var symbol = GetSymbolFromCSharpCode(code, "MyProperty");

            // Act
            var result = QualifiedNameService.GetFormattedSymbolName(symbol!, includeNamespace: true);

            // Assert
            Assert.AreEqual("MyNamespace.MyClass.MyProperty", result);
        }

        [TestMethod]
        public void GetFormattedSymbolName_CSharpNestedClass_WithNamespace_ReturnsFullyQualifiedName()
        {
            // Arrange
            var code = @"
namespace MyNamespace
{
    public class OuterClass
    {
        public class InnerClass { }
    }
}";
            var symbol = GetSymbolFromCSharpCode(code, "InnerClass");

            // Act
            var result = QualifiedNameService.GetFormattedSymbolName(symbol!, includeNamespace: true);

            // Assert
            Assert.AreEqual("MyNamespace.OuterClass.InnerClass", result);
        }

        [TestMethod]
        public void GetFormattedSymbolName_CSharpNestedClass_WithoutNamespace_ReturnsOuterAndInnerClass()
        {
            // Arrange
            var code = @"
namespace MyNamespace
{
    public class OuterClass
    {
        public class InnerClass { }
    }
}";
            var symbol = GetSymbolFromCSharpCode(code, "InnerClass");

            // Act
            var result = QualifiedNameService.GetFormattedSymbolName(symbol!, includeNamespace: false);

            // Assert
            Assert.AreEqual("OuterClass.InnerClass", result);
        }

        [TestMethod]
        public void GetFormattedSymbolName_CSharpGenericClass_WithNamespace_IncludesTypeParameter()
        {
            // Arrange
            var code = @"
namespace MyNamespace
{
    public class MyGenericClass<T> { }
}";
            var symbol = GetSymbolFromCSharpCode(code, "MyGenericClass");

            // Act
            var result = QualifiedNameService.GetFormattedSymbolName(symbol!, includeNamespace: true);

            // Assert
            Assert.AreEqual("MyNamespace.MyGenericClass<T>", result);
        }

        [TestMethod]
        public void GetFormattedSymbolName_CSharpGenericMethod_WithNamespace_IncludesTypeParameter()
        {
            // Arrange
            var code = @"
namespace MyNamespace
{
    public class MyClass
    {
        public void MyGenericMethod<T>() { }
    }
}";
            var symbol = GetSymbolFromCSharpCode(code, "MyGenericMethod");

            // Act
            var result = QualifiedNameService.GetFormattedSymbolName(symbol!, includeNamespace: true);

            // Assert
            Assert.AreEqual("MyNamespace.MyClass.MyGenericMethod<T>", result);
        }

        [TestMethod]
        public void GetFormattedSymbolName_CSharpNestedNamespace_ReturnsFullPath()
        {
            // Arrange
            var code = @"
namespace MyNamespace.SubNamespace
{
    public class MyClass { }
}";
            var symbol = GetSymbolFromCSharpCode(code, "MyClass");

            // Act
            var result = QualifiedNameService.GetFormattedSymbolName(symbol!, includeNamespace: true);

            // Assert
            Assert.AreEqual("MyNamespace.SubNamespace.MyClass", result);
        }

        #endregion

        #region VB.NET Tests

        [TestMethod]
        public void GetFormattedSymbolName_VBClass_WithNamespace_ReturnsFullyQualifiedName()
        {
            // Arrange
            var code = @"
Namespace foo
    Public Class bar
    End Class
End Namespace";
            var symbol = GetSymbolFromVBCode(code, "bar");

            // Act
            var result = QualifiedNameService.GetFormattedSymbolName(symbol!, includeNamespace: true);

            // Assert
            Assert.AreEqual("foo.bar", result);
        }

        [TestMethod]
        public void GetFormattedSymbolName_VBMethod_WithNamespace_ReturnsFullyQualifiedName()
        {
            // Arrange
            var code = @"
Namespace foo
    Public Class bar
        Public Sub HelloWorld()
        End Sub
    End Class
End Namespace";
            var symbol = GetSymbolFromVBCode(code, "HelloWorld");

            // Act
            var result = QualifiedNameService.GetFormattedSymbolName(symbol!, includeNamespace: true);

            // Assert
            Assert.AreEqual("foo.bar.HelloWorld", result);
        }

        [TestMethod]
        public void GetFormattedSymbolName_VBMethod_WithoutNamespace_ReturnsClassAndMethod()
        {
            // Arrange
            var code = @"
Namespace foo
    Public Class bar
        Public Sub HelloWorld()
        End Sub
    End Class
End Namespace";
            var symbol = GetSymbolFromVBCode(code, "HelloWorld");

            // Act
            var result = QualifiedNameService.GetFormattedSymbolName(symbol!, includeNamespace: false);

            // Assert
            Assert.AreEqual("bar.HelloWorld", result);
        }

        #endregion

        #region Helper Methods

        private static ISymbol? GetSymbolFromCSharpCode(string code, string symbolName)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            var compilation = CSharpCompilation.Create("TestAssembly")
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .AddSyntaxTrees(syntaxTree);

            var semanticModel = compilation.GetSemanticModel(syntaxTree);
            var root = syntaxTree.GetRoot();

            // Find all declared symbols and return the one matching the name
            var symbols = root.DescendantNodes()
                .Select(node => semanticModel.GetDeclaredSymbol(node))
                .Where(s => s != null && s.Name == symbolName)
                .ToList();

            return symbols.FirstOrDefault();
        }

        private static ISymbol? GetSymbolFromVBCode(string code, string symbolName)
        {
            var syntaxTree = Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxTree.ParseText(code);
            var compilation = Microsoft.CodeAnalysis.VisualBasic.VisualBasicCompilation.Create("TestAssembly")
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .AddSyntaxTrees(syntaxTree);

            var semanticModel = compilation.GetSemanticModel(syntaxTree);
            var root = syntaxTree.GetRoot();

            // Find all declared symbols and return the one matching the name
            var symbols = root.DescendantNodes()
                .Select(node => semanticModel.GetDeclaredSymbol(node))
                .Where(s => s != null && s.Name == symbolName)
                .ToList();

            return symbols.FirstOrDefault();
        }

        #endregion
    }
}
