
namespace TeaSpoons.StructuredDocuments.Editor.Tests
{
    using NUnit.Framework;

    public class HeadingPathTest
    {
        private HeadingPath headingPath;

        [SetUp]
        public void SetUp()
        {
            headingPath = new HeadingPath();
        }

        [Test]
        public void SetAndReplaceH1()
        {
            headingPath.UpdateHeader(0, "Foo");
            AssertHeadingPath("Foo");

            headingPath.UpdateHeader(0, "h1");
            AssertHeadingPath("h1");
        }

        [Test]
        public void SetH1AndH2()
        {
            headingPath.UpdateHeader(0, "h1");
            headingPath.UpdateHeader(1, "h2");
            AssertHeadingPath("h1", "h2");
        }

        [Test]
        public void ReturnToH1()
        {
            headingPath.UpdateHeader(0, "h1");
            headingPath.UpdateHeader(1, "h2");
            headingPath.UpdateHeader(0, "h1-new");
            AssertHeadingPath("h1-new");
        }

        [Test]
        public void SkipH2()
        {
            headingPath.UpdateHeader(0, "h1");
            headingPath.UpdateHeader(2, "h3");
            AssertHeadingPath("h1", "h3");
        }

        [Test]
        public void SkipH2AndReturnToH2()
        {
            headingPath.UpdateHeader(0, "h1");
            headingPath.UpdateHeader(2, "h3");
            headingPath.UpdateHeader(1, "h2");
            AssertHeadingPath("h1", "h2");
        }

        [Test]
        public void SkipReturnAndSkipAgain()
        {
            headingPath.UpdateHeader(0, "h1");
            headingPath.UpdateHeader(2, "h3");
            headingPath.UpdateHeader(1, "h2");
            headingPath.UpdateHeader(3, "h4");
            AssertHeadingPath("h1", "h2", "h4");
        }

        private void AssertHeadingPath(params string[] expected)
        {
            var expectedString = string.Join("/", expected);
            var actualString = headingPath.ToPath();

            Assert.AreEqual(expectedString, actualString);
        }
    }
}
