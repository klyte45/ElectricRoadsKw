using ElectricRoads;
using Kwytto.Utils;
using UnityEngine;

namespace Kwytto
{
    public static class CommonProperties
    {
        public static bool DebugMode => ModInstance.DebugMode;
        public static string Version => ModInstance.Version;
        public static string FullVersion => ModInstance.FullVersion;
        public static string ModName => ModInstance.Instance.SimpleName;
        public static string Acronym { get; } = "ER";
        public static string ModRootFolder => MainController.FOLDER_PATH;
        public static string ModIcon => ModInstance.Instance.IconName;
        public static string ModDllRootFolder => ModInstance.RootFolder;

        public static string GitHubRepoPath => "klyte45/ElectricRoadsKw";


        internal static readonly string[] AssetExtraDirectoryNames = new string[0];
        internal static readonly string[] AssetExtraFileNames = new string[] { };

        public static Color ModColor { get; } = ColorExtensions.FromRGB("643e00");
        public static float UIScale { get; } = 1f;
    }
}