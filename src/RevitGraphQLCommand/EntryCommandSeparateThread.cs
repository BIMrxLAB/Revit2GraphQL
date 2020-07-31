using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitWebServer;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RevitGraphQLCommand
{

    /// <summary>
    /// This is the ExternalCommand which gets executed from the ExternalApplication. In a WPF context,
    /// this can be lean, as it just needs to show the WPF. Without a UI, this could contain the main
    /// order of operations for executing the business logic.
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class EntryCommandSeparateThread : IExternalCommand
    {
        public virtual Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += new ResolveEventHandler(MyResolveEventHandler);

            try
            {
                App.ThisApp.ShowFormSeparateThread(commandData.Application);
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }


        }


        // thank you Ken
        // https://forums.autodesk.com/t5/navisworks-api/could-not-load-file-or-assembly-newtonsoft-json/m-p/7460535#M3467
        private Assembly MyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            if (args.Name.Contains("Microsoft.Owin"))
            {
                string assemblyFileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Microsoft.Owin.dll";
                return Assembly.LoadFrom(assemblyFileName);
            }
            else
            {
                return null;
            }
        }

    }
}
