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
                if (nameSpace is "" or null)
                    continue;

                WriteLine($"using {nameSpace};");
            }

            WriteLine("using VolatileVoodoo.Events.Base;");
            WriteLine();
            if (!string.IsNullOrEmpty(Namespace)) {
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
            if (!string.IsNullOrEmpty(Namespace)) {
                PopIndent();
                Write("}");
            }

            return Generator.ToString();
        }
    }
}