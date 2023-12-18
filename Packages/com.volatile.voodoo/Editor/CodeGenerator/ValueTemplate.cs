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
                if (nameSpace is "UnityEngine")
                    continue;

                WriteLine($"using {nameSpace};");
            }

            WriteLine("using VolatileVoodoo.Runtime.Values.Base;");
            WriteLine();
            WriteLine($"namespace {Namespace}");
            WriteLine("{");
            PushIndent();
            WriteLine($"[CreateAssetMenu(fileName = \"{ValueName}\", menuName = \"Voodoo/Values/{ValueName}\")]");
            WriteLine($"public class {ValueName} : GenericValue<{ValueType}> {{ }}");
            PopIndent();
            Write("}");
            return Generator.ToString();
        }
    }
}