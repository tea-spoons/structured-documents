
namespace TeaSpoons.StructuredDocuments.Editor.Tests
{
    using NUnit.Framework;
    using System.Collections.Generic;

    public class StructuredDocumentTest
    {
        private class TestDocument : PreloadingStructuredDocument
        {
            protected override void OnLoad(string source)
            {
                data.Add("heading1", new TextBlock("a"));
                data.Add("heading1", new TextBlock("b"));
                data.Add("heading1", new ListBlock(new List<string> { "first" }, null));
                data.Add("heading1", new ListBlock(new List<string> { "second" }, null));
                data.Add("heading1", new TextBlock("c"));
                data.Add("heading2", new TextBlock("aa"));
                data.Add("heading1", new TextBlock("d"));
            }
        }

        private StructuredDocument document;

        [SetUp]
        public void SetUp()
        {
            document = new TestDocument();
            document.Load(string.Empty);
        }

        [Test]
        public void CorrectIndices()
        {
            Assert.AreEqual("a", document.GetText("heading1"));
            Assert.AreEqual("a", document.GetText("heading1", 0));
            Assert.AreEqual("b", document.GetText("heading1", 1));
            Assert.AreEqual("c", document.GetText("heading1", 2));
            Assert.AreEqual("d", document.GetText("heading1", 3));

            Assert.AreEqual("first", document.GetList("heading1")[0]);
            Assert.AreEqual("second", document.GetList("heading1", 1)[0]);

            Assert.AreEqual("aa", document.GetText("heading2"));
        }
    }
}
