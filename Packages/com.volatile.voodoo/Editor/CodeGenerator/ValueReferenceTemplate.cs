namespace VolatileVoodoo.Editor.CodeGenerator
{
    public class ValueReferenceTemplate : CodeGeneratorBase
    {
        public string Namespace;
        public string[] Imports;
        public string ValueName;
        public string ValueReferenceName;
        public string ValueReferenceType;

        public ValueReferenceTemplate(string path) : base(path) { }
        
        protected override string TransformText()
        {
            WriteLine("using System;");
            foreach (var nameSpace in Imports)
            {
                if (nameSpace is "System") {
                    continue;
                }

                WriteLine($"using {nameSpace};");
            }
            
            WriteLine("using VolatileVoodoo.Runtime.Values.Base;");
            WriteLine();
            WriteLine($"namespace {Namespace}");
            WriteLine("{");
            PushIndent();
            WriteLine($"[Serializable]");
            WriteLine($"public class {ValueReferenceName} : GenericReference<{ValueName}, {ValueReferenceType}> {{ }}");
            PopIndent();
            Write("}");
            return Generator.ToString();
        }

        protected override string FileName => ValueReferenceName + ".cs";
    }
}