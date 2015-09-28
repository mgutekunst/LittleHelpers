
using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.ReSharper.TestFramework;
using JetBrains.TestFramework;
using JetBrains.TestFramework.Application.Zones;
using NUnit.Framework;

namespace LittleHelpers.RS9.Tests.ContextActions
{

    [ZoneDefinition]
    public interface IMyTestZone : ITestsZone, IRequire<PsiFeatureTestZone>
    {
    }

    [SetUpFixture]
    public class TrueFalseSwitcherTests : ExtensionTestEnvironmentAssembly<IMyTestZone>
    {
        
    }
}