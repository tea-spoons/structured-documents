
namespace TeaSpoons.StructuredDocuments
{
    using System.Collections.Generic;

    /// <summary>
    /// A <see cref="StructuredDocument"/> that preloads its blocks during loading rather than parsing them on the fly.
    /// </summary>
    public abstract class PreloadingStructuredDocument : StructuredDocument
    {
        protected readonly ListDictionary<string, DataBlock> data = new();

        public override void Clear()
        {
            base.Clear();

            data.Clear();
        }

        protected override bool TryGetDataBlock<T>(string path, int index, out T block)
        {
            if (index < 0) throw new System.ArgumentOutOfRangeException(nameof(index));

            foreach (var item in data[path])
            {
                block = item as T;
                if (block != null)
                {
                    if (index == 0)
                    {
                        return true;
                    }
                    index--;
                }
            }

            block = null;
            return false;
        }
    }
}
