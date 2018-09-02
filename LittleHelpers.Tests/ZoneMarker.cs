using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.ReSharper.Feature.Services;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.TestFramework;
using JetBrains.TestFramework.Application.Zones;

namespace LittleHelpers.Tests
{
    [ZoneDefinition]
    public class LittleHelpersTestEnvironmentZone : ITestsZone, IRequire<PsiFeatureTestZone>
    {
        
    }

    [ZoneMarker]
    public class ZoneMarker : IRequire<ICodeEditingZone>, IRequire<ILanguageCSharpZone>, IRequire<LittleHelpersTestEnvironmentZone>
    {
    }
}
