
namespace TeaSpoons.StructuredDocuments
{
    /// <summary>
    /// A simple <see cref="DataBlock"/> containing a string.
    /// </summary>
    public class TextBlock : DataBlock
    {
        public readonly string Text;

        internal TextBlock(string text)
        {
            Text = text;
        }
    }
}
