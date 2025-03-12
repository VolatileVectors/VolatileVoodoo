using System.IO;

namespace Capybutler.Editor.Build.VdfTemplates
{
    public sealed class AppbuildTemplate : FileGenerator
    {
        public int AppId;
        public BuildButler.BuildType BuildType;
        public int DepotId;
        public string Description;

        public AppbuildTemplate(string path) : base(path) { }

        protected override string FileName => $"app_{AppId}.vdf";

        protected override string TransformText()
        {
            var buildDescription = BuildType switch
            {
                BuildButler.BuildType.Development => "[Development]",
                BuildButler.BuildType.Beta => "[Beta]",
                BuildButler.BuildType.ReleaseCandidate => "[Release Candidate]",
                _ => "[Public]"
            } + (string.IsNullOrWhiteSpace(Description) ? "" : $" {Description}");

            WriteLine("\"appbuild\"");
            WriteLine("{");
            PushIndent();
            WriteLine($"\"appid\" \"{AppId}\"");
            WriteLine($"\"desc\" \"{buildDescription}\"");
            WriteLine($"\"buildoutput\" \"{PathUtils.ProjectPathToFullPath("Build/Output")}\"");
            WriteLine("\"contentroot\" \"\"");
            WriteLine($"\"setlive\" \"{BuildType.ToString().ToLower()}\"");
            WriteLine("\"preview\" \"0\"");
            WriteLine("\"local\" \"\"");
            WriteLine("\"depots\"");
            WriteLine("{");
            PushIndent();
            WriteLine($"\"{DepotId}\" \"{PathUtils.ProjectPathToFullPath($"Build/Scripts/depot_{DepotId}.vdf")}\"");
            PopIndent();
            WriteLine("}");
            PopIndent();
            Write("}");

            return Generator.ToString();
        }
    }
}