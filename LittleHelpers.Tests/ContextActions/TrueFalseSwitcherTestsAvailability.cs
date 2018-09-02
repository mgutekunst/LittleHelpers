using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using LittleHelpers.ContextActions;
using NUnit.Framework;

namespace LittleHelpers.Tests.ContextActions
{
    public class TrueFalseSwitcherTestsAvailability : CSharpContextActionAvailabilityTestBase<TrueFalseSwitcher>
    {
        protected override string ExtraPath {
            get
            {
                var x = nameof(TrueFalseSwitcherTests);
                return x;
            }
        }

        [Test]
        public void Availability()
        {
            this.DoNamedTest();
        }
    }
}