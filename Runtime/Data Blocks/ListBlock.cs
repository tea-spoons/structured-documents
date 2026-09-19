
namespace TeaSpoons.StructuredDocuments
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// A <see cref="DataBlock"/> representing a list of strings.
    /// </summary>
    public class ListBlock : DataBlock, IEnumerable<string>
    {
        public static readonly ListBlock Empty = new ListBlock(new(), EmptyKeyValueParser);

        public string this[int index] => items[index];
        public int Count => items.Count;

        private readonly List<string> items;
        private readonly DictionaryBlock.KeyValueParser dictionaryParser;
        public bool CanBeDictionary => dictionaryParser != null;

        internal ListBlock(List<string> items, DictionaryBlock.KeyValueParser dictionaryParser)
        {
            this.items = items;
            this.dictionaryParser = dictionaryParser;
        }

        /// <summary>
        /// Creates a new <see cref="DictionaryBlock"/> by parsing this list as a list of key-value pairs.
        /// </summary>
        public DictionaryBlock AsDictionary()
        {
            if (!CanBeDictionary) throw new System.InvalidOperationException("Cannot parse this ListBlock into a dictionary.");

            return new DictionaryBlock(this, dictionaryParser);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static bool EmptyKeyValueParser(string line, out string key, out string value)
        {
            key = null;
            value = null;
            return false;
        }
    }
}
