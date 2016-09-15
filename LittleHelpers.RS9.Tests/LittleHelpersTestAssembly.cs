using JetBrains.TestFramework;
using Littlehelpers.RS9.Tests;
using NUnit.Framework;

#pragma warning disable 618
[assembly: TestDataPathBase(@".\data")]
#pragma warning restore 618
// ReSharper disable once CheckNamespace

[SetUpFixture]
class LittleHelpersTestAssembly : ExtensionTestEnvironmentAssembly<LittleHelpersTestEnvironmentZone>
{
}
