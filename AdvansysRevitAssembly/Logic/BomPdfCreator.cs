using System;
using System.IO;
using System.Windows.Media.Imaging;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Windows.Forms; // Needed for SaveFileDialog
using Autodesk.Revit.UI;
using System.Diagnostics;
using Autodesk.Revit.DB;
using Element = iTextSharp.text.Element;
using System.Collections.Generic;
using System.Linq;

namespace AdvansysRevitAssembly.Logic
{
    public static class BomPdfCreator
    {
        public static Result CreateReport(ref string message, Autodesk.Revit.DB.Document doc, List<ElementId> snapperIds)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
                saveFileDialog.Title = "Save PDF File";
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = saveFileDialog.FileName;

                    // Ensure the filename ends with .pdf
                    if (!selectedFilePath.ToLower().EndsWith(".pdf"))
                    {
                        selectedFilePath += ".pdf";
                    }

                    List<FamilyInstance> instances = new List<FamilyInstance>();
                    snapperIds.ForEach(s => instances.Add(doc.GetElement(s) as FamilyInstance));
                    instances = instances.Where(s => s != null).ToList();
                    // Now you can proceed to save your PDF at 'selectedFilePath'
                    // Example: Export your Revit view to PDF
                    Create(selectedFilePath, instances, doc);
                    // Open the PDF file with the default PDF viewer
                    try
                    {
                        Process.Start(selectedFilePath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error opening PDF file: " + ex.Message);
                    }
                }

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }
        public static void Create(string exportpath, List<FamilyInstance> snappers , Autodesk.Revit.DB.Document doc)
        {
            // Create a new document
            iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4, 35f, 35f, 85f, 55f);
            //iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4);
            // Create a writer instance
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(exportpath, FileMode.Create));
            writer.PageEvent = new MyPageEventHandler();
            // Open the document for writing
            document.Open();

            // Create a table with the appropriate number of columns
            PdfPTable headertable = new PdfPTable(3); // Adjust the number of columns as needed
            headertable.WidthPercentage = 100;

            // Set custom widths for each column
            float[] columnWidths = new float[] { 0.8f, 0.9f, 1.1f }; // The first column is twice as wide as the second and third
            headertable.SetWidths(columnWidths);

            // Set font
            BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font headerFont = new Font(baseFont, 9, Font.BOLD);
            Font normalFont = new Font(baseFont, 12);
            Font cellheaderFont = new Font(baseFont, 8, Font.BOLD);
            Font cellFont = new Font(baseFont, 8);
            int headerBottomPadding = 5;
            float headerBottomWidth = 1f;

            // Create an empty cell
            PdfPCell emptyCell = new PdfPCell(new Phrase(""));
            emptyCell.Border = PdfPCell.NO_BORDER;
            //headertable.AddCell(emptyCell);

            // Adding a header
            PdfPCell header1 = new PdfPCell();// new PdfPCell(new Phrase("CONVEYOR INSTALLER REPORT", headerFont));
            header1.HorizontalAlignment = Element.ALIGN_RIGHT;
            header1.Border = PdfPCell.NO_BORDER;
            //headertable.AddCell(header1);


            string familyPath = new Uri(Path.Combine(UIConstants.ButtonIconsFolder, "Daifuku_logo.png"), UriKind.Absolute).AbsolutePath;
            // Add a logo image
            Image logo = Image.GetInstance(familyPath); // Replace with your logo path
            //logo.ScaleToFit(140f, 120f); // Scale the image to fit
            logo.ScaleToFit(100f, 60f); // Scale the image to fit

            PdfPCell imageCell = new PdfPCell();// new PdfPCell(logo);
            imageCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            imageCell.Border = PdfPCell.NO_BORDER;
            //imageCell.PaddingBottom = headerBottomPadding * 2.0f;
            imageCell.PaddingTop = headerBottomPadding * 4.0f;
            //headertable.AddCell(imageCell);

            //headertable.AddCell(emptyCell);
            emptyCell.Border = PdfPCell.NO_BORDER;
            //headertable.AddCell(emptyCell);
            emptyCell.Border = PdfPCell.NO_BORDER;
            PdfPCell page = new PdfPCell(new Phrase("PAGE: 1 OF 1", cellFont));
            page.HorizontalAlignment = Element.ALIGN_LEFT;
            page.Border = PdfPCell.NO_BORDER;
            //headertable.AddCell(emptyCell);

            // Adding a header
            PdfPCell header2 = new PdfPCell(); new PdfPCell(new Phrase("SALES ORDER NUMBER: 397788AA", headerFont));
            header2.HorizontalAlignment = Element.ALIGN_LEFT;
            header2.Border = PdfPCell.NO_BORDER;
            header2.Colspan = 2;
            //header2.PaddingBottom = headerBottomPadding*2.0f;
            headertable.AddCell(header2);


            PdfPCell printed = new PdfPCell(new Phrase($"PRINTED: {DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss  tt")}", cellFont));
            printed.HorizontalAlignment = Element.ALIGN_LEFT;
            printed.Border = PdfPCell.NO_BORDER;
            printed.PaddingLeft = headerBottomPadding * 2.0f;
            headertable.AddCell(printed);

            document.Add(headertable);
            // Create a table with the appropriate number of columns
            PdfPTable table = new PdfPTable(6); // Adjust the number of columns as needed
            table.WidthPercentage = 100;

            // Set custom widths for each column
            columnWidths = new float[] { 1, 1, 1 , 1, 1, 4}; // The first column is twice as wide as the second and third
            table.SetWidths(columnWidths);


            PdfPCell cell11 = new PdfPCell(new Phrase("CONV\n NO.", cellheaderFont));
            cell11.Border = PdfPCell.BOTTOM_BORDER;
            cell11.BorderWidthBottom = headerBottomWidth;
            cell11.VerticalAlignment = Element.ALIGN_BOTTOM;
            cell11.HorizontalAlignment = Element.ALIGN_CENTER;
            cell11.PaddingBottom = headerBottomPadding;
            PdfPCell cell12 = new PdfPCell(new Phrase("SO LINE\n NO.", cellheaderFont));
            cell12.Border = PdfPCell.BOTTOM_BORDER;
            cell12.BorderWidthBottom = headerBottomWidth;
            cell12.VerticalAlignment = Element.ALIGN_BOTTOM;
            cell12.HorizontalAlignment = Element.ALIGN_CENTER;
            cell12.PaddingBottom = headerBottomPadding;
            PdfPCell cell13 = new PdfPCell(new Phrase("SO PART\n NO.", cellheaderFont));
            cell13.Border = PdfPCell.BOTTOM_BORDER;
            cell13.BorderWidthBottom = headerBottomWidth;
            cell13.VerticalAlignment = Element.ALIGN_BOTTOM;
            cell13.HorizontalAlignment = Element.ALIGN_CENTER;
            cell13.PaddingBottom = headerBottomPadding;
            PdfPCell cell14 = new PdfPCell(new Phrase("AUOTMATION\n PART NO.", cellheaderFont));
            cell14.Border = PdfPCell.BOTTOM_BORDER;
            cell14.BorderWidthBottom = headerBottomWidth;
            cell14.VerticalAlignment = Element.ALIGN_BOTTOM;
            cell14.HorizontalAlignment = Element.ALIGN_CENTER;
            cell14.PaddingBottom = headerBottomPadding;
            PdfPCell cell15 = new PdfPCell(new Phrase("QTY", cellheaderFont));
            cell15.Border = PdfPCell.BOTTOM_BORDER;
            cell15.BorderWidthBottom = headerBottomWidth;
            cell15.VerticalAlignment = Element.ALIGN_BOTTOM;
            cell15.HorizontalAlignment = Element.ALIGN_CENTER;
            cell15.PaddingBottom = headerBottomPadding;
            PdfPCell cell16 = new PdfPCell(new Phrase("DESCRIPTION", cellheaderFont));
            cell16.Border = PdfPCell.BOTTOM_BORDER;
            cell16.BorderWidthBottom = headerBottomWidth;
            cell16.VerticalAlignment = Element.ALIGN_BOTTOM;
            cell16.HorizontalAlignment = Element.ALIGN_CENTER;
            cell16.PaddingBottom = headerBottomPadding;

            table.AddCell(cell11);
            table.AddCell(cell12);
            table.AddCell(cell13);
            table.AddCell(cell14);
            table.AddCell(cell15);
            table.AddCell(cell16);

            //foreach (var snapper in snappers)
            //{

            //}
            CreateRows(snappers, ref table, cellFont, doc);
            // Add the table to the document
            document.Add(table);


            // Load the logo image
            string advansyslogopath = new Uri(Path.Combine(UIConstants.ButtonIconsFolder, "advansyslogo.png"), UriKind.Absolute).AbsolutePath;
            Image advansyslogo = Image.GetInstance(advansyslogopath); // Replace with your logo path
            advansyslogo.ScaleAbsolute(100f, 50f); // Set the absolute size of the logo

            // Get the PDF content byte to add image
            PdfContentByte contentByte = writer.DirectContent;

            // Calculate position for the image (bottom right)
            float xPosition = document.PageSize.Width - logo.ScaledWidth - 10; // 10 is a margin from the right
            float yPosition = 10; // 10 is a margin from the bottom

            // Add image to the specific position
            advansyslogo.SetAbsolutePosition(xPosition, yPosition);
            //contentByte.AddImage(advansyslogo);


            // Close the document
            document.Close();
        }

        public static void CreateRows(List<FamilyInstance> instances, ref PdfPTable table, Font cellFont, Autodesk.Revit.DB.Document doc)
        {
            var groups = instances.GroupBy(s => s.Symbol.Name);
            foreach (var group in groups)
            {
                if (group.Key.Contains("2005"))
                {
                    CreateRow2005(group.ToList(), ref table, cellFont, doc);
                }
                else if (group.Key.Contains("3005"))
                {
                    CreateRow3005(group.ToList(), ref table, cellFont, doc);
                }
                else if (group.Key.Contains("1006") || group.Key.Contains("1005"))
                {
                    CreateRow1005(group.ToList(), ref table, cellFont, doc);
                }
            }
        }
        public static void CreateRow3005(List<FamilyInstance> instances, ref PdfPTable table, Font cellFont, Autodesk.Revit.DB.Document doc)
        {
            PdfPCell cell21 = new PdfPCell(new Phrase("3005", cellFont));
            cell21.Border = PdfPCell.NO_BORDER;
            cell21.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell cell22 = new PdfPCell(new Phrase("13 CAR-3280", cellFont));
            cell22.Border = PdfPCell.NO_BORDER;
            cell22.HorizontalAlignment = Element.ALIGN_CENTER;
            cell22.Colspan = 2;
            PdfPCell cell23 = new PdfPCell(new Phrase("CARB-3283", cellFont));
            cell23.Border = PdfPCell.NO_BORDER;
            cell23.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell cell24 = new PdfPCell(new Phrase(instances.Count.ToString(), cellFont));
            cell24.Border = PdfPCell.NO_BORDER;
            cell24.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell cell25 = new PdfPCell(new Phrase("ADV+3005 Curved Module\nAngle XX DEGREE\nWidth: XXXX - 2 in Center Roller R\nRIGHT HAND", cellFont));
            cell25.Border = PdfPCell.NO_BORDER;
            PdfPCell[] cellsarr2 = new PdfPCell[5] { cell21, cell22, cell23, cell24, cell25 };

            table.AddCell(cell21);
            table.AddCell(cell22);
            table.AddCell(cell23);
            table.AddCell(cell24);
            table.AddCell(cell25);
            Unit3005BOMGenerator.AddParts(table, cellFont, instances, doc);
        }
        public static void CreateRow2005(List<FamilyInstance> instances, ref PdfPTable table, Font cellFont, Autodesk.Revit.DB.Document doc)
        {
            PdfPCell cell21 = new PdfPCell(new Phrase("2005", cellFont));
            cell21.Border = PdfPCell.NO_BORDER;
            cell21.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell cell22 = new PdfPCell(new Phrase("13 CAR-3280", cellFont));
            cell22.Border = PdfPCell.NO_BORDER;
            cell22.HorizontalAlignment = Element.ALIGN_CENTER;
            cell22.Colspan = 2;
            PdfPCell cell23 = new PdfPCell(new Phrase("CARB-3283", cellFont));
            cell23.Border = PdfPCell.NO_BORDER;
            cell23.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell cell24 = new PdfPCell(new Phrase(instances.Count.ToString(), cellFont));
            cell24.Border = PdfPCell.NO_BORDER;
            cell24.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell cell25 = new PdfPCell(new Phrase("ADV+2005 Accumulation Module\n3\" center roller\nDrive type: Center drive / Right Hand\nWidth: XXXX - OAL:XXXX\nBelt on Roller: WITH / WITHOUT\n36\" ZONE - SIDE-MOUNT ZONE SENSOR", cellFont));
            cell25.Border = PdfPCell.NO_BORDER;
            PdfPCell[] cellsarr2 = new PdfPCell[5] { cell21, cell22, cell23, cell24, cell25 };

            table.AddCell(cell21);
            table.AddCell(cell22);
            table.AddCell(cell23);
            table.AddCell(cell24);
            table.AddCell(cell25);
            Unit2005BOMGenerator.AddParts(table, cellFont, instances, doc);
        }
        public static void CreateRow1005(List<FamilyInstance> instances, ref PdfPTable table, Font cellFont, Autodesk.Revit.DB.Document doc)
        {
            PdfPCell cell21 = new PdfPCell(new Phrase("1005", cellFont));
            cell21.Border = PdfPCell.NO_BORDER;
            cell21.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell cell22 = new PdfPCell(new Phrase("13 CAR-3280", cellFont));
            cell22.Border = PdfPCell.NO_BORDER;
            cell22.HorizontalAlignment = Element.ALIGN_CENTER;
            cell22.Colspan = 2;
            PdfPCell cell23 = new PdfPCell(new Phrase("CARB-3283", cellFont));
            cell23.Border = PdfPCell.NO_BORDER;
            cell23.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell cell24 = new PdfPCell(new Phrase(instances.Count.ToString(), cellFont));
            cell24.Border = PdfPCell.NO_BORDER;
            cell24.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell cell25 = new PdfPCell(new Phrase("ADV+1005 Inclined\\Declined Module \nDrive type: Center drive / Right Hand\nWidth: XXXX - OAL:XXXX - Angle: XXXX\nBelt on: Roller / Slider", cellFont));
            cell25.Border = PdfPCell.NO_BORDER;
            PdfPCell[] cellsarr2 = new PdfPCell[5] { cell21, cell22, cell23, cell24, cell25 };

            table.AddCell(cell21);
            table.AddCell(cell22);
            table.AddCell(cell23);
            table.AddCell(cell24);
            table.AddCell(cell25);
            Unit1005BOMGenerator.AddParts(table, cellFont, instances, doc);
        }
        public static void AddTableRow(string convNo, string soLineNo, string soPartNo, string automationPartNo, string qty, string description, PdfPTable table, Font cellFont)
        {
            PdfPCell cell21 = new PdfPCell(new Phrase(convNo, cellFont));
            cell21.Border = PdfPCell.NO_BORDER;
            cell21.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell cell22 = new PdfPCell(new Phrase(soLineNo, cellFont));
            cell22.Border = PdfPCell.NO_BORDER;
            cell22.HorizontalAlignment = Element.ALIGN_CENTER;
            cell22.Colspan = 2;
            PdfPCell cell23 = new PdfPCell(new Phrase(soPartNo, cellFont));
            cell23.Border = PdfPCell.NO_BORDER;
            cell23.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell cell24 = new PdfPCell(new Phrase(qty, cellFont));
            cell24.Border = PdfPCell.NO_BORDER;
            cell24.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell cell25 = new PdfPCell(new Phrase(description, cellFont));
            cell25.Border = PdfPCell.NO_BORDER;
            PdfPCell[] cellsarr2 = new PdfPCell[5] { cell21, cell22, cell23, cell24, cell25 };

            table.AddCell(cell21);
            table.AddCell(cell22);
            table.AddCell(cell23);
            table.AddCell(cell24);
            table.AddCell(cell25);
        }
    }
}
