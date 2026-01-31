using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Shell;

namespace CopyQualifiedName
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideAutoLoad(Microsoft.VisualStudio.VSConstants.UICONTEXT.NoSolution_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(Microsoft.VisualStudio.VSConstants.UICONTEXT.SolutionExists_string, PackageAutoLoadFlags.BackgroundLoad)]
    public sealed class CopyQualifiedNamePackage : ToolkitPackage
    {
        public const string PackageGuidString = "f8b3c5a7-9d2e-4f1a-b6c8-3e7d9f0a1b2c";

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            System.Diagnostics.Debug.WriteLine("CopyQualifiedName: Package InitializeAsync starting...");
            
            await base.InitializeAsync(cancellationToken, progress);

            // Switch to main thread for command registration
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            // Register commands
            await this.RegisterCommandsAsync();
            
            System.Diagnostics.Debug.WriteLine("CopyQualifiedName: Package InitializeAsync complete - commands registered.");
        }
    }
}
