namespace Capybutler.Editor.Build.VdfTemplates
{
    public sealed class DepotbuildTemplate : FileGenerator
    {
        public string BuildPath;
        public int DepotId;

        public DepotbuildTemplate(string path) : base(path) { }

        protected override string FileName => $"depot_{DepotId}.vdf";

        protected override string TransformText()
        {
            WriteLine("\"DepotBuildConfig\"");
            WriteLine("{");
            PushIndent();
            WriteLine($"\"DepotID\" \"{DepotId}\"");
            WriteLine($"\"contentroot\" \"{BuildPath}\"");
            WriteLine("\"FileMapping\"");
            WriteLine("{");
            PushIndent();
            WriteLine("\"LocalPath\" \"*\"");
            WriteLine("\"DepotPath\" \".\"");
            WriteLine("\"recursive\" \"1\"");
            PopIndent();
            WriteLine("}");
            WriteLine("\"FileExclusion\" \"*.pdb\"");
            PopIndent();
            Write("}");

            return Generator.ToString();
        }
    }
}