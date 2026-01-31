using System;
using System.Threading.Tasks;
using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Shell;

namespace CopyQualifiedName
{
    /// <summary>
    /// Command to copy the fully qualified name (with namespace) to clipboard
    /// </summary>
    [Command(PackageGuids.CmdSetGuidString, 0x0100)]
    internal sealed class CopyQualifiedNameCommand : BaseCommand<CopyQualifiedNameCommand>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            try
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                var qualifiedName = await QualifiedNameService.GetQualifiedNameAtCaretAsync(includeNamespace: true);
                await QualifiedNameService.CopyToClipboardAndNotifyAsync(qualifiedName);
            }
            catch (Exception ex)
            {
                await VS.StatusBar.ShowMessageAsync($"Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"CopyQualifiedNameCommand error: {ex}");
            }
        }
    }

    /// <summary>
    /// Command to copy the qualified name without namespace to clipboard
    /// </summary>
    [Command(PackageGuids.CmdSetGuidString, 0x0101)]
    internal sealed class CopyQualifiedNameWithoutNamespaceCommand : BaseCommand<CopyQualifiedNameWithoutNamespaceCommand>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            try
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                var qualifiedName = await QualifiedNameService.GetQualifiedNameAtCaretAsync(includeNamespace: false);
                await QualifiedNameService.CopyToClipboardAndNotifyAsync(qualifiedName);
            }
            catch (Exception ex)
            {
                await VS.StatusBar.ShowMessageAsync($"Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"CopyQualifiedNameWithoutNamespaceCommand error: {ex}");
            }
        }
    }
}
