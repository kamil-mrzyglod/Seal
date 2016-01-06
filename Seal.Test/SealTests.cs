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

        private Assembly _assembly;

        [SetUp]
        public void SetUp()
        {
            _weavedAssemblyName = AssemblyDirectory + $"Seal.TestAssembly{DateTime.Now.Ticks}.dll";

            var md = ModuleDefinition.ReadModule(Path.GetFullPath(AssemblyDirectory + AssemblyPath));
            var weaver = new ModuleWeaver { ModuleDefinition = md };

            weaver.Execute();
            md.Write(_weavedAssemblyName);

            _assembly = Assembly.LoadFile(_weavedAssemblyName);
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

        [Test]
        public void GivenNonSealedClass_ShouldSealIt()
        {
            var type = _assembly.GetExportedTypes().First(x => x.Name == "NonSealedClass");

            Assert.That(type.IsSealed, Is.True);
        }

        [Test]
        public void GivenAbstractClass_ShouldLeaveItAsItIs()
        {
            var type = _assembly.GetExportedTypes().First(x => x.Name == "AbstractClass");

            Assert.That(type.IsSealed, Is.False);
        }

        [Test]
        public void GivenBaseAndDerivedClass_ShouldMarkDerivedAsSealed()
        {
            var baseType = _assembly.GetExportedTypes().First(x => x.Name == "BaseClass");
            var derivedType = _assembly.GetExportedTypes().First(x => x.Name == "DerivedClass");

            Assert.That(baseType.IsSealed, Is.False);
            Assert.That(derivedType.IsSealed, Is.True);
        }

        [Test]
        public void GivenClassDerivingFromAbstractClass_ShouldMarkDerivedAsSealed()
        {
            var baseType = _assembly.GetExportedTypes().First(x => x.Name == "AbstractClass");
            var derivedType = _assembly.GetExportedTypes().First(x => x.Name == "DerivedFromAbstractClass");

            Assert.That(baseType.IsSealed, Is.False);
            Assert.That(derivedType.IsSealed, Is.True);
        }

        [Test]
        public void GivenNestedClass_ShouldMarkIsAsSealed()
        {
            var top = _assembly.GetExportedTypes().First(x => x.Name == "TopNestedClass");
            var nested = _assembly.GetExportedTypes().First(x => x.Name == "NestedClass");

            Assert.That(top.IsSealed, Is.True);
            Assert.That(nested.IsSealed, Is.True);
        }
    }
}