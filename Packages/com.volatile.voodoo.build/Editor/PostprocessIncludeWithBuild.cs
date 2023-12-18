using System.IO;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using VolatileVoodoo.Runtime.Utils;

namespace VolatileVoodooBuild.Editor
{
    public class PostprocessIncludeWithBuild : IPostprocessBuildWithReport
    {
        private const string IncludeWithBuildPath = "IncludeWithBuild";
        public int callbackOrder => 0;

        public void OnPostprocessBuild(BuildReport report)
        {
            var basePath = Voodoo.ApplicationProjectPath;
            var includePath = Voodoo.ProjectPathToFullPath(IncludeWithBuildPath);
            var outputPath = Path.GetDirectoryName(report.summary.outputPath);

            Debug.Log("[PostprocessIncludeWithBuild] Adding " + Path.GetRelativePath(basePath, includePath) + "content to " + Path.GetRelativePath(basePath, outputPath));
            CopyDirectory(includePath, outputPath);
            Debug.Log("[PostprocessIncludeWithBuild] Successful");
        }

        private static void CopyDirectory(string sourcePath, string destinationPath)
        {
            var sourceDirectory = new DirectoryInfo(sourcePath);
            if (!sourceDirectory.Exists)
                return;

            if (!Directory.Exists(destinationPath))
                Directory.CreateDirectory(destinationPath);

            var sourceFiles = sourceDirectory.GetFiles();
            foreach (var file in sourceFiles)
                file.CopyTo(Path.Combine(destinationPath, file.Name), true);

            var subDirectories = sourceDirectory.GetDirectories();
            foreach (var subDirectory in subDirectories)
                CopyDirectory(subDirectory.FullName, Path.Combine(destinationPath, subDirectory.Name));
        }
    }
}