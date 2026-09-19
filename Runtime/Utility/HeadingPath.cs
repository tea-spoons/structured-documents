
namespace TeaSpoons.StructuredDocuments
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Represents a dynamic path within a structure of named and levelled headings.
    /// Can be used during traversal of a document that has headings of different sizes or nodes within nodes.
    /// </summary>
    /// <example>
    /// # Foo -> Foo
    /// ## Bar -> Foo/Bar
    /// ### Baz -> Foo/Bar/Baz
    /// ## Foobar -> Foo/Foobar
    /// </example>
    internal class HeadingPath
    {
        private static readonly StringBuilder pathBuilder = new();

        private readonly List<string> headings = new();
        public int Count { get; private set; }

        public void UpdateHeader(byte level, string name)
        {
            if (level == 0)
            {
                headings.Clear();
            }
            else
            {
                SetLength(level);
            }

            headings.Add(name);

            UpdateCount();
        }

        public void Clear()
        {
            headings.Clear();
            Count = 0;
        }

        public string ToPath()
        {
            pathBuilder.Clear();

            foreach (var heading in headings)
            {
                if (string.IsNullOrEmpty(heading))
                {
                    continue;
                }

                if (pathBuilder.Length > 0)
                {
                    pathBuilder.Append('/');
                }
                pathBuilder.Append(heading);
            }

            return pathBuilder.ToString();
        }

        private void SetLength(byte maxLevel)
        {
            if (maxLevel < headings.Count)
            {
                headings.RemoveRange(maxLevel, headings.Count - maxLevel);
            }
            else if (maxLevel > headings.Count)
            {
                for (var i = headings.Count; i < maxLevel; i++)
                {
                    headings.Add(null);
                }
            }
        }

        private void UpdateCount()
        {
            Count = 0;

            foreach (var header in headings)
            {
                if (header != null)
                {
                    Count++;
                }
            }
        }
    }
}
