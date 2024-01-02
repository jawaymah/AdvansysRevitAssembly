using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace AdvansysRevitAssembly.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class Conveyor3Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                Document doc = commandData.Application.ActiveUIDocument.Document;


                //bool isLoaded = doc.LoadFamilySymbol(@"C:\Users\Jemmy\AppData\Roaming\Autodesk\Revit\Addins\2024\roller7.rfa", "roller7", out FamilySymbol roller);
                //if (!isLoaded) tr.RollBack();
                //isLoaded = doc.LoadFamilySymbol(@"C:\Users\Jemmy\AppData\Roaming\Autodesk\Revit\Addins\2024\side channel2.rfa", "side channel2", out FamilySymbol sidechannel2);
                //if (!isLoaded) tr.RollBack();
                bool isLoaded = doc.LoadFamilySymbol(@"C:\Users\Jemmy\AppData\Roaming\Autodesk\Revit\Addins\2024\side channel3.rfa", "side channel3", out FamilySymbol familySymbol);
                //if (!isLoaded) tr.RollBack();
                //tr.Commit();

                //// Absolute path to the family file
                //string familyPath = @"C:\Users\Jemmy\AppData\Roaming\Autodesk\Revit\Addins\2024\side channel3.rfa";

                //Family family;
                //if (!doc.LoadFamily(familyPath, out family))
                //{
                //    message = "Could not load family.";
                //    return Result.Failed;
                //}

                //// Assume the family has a family symbol (family type)
                //FamilySymbol familySymbol = null;
                //foreach (ElementId id in family.GetFamilySymbolIds())
                //{
                //    familySymbol = doc.GetElement(id) as FamilySymbol;
                //    break; // For simplicity, using the first available symbol
                //}

                if (familySymbol == null)
                {
                    message = "No family symbols found in the family.";
                    return Result.Failed;
                }

                XYZ location = new XYZ(0, 0, 0); // Location to place the family instance

                using (Transaction t = new Transaction(doc, "Place Family Instance"))
                {
                    t.Start();

                    if (!familySymbol.IsActive)
                        familySymbol.Activate();

                    // Create a new instance of the family symbol at the specified location
                    doc.Create.NewFamilyInstance(location, familySymbol, StructuralType.NonStructural);

                    t.Commit();
                }

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }
    }

}
