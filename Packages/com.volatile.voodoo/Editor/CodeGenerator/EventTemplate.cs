using Sirenix.Utilities;

namespace VolatileVoodoo.Editor.CodeGenerator
{
    public class EventTemplate : CodeGeneratorBase
    {
        public string Namespace;
        public string[] Imports;
        public string EventName;
        public string[] EventPayloads;

        public EventTemplate(string path) : base(path) { }
        
        protected override string TransformText()
        {
            var payloads = string.Join(", ", EventPayloads);

            WriteLine("using System;");
            WriteLine("using UnityEngine;");

            foreach (var nameSpace in Imports)
            {
                if (nameSpace is "System" or "UnityEngine") {
                    continue;
                }
                
                WriteLine($"using {nameSpace};");
            }

            WriteLine("using VolatileVoodoo.Runtime.Events.Base;");
            WriteLine();
            WriteLine($"namespace {Namespace}");
            WriteLine("{");
            PushIndent();
            WriteLine($"[CreateAssetMenu(fileName = \"{EventName}\", menuName = \"Voodoo/Events/{EventName}\")]");
            if (string.IsNullOrEmpty(payloads)) {
                WriteLine($"public class {EventName} : GenericEvent {{ }}");
            } else {
                Write($"public class {EventName} : GenericEvent<");
                Write(payloads);
                WriteLine("> { }");    
            }
            PopIndent();
            WriteLine();
            PushIndent();
            WriteLine("[Serializable]");
            Write($"public class {EventName}Source : GenericEventSource<{EventName}");
            if (!string.IsNullOrEmpty(payloads)) {
                Write($", {payloads}");
            }
            WriteLine("> { }");
            PopIndent();
            Write("}");
            return Generator.ToString();
        }

        protected override string FileName => EventName + ".cs";
    }
}