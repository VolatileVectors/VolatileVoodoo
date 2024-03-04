namespace VolatileVoodoo.Editor.CodeGenerator
{
    public class ValueTemplate : CodeGeneratorBase
    {
        public string[] Imports;
        public string Namespace;
        public string ValueName;
        public string ValueType;

        public ValueTemplate(string path) : base(path) { }

        protected override string FileName => ValueName + ".cs";

        protected override string TransformText()
        {
            WriteLine("using UnityEngine;");
            foreach (var nameSpace in Imports) {
                if (nameSpace is "UnityEngine" or "")
                    continue;

                WriteLine($"using {nameSpace};");
            }

            WriteLine("using VolatileVoodoo.Values.Base;");
            WriteLine();
            if (Namespace != "") {
                WriteLine($"namespace {Namespace}");
                WriteLine("{");
                PushIndent();
            }

            WriteLine($"[CreateAssetMenu(fileName = \"{ValueName}\", menuName = \"Voodoo/Values/{ValueName}\")]");
            WriteLine($"public class {ValueName} : GenericValue<{ValueType}> {{ }}");
            if (Namespace != "") {
                PopIndent();
                Write("}");
            }

            return Generator.ToString();
        }
    }
}