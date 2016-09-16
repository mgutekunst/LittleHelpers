using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using LittleHelpers.ContextActions;
using NUnit.Framework;

namespace LittleHelpers.Tests.ContextActions
{
    public class TrueFalseSwitcherTests : CSharpContextActionExecuteTestBase<TrueFalseSwitcher>
    {
        protected override string ExtraPath
        {
            get
            {
                var x = this.GetType().Name;
                return x;
            }
        }


        [Test]
        public void TrueFalseWorksOnTrue()
        {
            this.DoNamedTest();
        }

        [Test]
        public void TrueFalseWorksOnFalse()
        {
            this.DoNamedTest();
        }

    }
}