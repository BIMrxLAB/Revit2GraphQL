using Autodesk.Revit.DB;
using GraphQL.Language.AST;
using System;
using System.Collections.Generic;
using System.Text;

namespace RevitGraphQLResolver.GraphQLModel
{
    public class QLFamilySymbol
    {
        public string id { get; set; }
        public string name { get; set; }

        public QLFamilySymbol()
        {

        }
        public QLFamilySymbol(FamilySymbol _familySymbol)
        {
            id = _familySymbol.Id.ToString();
            name = _familySymbol.Name;
        }
    }
}
