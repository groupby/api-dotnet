using NUnit.Framework;

namespace GroupByInc.Api.Tests.Api
{
    [TestFixture]
    public class AbstractQueryTest
    {
        private BaseQuery _query;

        [SetUp]
        public void Setup()
        {
            _query = new BaseQuery();
        }

        [Test]
        public void SplitTestRange()
        {
            string[] split = _query.SplitRefinements("test=bob~price:10..20");
            Assert.AreEqual(new[] { "test=bob", "price:10..20" }, split);
        }

        [Test]
        public void SplitTestNoCategory()
        {
            string[] split = _query.SplitRefinements("~gender=Women~simpleColorDesc=Pink~product=Clothing");
            Assert.AreEqual(new[] { "gender=Women", "simpleColorDesc=Pink", "product=Clothing" }, split);
        }

        [Test]
        public void SplitTestCategory()
        {
            string[] split = _query.SplitRefinements("~category_leaf_expanded=Category Root~Athletics~Men's~Sneakers");
            Assert.AreEqual(new[] { "category_leaf_expanded=Category Root~Athletics~Men's~Sneakers" }, split);
        }

        [Test]
        public void SplitTestMultipleCategory()
        {
            string[] split = _query.SplitRefinements("~category_leaf_expanded=Category Root~Athletics~Men's~Sneakers~category_leaf_id=580003");
            Assert.AreEqual(new[] {"category_leaf_expanded=Category Root~Athletics~Men's~Sneakers", "category_leaf_id=580003"}, split);            
        }

        [Test]
        public void SplitTestCategoryLong()
        {
            const string reallyLongString = "~category_leaf_expanded=Category Root~Athletics~Men's~Sneakers~category_leaf_id=580003~" +
                                            "color=BLUE~color=YELLOW~color=GREY~feature=Lace Up~feature=Light Weight~brand=Nike";

            string[] split = _query.SplitRefinements(reallyLongString);
            Assert.AreEqual(new[]{"category_leaf_expanded=Category Root~Athletics~Men's~Sneakers", "category_leaf_id=580003",
                        "color=BLUE", "color=YELLOW", "color=GREY", "feature=Lace Up", "feature=Light Weight",
                        "brand=Nike"
                },
                split);
        }

        [Test]
        public void TestNull()
        {
            string[] split = _query.SplitRefinements(null);
            Assert.AreEqual(new string []{}, split);
        }

        [Test]
        public void TestEmpty()
        {
            string[] split = _query.SplitRefinements("");
            Assert.AreEqual(new string[] { }, split);
        }

        [Test]
        public void TestUtf8()
        {
            string[] split = _query.SplitRefinements("tëst=bäb~price:10..20");
            Assert.AreEqual(new[] { "tëst=bäb", "price:10..20" }, split);
        }

    }
}
