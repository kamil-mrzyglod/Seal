using System.Linq;
using Mono.Cecil;

namespace Seal.Fody
{
    using System.Collections.Generic;

    public class ModuleWeaver
    {
        public ModuleDefinition ModuleDefinition { get; set; }

        public void Execute()
        {
            // We want to mark as 'sealed' only those classes, which
            // are not sealed already, are not abstract and don't
            // have derived classes
            foreach (var type in FilterProperTypes(ModuleDefinition.Types))
            {
                type.IsSealed = true;

                foreach (var nestedType in FilterProperTypes(type.NestedTypes))
                {
                    nestedType.IsSealed = true;
                }
            }
        }

        private IEnumerable<TypeDefinition> FilterProperTypes(IEnumerable<TypeDefinition> types)
        {
            return
                types.Where(type => type.IsAbstract == false && 
                            type.IsSealed == false && 
                            type.CustomAttributes.Any(a => a.AttributeType.ToString() == typeof(LeaveUnsealedAttribute).ToString()) == false)
                    .Where(type => ModuleDefinition.Types.All(derived => derived.BaseType != type));
        } 
    }
}