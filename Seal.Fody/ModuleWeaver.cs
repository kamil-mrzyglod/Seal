using System.Linq;
using Mono.Cecil;

namespace Seal.Fody
{
    public class ModuleWeaver
    {
        public ModuleDefinition ModuleDefinition { get; set; }

        public void Execute()
        {
            // We want to mark as 'sealed' only those classes, which
            // are not sealed already, are not abstract and don't
            // have derived classes
            foreach (
                var type in
                    ModuleDefinition.Types.Where(type => type.IsAbstract == false && type.IsSealed == false)
                        .Where(type => ModuleDefinition.Types.All(derived => derived.BaseType != type)))
            {
                type.IsSealed = true;
            }
        }
    }
}