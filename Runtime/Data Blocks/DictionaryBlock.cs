
namespace TeaSpoons.StructuredDocuments
{
    using System.Collections.Generic;

    /// <summary>
    /// A <see cref="DataBlock"/> containing a list of key-value pairs.
    /// </summary>
    public class DictionaryBlock : DataBlock
    {
        internal delegate bool KeyValueParser(string line, out string key, out string value);

        public static readonly DictionaryBlock Empty = new DictionaryBlock(null, null);

        public string this[string key] => properties[key.ToLowerInvariant()];

        private readonly Dictionary<string, string> properties = new();

        internal DictionaryBlock(ListBlock source, KeyValueParser parser)
        {
            if (source == null || source.Count == 0)
            {
                return;
            }

            if (parser == null)
            {
                throw new System.ArgumentNullException(nameof(parser));
            }

            foreach (var item in source)
            {
                if (parser(item, out var key, out var value))
                {
                    properties[key] = value;
                }
            }
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
            return properties.TryGetValue(key.ToLowerInvariant(), out value);
        }
    }
}
