using JetBrains.TestFramework;
using NUnit.Framework;

#pragma warning disable 618
[assembly: TestDataPathBase(@".\data")]
#pragma warning restore 618
// ReSharper disable once CheckNamespace

namespace LittleHelpers.Tests
{
    [SetUpFixture]
    class LittleHelpersTestAssembly : ExtensionTestEnvironmentAssembly<LittleHelpersTestEnvironmentZone>
    {
    }
}
