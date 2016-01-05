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

        private string _weavedAssemblyName;

        [SetUp]
        public void SetUp()
        {
            _weavedAssemblyName = AssemblyDirectory + $"Seal.TestAssembly{DateTime.Now.Ticks}.dll";

            var md = ModuleDefinition.ReadModule(Path.GetFullPath(AssemblyDirectory + AssemblyPath));
            var weaver = new ModuleWeaver { ModuleDefinition = md };

            weaver.Execute();
            md.Write(_weavedAssemblyName);
        }

        [Test]
        public void GivenNonSealedClass_ShouldSealIt()
        {
            var assembly = Assembly.LoadFile(_weavedAssemblyName);
            var type = assembly.GetExportedTypes().First(x => x.Name == "NonSealedClass");

            Assert.That(type.IsSealed, Is.True);
        }

        [Test]
        public void GivenAbstractClass_ShouldLeaveItAsItIs()
        {
            var assembly = Assembly.LoadFile(_weavedAssemblyName);
            var type = assembly.GetExportedTypes().First(x => x.Name == "AbstractClass");

            Assert.That(type.IsSealed, Is.False);
        }

        private static string AssemblyDirectory
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