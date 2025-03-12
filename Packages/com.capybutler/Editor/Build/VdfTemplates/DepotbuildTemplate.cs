using UnityEngine;

namespace Capybutler.Editor.Build.VdfTemplates
{
    public sealed class DepotbuildTemplate : FileGenerator
    {
        public int DepotId;

        public DepotbuildTemplate(string path) : base(path) { }

        protected override string FileName => $"depot_{DepotId}.vdf";

        protected override string TransformText()
        {
            WriteLine("\"DepotBuildConfig\"");
            WriteLine("{");
            PushIndent();
            WriteLine($"\"DepotID\" \"{DepotId}\"");
            WriteLine($"\"contentroot\" \"{PathUtils.ProjectPathToFullPath($"Build/{Application.productName}")}\"");
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