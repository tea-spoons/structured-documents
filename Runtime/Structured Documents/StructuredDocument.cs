
namespace TeaSpoons.StructuredDocuments
{
    /// <summary>
    /// Base class for "structured documents", which consist of a set of <see cref="DataBlock"/>s mapped to <see cref="DataPath"/>s.
    /// Use any of the available <c>Get</c> methods to read the data regardless of source format.
    /// </summary>
    public abstract class StructuredDocument
    {
        public void Load(string source)
        {
            Clear();
            OnLoad(source);
        }

        protected abstract void OnLoad(string source);

        public virtual void Clear()
        {
        }

        /// <summary>
        /// Returns the text at the given <paramref name="path"/>.
        /// </summary>
        /// <param name="defaultValue">This value will be returned if there was no text at the given <paramref name="path"/>.</param>
        public string GetText(string path, int index = 0, string defaultValue = "")
        {
            if (TryGetDataBlock<TextBlock>(path, index, out var block))
            {
                return block.Text;
            }
            return defaultValue ?? string.Empty;
        }

        /// <summary>
        /// Returns the <see cref="ListBlock"/> found at the given <paramref name="path"/>, or an empty list if there is none.
        /// </summary>
        public ListBlock GetList(string path, int index = 0)
        {
            if (TryGetDataBlock<ListBlock>(path, index, out var result))
            {
                return result;
            }
            return ListBlock.Empty;
        }

        /// <summary>
        /// Returns the <see cref="TableBlock"/> found at the given <paramref name="path"/>, or an empty table if there is none.
        /// </summary>
        public TableBlock GetTable(string path, int index = 0)
        {
            if (TryGetDataBlock<TableBlock>(path, index, out var result))
            {
                return result;
            }

            return TableBlock.Empty;
        }

        protected abstract bool TryGetDataBlock<T>(string path, int index, out T block)
            where T : DataBlock;
    }
}
