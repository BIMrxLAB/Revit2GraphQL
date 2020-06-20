using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using RevitGraphQLSchema.GraphQLModel;

namespace RevitGraphQLResolver.GraphQLModel
{
    public class QLMepSystemResolve        : QLMepSystem 
    {

        public QLMepSystemResolve(MEPSystem _mepSystem)
        {
            id = _mepSystem.Id.ToString();
            name = _mepSystem.Name;

            mepDomain = GetMepDomain(_mepSystem).ToString();
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
