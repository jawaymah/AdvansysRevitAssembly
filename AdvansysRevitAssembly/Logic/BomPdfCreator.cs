using System;
using System.IO;
using System.Windows.Media.Imaging;
using iTextSharp.text;
using iTextSharp.text.pdf;
namespace AdvansysRevitAssembly.Logic
{
    public static class BomPdfCreator
    {
        public static void Create()
        {
            // Create a new document
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);

            // Create a writer instance
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream("example.pdf", FileMode.Create));

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
            headertable.AddCell(emptyCell);

            // Adding a header
            PdfPCell header1 = new PdfPCell(new Phrase("CONVEYOR INSTALLER REPORT", headerFont));
            header1.HorizontalAlignment = Element.ALIGN_RIGHT;
            header1.Border = PdfPCell.NO_BORDER;
            headertable.AddCell(header1);


            string familyPath = new Uri(Path.Combine(UIConstants.ButtonIconsFolder, "Daifuku_logo.png"), UriKind.Absolute).AbsolutePath;
            // Add a logo image
            Image logo = Image.GetInstance(familyPath); // Replace with your logo path
            //logo.ScaleToFit(140f, 120f); // Scale the image to fit
            logo.ScaleToFit(100f, 80f); // Scale the image to fit

            PdfPCell imageCell = new PdfPCell(logo);
            imageCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            imageCell.Border = PdfPCell.NO_BORDER;
            imageCell.PaddingBottom = headerBottomPadding * 2.0f;
            headertable.AddCell(imageCell);

            headertable.AddCell(emptyCell);
            emptyCell.Border = PdfPCell.NO_BORDER;
            headertable.AddCell(emptyCell);
            emptyCell.Border = PdfPCell.NO_BORDER;
            PdfPCell page = new PdfPCell(new Phrase("PAGE: 1 OF 1", cellFont));
            page.HorizontalAlignment = Element.ALIGN_LEFT;
            page.Border = PdfPCell.NO_BORDER;
            headertable.AddCell(page);

            // Adding a header
            PdfPCell header2 = new PdfPCell(new Phrase("SALES ORDER NUMBER: 397788AA", headerFont));
            header2.HorizontalAlignment = Element.ALIGN_CENTER;
            header2.Border = PdfPCell.NO_BORDER;
            header2.PaddingBottom = headerBottomPadding*2.0f;
            headertable.AddCell(header2);

            headertable.AddCell(emptyCell);

            PdfPCell printed = new PdfPCell(new Phrase($"PRINTED: {DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss  tt")}", cellFont));
            printed.HorizontalAlignment = Element.ALIGN_LEFT;
            printed.Border = PdfPCell.NO_BORDER;
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
            PdfPCell[] cellsarr = new PdfPCell[6] { cell11, cell12, cell13,cell14,cell15,cell16};
            //PdfPRow row1 = new PdfPRow(cellsarr);
            table.AddCell(cell11);
            table.AddCell(cell12);
            table.AddCell(cell13);
            table.AddCell(cell14);
            table.AddCell(cell15);
            table.AddCell(cell16);


            PdfPCell cell21 = new PdfPCell(new Phrase("1304", cellFont));
            cell21.Border = PdfPCell.NO_BORDER;
            cell21.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell cell22 = new PdfPCell(new Phrase("13 CAR-3280", cellFont));
            cell22.Border = PdfPCell.NO_BORDER;
            cell22.HorizontalAlignment = Element.ALIGN_CENTER;
            cell22.Colspan = 2;
            PdfPCell cell23 = new PdfPCell(new Phrase("CARB-3283", cellFont));
            cell23.Border = PdfPCell.NO_BORDER;
            cell23.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell cell24 = new PdfPCell(new Phrase("1", cellFont));
            cell24.Border = PdfPCell.NO_BORDER;
            cell24.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell cell25 = new PdfPCell(new Phrase("AR+ C762 ACCUMULATION MODULE \n30NW X 108L- 3 IN ROLLER CENTERS - BELT OVER ROLLER: STRIP \n36 IN ZONES - RIGHT HAND- SIDE- MOUNT ZONE SENSOR \nERSC CARDS \nNOMINAL SPEED 160 \nPAINT CODE:038", cellFont));
            cell25.Border = PdfPCell.NO_BORDER;
            PdfPCell[] cellsarr2 = new PdfPCell[5] { cell21, cell22, cell23, cell24, cell25 };
            //PdfPRow row2 = new PdfPRow(cellsarr2);
            //table.Rows.Add(row2);
            table.AddCell(cell21);
            table.AddCell(cell22);
            table.AddCell(cell23);
            table.AddCell(cell24);
            table.AddCell(cell25);
            // Add the table to the document
            document.Add(table);

            // Close the document
            document.Close();
        }
        static void MainPDF()
        {
            // Create a new document
            Document document = new Document();

            // Create a writer instance
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream("output.pdf", FileMode.Create));

            // Open the document for writing
            document.Open();

            // Add a logo image
            Image logo = Image.GetInstance("path_to_logo.jpg"); // Replace with your logo path
            logo.ScaleToFit(140f, 120f); // Scale the image to fit
            logo.Alignment = Element.ALIGN_LEFT;
            document.Add(logo);

            // Add a header
            Paragraph header = new Paragraph("Your Header Text", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD));
            header.Alignment = Element.ALIGN_CENTER;
            document.Add(header);

            // Add a table
            PdfPTable table = new PdfPTable(3); // Replace 3 with the number of your columns
            table.AddCell("Column 1 Header");
            table.AddCell("Column 2 Header");
            table.AddCell("Column 3 Header");
            // Add rows
            table.AddCell("Row 1, Col 1");
            table.AddCell("Row 1, Col 2");
            table.AddCell("Row 1, Col 3");
            // Add more cells as needed
            document.Add(table);

            // Add paragraphs
            document.Add(new Paragraph("Your paragraph text here."));
            // Add more paragraphs as needed

            // Close the document
            document.Close();
        }
    }
}
