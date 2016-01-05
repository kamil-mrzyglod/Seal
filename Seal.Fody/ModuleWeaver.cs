using System.Linq;
using Mono.Cecil;

namespace Seal.Fody
{
    public class ModuleWeaver
    {
        public ModuleDefinition ModuleDefinition { get; set; }

        public void Execute()
        {
            foreach (var type in ModuleDefinition.Types.Where(type => type.IsAbstract == false && type.IsSealed == false))
            {
                type.IsSealed = true;
            }
        }
    }
}