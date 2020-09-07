using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using RevitGraphQLSchema.GraphQLModel;
using System.Text.Json;
using TraverseAllSystems;

namespace RevitGraphQLResolver.GraphQLModel
{
    public class QLMepSystemResolve        : QLMepSystem 
    {

        public QLMepSystemResolve(MEPSystem _mepSystem)
        {
            id = _mepSystem.Id.ToString();
            name = _mepSystem.Name;

            mepDomain = GetMepDomain(_mepSystem).ToString();


            //https://thebuildingcoder.typepad.com/blog/2016/06/traversing-and-exporting-all-mep-system-graphs.html
            // to travers mep system from the root
            FamilyInstance root = _mepSystem.BaseEquipment;

            if (root != null && (_mepSystem.GetType() == typeof(MechanicalSystem)))
            {

                // Traverse the system and dump the 
                // traversal graph into an XML file

                TraversalTree tree = new TraversalTree(_mepSystem);

                if (tree.Traverse())
                {
                    qlTammTreeNode = new QLTammTreeNodeResolve(JsonSerializer.Deserialize<JsonElement>(tree.DumpToJsonTopDown()));
                }
            }
        }

        /// <summary>
        /// The thee MEP disciplines
        /// </summary>
        public enum MepDomain
        {
            Invalid = -1,
            Mechanical = 0,
            Electrical = 1,
            Piping = 2,
            Count = 3
        }
        MepDomain GetMepDomain(MEPSystem s)
        {
            return (s is MechanicalSystem) ? MepDomain.Mechanical
              : ((s is ElectricalSystem) ? MepDomain.Electrical
                : ((s is PipingSystem) ? MepDomain.Piping
                  : MepDomain.Invalid));
        }

    }
}
