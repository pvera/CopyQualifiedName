// Package IDs and GUIDs for the extension
// These must match the VSCT file exactly 
namespace CopyQualifiedName
{
    /// <summary>
    /// Package GUIDs matching the VSCT file  
    /// </summary>
    internal static class PackageGuids
    {
        public const string PackageGuidString = "f8b3c5a7-9d2e-4f1a-b6c8-3e7d9f0a1b2c";
        public const string CmdSetGuidString = "a1b2c3d4-e5f6-7890-abcd-ef1234567890";

        public static readonly System.Guid PackageGuid = new System.Guid(PackageGuidString);
        public static readonly System.Guid CmdSetGuid = new System.Guid(CmdSetGuidString);
    }

    /// <summary>
    /// Command IDs matching the VSCT file
    /// The Command attribute expects the GUID and ID in format "guid,id"
    /// </summary>
    internal static class PackageIds
    {
        // Format: "GUID,ID" - The Community Toolkit parses this format
        public const string CopyQualifiedNameCommandId = PackageGuids.CmdSetGuidString + "," + "0x0100";
        public const string CopyQualifiedNameWithoutNamespaceCommandId = PackageGuids.CmdSetGuidString + "," + "0x0101";
    }
}
