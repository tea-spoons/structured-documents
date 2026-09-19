#if !TEASPOONS_COLLECTIONS
namespace TeaSpoons.StructuredDocuments
{
    using System.Collections.Generic;

    public abstract partial class PreloadingStructuredDocument
    {
        /// <summary>
        /// A dictionary of lists, with just what this class needs.
        /// Stand-in for <c>ListDictionary</c> of the collections package (<c>System.Collections.Generic</c>),
        /// which <see cref="data"/> uses instead when that package is in the project.
        /// </summary>
        /// <remarks>
        /// Nested and protected so it is only visible to subclasses, like <see cref="data"/> itself.
        /// </remarks>
        protected sealed class ListDictionary<TKey, TValue>
        {
            private readonly Dictionary<TKey, List<TValue>> dictionary = new();

            /// <summary>
            /// The values of <paramref name="key"/>, or nothing if it has none.
            /// </summary>
            public IEnumerable<TValue> this[TKey key]
            {
                get
                {
                    return dictionary.TryGetValue(key, out var list) ? list : System.Linq.Enumerable.Empty<TValue>();
                }
            }

            public int GetValueCount(TKey key)
            {
                return dictionary.TryGetValue(key, out var list) ? list.Count : 0;
            }

            public bool Add(TKey key, TValue value)
            {
                if (!dictionary.TryGetValue(key, out var list))
                {
                    list = new List<TValue>();
                    dictionary.Add(key, list);
                }

                list.Add(value);
                return true;
            }

            public void Clear() => dictionary.Clear();
        }
    }
}
#endif
