namespace VolatileVoodoo.Editor.CodeGenerator
{
    public sealed class EventListenerTemplate : CodeGeneratorBase
    {
        public string Namespace;
        public string[] Imports;
        public string EventName;
        public string[] EventPayloads;

        public EventListenerTemplate(string path) : base(path) { }
        
        protected override string TransformText()
        {
            foreach (var nameSpace in Imports) {
                WriteLine($"using {nameSpace};");
            }
            
            WriteLine("using VolatileVoodoo.Runtime.Events.Base;");
            WriteLine();
            WriteLine($"namespace {Namespace}");
            WriteLine("{");
            PushIndent();
            Write("public class ");
            Write($"{EventName}Listener");
            
            Write(" : GenericEventListener<");
            Write(EventName);
            foreach (var payload in EventPayloads) {
                Write($", {payload}");
            }
            WriteLine("> { }");
            PopIndent();
            Write("}");
            return Generator.ToString();
        }

        protected override string FileName => EventName + "Listener.cs";
    }
}