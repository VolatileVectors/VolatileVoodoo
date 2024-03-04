namespace VolatileVoodoo.Editor.CodeGenerator
{
    public sealed class EventListenerTemplate : CodeGeneratorBase
    {
        public string EventName;
        public string[] EventPayloads;
        public string[] Imports;
        public string Namespace;

        public EventListenerTemplate(string path) : base(path) { }

        protected override string FileName => EventName + "Listener.cs";

        protected override string TransformText()
        {
            foreach (var nameSpace in Imports) {
                if (nameSpace is "")
                    continue;

                WriteLine($"using {nameSpace};");
            }

            WriteLine("using VolatileVoodoo.Events.Base;");
            WriteLine();
            if (Namespace != "") {
                WriteLine($"namespace {Namespace}");
                WriteLine("{");
                PushIndent();
            }

            Write("public class ");
            Write($"{EventName}Listener");

            Write(" : GenericEventListener<");
            Write(EventName);
            foreach (var payload in EventPayloads)
                Write($", {payload}");

            WriteLine("> { }");
            if (Namespace != "") {
                PopIndent();
                Write("}");
            }

            return Generator.ToString();
        }
    }
}