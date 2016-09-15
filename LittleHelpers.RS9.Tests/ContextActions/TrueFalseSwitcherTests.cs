using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using LittleHelpers.RS9.ContextActions;
using NUnit.Framework;

namespace Littlehelpers.RS9.Tests.ContextActions
{
    public class TrueFalseSwitcherTests : CSharpContextActionAvailabilityTestBase<TrueFalseSwitcher>
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