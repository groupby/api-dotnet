using System.Collections.Generic;
using System.Text;
using GroupByInc.Api.Exceptions;
using GroupByInc.Api.Url;
using NUnit.Framework;

namespace GroupByInc.Api.Tests.Api.Url
{
    [TestFixture]
    public class UrlReplacementTest
    {
        private void TestToAndFromString(string replacementString)
        {
            UrlReplacement r = UrlReplacement.FromString(replacementString);
            string urlReplaceString = r.ToString();
            UrlReplacement r2 = UrlReplacement.FromString(urlReplaceString);
            Assert.AreEqual(r, r2);
            Assert.AreEqual(r2.ToString(), replacementString);
        }

        private void TestBadApply(string pInput, UrlReplacement urlReplacement)
        {
            StringBuilder stringBuilder = new StringBuilder(pInput);
            urlReplacement.Apply(stringBuilder, 0);
            Assert.AreEqual(pInput, stringBuilder.ToString());
        }

        private void TestApply(string pInput, string pExpected, UrlReplacement pUrlReplacement)
        {
            StringBuilder stringBuilder = new StringBuilder(pInput);
            pUrlReplacement.Apply(stringBuilder, 0);
            Assert.AreEqual(pExpected, stringBuilder.ToString());
        }

        private void TestParseQuery(params string[] pReplacementString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string replacement in pReplacementString)
            {
                if (sb.Length != 0)
                {
                    sb.Append('-');
                }
                sb.Append(replacement);
            }
            List<UrlReplacement> urlReplacements = UrlReplacement.ParseQueryString(sb.ToString());

            Assert.AreEqual(pReplacementString.Length, urlReplacements.Count);

            for (int i = 0; i < urlReplacements.Count; i++)
            {
                Assert.AreEqual(urlReplacements[i].ToString(), pReplacementString[pReplacementString.Length - (i + 1)]);
            }
        }

        [Test]
        public void TestEqualsAndHashCode()
        {
            UrlReplacement urlReplacement1 = new UrlReplacement(5, "3", UrlReplacement.OperationType.Insert);
            UrlReplacement urlReplacement2 = new UrlReplacement(5, "3", UrlReplacement.OperationType.Insert);
            UrlReplacement urlReplacement3 = new UrlReplacement(6, "3", UrlReplacement.OperationType.Insert);
            UrlReplacement urlReplacement4 = new UrlReplacement(6, "4", UrlReplacement.OperationType.Insert);
            UrlReplacement urlReplacement5 = new UrlReplacement(6, "4", UrlReplacement.OperationType.Swap);
            Assert.IsFalse(urlReplacement1.Equals(null));
            Assert.IsTrue(urlReplacement1.Equals(urlReplacement1));
            Assert.IsTrue(urlReplacement1.Equals(urlReplacement2));
            Assert.IsFalse(urlReplacement2.Equals(urlReplacement3));
            Assert.IsFalse(urlReplacement3.Equals(urlReplacement4));
            Assert.IsFalse(urlReplacement4.Equals(urlReplacement5));

            Assert.IsTrue(urlReplacement1.GetHashCode() == urlReplacement1.GetHashCode());
            Assert.IsTrue(urlReplacement1.GetHashCode() == urlReplacement2.GetHashCode());
            Assert.IsFalse(urlReplacement2.GetHashCode() == urlReplacement3.GetHashCode());
            Assert.IsFalse(urlReplacement3.GetHashCode() == urlReplacement4.GetHashCode());
            Assert.IsFalse(urlReplacement4.GetHashCode() == urlReplacement5.GetHashCode());
        }

        [Test]
        [ExpectedException(typeof(ParserException))]
        public void TestFromStringInvalidString()
        {
            UrlReplacement.FromString("a2-a");
        }

        [Test]
        public void TestParseQueryString()
        {
            TestParseQuery("2-a", "3-b", "4-c");
        }

        [Test]
        public void TestParseQueryStringWithDash()
        {
            TestParseQuery("2--", "i3-b", "4-c");
        }

        [Test]
        public void TestParseQueryStringWithDash2()
        {
            TestParseQuery("2--", "i3-b", "4--");
        }

        [Test]
        [ExpectedException(typeof(ParserException))]
        public void TestParseQueryStringWithDashMismatch()
        {
            UrlReplacement.ParseQueryString("2-a-i3-b--4-c");
        }

        [Test]
        public void TestParseQueryStringWithInserts()
        {
            TestParseQuery("2-a", "i3-b", "4-c");
        }

        [Test]
        public void TestSimpleApplyInsertAtEnd()
        {
            TestApply("abc12", "abc123", new UrlReplacement(5, "3", UrlReplacement.OperationType.Insert));
        }

        [Test]
        public void TestSimpleApplyInsertAtEndBadIndex()
        {
            TestBadApply("abc12", new UrlReplacement(6, "3", UrlReplacement.OperationType.Insert));
        }

        [Test]
        public void TestSimpleApplyInsertAtStart()
        {
            TestApply("bc123", "abc123", new UrlReplacement(0, "a", UrlReplacement.OperationType.Insert));
        }

        [Test]
        public void TestSimpleApplyInsertAtStartBadIndex()
        {
            TestBadApply("bc123", new UrlReplacement(-1, "a", UrlReplacement.OperationType.Insert));
        }

        [Test]
        public void TestSimpleApplyReplace()
        {
            TestApply("avc123", "abc123", new UrlReplacement(1, "b", UrlReplacement.OperationType.Swap));
        }

        [Test]
        public void TestSimpleApplyReplaceAtEnd()
        {
            TestApply("abc124", "abc123", new UrlReplacement(5, "3", UrlReplacement.OperationType.Swap));
        }

        [Test]
        public void TestSimpleApplyReplaceAtEndBadIndex()
        {
            TestBadApply("abc124", new UrlReplacement(6, "3", UrlReplacement.OperationType.Swap));
        }

        [Test]
        public void TestSimpleApplyReplaceAtStart()
        {
            TestApply("zbc123", "abc123", new UrlReplacement(0, "a", UrlReplacement.OperationType.Swap));
        }

        [Test]
        public void TestSimpleApplyReplaceAtStartBadIndex()
        {
            TestBadApply("zbc123", new UrlReplacement(-1, "a", UrlReplacement.OperationType.Swap));
        }

        [Test]
        public void TestToStringAndFromStringWithDash()
        {
            TestToAndFromString("35--");
        }

        [Test]
        public void TestToStringAndFromStringWithInsert()
        {
            TestToAndFromString("i2-/");
        }

        [Test]
        public void TestToStringAndFromStringWithSwap()
        {
            TestToAndFromString("35-9");
        }

        [Test]
        public void TestToString()
        {
            UrlReplacement r = new UrlReplacement(2, "a", UrlReplacement.OperationType.Swap);
            Assert.AreEqual("2-a", r.ToString());
            r = new UrlReplacement(20, "%", UrlReplacement.OperationType.Insert);
            Assert.AreEqual("i20-%", r.ToString());
        }
    }
}