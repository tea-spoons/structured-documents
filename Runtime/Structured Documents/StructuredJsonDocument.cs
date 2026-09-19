
#if NEWTONSOFT_JSON
namespace TeaSpoons.StructuredDocuments
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A <see cref="StructuredDocument"/> based on a json file.
    /// </summary>
    public class StructuredJsonDocument : StructuredDocument
    {
        private static class JsonBlockConverters
        {
            private static readonly Dictionary<Type, Func<JToken, DataBlock>> converters = new();

            static JsonBlockConverters()
            {
                converters.Add(typeof(TextBlock), token =>
                {
                    return new TextBlock(token.ToObject<string>());
                });

                converters.Add(typeof(TableBlock), token =>
                {
                    var table = new TableBlock();
                    if (token is JProperty property)
                    {
                        foreach (var rowToken in property.Value.Children())
                        {
                            var cells = new Dictionary<string, string>();
                            foreach (var cellToken in rowToken.Children())
                            {
                                if (cellToken is JProperty cellProperty)
                                {
                                    cells.Add(cellProperty.Name.ToLowerInvariant(), cellProperty.Value.ToObject<string>());
                                }
                            }
                            table.AddRow(cells);
                        }
                    }
                    return table;
                });
            }

            public static bool TryConvert<T>(JToken node, out T dataBlock)
                where T : DataBlock
            {
                if (converters.TryGetValue(typeof(T), out var converter))
                {
                    dataBlock = (T)converter(node);
                    return true;
                }

                dataBlock = null;
                return false;
            }
        }
        private JObject document;

        protected override void OnLoad(string source)
        {
            document = JObject.Parse(source);
        }

        protected override bool TryGetDataBlock<T>(string path, int index, out T block)
        {
            path = path.ToLowerInvariant();
            var node = document.Root;

            block = null;

            foreach (var pathItem in path.Split('/', StringSplitOptions.RemoveEmptyEntries))
            {
                var found = false;
                foreach (var child in node.Children())
                {
                    if (child is JProperty property && property.Name.ToLowerInvariant() == pathItem)
                    {
                        node = child;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    return false;
                }
            }

            return JsonBlockConverters.TryConvert(node, out block);
        }
    }
}
#endif
