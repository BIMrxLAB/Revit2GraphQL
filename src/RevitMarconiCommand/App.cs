using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using RevitMarconiCommand.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace RevitMarconiCommand
{
    class App : IExternalApplication
    {
        // class instance
        public static App ThisApp;

        // ModelessForm instance
        private Ui _mMyForm;

        // Separate thread to run Ui on
        private Thread _uiThread;

        private MsalAuthHelper _msalAuthHelper;
        public MsalAuthHelper msalAuthHelper
        {
            get
            {
                if (_msalAuthHelper == null) _msalAuthHelper = new MsalAuthHelper();
                return _msalAuthHelper;
            }
        }

        public Result OnStartup(UIControlledApplication a)
        {
            _mMyForm = null; // no dialog needed yet; the command will bring it
            ThisApp = this; // static access to this application instance

            // Method to add Tab and Panel 
            RibbonPanel panel = RibbonPanel(a);
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            // BUTTON FOR THE MULTI-THREADED WPF OPTION
            if (panel.AddItem(
                new PushButtonData("BIMrx.Marconi", "BIMrx.Marconi", thisAssemblyPath,
                    "RevitMarconiCommand.EntryCommandSeparateThread")) is PushButton button2)
            {
                button2.ToolTip = "Visual interface to start/stop BIMrx.Marconi.";
                Uri uriImage = new Uri("pack://application:,,,/RevitMarconiCommand;component/Resources/phone.png");
                BitmapImage largeImage = new BitmapImage(uriImage);
                button2.LargeImage = largeImage;
            }



            // listeners/watchers for external events (if you choose to use them)
            a.ApplicationClosing += a_ApplicationClosing; //Set Application to Idling
            a.Idling += a_Idling;

            AppDomain currentDomain = AppDomain.CurrentDomain;

            currentDomain.AssemblyResolve += new ResolveEventHandler(MyResolveEventHandler);


            return Result.Succeeded;
        }

        private Assembly MyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            if (args.Name.Contains("System.Buffers"))
            {
                string assemblyFileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\System.Buffers.dll";
                return Assembly.LoadFrom(assemblyFileName);
            }
            else if (args.Name.Contains("System.Runtime.CompilerServices.Unsafe"))
            {
                string assemblyFileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\System.Runtime.CompilerServices.Unsafe.dll";
                return Assembly.LoadFrom(assemblyFileName);
            }
            else if (args.Name.Contains("System.Threading.Tasks.Extensions"))
            {
                string assemblyFileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\System.Threading.Tasks.Extensions.dll";
                return Assembly.LoadFrom(assemblyFileName);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// What to do when the application is shut down.
        /// </summary>
        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }

        /// <summary>
        /// This is the method which launches the WPF window in a separate thread, and injects any methods that are
        /// wrapped by ExternalEventHandlers. This can be done in a number of different ways, and
        /// implementation will differ based on how the WPF is set up.
        /// </summary>
        /// <param name="uiapp">The Revit UIApplication within the add-in will operate.</param>
        public void ShowFormSeparateThread(UIApplication uiapp)
        {
            // If we do not have a thread started or has been terminated start a new one
            if (!(_uiThread is null) && _uiThread.IsAlive) return;

            //https://thebuildingcoder.typepad.com/blog/2020/02/external-communication-and-async-await-event-wrapper.html#2
            //https://github.com/WhiteSharq/RevitTask

            RevitTask aRevitTask = new RevitTask();

            _uiThread = new Thread(() =>
            {
                SynchronizationContext.SetSynchronizationContext(
                    new DispatcherSynchronizationContext(
                        Dispatcher.CurrentDispatcher));
                // The dialog becomes the owner responsible for disposing the objects given to it.
                _mMyForm = new Ui(uiapp, aRevitTask, msalAuthHelper);
                _mMyForm.Closed += (s, e) => Dispatcher.CurrentDispatcher.InvokeShutdown();
                _mMyForm.Show();
                Dispatcher.Run();
            });

            _uiThread.SetApartmentState(ApartmentState.STA);
            _uiThread.IsBackground = true;
            _uiThread.Start();
        }

        #region Idling & Closing

        /// <summary>
        /// What to do when the application is idling. (Ideally nothing)
        /// </summary>
        void a_Idling(object sender, IdlingEventArgs e)
        {
        }

        /// <summary>
        /// What to do when the application is closing.)
        /// </summary>
        void a_ApplicationClosing(object sender, ApplicationClosingEventArgs e)
        {
        }

        #endregion

        #region Ribbon Panel

        public RibbonPanel RibbonPanel(UIControlledApplication a)
        {

            string tab = "GraphQL"; // Tab name

            // Empty ribbon panel 
            RibbonPanel ribbonPanel = null;
            // Try to create ribbon tab. 
            try
            {
                a.CreateRibbonTab(tab);
            }
            catch (Exception ex)
            {
                Util.HandleError(ex);
            }

            // Try to create ribbon panel.
            try
            {
                RibbonPanel panel = a.CreateRibbonPanel(tab, "Remote");
            }
            catch (Exception ex)
            {
                Util.HandleError(ex);
            }

            // Search existing tab for your panel.
            List<RibbonPanel> panels = a.GetRibbonPanels(tab);
            foreach (RibbonPanel p in panels.Where(p => p.Name == "Remote"))
            {
                ribbonPanel = p;
            }

            //return panel 
            return ribbonPanel;
        }

        #endregion
    }
}
