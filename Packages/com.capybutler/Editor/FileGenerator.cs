using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Capybutler.Editor
{
    public abstract class FileGenerator
    {
        protected readonly StringBuilder Generator = new();
        private readonly List<int> indentLengths = new();
        private readonly string outputPath;
        private string currentIndentField = "";
        private bool endsWithNewline;

        protected FileGenerator(string path)
        {
            outputPath = path;
        }

        protected abstract string FileName { get; }
        public string FullFileName => Path.Combine(outputPath, FileName);

        protected void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
                return;

            if (Generator.Length == 0 || endsWithNewline) {
                Generator.Append(currentIndentField);
                endsWithNewline = false;
            }

            if (textToAppend.EndsWith(Environment.NewLine, StringComparison.CurrentCulture))
                endsWithNewline = true;

            if (currentIndentField.Length == 0) {
                Generator.Append(textToAppend);
                return;
            }

            textToAppend = textToAppend.Replace(Environment.NewLine, Environment.NewLine + currentIndentField);
            if (endsWithNewline)
                Generator.Append(textToAppend, 0, textToAppend.Length - currentIndentField.Length);
            else
                Generator.Append(textToAppend);
        }

        protected void WriteLine(string textToAppend = "")
        {
            Write(textToAppend);
            Generator.AppendLine();
            endsWithNewline = true;
        }

        protected void PushIndent(string indent = "    ")
        {
            if (string.IsNullOrEmpty(indent))
                return;

            currentIndentField += indent;
            indentLengths.Add(indent.Length);
        }

        protected void PopIndent()
        {
            if (indentLengths.Count == 0)
                return;

            var indentLength = indentLengths[^1];
            indentLengths.RemoveAt(indentLengths.Count - 1);
            if (indentLength == 0)
                return;

            currentIndentField = currentIndentField.Remove(currentIndentField.Length - indentLength);
        }

        protected void ClearIndent()
        {
            indentLengths.Clear();
            currentIndentField = "";
        }

        protected abstract string TransformText();

        public void Create()
        {
            File.WriteAllText(FullFileName, TransformText());
        }
    }
}