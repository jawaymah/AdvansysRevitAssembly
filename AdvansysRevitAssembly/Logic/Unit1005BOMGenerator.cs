using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using AdvansysRevitAssembly.Logic;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace AdvansysRevitAssembly.Logic
{
    public static class Unit1005BOMGenerator
    {
        public static void AddParts(PdfPTable table, Font cellFont, List<FamilyInstance> families, Autodesk.Revit.DB.Document doc)
        {
            List<FamilyInstance> subcomponents = new List<FamilyInstance>();
            foreach (var family in families)
            {
                var nestedfamilies = family.GetSubComponentIds();
                foreach (var fam in nestedfamilies)
                {
                    FamilyInstance instance = doc.GetElement(fam) as FamilyInstance;
                    if (instance != null) subcomponents.Add(instance);
                }
            }

            int ADV_1005_100qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("1005-100")).Count();
            ADV_1005_100qty = ADV_1005_100qty > 0 ? ADV_1005_100qty : families.Count;
            StringBuilder ADV_1005_100descbuilder = new StringBuilder("ADV+1005 MED BED - Roller/Slider Bed\nWidth: XXXX - L:12\'");
            BomPdfCreator.AddTableRow("ADV-1005","", "ADV-1005-100","", ADV_1005_100qty.ToString(), ADV_1005_100descbuilder.ToString(), table, cellFont);

            int ADV_1005_200qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("1005-200")).Count();
            ADV_1005_200qty = ADV_1005_200qty > 0 ? ADV_1005_200qty : families.Count;
            StringBuilder ADV_1005_200descbuilder = new StringBuilder("ADV+1005 End Bed\nSLIDER BED\nWidth: XXXX - L:18\"");
            BomPdfCreator.AddTableRow("ADV-1005", "", "ADV-1005-200", "", ADV_1005_200qty.ToString(), ADV_1005_200descbuilder.ToString(), table, cellFont);

            int ADV_1005_300qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("1005-300")).Count();
            ADV_1005_300qty = ADV_1005_300qty > 0 ? ADV_1005_300qty : families.Count;
            StringBuilder ADV_1005_300descbuilder = new StringBuilder("ADV+1005 Power Fedder Module 1\nSLIDER BED\nWidth: XXXX - L:18\"");
            BomPdfCreator.AddTableRow("ADV-1005", "", "ADV-1005-300", "", ADV_1005_300qty.ToString(), ADV_1005_300descbuilder.ToString(), table, cellFont);

            int ADV_1005_400qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("1005-400")).Count();
            ADV_1005_400qty = ADV_1005_400qty > 0 ? ADV_1005_400qty : families.Count;
            StringBuilder ADV_1005_400descbuilder = new StringBuilder("ADV+1005 Power Fedder Module 2\nSLIDER BED\nWidth: XXXX - L:21\"");
            BomPdfCreator.AddTableRow("ADV-1005", "", "ADV-1005-400", "", ADV_1005_400qty.ToString(), ADV_1005_400descbuilder.ToString(), table, cellFont);

            int ADV_1005_500qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("1005-500")).Count();
            ADV_1005_500qty = ADV_1005_500qty > 0 ? ADV_1005_500qty : families.Count;
            StringBuilder ADV_1005_500descbuilder = new StringBuilder("ADV+1005 Power Fedder connector\nSnub Roller: 2 1 / 2″ dia.x 11 ga.roller, 11 / 16″ hex axle.\nTerminal Pulley: 2 5 / 8″ dia.");
            BomPdfCreator.AddTableRow("ADV-1005", "", "ADV-1005-500", "", ADV_1005_500qty.ToString(), ADV_1005_500descbuilder.ToString(), table, cellFont);

            int ADV_1005_600qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("1005-600")).Count();
            ADV_1005_600qty = ADV_1005_600qty > 0 ? ADV_1005_600qty : families.Count;
            StringBuilder ADV_1005_600descbuilder = new StringBuilder("ADV+1005 Noseover Bed\nSnub Roller:\n2 1 / 2″ dia.x 11 ga.roller, 11 / 16″ hex axle.");
            BomPdfCreator.AddTableRow("ADV-1005", "", "ADV-1005-600", "", ADV_1005_600qty.ToString(), ADV_1005_600descbuilder.ToString(), table, cellFont);

            //accessories
            int ADV_1005_4000qty = subcomponents.Where(s => s.Name.ToLower().Contains("support")).Count();
            StringBuilder ADV_1005_4000descbuilder = new StringBuilder("ADV Accessories");
            BomPdfCreator.AddTableRow("ADV-1005", "", "ADV-1005-4000", "", ADV_1005_4000qty.ToString(), ADV_1005_4000descbuilder.ToString(), table, cellFont);

            int ADV_1005_4000_100qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("4000-100")).Count();
            ADV_1005_4000_100qty = ADV_1005_4000_100qty > 0 ? ADV_1005_4000_100qty : families.Count;
            StringBuilder ADV_1005_4000_100descbuilder = new StringBuilder("ADV+4000 Belt \nWidth: XXXX - L:XXXX / CONTINUOUS TYPE 10A BELT -3 \nDEG.BIAS - SBR BLACK FRICTION SURFACE\nBELT LACED AT ONE END");
            BomPdfCreator.AddTableRow("ADV-1005", "", "ADV-1005-4000-100", "", ADV_1005_4000_100qty.ToString(), ADV_1005_4000_100descbuilder.ToString(), table, cellFont);

            int ADV_1005_4000_200qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("4000-200")).Count();
            ADV_1005_4000_200qty = ADV_1005_4000_200qty > 0 ? ADV_1005_4000_200qty : families.Count;
            StringBuilder ADV_1005_4000_200descbuilder = new StringBuilder("ADV+4000 Integral Solenoid\nSIDE - MOUNT ZONE SENSOR");
            BomPdfCreator.AddTableRow("ADV-1005", "", "ADV-1005-4000-200", "", ADV_1005_4000_200qty.ToString(), ADV_1005_4000_200descbuilder.ToString(), table, cellFont);

            int ADV_1005_4000_205qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("4000-205")).Count();
            ADV_1005_4000_205qty = ADV_1005_4000_205qty > 0 ? ADV_1005_4000_205qty : families.Count;
            StringBuilder ADV_1005_4000_205descbuilder = new StringBuilder("ADV+4000 Photo Eye\nSIDE - MOUNT ZONE SENSOR");
            BomPdfCreator.AddTableRow("ADV-1005", "", "ADV-1005-4000-205", "", ADV_1005_4000_205qty.ToString(), ADV_1005_4000_205descbuilder.ToString(), table, cellFont);

            int ADV_1005_4000_300qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("4000-300")).Count();
            ADV_1005_4000_300qty = ADV_1005_4000_300qty > 0 ? ADV_1005_4000_300qty : families.Count;
            StringBuilder ADV_1005_4000_300descbuilder = new StringBuilder("ADV+4000 Floor Support\nWidth: XXXX - L:18 - 26 Support Height");
            BomPdfCreator.AddTableRow("ADV-1005", "", "ADV-1005-4000-300", "", ADV_1005_4000_300qty.ToString(), ADV_1005_4000_300descbuilder.ToString(), table, cellFont);

            int ADV_1005_4000_301qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("4000-301")).Count();
            ADV_1005_4000_301qty = ADV_1005_4000_301qty > 0 ? ADV_1005_4000_301qty : families.Count;
            StringBuilder ADV_1005_4000_301descbuilder = new StringBuilder("ADV+4000 Leg Support \nWidth: XXXX - L:18 - 26 Support Height");
            BomPdfCreator.AddTableRow("ADV-1005", "", "ADV-1005-4000-301", "", ADV_1005_4000_301qty.ToString(), ADV_1005_4000_301descbuilder.ToString(), table, cellFont);

            int ADV_1005_4000_302qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("4000-302")).Count();
            ADV_1005_4000_302qty = ADV_1005_4000_302qty > 0 ? ADV_1005_4000_302qty : families.Count;
            StringBuilder ADV_1005_4000_302descbuilder = new StringBuilder("ADV+4000 Support Channel\n12 GA.x 2 - 1 / 2” x 1 - 3 / 8”");
            BomPdfCreator.AddTableRow("ADV-1005", "", "ADV-1005-4000-302", "", ADV_1005_4000_302qty.ToString(), ADV_1005_4000_302descbuilder.ToString(), table, cellFont);

            int ADV_1005_4000_303qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("4000-303")).Count();
            ADV_1005_4000_303qty = ADV_1005_4000_303qty > 0 ? ADV_1005_4000_303qty : families.Count;
            StringBuilder ADV_1005_4000_303descbuilder = new StringBuilder("ADV+4000 Support Boot\n11 GA.x 6 - 1 / 2” x 2”");
            BomPdfCreator.AddTableRow("ADV-1005", "", "ADV-1005-4000-303", "", ADV_1005_4000_303qty.ToString(), ADV_1005_4000_303descbuilder.ToString(), table, cellFont);

            int ADV_1005_4000_304qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("4000-304")).Count();
            ADV_1005_4000_304qty = ADV_1005_4000_304qty > 0 ? ADV_1005_4000_304qty : families.Count;
            StringBuilder ADV_1005_4000_304descbuilder = new StringBuilder("ADV+4000 Standhead/n12 GA.with pull cord tabs");
            BomPdfCreator.AddTableRow("ADV-1005", "", "ADV-1005-4000-304", "", ADV_1005_4000_304qty.ToString(), ADV_1005_4000_304descbuilder.ToString(), table, cellFont);

            int ADV_1005_4000_305qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("4000-305")).Count();
            ADV_1005_4000_305qty = ADV_1005_4000_305qty > 0 ? ADV_1005_4000_305qty : families.Count;
            StringBuilder ADV_1005_4000_305descbuilder = new StringBuilder("ADV+4000 Support Cross member  \n11 GA.x 2” diameter tube");
            BomPdfCreator.AddTableRow("ADV-1005", "", "ADV-1005-4000-305", "", ADV_1005_4000_305qty.ToString(), ADV_1005_4000_305descbuilder.ToString(), table, cellFont);

            int ADV_1005_4000_400qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("4000-400")).Count();
            ADV_1005_4000_400qty = ADV_1005_4000_400qty > 0 ? ADV_1005_4000_400qty : families.Count;
            StringBuilder ADV_1005_4000_400descbuilder = new StringBuilder("ADV+4000 Stight Conveyor Roller \n1 7 / 8″ dia.x 16 ga.roller, 7 / 16″ hex axle with axle.");
            BomPdfCreator.AddTableRow("ADV-1005", "", "ADV-1005-4000-400", "", ADV_1005_4000_400qty.ToString(), ADV_1005_4000_400descbuilder.ToString(), table, cellFont);

            int ADV_1005_4000_500qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("4000-500")).Count();
            ADV_1005_4000_500qty = ADV_1005_4000_500qty > 0 ? ADV_1005_4000_500qty : families.Count;
            StringBuilder ADV_1005_4000_500descbuilder = new StringBuilder("ADV+4000 Side Channel Left side \n6″ deep x 1 3 / 4″ flange x 10 ga.steel channel frame with 3 / 4″ return flange.");
            BomPdfCreator.AddTableRow("ADV-1005", "", "ADV-1005-4000-500", "", ADV_1005_4000_500qty.ToString(), ADV_1005_4000_500descbuilder.ToString(), table, cellFont);

            int ADV_1005_4000_600qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("4000-600")).Count();
            ADV_1005_4000_600qty = ADV_1005_4000_600qty > 0 ? ADV_1005_4000_600qty : families.Count;
            StringBuilder ADV_1005_4000_600descbuilder = new StringBuilder("ADV+4000 Side Channel Right side \n6″ deep x 1 3 / 4″ flange x 10 ga.steel channel frame with \n3 / 4″ return flange.");
            BomPdfCreator.AddTableRow("ADV-1005", "", "ADV-1005-4000-600", "", ADV_1005_4000_600qty.ToString(), ADV_1005_4000_600descbuilder.ToString(), table, cellFont);

            int ADV_1005_4000_700qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("4000-700")).Count();
            ADV_1005_4000_700qty = ADV_1005_4000_700qty > 0 ? ADV_1005_4000_700qty : families.Count;
            StringBuilder ADV_1005_4000_700descbuilder = new StringBuilder("ADV+4000 End Cover");
            BomPdfCreator.AddTableRow("ADV-1005", "", "ADV-1005-4000-700", "", ADV_1005_4000_700qty.ToString(), ADV_1005_4000_700descbuilder.ToString(), table, cellFont);
        }
    }
}
