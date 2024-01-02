#region Namespaces
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Media.Imaging;
using AdvansysRevitAssembly.Commands;
using AdvansysRevitAssembly.Properties;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Windows;
#endregion

namespace AdvansysRevitAssembly
{
    class App : IExternalApplication
    {

        protected string ManagerPanelName => "Fabrication Manager";
        private static DockablePaneId FabricationManagerPaneId { get; } = new DockablePaneId(new Guid("DB7FB22A-A5E5-4344-8009-048CCFEE679A"));

        public Result OnStartup(UIControlledApplication a)
        {
            //a.CreateRibbonTab("Advansys");
            //var panel = a.CreateRibbonPanel("Advansys", "Conveyors");

            //var panelControls = a.CreateRibbonPanel("Advansys", "Controls");

            //var panelTools = a.CreateRibbonPanel("Advansys", "Tools");
            //var panelSettings = a.CreateRibbonPanel("Advansys", "Settings");


            string tabName = "Advansys";
            try
            {
                // Create a custom tab
                a.CreateRibbonTab(tabName);

                // Add panels to the custom tab
                AddRibbonPanelNew(a, tabName, "Conveyors");
                AddRibbonPanelNew(a, tabName, "Supports");
                AddRibbonPanelNew(a, tabName, "Pallet Conveyors");
                AddRibbonPanelNew(a, tabName, "Resale");
                AddRibbonPanelNew(a, tabName, "Structural");
                AddRibbonPanelNew(a, tabName, "Bill Of Material");
                AddRibbonPanelNew(a, tabName, "Control Parts");
                AddRibbonPanelNew(a, tabName, "Manager");
                a.SelectionChanged += Elements_SelectionChanged;
                CurrentApplication = a;
                FabricationManagerView FabricationManagerView = new FabricationManagerView();
                a.RegisterDockablePane(FabricationManagerPaneId, ManagerPanelName, FabricationManagerView);

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                Autodesk.Revit.UI.TaskDialog.Show("Error", ex.Message);
                return Result.Failed;
            }

            a.ControlledApplication.DocumentOpened += ControlledApplication_DocumentOpened;
            a.ViewActivated += A_ViewActivated;

            return Result.Succeeded;
        }

        private void Elements_SelectionChanged(object sender, Autodesk.Revit.UI.Events.SelectionChangedEventArgs e)
        {
            var selected  =  e.GetSelectedElements();
            var doc = e.GetDocument();
        }


        private static RibbonTab ModifyTab;


        /// <summary>
        /// Public method for get Fabrication Manager Pane.
        /// </summary>
        public static DockablePane GetFabricationManagerPane()
        {
            try
            {
                return CurrentApplication.GetDockablePane(FabricationManagerPaneId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Current Application.
        /// </summary>
        private static UIControlledApplication CurrentApplication { get; set; }

        private void A_ViewActivated(object sender, Autodesk.Revit.UI.Events.ViewActivatedEventArgs e)
        {
            Globals.Doc = e.Document;
        }

        private void ControlledApplication_DocumentOpened(object sender, Autodesk.Revit.DB.Events.DocumentOpenedEventArgs e)
        {
            Globals.Doc = e.Document;
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }

        private void AddRibbonPanel(UIControlledApplication application, string tabName, string panelName)
        {
            Autodesk.Revit.UI.RibbonPanel panel = application.CreateRibbonPanel(tabName, panelName);

            if (panelName == "Conveyors")
            {
                // Create a pulldown button for MCS Conveyors
                PulldownButtonData pdData = new PulldownButtonData("MCSConveyors", "MCS Conveyors");
                pdData.LargeImage = new BitmapImage(new Uri(Path.Combine(@"C:\Users\Jemmy\source\repos\AdvansysRevitAssembly\AdvansysRevitAssembly\Resources", "conveyor32.png"), UriKind.Absolute));

                PulldownButton pdButton = panel.AddItem(pdData) as PulldownButton;


                //RevitUi.AddPushButton(panel, "Conveyor1", typeof(Conveyor1Command), Resources.add32, Resources.add32, typeof(DocumentAvailablility));
                //RevitUi.AddPushButton(panel, "Conveyor2", typeof(Conveyor2Command), Resources.add32, Resources.add32, typeof(DocumentAvailablility));
                //RevitUi.AddPushButton(panel, "Conveyor2", typeof(Conveyor3Command), Resources.add32, Resources.add32, typeof(DocumentAvailablility));

                // Add conveyors to the pulldown button
                AddConveyorItem(pdButton, "Conveyor 1", "add32.png", "AdvansysRevitAssembly.Commands.Conveyor1Command");
                AddConveyorItem(pdButton, "Conveyor 2", "add32.png", "AdvansysRevitAssembly.Commands.Conveyor2Command");
                AddConveyorItem(pdButton, "Conveyor 3", "add32.png", "AdvansysRevitAssembly.Commands.Conveyor3Command");
                AddConveyorItem(pdButton, "Conveyor 4", "add32.png", "AdvansysRevitAssembly.Commands.Conveyor3Command");
            }
            if (panelName == "Controls")
            {
                // Create a pulldown button for MCS Conveyors
                PulldownButtonData pdData = new PulldownButtonData("MCSControls", "MCS Controls");
                pdData.LargeImage = new BitmapImage(new Uri(Path.Combine(@"C:\Users\Jemmy\source\repos\AdvansysRevitAssembly\AdvansysRevitAssembly\Resources", "photoeye32.png"), UriKind.Absolute));

                PulldownButton pdButton = panel.AddItem(pdData) as PulldownButton;


                //RevitUi.AddPushButton(panel, "Conveyor1", typeof(Conveyor1Command), Resources.add32, Resources.add32, typeof(DocumentAvailablility));
                //RevitUi.AddPushButton(panel, "Conveyor2", typeof(Conveyor2Command), Resources.add32, Resources.add32, typeof(DocumentAvailablility));
                //RevitUi.AddPushButton(panel, "Conveyor2", typeof(Conveyor3Command), Resources.add32, Resources.add32, typeof(DocumentAvailablility));

                // Add conveyors to the pulldown button
                AddConveyorItem(pdButton, "PhotoEye", "add32.png", "AdvansysRevitAssembly.Commands.Conveyor1Command");
                AddConveyorItem(pdButton, "ECC", "add32.png", "AdvansysRevitAssembly.Commands.Conveyor2Command");
                AddConveyorItem(pdButton, "Bacon", "add32.png", "AdvansysRevitAssembly.Commands.Conveyor3Command");
                AddConveyorItem(pdButton, "ECG", "add32.png", "AdvansysRevitAssembly.Commands.Conveyor3Command");
            }
            // Add other panels as necessary
        }
        private void AddRibbonPanelNew(UIControlledApplication application, string tabName, string panelName)
        {
            Autodesk.Revit.UI.RibbonPanel panel = application.CreateRibbonPanel(tabName, panelName);

            if (panelName == "Conveyors")
            {
                PulldownButtonData pullButtonDataBuilding = new PulldownButtonData("Transportation", "Transportation");
                //pullButtonDataBuilding.LargeImage = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                //    "conveyor16.png"), UriKind.Absolute));
                //pullButtonDataBuilding.Image = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                //    "conveyor16.png"), UriKind.Absolute));

                PulldownButtonData pullButtonHeaderSteelData = new PulldownButtonData("Accumulation", "Accumulation");
                pullButtonHeaderSteelData.LargeImage = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                    "curved.png"), UriKind.Absolute));
                pullButtonHeaderSteelData.Image = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                    "curved.png"), UriKind.Absolute));

                PulldownButtonData pullButtonAccessoriesData = new PulldownButtonData("Gapping", "Gapping");
                pullButtonAccessoriesData.LargeImage = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                    "gappingconv16.png"), UriKind.Absolute));
                pullButtonAccessoriesData.Image = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                    "gappingconv16.png"), UriKind.Absolute));

                IList<Autodesk.Revit.UI.RibbonItem> stackedPulldownButtons = panel.AddStackedItems(pullButtonDataBuilding, pullButtonHeaderSteelData, pullButtonAccessoriesData);

                AddConveyorItem(stackedPulldownButtons[0] as PulldownButton, "Straight Conveyor", "conv16.png", "AdvansysRevitAssembly.Commands.Conveyor1Command");
                AddConveyorItem(stackedPulldownButtons[0] as PulldownButton, "Curve", "curved16.png", "AdvansysRevitAssembly.Commands.Conveyor2Command");
                AddConveyorItem(stackedPulldownButtons[0] as PulldownButton, "Incline", "conv16.png", "AdvansysRevitAssembly.Commands.Conveyor3Command");
                AddConveyorItem(stackedPulldownButtons[0] as PulldownButton, "ADV2005", "conv16.png", "AdvansysRevitAssembly.Commands.Conveyor4Command");

                PulldownButtonData pullButtonDataAlignment = new PulldownButtonData("Alignment", "Alignment");
                pullButtonDataAlignment.LargeImage = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                    "allignment16.png"), UriKind.Absolute));
                pullButtonDataAlignment.Image = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                    "allignment16.png"), UriKind.Absolute));



                PulldownButtonData pullButtonSortandMergeData = new PulldownButtonData("SortandMerge", "Sort and Merge");
                pullButtonSortandMergeData.LargeImage = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                    "sortandmerge16.png"), UriKind.Absolute));
                pullButtonSortandMergeData.Image = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                    "sortandmerge16.png"), UriKind.Absolute));

                PulldownButtonData pullButtonSpecialFunctionData = new PulldownButtonData("SpecialFunction", "Special Function");
                pullButtonSpecialFunctionData.LargeImage = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                    "specialfunction.png"), UriKind.Absolute));
                pullButtonSpecialFunctionData.Image = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                    "specialfunction.png"), UriKind.Absolute));

                IList<Autodesk.Revit.UI.RibbonItem> stackedPulldownButtons2 = panel.AddStackedItems(pullButtonDataAlignment, pullButtonSortandMergeData, pullButtonSpecialFunctionData);

                PushButtonData buttonDataAdd = new PushButtonData("Add", "Add", Assembly.GetExecutingAssembly().Location, "AdvansysRevitAssembly.Commands.Conveyor2Command");
                //buttonDataAdd.LargeImage = new BitmapImage(new Uri(Path.Combine(@"C:\Users\Jemmy\source\repos\AdvansysRevitAssembly\AdvansysRevitAssembly\Resources", "add32.png"), UriKind.Absolute));
                buttonDataAdd.LargeImage = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
    "curved.png"), UriKind.Absolute));
                buttonDataAdd.Image = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                    "curved.png"), UriKind.Absolute));
                Autodesk.Revit.UI.RibbonItem buttonDataAddPulldownButtons2 = panel.AddItem(buttonDataAdd);

                PushButtonData buttonDataDimensions = new PushButtonData("Dimensions", "Dimensions", Assembly.GetExecutingAssembly().Location, "AdvansysRevitAssembly.Commands.Conveyor2Command");
                buttonDataDimensions.LargeImage = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
"dimension32.png"), UriKind.Absolute));
                buttonDataDimensions.Image = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                    "dimension32.png"), UriKind.Absolute));
                //buttonDataDimensions.LargeImage = new BitmapImage(new Uri(Path.Combine(@"C:\Users\Jemmy\source\repos\AdvansysRevitAssembly\AdvansysRevitAssembly\Resources", "add32.png"), UriKind.Absolute));
                Autodesk.Revit.UI.RibbonItem PulldownButtons3 = panel.AddItem(buttonDataDimensions);
            }
            if (panelName == "Supports")
            {
                PulldownButtonData pullButtonDataRegular = new PulldownButtonData("Regular", "Regular");
                pullButtonDataRegular.LargeImage = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                    "support16.png"), UriKind.Absolute));
                pullButtonDataRegular.Image = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                    "support16.png"), UriKind.Absolute));



                PulldownButtonData pullButtonBracingData = new PulldownButtonData("Bracing", "Bracing");
                pullButtonBracingData.LargeImage = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                    "supportbracing16.png"), UriKind.Absolute));
                pullButtonBracingData.Image = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                    "supportbracing16.png"), UriKind.Absolute));

                PulldownButtonData pullButtonSpecialData = new PulldownButtonData("Special", "Special");
                pullButtonSpecialData.LargeImage = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                    "supportspecial16.png"), UriKind.Absolute));
                pullButtonSpecialData.Image = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                    "supportspecial16.png"), UriKind.Absolute));

                IList<Autodesk.Revit.UI.RibbonItem> stackedPulldownButtons = panel.AddStackedItems(pullButtonDataRegular, pullButtonBracingData, pullButtonSpecialData);

            }
            if (panelName == "Pallet Conveyors")
            {
                PushButtonData buttonData = new PushButtonData("PalletConveyor", "Pallet Conveyor", Assembly.GetExecutingAssembly().Location, "AdvansysRevitAssembly.Commands.Conveyor2Command");
                buttonData.LargeImage = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
    "palletconveyor32.png"), UriKind.Absolute));
                buttonData.Image = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                    "palletconveyor32.png"), UriKind.Absolute));
                //buttonData.LargeImage = new BitmapImage(new Uri(Path.Combine(@"C:\Users\Jemmy\source\repos\AdvansysRevitAssembly\AdvansysRevitAssembly\Resources", "add32.png"), UriKind.Absolute));
                Autodesk.Revit.UI.RibbonItem stackedPulldownButtons2 = panel.AddItem(buttonData);
            }
            if (panelName == "Resale")
            {
                PushButtonData buttonData = new PushButtonData("Resale", "Resale", Assembly.GetExecutingAssembly().Location, "AdvansysRevitAssembly.Commands.Conveyor2Command");
                buttonData.LargeImage = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
"resale32.png"), UriKind.Absolute));
                buttonData.Image = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                    "resale32.png"), UriKind.Absolute));
                Autodesk.Revit.UI.RibbonItem stackedPulldownButtons2 = panel.AddItem(buttonData);
            }
            if (panelName == "Structural")
            {
                PulldownButtonData pullButtonDataBuilding = new PulldownButtonData("Building", "Building");
                pullButtonDataBuilding.LargeImage = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                    "building.png"), UriKind.Absolute));
                pullButtonDataBuilding.Image = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                    "building.png"), UriKind.Absolute));



                PulldownButtonData pullButtonHeaderSteelData = new PulldownButtonData("HeaderSteel", "HeaderSteel");
                pullButtonHeaderSteelData.LargeImage = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                    "headersteel16.png"), UriKind.Absolute));
                pullButtonHeaderSteelData.Image = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                    "headersteel16.png"), UriKind.Absolute));

                PulldownButtonData pullButtonAccessoriesData = new PulldownButtonData("Accessories", "Accessories");
                pullButtonAccessoriesData.LargeImage = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                    "adapter.png"), UriKind.Absolute));
                pullButtonAccessoriesData.Image = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                    "adapter.png"), UriKind.Absolute));

                IList<Autodesk.Revit.UI.RibbonItem> stackedPulldownButtons = panel.AddStackedItems(pullButtonDataBuilding, pullButtonHeaderSteelData, pullButtonAccessoriesData);

            }
            if (panelName == "Bill Of Material")
            {
                PushButtonData AutomaticbuttonData = new PushButtonData("Automatic", "Automatic", Assembly.GetExecutingAssembly().Location, "AdvansysRevitAssembly.Commands.Conveyor2Command");
                //AutomaticbuttonData.LargeImage = new BitmapImage(new Uri(Path.Combine(@"C:\Users\Jemmy\source\repos\AdvansysRevitAssembly\AdvansysRevitAssembly\Resources", "add32.png"), UriKind.Absolute));
                AutomaticbuttonData.LargeImage = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
"auto.png"), UriKind.Absolute));
                AutomaticbuttonData.Image = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                    "auto.png"), UriKind.Absolute));
                PushButtonData EditbuttonData = new PushButtonData("  Edit", "Edit", Assembly.GetExecutingAssembly().Location, "AdvansysRevitAssembly.Commands.Conveyor2Command");
                //EditbuttonData.LargeImage = new BitmapImage(new Uri(Path.Combine(@"C:\Users\Jemmy\source\repos\AdvansysRevitAssembly\AdvansysRevitAssembly\Resources", "add32.png"), UriKind.Absolute));
                EditbuttonData.LargeImage = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
"edit.png"), UriKind.Absolute));
                EditbuttonData.Image = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                    "edit.png"), UriKind.Absolute));
                PushButtonData DeletebuttonData = new PushButtonData(" Delete", "Delete", Assembly.GetExecutingAssembly().Location, "AdvansysRevitAssembly.Commands.Conveyor2Command");
                //DeletebuttonData.LargeImage = new BitmapImage(new Uri(Path.Combine(@"C:\Users\Jemmy\source\repos\AdvansysRevitAssembly\AdvansysRevitAssembly\Resources", "add32.png"), UriKind.Absolute));
                DeletebuttonData.LargeImage = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
"delete.png"), UriKind.Absolute));
                DeletebuttonData.Image = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                    "delete.png"), UriKind.Absolute));
                IList<Autodesk.Revit.UI.RibbonItem> stackedPulldownButtons = panel.AddStackedItems(AutomaticbuttonData, EditbuttonData, DeletebuttonData);

                PushButtonData AddbuttonData = new PushButtonData("AddItem", "Add Item", Assembly.GetExecutingAssembly().Location, "AdvansysRevitAssembly.Commands.Conveyor2Command");
                //EditbuttonData.LargeImage = new BitmapImage(new Uri(Path.Combine(@"C:\Users\Jemmy\source\repos\AdvansysRevitAssembly\AdvansysRevitAssembly\Resources", "add32.png"), UriKind.Absolute));
                AddbuttonData.LargeImage = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
"bom32.png"), UriKind.Absolute));
                AddbuttonData.Image = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                    "bom32.png"), UriKind.Absolute));
                //PushButtonData SearchbuttonData = new PushButtonData("Search", "Search", Assembly.GetExecutingAssembly().Location, "AdvansysRevitAssembly.Commands.Conveyor2Command");
                //DeletebuttonData.LargeImage = new BitmapImage(new Uri(Path.Combine(@"C:\Users\Jemmy\source\repos\AdvansysRevitAssembly\AdvansysRevitAssembly\Resources", "add32.png"), UriKind.Absolute));

                Autodesk.Revit.UI.RibbonItem stackedPulldownButtons2 = panel.AddItem(AddbuttonData);


            }
            if (panelName == "Control Parts")
            {

                // Create a pulldown button for MCS Conveyors
                PulldownButtonData pdData = new PulldownButtonData("ControlParts", "Control Parts");
                //pdData.LargeImage = new BitmapImage(new Uri(Path.Combine(@"C:\Users\Jemmy\source\repos\AdvansysRevitAssembly\AdvansysRevitAssembly\Resources", "conveyor32.png"), UriKind.Absolute));
                pdData.LargeImage = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
"controls.png"), UriKind.Absolute));
                pdData.Image = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                    "controls.png"), UriKind.Absolute));
                PulldownButton pdButton = panel.AddItem(pdData) as PulldownButton;

                // Add conveyors to the pulldown button
                AddConveyorItem(pdButton, "Control Part 1", "add32.png", "AdvansysRevitAssembly.Commands.Conveyor2Command");
                AddConveyorItem(pdButton, "Control Part 2", "add32.png", "AdvansysRevitAssembly.Commands.Conveyor2Command");
                AddConveyorItem(pdButton, "Control Part 3", "add32.png", "AdvansysRevitAssembly.Commands.Conveyor3Command");
                AddConveyorItem(pdButton, "Control Part 4", "add32.png", "AdvansysRevitAssembly.Commands.Conveyor3Command");
            }
            if (panelName == "Manager")
            {
                PushButtonData AddbuttonData = new PushButtonData("Manager", "Manager", Assembly.GetExecutingAssembly().Location, "AdvansysRevitAssembly.Commands.FabricationManagerDisplayCommand");
                //EditbuttonData.LargeImage = new BitmapImage(new Uri(Path.Combine(@"C:\Users\Jemmy\source\repos\AdvansysRevitAssembly\AdvansysRevitAssembly\Resources", "add32.png"), UriKind.Absolute));
                AddbuttonData.LargeImage = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
"add32.png"), UriKind.Absolute));
                AddbuttonData.Image = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder,
                    "add32.png"), UriKind.Absolute));
                //PushButtonData SearchbuttonData = new PushButtonData("Search", "Search", Assembly.GetExecutingAssembly().Location, "AdvansysRevitAssembly.Commands.Conveyor2Command");
                //DeletebuttonData.LargeImage = new BitmapImage(new Uri(Path.Combine(@"C:\Users\Jemmy\source\repos\AdvansysRevitAssembly\AdvansysRevitAssembly\Resources", "add32.png"), UriKind.Absolute));

                //Autodesk.Revit.UI.RibbonItem stackedPulldownButtons2 = panel.AddItem(AddbuttonData);
            }
            // Add other panels as necessary
        }

        private void AddConveyorItem(PulldownButton pdButton, string name, string imagePath, string className)
        {
            PushButtonData buttonData = new PushButtonData(name, name, Assembly.GetExecutingAssembly().Location, className);

            //buttonData.LargeImage = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
            //buttonData.LargeImage = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder, imagePath), UriKind.Absolute));

            buttonData.LargeImage = new BitmapImage(new Uri(Path.Combine(UIConstants.ButtonIconsFolder, imagePath), UriKind.Absolute));

            pdButton.AddPushButton(buttonData);
        }


    }
    class DocumentAvailablility : IExternalCommandAvailability
    {
        public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
        {
            if (applicationData.ActiveUIDocument != null && applicationData.ActiveUIDocument.Document != null)
                return true;
            return false;
        }
    }
}
