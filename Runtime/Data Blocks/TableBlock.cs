
namespace TeaSpoons.StructuredDocuments
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// A <see cref="DataBlock"/> representing a table - a list of rows with key-value pairs with equal keys.
    /// </summary>
    public class TableBlock : DataBlock, IEnumerable<TableBlock.Row>
    {
        public class Row
        {
            private readonly Dictionary<string, string> cells;

            public string this[string key] => cells[key.ToLowerInvariant()];

            internal Row(List<string> cellsContents, List<string> columnKeys)
            {
                cells = new();
                for (var index = 0; index < cellsContents.Count; index++)
                {
                    cells.Add(columnKeys[index], cellsContents[index]);
                }
            }

            internal Row(Dictionary<string, string> cells)
            {
                this.cells = cells;
            }

            /// <summary>
            /// Returns the value assigned to <paramref name="key"/>, or <c>null</c> if <paramref name="key"/> doesn't exist.
            /// </summary>
            public string GetOptional(string key)
            {
                if (TryGetValue(key, out var value))
                {
                    return value;
                }
                return null;
            }

            public bool TryGetValue(string key, out string value)
            {
                return cells.TryGetValue(key.ToLowerInvariant(), out value);
            }
        }

        public static readonly TableBlock Empty = new TableBlock();

        private List<string> columnKeys;
        private readonly List<Row> rows = new();

        internal TableBlock()
        {
        }

        internal void AddRow(List<string> cells)
        {
            var row = new Row(cells, columnKeys);
            rows.Add(row);
        }

        internal void AddRow(Dictionary<string, string> cells)
        {
            var row = new Row(cells);
            rows.Add(row);
        }

        internal void SetColumnKeys(List<string> columnKeys)
        {
            this.columnKeys = columnKeys;
        }

        public IEnumerator<Row> GetEnumerator()
        {
            return rows.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
