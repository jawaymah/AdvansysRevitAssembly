﻿using System;
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
    public static class Unit3005BOMGenerator
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
            int ADV_3005_100qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("3005-100")).Count();
            ADV_3005_100qty = ADV_3005_100qty > 0 ? ADV_3005_100qty : families.Count;
            StringBuilder ADV_3005_100descbuilder = new StringBuilder("ADV Curve BED");
            BomPdfCreator.AddTableRow("ADV-3005", "", "ADV-3005-100", "", ADV_3005_100qty.ToString(), ADV_3005_100descbuilder.ToString(), table, cellFont);

            int ADV_3005_101qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("3005-101")).Count();
            ADV_3005_101qty = ADV_3005_101qty > 0 ? ADV_3005_101qty : families.Count;
            StringBuilder ADV_3005_101descbuilder = new StringBuilder("ADV+3005 Side Channel Left side \n10″ deep x 1 3 / 4″ flange x 10 gauge\nsteel channel frame with axle\nisolators included.Inside radius\nequals 29 7 / 8².");
            BomPdfCreator.AddTableRow("ADV-3005", "", "ADV-3005-101", "", ADV_3005_101qty.ToString(), ADV_3005_101descbuilder.ToString(), table, cellFont);

            int ADV_3005_102qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("3005-102")).Count();
            ADV_3005_102qty = ADV_3005_102qty > 0 ? ADV_3005_102qty : families.Count;
            StringBuilder ADV_3005_102descbuilder = new StringBuilder("ADV+3005 Side Channel Right side \n10″ deep x 1 3 / 4″ flange x 10 gauge\nsteel channel frame with axle\nisolators included.Inside radius\nequals 29 7 / 8².");
            BomPdfCreator.AddTableRow("ADV-3005", "", "ADV-3005-102", "", ADV_3005_102qty.ToString(), ADV_3005_102descbuilder.ToString(), table, cellFont);

            int ADV_3005_103qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("3005-103")).Count();
            ADV_3005_103qty = ADV_3005_103qty > 0 ? ADV_3005_103qty : families.Count;
            StringBuilder ADV_3005_103descbuilder = new StringBuilder("ADV+3005 Curve Roller\n1 7 / 8″ dia. (small end) tapered\ncarrying rollers with 7 / 16″ hex axles.");
            BomPdfCreator.AddTableRow("ADV-3005", "", "ADV-3005-103", "", ADV_3005_103qty.ToString(), ADV_3005_103descbuilder.ToString(), table, cellFont);

            int ADV_3005_200qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("3005-200")).Count();
            ADV_3005_200qty = ADV_3005_200qty > 0 ? ADV_3005_200qty : families.Count;
            StringBuilder ADV_3005_200descbuilder = new StringBuilder("ADV+3005 curve End Module \nWidth: XXXX - L:18\"");
            BomPdfCreator.AddTableRow("ADV-3005", "", "ADV-3005-200", "", ADV_3005_200qty.ToString(), ADV_3005_200descbuilder.ToString(), table, cellFont);

            int ADV_3005_300qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("3005-300")).Count();
            ADV_3005_300qty = ADV_3005_300qty > 0 ? ADV_3005_300qty : families.Count;
            StringBuilder ADV_3005_300descbuilder = new StringBuilder("ADV+3005 curve drive Module\nWidth: XXXX - L:18\"");
            BomPdfCreator.AddTableRow("ADV-3005", "", "ADV-3005-300", "", ADV_3005_300qty.ToString(), ADV_3005_300descbuilder.ToString(), table, cellFont);

            //accessories
            int ADV_3005_4000qty = 2;// subcomponents.Where(s=>s.Name.ToLower().Contains("support")).Count();
            StringBuilder ADV_3005_4000descbuilder = new StringBuilder("ADV Accessories");
            BomPdfCreator.AddTableRow("ADV-3005", "", "ADV-3005-4000", "", ADV_3005_4000qty.ToString(), ADV_3005_4000descbuilder.ToString(), table, cellFont);

            int ADV_3005_4000_300qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("4000-300")).Count();
            ADV_3005_4000_300qty = ADV_3005_4000_300qty > 0 ? ADV_3005_4000_300qty : families.Count;
            StringBuilder ADV_3005_4000_300descbuilder = new StringBuilder("ADV+4000 Floor Support\nWidth: XXXX - L:18 - 26 Support Height");
            BomPdfCreator.AddTableRow("ADV-3005", "", "ADV-3005-4000-300", "", ADV_3005_4000_300qty.ToString(), ADV_3005_4000_300descbuilder.ToString(), table, cellFont);

            int ADV_3005_4000_301qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("4000-301")).Count();
            ADV_3005_4000_301qty = ADV_3005_4000_301qty > 0 ? ADV_3005_4000_301qty : families.Count;
            StringBuilder ADV_3005_4000_301descbuilder = new StringBuilder("ADV+4000 Leg Support \nWidth: XXXX - L:18 - 26 Support Height");
            BomPdfCreator.AddTableRow("ADV-3005", "", "ADV-3005-4000-301", "", ADV_3005_4000_301qty.ToString(), ADV_3005_4000_301descbuilder.ToString(), table, cellFont);

            int ADV_3005_4000_302qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("4000-302")).Count();
            ADV_3005_4000_302qty = ADV_3005_4000_302qty > 0 ? ADV_3005_4000_302qty : families.Count;
            StringBuilder ADV_3005_4000_302descbuilder = new StringBuilder("ADV+4000 Support Channel\n12 GA.x 2 - 1 / 2” x 1 - 3 / 8”");
            BomPdfCreator.AddTableRow("ADV-3005", "", "ADV-3005-4000-302", "", ADV_3005_4000_302qty.ToString(), ADV_3005_4000_302descbuilder.ToString(), table, cellFont);

            int ADV_3005_4000_303qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("4000-303")).Count();
            ADV_3005_4000_303qty = ADV_3005_4000_303qty > 0 ? ADV_3005_4000_303qty : families.Count;
            StringBuilder ADV_3005_4000_303descbuilder = new StringBuilder("ADV+4000 Support Boot\n11 GA.x 6 - 1 / 2” x 2”");
            BomPdfCreator.AddTableRow("ADV-3005", "", "ADV-3005-4000-303", "", ADV_3005_4000_303qty.ToString(), ADV_3005_4000_303descbuilder.ToString(), table, cellFont);

            int ADV_3005_4000_304qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("4000-304")).Count();
            ADV_3005_4000_304qty = ADV_3005_4000_304qty > 0 ? ADV_3005_4000_304qty : families.Count;
            StringBuilder ADV_3005_4000_304descbuilder = new StringBuilder("ADV+4000 Standhead\n12 GA.with pull cord tabs");
            BomPdfCreator.AddTableRow("ADV-3005", "", "ADV-3005-4000-304", "", ADV_3005_4000_304qty.ToString(), ADV_3005_4000_304descbuilder.ToString(), table, cellFont);

            int ADV_3005_4000_305qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("4000-305")).Count();
            ADV_3005_4000_305qty = ADV_3005_4000_305qty > 0 ? ADV_3005_4000_305qty : families.Count;
            StringBuilder ADV_3005_4000_305descbuilder = new StringBuilder("ADV+4000 Support Cross member\n11 GA.x 2” diameter tube");
            BomPdfCreator.AddTableRow("ADV-3005", "", "ADV-3005-4000-305", "", ADV_3005_4000_305qty.ToString(), ADV_3005_4000_305descbuilder.ToString(), table, cellFont);

            int ADV_3005_4000_400qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("4000-400")).Count();
            ADV_3005_4000_400qty = ADV_3005_4000_400qty > 0 ? ADV_3005_4000_400qty : families.Count;
            StringBuilder ADV_3005_4000_400descbuilder = new StringBuilder("ADV+4000 Stight Conveyor Roller \n1 7 / 8″ dia.x 16 ga.roller, 7 / 16″ hex axle with axle.");
            BomPdfCreator.AddTableRow("ADV-3005", "", "ADV-3005-4000-400", "", ADV_3005_4000_400qty.ToString(), ADV_3005_4000_400descbuilder.ToString(), table, cellFont);

            int ADV_3005_4000_500qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("4000-500")).Count();
            ADV_3005_4000_500qty = ADV_3005_4000_500qty > 0 ? ADV_3005_4000_500qty : families.Count;
            StringBuilder ADV_3005_4000_500descbuilder = new StringBuilder("ADV+4000 Side Channel Left side \n6″ deep x 1 3 / 4″ flange x 10 ga.steel channel frame\nwith 3 / 4″ return flange.");
            BomPdfCreator.AddTableRow("ADV-3005", "", "ADV-3005-4000-500", "", ADV_3005_4000_500qty.ToString(), ADV_3005_4000_500descbuilder.ToString(), table, cellFont);

            int ADV_3005_4000_600qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("4000-600")).Count();
            ADV_3005_4000_600qty = ADV_3005_4000_600qty > 0 ? ADV_3005_4000_600qty : families.Count;
            StringBuilder ADV_3005_4000_600descbuilder = new StringBuilder("ADV+4000 Side Channel Right side \n6″ deep x 1 3 / 4″ flange x 10 ga.steel channel frame\nwith 3 / 4″ return flange.");
            BomPdfCreator.AddTableRow("ADV-3005", "", "ADV-3005-4000-600", "", ADV_3005_4000_600qty.ToString(), ADV_3005_4000_600descbuilder.ToString(), table, cellFont);

            int ADV_3005_4000_700qty = subcomponents.Where(s => s.Symbol.FamilyName.Contains("4000-700")).Count();
            ADV_3005_4000_700qty = ADV_3005_4000_700qty > 0 ? ADV_3005_4000_700qty : families.Count;
            StringBuilder ADV_3005_4000_700descbuilder = new StringBuilder("ADV+4000 End Cover");
            BomPdfCreator.AddTableRow("ADV-3005", "", "ADV-3005-4000-700", "", ADV_3005_4000_700qty.ToString(), ADV_3005_4000_700descbuilder.ToString(), table, cellFont);
        }
    }
}
