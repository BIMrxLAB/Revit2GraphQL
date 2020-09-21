using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitWebServer;
using System;
using System.Windows;

namespace RevitGraphQLCommand
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class Ui : Window
    {
        private readonly UIDocument _uiDoc;
        private readonly Document _doc;
        public WebServer aWebServer { get; set; }

        RevitTask aRevitTask;
        public Ui(UIApplication uiApp, RevitTask _aRevitTask)
        {
            _uiDoc = uiApp.ActiveUIDocument;
            _doc = _uiDoc.Document;

            Closed += MainWindow_Closed;

            InitializeComponent();
            aRevitTask = _aRevitTask;

        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Close();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            aWebServer = new WebServer("localhost", txtPort.Text, _doc, _uiDoc, aRevitTask);
            aWebServer.Start();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            aWebServer.Stop();
        }

    }
}
