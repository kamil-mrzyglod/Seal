using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using NUnit.Framework;
using Seal.Fody;

namespace Seal.Test
{
    public class SealTests
    {
        private const string AssemblyPath = @"../../../../Seal.TestAssembly/bin/Debug/Seal.TestAssembly.dll";

        [SetUp]
        public void SetUp()
        {
            var md = ModuleDefinition.ReadModule(Path.GetFullPath(AssemblyDirectory + AssemblyPath));
            var weaver = new ModuleWeaver { ModuleDefinition = md };

            weaver.Execute();
            md.Write(AssemblyDirectory + "Seal.TestAssembly2.dll");
        }

        [Test]
        public void GivenNonSealedClass_ShouldSealIt()
        {
            var assembly = Assembly.LoadFile(AssemblyDirectory + "Seal.TestAssembly2.dll");
            var type = assembly.GetExportedTypes().First(x => x.Name == "NonSealedClass");

            Assert.That(type.IsSealed, Is.True);
        }

        public static string AssemblyDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}