using System.IO;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Capybutler.Editor.Build
{
    public class PostprocessIncludeWithBuild : IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPostprocessBuild(BuildReport report)
        {
            var basePath = PathUtils.ApplicationProjectPath;
            var includePath = PathUtils.ProjectPathToFullPath("IncludeWithBuild");
            var outputPath = Path.GetDirectoryName(report.summary.outputPath);

            LogButler.Info("[PostprocessIncludeWithBuild] Adding " + Path.GetRelativePath(basePath, includePath) + " content to " + Path.GetRelativePath(basePath, outputPath));
            CopyDirectory(includePath, outputPath);
            LogButler.Info("[PostprocessIncludeWithBuild] Successful");
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