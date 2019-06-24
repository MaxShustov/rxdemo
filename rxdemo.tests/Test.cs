using NUnit.Framework;
using Microsoft.Reactive.Testing;
using ReactiveUI.Testing;
using rxdemo.core;

namespace rxdemo.tests
{
    [TestFixture()]
    public class Test
    {
        [Test]
        public void TestCase() => new TestScheduler().With(scheduler =>
        {
            var myViewModel = new MyViewModel();

            scheduler.AdvanceBy(2);

            myViewModel.FirstName = "Maksym";
            myViewModel.LastName = "Shustov";

            scheduler.AdvanceBy(2);

            Assert.AreEqual(myViewModel.FullName, "Maksym Shustov");
        });
    }
}
