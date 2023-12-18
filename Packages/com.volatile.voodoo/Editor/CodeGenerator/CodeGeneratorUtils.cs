using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.CSharp;
using Sirenix.Utilities;
using UnityEditor;

namespace VolatileVoodoo.Editor.CodeGenerator
{
    public static class CodeGeneratorUtils
    {
        // Type resolver
        private static List<Type> typeCache;
        private static Dictionary<string, Type> friendlyNamesTypeCache;
        private static Dictionary<string, string> friendlyNamesReverseTypeCache;

        [InitializeOnLoadMethod]
        public static void InitTypeCache()
        {
            if (typeCache != null && friendlyNamesTypeCache != null)
                return;

            typeCache = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(assembly => !assembly.IsDynamic)
                .Aggregate(new List<Type>(), (typeList, assembly) => typeList.Union(assembly.GetExportedTypes()).ToList());

            using var provider = new CSharpCodeProvider();
            friendlyNamesTypeCache = Assembly
                .GetAssembly(typeof(int)).DefinedTypes
                .Where(definedType => string.Equals(definedType.Namespace, "System"))
                .Select(definedType => new KeyValuePair<string, Type>(provider.GetTypeOutput(new CodeTypeReference(definedType)), Type.GetType(definedType.FullName ?? "")))
                .Where(item => item.Key.IndexOf('.') == -1)
                .ToDictionary(item => item.Key, item => item.Value);

            friendlyNamesReverseTypeCache = friendlyNamesTypeCache.ToDictionary(item => item.Value.Name, item => item.Key);
        }

        private static string ArraysParser(string typeName, out List<int> arrays)
        {
            arrays = new List<int>();

            var rank = 1;
            var inArray = false;
            int pos;
            for (pos = typeName.Length - 1; pos >= 0; --pos)
                if (typeName[pos] == ']') {
                    if (inArray) return null;
                    inArray = true;
                } else if (typeName[pos] == '[') {
                    if (!inArray)
                        return null;

                    inArray = false;
                    arrays.Add(rank);
                    rank = 1;
                } else if (typeName[pos] == ',') {
                    if (!inArray)
                        return null;

                    rank++;
                } else if (typeName[pos] != ' ') {
                    break;
                }

            return inArray ? null : typeName[..(pos + 1)];
        }

        private static bool GenericsParser(string generics, out List<string> genericTypeParameters)
        {
            genericTypeParameters = new List<string>();

            var depth = 0;
            var subTypeStart = 0;

            for (var i = 0; i < generics.Length; ++i) {
                switch (generics[i]) {
                    case '<':
                    case '[':
                        depth++;
                        break;
                    case '>':
                    case ']':
                        depth--;
                        break;
                    case ',':
                        if (depth != 0)
                            continue;

                        genericTypeParameters.Add(generics[subTypeStart..i].Trim());
                        subTypeStart = i + 1;
                        break;
                }

                if (depth < 0)
                    return false;
            }

            if (subTypeStart >= generics.Length)
                return false;

            genericTypeParameters.Add(generics[subTypeStart..].Trim());
            return depth == 0;
        }

        public static Type ResolveTypeFromString(string typeName, out string[] nameSpaces)
        {
            var result = ResolveNestedTypeFromString(typeName.Trim(), out var listNameSpaces);
            nameSpaces = result == null ? new string[] { } : listNameSpaces.Distinct().ToArray();
            return result;
        }

        private static Type ResolveNestedTypeFromString(string typeName, out List<string> nameSpaces)
        {
            // init out parameter
            nameSpaces = new List<string>();

            // null for empty strings
            if (typeName.IsNullOrWhitespace())
                return null;

            // handle arrays
            typeName = ArraysParser(typeName, out var arrays);
            if (typeName == null)
                return null;

            // handle nullable
            var isNullable = false;
            if (typeName.LastIndexOf('?') == typeName.Length - 1) {
                isNullable = true;
                typeName = typeName[..^1].Trim();
            }

            // handle generic type parameters
            var generics = new List<Type>();

            var openGeneric = typeName.IndexOf('<');
            var closeGeneric = typeName.LastIndexOf('>');

            if (openGeneric > 0 && closeGeneric == typeName.Length - 1) {
                var subTypesString = typeName[(openGeneric + 1)..closeGeneric];
                if (!GenericsParser(subTypesString, out var subTypes))
                    return null;

                foreach (var subType in subTypes) {
                    var st = ResolveNestedTypeFromString(subType, out var subNameSpaces);
                    if (st == null)
                        return null;

                    generics.Add(st);
                    nameSpaces = nameSpaces.Union(subNameSpaces).ToList();
                }

                typeName = typeName[..openGeneric].Trim() + $"`{subTypes.Count}";
            } else if (!(openGeneric == -1 && closeGeneric == -1)) {
                return null;
            }

            // try to find find first matching type in assemblies type cache,try friendly type names cache in case of a miss
            var t = typeCache.FirstOrDefault(item => string.Equals(item.Name, typeName)) ?? friendlyNamesTypeCache.GetValueOrDefault(typeName);

            if (t == null)
                return null;

            nameSpaces.Add(t.Namespace);

            // handle generic type parameters
            if (generics.Count > 0)
                t = t.MakeGenericType(generics.ToArray());

            // handle nullable
            if (isNullable) {
                if (!t.IsValueType)
                    return null;

                t = typeof(Nullable<>).MakeGenericType(t);
            }

            // handle arrays
            if (arrays.Count > 0)
                t = arrays.Aggregate(t, (current, rank) => rank > 1 ? current.MakeArrayType(rank) : current.MakeArrayType());

            return t;
        }

        public static string PrettyName(this Type t)
        {
            var prettyName = "";
            var name = t.Name;
            var isNullable = t.Name.Contains("Nullable");

            if (t.HasElementType) {
                prettyName = t.GetElementType().PrettyName();
            } else if (isNullable) {
                prettyName = t.GetGenericArguments()[0].PrettyName();
            } else if (t.IsGenericType) {
                var generics = t.GetGenericArguments();
                for (var i = 0; i < generics.Length; ++i) {
                    if (i != 0)
                        prettyName += ", ";

                    prettyName += generics[i].PrettyName();
                }

                prettyName = '<' + prettyName + '>';
                name = name[..name.LastIndexOf('`')];
            }

            if (!t.HasElementType) {
                if (isNullable)
                    prettyName += '?';
                else
                    prettyName = (friendlyNamesReverseTypeCache.GetValueOrDefault(name) ?? name) + prettyName;
            } else {
                ArraysParser(name, out var arrays);
                foreach (var arrayRank in arrays) {
                    prettyName += '[';
                    prettyName += new string(',', arrayRank - 1);
                    prettyName += ']';
                }
            }

            return prettyName;
        }

        public static int AdjustCursor(string origText, string updatedText, int cursorPos)
        {
            var insertion = 0;
            var deletion = 0;
            for (var i = 0; i < Math.Min(cursorPos, updatedText.Length); ++i) {
                var j = i + insertion - deletion;
                if (origText[i] == updatedText[j])
                    continue;

                if (origText[i] == ' ')
                    while (origText[i] != updatedText[j] && i < Math.Min(cursorPos, updatedText.Length)) {
                        deletion++;
                        i++;
                    }
                else if (updatedText[j] == ' ')
                    insertion++;
            }

            return Math.Min(cursorPos + insertion - deletion, updatedText.Length);
        }

        public static string PrettyIdentifier(string typeName)
        {
            return BuildPrettyIdentifier(typeName.Replace(" ", ""));
        }

        private static string BuildPrettyIdentifier(string typeName)
        {
            if (typeName.IsNullOrWhitespace())
                return "";

            var isArray = false;
            if (typeName.LastIndexOf(']') == typeName.Length - 1) {
                isArray = true;
                int pos;
                for (pos = typeName.Length; pos > 0; --pos)
                    if (typeName[pos - 1] != '[' && typeName[pos - 1] != ']' && typeName[pos - 1] != ',')
                        break;

                typeName = typeName[..pos];
            }

            var isNullable = false;
            if (typeName.LastIndexOf('?') == typeName.Length - 1) {
                isNullable = true;
                typeName = typeName[..^1];
            }

            var generics = "";

            var openGeneric = typeName.IndexOf('<');
            var closeGeneric = typeName.LastIndexOf('>');

            if (openGeneric > 0 && closeGeneric == typeName.Length - 1) {
                var subTypesString = typeName[(openGeneric + 1)..closeGeneric];
                if (!GenericsParser(subTypesString, out var subTypes))
                    return "";

                generics = subTypes.Aggregate(generics, (current, subType) => current + BuildPrettyIdentifier(subType));
                typeName = typeName[..openGeneric];
            } else if (!(openGeneric == -1 && closeGeneric == -1)) {
                return "";
            }

            return (isNullable ? "Nullable" : "") + FirstToUpper(typeName) + (isArray ? "Array" : "") + generics;
        }

        private static string FirstToUpper(string s)
        {
            var a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
    }
}