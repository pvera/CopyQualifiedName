using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Community.VisualStudio.Toolkit;
using EnvDTE;
using EnvDTE80;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.LanguageServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;

namespace CopyQualifiedName
{
    /// <summary>
    /// Service to get the fully qualified name of symbols at the caret position
    /// </summary>
    public static class QualifiedNameService
    {
        /// <summary>
        /// Gets the fully qualified name of the symbol at the current caret position
        /// </summary>
        /// <param name="includeNamespace">Whether to include the namespace in the result</param>
        /// <returns>The qualified name, or null if no symbol found</returns>
        public static async Task<string?> GetQualifiedNameAtCaretAsync(bool includeNamespace = true)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            try
            {
                // Get the active document view
                var docView = await VS.Documents.GetActiveDocumentViewAsync();
                if (docView?.TextView == null)
                    return null;

                var textView = docView.TextView;
                var caretPosition = textView.Caret.Position.BufferPosition;

                // Get Roslyn workspace
                var componentModel = await VS.Services.GetComponentModelAsync();
                if (componentModel == null)
                    return null;

                var workspace = componentModel.GetService<VisualStudioWorkspace>();
                if (workspace == null)
                    return null;

                // Get the document from the text buffer
                var document = caretPosition.Snapshot.GetOpenDocumentInCurrentContextWithChanges();
                if (document == null)
                    return null;

                // Get semantic model and syntax tree
                var semanticModel = await document.GetSemanticModelAsync();
                var syntaxRoot = await document.GetSyntaxRootAsync();
                
                if (semanticModel == null || syntaxRoot == null)
                    return null;

                var position = caretPosition.Position;
                var token = syntaxRoot.FindToken(position);

                // Try to find the symbol - walk up the syntax tree
                ISymbol? symbol = null;
                var node = token.Parent;

                while (node != null && symbol == null)
                {
                    symbol = semanticModel.GetDeclaredSymbol(node);
                    
                    // Also check if we're on a reference to a symbol
                    if (symbol == null)
                    {
                        var symbolInfo = semanticModel.GetSymbolInfo(node);
                        symbol = symbolInfo.Symbol;
                    }

                    node = node.Parent;
                }

                // If still no symbol, try getting it from the token directly
                if (symbol == null)
                {
                    var symbolInfo = semanticModel.GetSymbolInfo(token.Parent!);
                    symbol = symbolInfo.Symbol ?? symbolInfo.CandidateSymbols.FirstOrDefault();
                }

                if (symbol == null)
                    return null;

                return GetFormattedSymbolName(symbol, includeNamespace);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting qualified name: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Formats the symbol name according to the specified options
        /// </summary>
        private static string GetFormattedSymbolName(ISymbol symbol, bool includeNamespace)
        {
            if (includeNamespace)
            {
                // Full qualified name: Namespace.Class.Member
                var format = new SymbolDisplayFormat(
                    globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Omitted,
                    typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
                    memberOptions: SymbolDisplayMemberOptions.IncludeContainingType,
                    genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
                    parameterOptions: SymbolDisplayParameterOptions.None,
                    miscellaneousOptions: SymbolDisplayMiscellaneousOptions.UseSpecialTypes
                );

                return symbol.ToDisplayString(format);
            }
            else
            {
                // Without namespace: Class.Member
                var format = new SymbolDisplayFormat(
                    globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Omitted,
                    typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypes,
                    memberOptions: SymbolDisplayMemberOptions.IncludeContainingType,
                    genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
                    parameterOptions: SymbolDisplayParameterOptions.None,
                    miscellaneousOptions: SymbolDisplayMiscellaneousOptions.UseSpecialTypes
                );

                return symbol.ToDisplayString(format);
            }
        }

        /// <summary>
        /// Copies text to the clipboard and shows a status bar message
        /// </summary>
        public static async Task CopyToClipboardAndNotifyAsync(string? text)
        {
            if (string.IsNullOrEmpty(text))
            {
                await VS.StatusBar.ShowMessageAsync("No symbol found at caret position");
                return;
            }

            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            try
            {
                Clipboard.SetText(text);
                await VS.StatusBar.ShowMessageAsync($"Copied: {text}");
            }
            catch (Exception ex)
            {
                await VS.StatusBar.ShowMessageAsync($"Failed to copy: {ex.Message}");
            }
        }
    }
}
