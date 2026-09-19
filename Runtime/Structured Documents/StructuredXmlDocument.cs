
namespace TeaSpoons.StructuredDocuments
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    /// <summary>
    /// A <see cref="StructuredDocument"/> based on an xml file.
    /// </summary>
    public class StructuredXmlDocument : StructuredDocument
    {
        private static class XmlBlockConverters
        {
            private static readonly Dictionary<Type, Func<XmlNode, DataBlock>> converters = new();

            static XmlBlockConverters()
            {
                converters.Add(typeof(TextBlock), node =>
                {
                    return new TextBlock(node.InnerText);
                });

                converters.Add(typeof(TableBlock), node =>
                {
                    var table = new TableBlock();
                    foreach (XmlNode rowNode in node.ChildNodes)
                    {
                        var cells = new Dictionary<string, string>();
                        foreach (XmlNode cellNode in rowNode.ChildNodes)
                        {
                            cells.Add(cellNode.Name.ToLowerInvariant(), cellNode.InnerText);
                        }
                        table.AddRow(cells);
                    }
                    return table;
                });
            }

            public static bool TryConvert<T>(XmlNode node, out T dataBlock)
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
        private XmlDocument document;

        protected override void OnLoad(string source)
        {
            document = new XmlDocument();
            document.LoadXml(source);
        }

        protected override bool TryGetDataBlock<T>(string path, int index, out T block)
        {
            path = path.ToLowerInvariant();
            XmlNode node = document.DocumentElement;

            block = null;

            foreach (var pathItem in path.Split('/', StringSplitOptions.RemoveEmptyEntries))
            {
                var found = false;
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (child.Name.ToLowerInvariant() == pathItem)
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

            return XmlBlockConverters.TryConvert(node, out block);
        }
    }
}
