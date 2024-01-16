using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdvansysRevitAssembly;
using AdvansysRevitAssembly.Logic.ElementsViewsHelper;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace AdvansysRevitAssembly.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class CreateDrawingCommand : IExternalCommand
    {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements)
    {
        try
        {
            // Get the active document and application
            Document doc = commandData.Application.ActiveUIDocument.Document;
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Application app = uiDoc.Application.Application;

            // Prompt user to select an element (floor plan)
            Reference selectedRef = uiDoc.Selection.PickObject(ObjectType.Element, "Select a floor plan");
            Element selectedElement = doc.GetElement(selectedRef);

            // Specify the title block family name
            string titleBlockFamilyName = "YourTitleBlockFamilyName"; // Replace with your actual title block family name

            // Create a new drafting view with a floor plan viewport
            using (Transaction transaction = new Transaction(doc, "Create Drawing"))
            {
                transaction.Start();

                // Find the title block family type
                FilteredElementCollector titleBlockCollector = new FilteredElementCollector(doc)
                    .OfClass(typeof(ViewFamilyType))
                    .WhereElementIsElementType();

                ViewFamilyType titleBlockType = titleBlockCollector.Cast<ViewFamilyType>().FirstOrDefault(type => type.FamilyName == titleBlockFamilyName);

                if (titleBlockType != null)
                {
                    // Create a new drafting view
                    ViewDrafting newDraftingView = ViewDrafting.Create(doc, titleBlockType.Id);
                    newDraftingView.Name = "Custom Drawing";

                    // Create a floor plan viewport in the new drafting view
                    Viewport.Create(doc, newDraftingView.Id, selectedElement.Id, new XYZ(0, 0, 0));

                    transaction.Commit();

                    return Result.Succeeded;
                }
                else
                {
                    message = "Title block family not found.";
                    transaction.RollBack();
                    return Result.Failed;
                }
            }
        }
        catch (Exception ex)
        {
            message = ex.Message;
            return Result.Failed;
        }
    }
    }

    [Transaction(TransactionMode.Manual)]
    public class CreateDrawingCommand2 : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            try
            {
                // Get the active document and application
                Document doc = commandData.Application.ActiveUIDocument.Document;
                UIDocument uiDoc = commandData.Application.ActiveUIDocument;
                Application app = uiDoc.Application.Application;


                // Retrieve pre-selected elements.

                ICollection<ElementId> ids = uiDoc.Selection.GetElementIds();

                if (0 == ids.Count)
                {
                    message = "Please pre-select some elements "
                      + "before launching this command to list "
                      + "the views displaying them.";

                    return Result.Failed;
                }

                // Determine views displaying them.

                IEnumerable<Element> targets = from id in ids select doc.GetElement(id);

                IEnumerable<View> views = targets.FindAllViewsWhereAllElementsVisible();
                var planViews = views.Where(s => s is ViewPlan);
                if (0 == planViews.Count())
                {
                    message = "There is no views that include this elements";
                    return Result.Failed;
                }

                View view = views.FirstOrDefault();

                // Prompt user to select an element (floor plan)
                //Reference selectedRef = uiDoc.Selection.PickObject(ObjectType.Element, "Select a floor plan");
                //Element selectedElement = doc.GetElement(selectedRef);

                // Specify the title block family name
                string titleBlockFamilyName = "advansystitleblockE130x42Horizontal"; // Replace with your actual title block family name

                // Check if the title block family is loaded, if not, load it
                FilteredElementCollector titleBlockCollector = new FilteredElementCollector(doc)
                    .OfClass(typeof(Family))
                    .OfCategory(BuiltInCategory.OST_TitleBlocks)
                    .WhereElementIsElementType();

                Family titleBlockFamily = titleBlockCollector.Cast<Family>()
                    .FirstOrDefault(family => family.Name == titleBlockFamilyName);

                if (titleBlockFamily == null)
                {
                    // Load the title block family if it does not exist
                    if (LoadFamily(doc, titleBlockFamilyName))
                    {
                        // Now the title block family is loaded, find its FamilySymbol
                        titleBlockCollector = new FilteredElementCollector(doc)
                            .OfClass(typeof(FamilySymbol))
                            .OfCategory(BuiltInCategory.OST_TitleBlocks)
                            .WhereElementIsElementType();

                        FamilySymbol titleBlockSymbol = titleBlockCollector.Cast<FamilySymbol>()
                            .FirstOrDefault(symbol => symbol.FamilyName == titleBlockFamilyName);

                        if (titleBlockSymbol != null)
                        {
                            // Create a new drafting view with a floor plan viewport
                            using (Transaction transaction = new Transaction(doc, "Create Drawing"))
                            {
                                transaction.Start();

                                // Create a new drafting view
                                //ViewDrafting newDraftingView = ViewDrafting.Create(doc, titleBlockSymbol.Id);
                                ViewSheet newsheet = ViewSheet.Create(doc, titleBlockSymbol.Id);
                                newsheet.Name = "Custom Drawing";
                                SetCropRegion(doc, view, targets.ToList());
                                // Create a floor plan viewport in the new drafting view
                                Viewport vp = Viewport.Create(doc, newsheet.Id, view.Id, new XYZ(0, 0, 0));
                                CenterViewportOnSheet(doc, newsheet, vp);
                                transaction.Commit();
                                uiDoc.ActiveView = newsheet;

                                return Result.Succeeded;
                            }
                        }
                    }
                }
                else
                {
                    // The title block family is already loaded, proceed with creating the drawing
                    // Create a new drafting view with a floor plan viewport
                    using (Transaction transaction = new Transaction(doc, "Create Drawing"))
                    {
                        transaction.Start();
                        // Assume the family has a family symbol (family type)
                        FamilySymbol titleBlockSymbol = null;
                        foreach (ElementId id in titleBlockFamily.GetFamilySymbolIds())
                        {
                            titleBlockSymbol = doc.GetElement(id) as FamilySymbol;
                            break; // For simplicity, using the first available symbol
                        }
                        // Create a new drafting view
                        ViewSheet newsheet = ViewSheet.Create(doc, titleBlockSymbol.Id);
                        newsheet.Name = "Custom Drawing";
                        SetCropRegion(doc, view, targets.ToList());
                        // Create a floor plan viewport in the new drafting view
                        Viewport vp = Viewport.Create(doc, newsheet.Id, view.Id, new XYZ(0, 0, 0));
                        CenterViewportOnSheet(doc, newsheet, vp);

                        transaction.Commit();
                        uiDoc.ActiveView = newsheet;

                        return Result.Succeeded;
                    }
                }

                message = "Failed to load or find the title block family.";
                return Result.Failed;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }

        // Function to load a family into the document
        private bool LoadFamily(Document doc, string familyName)
        {
            try
            {
                // Specify the path to the title block family file
                string familyPath = new Uri(Path.Combine(UIConstants.ButtonFamiliesFolder, "advansystitleblockE130x42Horizontal.rfa"), UriKind.Absolute).AbsolutePath; // Replace with the actual path

                // Check if the family file exists
                if (System.IO.File.Exists(familyPath))
                {
                    // Load the family into the document
                    Family family = null;
                    using (Transaction transaction = new Transaction(doc, "Load TitleBlock Family"))
                    {
                        transaction.Start();

                        FamilySymbol familySymbol = null;
                        try
                        {
                            doc.LoadFamilySymbol(familyPath, "E1 30x42 Horizontal", out familySymbol);
                        }
                        catch (Autodesk.Revit.Exceptions.ArgumentException)
                        {
                            // Family with the same name may already exist
                            familySymbol = doc.GetElement(familySymbol.GetTypeId()) as FamilySymbol;
                        }

                        if (familySymbol != null)
                        {
                            family = familySymbol.Family;
                        }

                        transaction.Commit();
                    }

                    return family != null;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        // Function to center the viewport on the sheet
        private void CenterViewportOnSheet(Document doc, ViewSheet sheet, Viewport viewport)
        {
            // Get the sheet width and height
            double sheetWidth = (sheet.Outline.Max.U - sheet.Outline.Min.U) *0.875;
            double sheetHeight = sheet.Outline.Max.V - sheet.Outline.Min.V;

            // Get the viewport width and height
            double viewportWidth = viewport.GetBoxCenter().X * 2;
            double viewportHeight = viewport.GetBoxCenter().Y * 2;

            // Calculate the offset to center the viewport on the sheet
            double offsetX = (sheetWidth - viewportWidth) / 2;
            double offsetY = (sheetHeight - viewportHeight) / 2;

            // Set the new viewport location on the sheet
            XYZ newLocation = new XYZ(offsetX, offsetY, 0);
            ElementTransformUtils.MoveElement(doc, viewport.Id, newLocation);
        }
        // Function to get the combined bounding box for a list of elements
        private BoundingBoxXYZ GetCombinedBoundingBox(Document doc, View view, IList<Element> elements)
        {
            BoundingBoxXYZ combinedBoundingBox = elements[0].get_BoundingBox(view);

            foreach (Element element in elements)
            {
                BoundingBoxXYZ elementBoundingBox = element.get_BoundingBox(view);

                if (elementBoundingBox != null)
                {
                    XYZ minPoint = new XYZ(
                        Math.Min(combinedBoundingBox.Min.X, elementBoundingBox.Min.X),
                        Math.Min(combinedBoundingBox.Min.Y, elementBoundingBox.Min.Y),
                        Math.Min(combinedBoundingBox.Min.Z, elementBoundingBox.Min.Z));

                    XYZ maxPoint = new XYZ(
                        Math.Max(combinedBoundingBox.Max.X, elementBoundingBox.Max.X),
                        Math.Max(combinedBoundingBox.Max.Y, elementBoundingBox.Max.Y),
                        Math.Max(combinedBoundingBox.Max.Z, elementBoundingBox.Max.Z));

                    combinedBoundingBox.Min = minPoint;
                    combinedBoundingBox.Max = maxPoint;
                }
            }

            return combinedBoundingBox;
        }


        // Function to set the crop region to match the selected element
        private void SetCropRegion(Document doc, View view, List<Element> selectedElements)
        {
            // Get the combined bounding box for all selected elements
            BoundingBoxXYZ combinedBoundingBox = GetCombinedBoundingBox(doc, view, selectedElements);

            if (combinedBoundingBox != null)
            {
                Outline outline = new Outline(combinedBoundingBox.Min, combinedBoundingBox.Max);

                // Set the crop region to match the selected element
                //using (Transaction transaction = new Transaction(doc, "Set Crop Region"))
                //{
                  //  transaction.Start();

                    if (view != null)
                    {
                        // Set the crop region
                        view.CropBox = combinedBoundingBox;

                        // Hide other elements in the viewport
                        //HideOtherElementsInView(doc, viewport.ViewId, selectedElement);
                    }

                   // transaction.Commit();
                //}
            }
        }
        // Function to hide other elements in the specified view
        private void HideOtherElementsInView(Document doc, View view, List<Element> selectedElements)
        {
            // Collect all elements in the view
            FilteredElementCollector collector = new FilteredElementCollector(doc, view.Id);
            ICollection<ElementId> elementIdsInView = collector.ToElementIds();

            // Hide elements that are not the selected element
            //using (Transaction transaction = new Transaction(doc, "Hide Other Elements"))
            //{
                //transaction.Start();
                List<ElementId> ids = new List<ElementId>();
                foreach (ElementId elementIdInView in elementIdsInView)
                {
                    if (selectedElements.Where(s=>s.Id == elementIdInView).Count() == 0) //if (elementIdInView != selectedElement.Id)
                    {
                        // Check if the element is visible before hiding
                        if (doc.GetElement(elementIdInView)?.CanBeHidden(view) == true)
                        {
                            ids.Add(elementIdInView);
                            //doc.GetElement(elementIdInView)?.get_Parameter(BuiltInParameter.MODEL_GRAPHICS_STYLE).Set(0); // Set visibility to invisible
                        }
                    }
                }
                view.HideElements(ids);
                //transaction.Commit();
            //}
        }

    }
}


