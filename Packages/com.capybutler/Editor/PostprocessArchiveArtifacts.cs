using System.IO;
using System.Linq;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Capybutler.Editor
{
    public class PostprocessArchiveArtifacts : IPostprocessBuildWithReport
    {
        private const string ArchivePath = "Build/DoNotShip";

        private static readonly string[] ArchiveButDoNotShipPathSuffixes =
        {
            "BackUpThisFolder_ButDontShipItWithYourGame",
            "BurstDebugInformation_DoNotShip"
        };

        public int callbackOrder => 1;

        public void OnPostprocessBuild(BuildReport report)
        {
            var projectPath = Capyutils.ApplicationProjectPath;
            var destinationPath = Capyutils.ProjectPathToFullPath(ArchivePath);
            Debug.Log("[PostprocessArchiveArtifacts] Archiving 'DoNotShip' artifacts in " + Path.GetRelativePath(projectPath, destinationPath));

            var subDirectories = new DirectoryInfo(Path.GetDirectoryName(report.summary.outputPath) ?? "").GetDirectories();
            foreach (var source in subDirectories)
            {
                if (!ArchiveButDoNotShipPathSuffixes.Any(suffix => source.Name.Contains(suffix)))
                    continue;

                var target = Path.Combine(destinationPath, source.Name);
                if (Directory.Exists(target))
                    Directory.Delete(target, true);

                Debug.Log("[PostprocessArchiveArtifacts] Moving: " + Path.GetRelativePath(projectPath, source.FullName) + " => " + Path.GetRelativePath(projectPath, target));
                source.MoveTo(target);
            }

            Debug.Log("[PostprocessArchiveArtifacts] Successful");
        }
    }
}