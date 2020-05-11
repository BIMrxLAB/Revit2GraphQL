using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitWebServer;

namespace RevitCommand
{
    [Transaction(TransactionMode.Manual)]

    public class Entry : IExternalCommand
    {
        public static Document doc { get; set; }
        public Result Execute(ExternalCommandData revit, ref string message, ElementSet elements)
        {

            doc = revit.Application.ActiveUIDocument.Document;

            WebServer aWebServer = new WebServer("localhost", "9000", doc);
            aWebServer.Start();

            return Result.Succeeded;

        }
    }
}
