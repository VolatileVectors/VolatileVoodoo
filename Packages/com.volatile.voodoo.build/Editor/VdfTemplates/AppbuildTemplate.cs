using VolatileVoodoo.Editor.CodeGenerator;
using VolatileVoodoo.Runtime.Utils;

namespace VolatileVoodooBuild.Editor.VdfTemplates
{
    public sealed class AppbuildTemplate : CodeGeneratorBase
    {
        public int AppId;
        public BuildTool.BuildType BuildType;
        public int DepotId;
        public string Description;

        public AppbuildTemplate(string path) : base(path) { }

        protected override string FileName => $"app_{AppId}.vdf";

        protected override string TransformText()
        {
            var buildDescription = BuildType switch {
                BuildTool.BuildType.Development => "[Development]",
                BuildTool.BuildType.Beta => "[Beta]",
                BuildTool.BuildType.ReleaseCandidate => "[Release Candidate]",
                _ => "[Public]"
            };
            buildDescription += string.IsNullOrWhiteSpace(Description) ? "" : $" {Description}";

            WriteLine("\"appbuild\"");
            WriteLine("{");
            PushIndent();
            WriteLine($"\"appid\" \"{AppId}\"");
            WriteLine($"\"desc\" \"{buildDescription}\"");
            WriteLine($"\"buildoutput\" \"{Voodoo.ProjectPathToFullPath("Build/Output")}\"");
            WriteLine("\"contentroot\" \"\"");
            WriteLine($"\"setlive\" \"{BuildType.ToString().ToLower()}\"");
            WriteLine("\"preview\" \"0\"");
            WriteLine("\"local\" \"\"");
            WriteLine("\"depots\"");
            WriteLine("{");
            PushIndent();
            WriteLine($"\"{DepotId}\" \"{Voodoo.ProjectPathToFullPath($"Build/Scripts/depot_{DepotId}.vdf")}\"");
            PopIndent();
            WriteLine("}");
            PopIndent();
            Write("}");

            return Generator.ToString();
        }
    }
}