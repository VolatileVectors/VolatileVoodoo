namespace VolatileVoodoo.Editor.CodeGenerator
{
    public class ValueReferenceTemplate : CodeGeneratorBase
    {
        public string[] Imports;
        public string Namespace;
        public string ValueName;
        public string ValueReferenceName;
        public string ValueReferenceType;

        public ValueReferenceTemplate(string path) : base(path) { }

        protected override string FileName => ValueReferenceName + ".cs";

        protected override string TransformText()
        {
            WriteLine("using System;");
            foreach (var nameSpace in Imports) {
                if (nameSpace is "System" or "" or null)
                    continue;

                WriteLine($"using {nameSpace};");
            }

            WriteLine("using VolatileVoodoo.Values.Base;");
            WriteLine();
            if (!string.IsNullOrEmpty(Namespace)) {
                WriteLine($"namespace {Namespace}");
                WriteLine("{");
                PushIndent();
            }

            WriteLine("[Serializable]");
            WriteLine($"public class {ValueReferenceName} : GenericReference<{ValueName}, {ValueReferenceType}> {{ }}");
            if (!string.IsNullOrEmpty(Namespace)) {
                PopIndent();
                Write("}");
            }

            return Generator.ToString();
        }
    }
}