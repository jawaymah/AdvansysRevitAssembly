using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace AdvansysRevitAssembly.Logic
{

    public class ITextEvents : PdfPageEventHelper
    {
        // This is the contentbyte object of the writer
        PdfContentByte cb;
        // we will put the final number of pages in a template
        PdfTemplate headerTemplate, footerTemplate;
        // this is the BaseFont we are going to use for the header / footer
        BaseFont bf = null;
        // This keeps track of the creation time
        DateTime PrintTime = DateTime.Now;
        #region Fields
        private string _header;
        #endregion
        #region Properties
        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }
        #endregion
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                PrintTime = DateTime.Now;
                bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                headerTemplate = cb.CreateTemplate(100, 100);
                footerTemplate = cb.CreateTemplate(50, 50);
            }
            catch (DocumentException de)
            {
            }
            catch (System.IO.IOException ioe)
            {
            }
        }
        public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
        {
            base.OnEndPage(writer, document);
            iTextSharp.text.Font baseFontNormal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font baseFontBig = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Phrase p1Header = new Phrase("Sample Header Here", baseFontNormal);
            //Create PdfTable object
            PdfPTable pdfTab = new PdfPTable(3);
            //We will have to create separate cells to include image logo and 2 separate strings
            //Row 1
            PdfPCell pdfCell1 = new PdfPCell();
            PdfPCell pdfCell2 = new PdfPCell(p1Header);
            PdfPCell pdfCell3 = new PdfPCell();
            String text = "Page " + writer.PageNumber + " of ";
            //Add paging to header
            {
                cb.BeginText();
                cb.SetFontAndSize(bf, 12);
                cb.SetTextMatrix(document.PageSize.GetRight(200), document.PageSize.GetTop(45));
                cb.ShowText(text);
                cb.EndText();
                float len = bf.GetWidthPoint(text, 12);
                //Adds "12" in Page 1 of 12
                cb.AddTemplate(headerTemplate, document.PageSize.GetRight(200) + len, document.PageSize.GetTop(45));
            }
            //Add paging to footer
            {
                cb.BeginText();
                cb.SetFontAndSize(bf, 12);
                cb.SetTextMatrix(document.PageSize.GetRight(180), document.PageSize.GetBottom(30));
                cb.ShowText(text);
                cb.EndText();
                float len = bf.GetWidthPoint(text, 12);
                cb.AddTemplate(footerTemplate, document.PageSize.GetRight(180) + len, document.PageSize.GetBottom(30));
            }
            //Row 2
            PdfPCell pdfCell4 = new PdfPCell(new Phrase("Sub Header Description", baseFontNormal));
            //Row 3 
            PdfPCell pdfCell5 = new PdfPCell(new Phrase("Date:" + PrintTime.ToShortDateString(), baseFontBig));
            PdfPCell pdfCell6 = new PdfPCell();
            PdfPCell pdfCell7 = new PdfPCell(new Phrase("TIME:" + string.Format("{0:t}", DateTime.Now), baseFontBig));
            //set the alignment of all three cells and set border to 0
            pdfCell1.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell2.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell3.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell4.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell5.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell6.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell7.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell2.VerticalAlignment = Element.ALIGN_BOTTOM;
            pdfCell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell4.VerticalAlignment = Element.ALIGN_TOP;
            pdfCell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell6.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell7.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell4.Colspan = 3;
            pdfCell1.Border = 0;
            pdfCell2.Border = 0;
            pdfCell3.Border = 0;
            pdfCell4.Border = 0;
            pdfCell5.Border = 0;
            pdfCell6.Border = 0;
            pdfCell7.Border = 0;
            //add all three cells into PdfTable
            pdfTab.AddCell(pdfCell1);
            pdfTab.AddCell(pdfCell2);
            pdfTab.AddCell(pdfCell3);
            pdfTab.AddCell(pdfCell4);
            pdfTab.AddCell(pdfCell5);
            pdfTab.AddCell(pdfCell6);
            pdfTab.AddCell(pdfCell7);
            pdfTab.TotalWidth = document.PageSize.Width - 80f;
            pdfTab.WidthPercentage = 70;
            //pdfTab.HorizontalAlignment = Element.ALIGN_CENTER;    
            //call WriteSelectedRows of PdfTable. This writes rows from PdfWriter in PdfTable
            //first param is start row. -1 indicates there is no end row and all the rows to be included to write
            //Third and fourth param is x and y position to start writing
            pdfTab.WriteSelectedRows(0, -1, 40, document.PageSize.Height - 30, writer.DirectContent);
            //set pdfContent value
            //Move the pointer and draw line to separate header section from rest of page
            cb.MoveTo(40, document.PageSize.Height - 100);
            cb.LineTo(document.PageSize.Width - 40, document.PageSize.Height - 100);
            cb.Stroke();
            //Move the pointer and draw line to separate footer section from rest of page
            cb.MoveTo(40, document.PageSize.GetBottom(50));
            cb.LineTo(document.PageSize.Width - 40, document.PageSize.GetBottom(50));
            cb.Stroke();
        }
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
            headerTemplate.BeginText();
            headerTemplate.SetFontAndSize(bf, 12);
            headerTemplate.SetTextMatrix(0, 0);
            headerTemplate.ShowText((writer.PageNumber - 1).ToString());
            headerTemplate.EndText();
            footerTemplate.BeginText();
            footerTemplate.SetFontAndSize(bf, 12);
            footerTemplate.SetTextMatrix(0, 0);
            footerTemplate.ShowText((writer.PageNumber - 1).ToString());
            footerTemplate.EndText();
        }
    }

    public class PdfHeaderFooter : PdfPageEventHelper
    {
        private readonly string headerImagePath;
        private readonly string footerImagePath;

        public PdfHeaderFooter(string headerImagePath, string footerImagePath)
        {
            this.headerImagePath = headerImagePath;
            this.footerImagePath = footerImagePath;
        }

        public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
        {
            base.OnEndPage(writer, document);

            float pageSize = 30;

            // Add header image
            if (!string.IsNullOrEmpty(headerImagePath))
            {
                float x = pageSize;
                int y = document.PageNumber;



                //iTextSharp. canvas = new iText.Layout.Canvas(document, pdfDocument.GetLastPage());
                //canvas.Add(new Image(ImageDataFactory.Create(headerImagePath))
                //    .ScaleToFit(100, 100)
                //    .SetFixedPosition(x, y));
            }

            // Add footer image
            if (!string.IsNullOrEmpty(footerImagePath))
            {
                float x = pageSize;
                int y = document.PageNumber;

                //iText.Layout.Canvas canvas = new iText.Layout.Canvas(document, pdfDocument.GetLastPage());
                //canvas.Add(new Image(ImageDataFactory.Create(footerImagePath))
                //    .ScaleToFit(100, 100)
                //    .SetFixedPosition(x, y));
            }
        }
    }

    class MyPageEventHandler : PdfPageEventHelper
    {
        // Override the OnEndPage method to add header and footer
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            // Add header
            PdfPTable headerTable = new PdfPTable(1);
            headerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            headerTable.DefaultCell.Border = 0;

            //string familyPath = new Uri(Path.Combine(UIConstants.ButtonIconsFolder, "Daifuku_logo.png"), UriKind.Absolute).AbsolutePath;
            //// Add a logo image
            //Image logo = Image.GetInstance(familyPath); // Replace with your logo path
            //logo.ScaleToFit(100f, 60f); // Scale the image to fit

            //PdfPCell imageCell = new PdfPCell(logo);
            //imageCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //imageCell.Border = PdfPCell.NO_BORDER;
            //imageCell.PaddingBottom = 5 * 2.0f;
            //headerTable.AddCell(imageCell);

            float xPosition = document.PageSize.Width / 2.0f; // 10 is a margin from the right
            float yPosition = document.PageSize.Height - 20; // 10 is a margin from the bottom

            string familyPath = new Uri(Path.Combine(UIConstants.ButtonIconsFolder, "Daifuku_logo.png"), UriKind.Absolute).AbsolutePath;
            Image headerImage = Image.GetInstance(familyPath); // Replace with your logo path
            headerImage.ScaleAbsolute(60f, 20f); // Set the absolute size of the logo
            PdfContentByte contentByte = writer.DirectContent;
            xPosition = document.PageSize.Width - headerImage.ScaledWidth - 20; // 10 is a margin from the right
            yPosition = document.PageSize.Height - headerImage.ScaledHeight - 30; // 10 is a margin from the bottom
            headerImage.SetAbsolutePosition(xPosition, yPosition);
            contentByte.AddImage(headerImage);

            xPosition = document.PageSize.Width /2.0f; // 10 is a margin from the right
            yPosition = document.PageSize.Height - 40; // 10 is a margin from the bottom

            BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font headerFont = new Font(baseFont, 9, Font.BOLD);
            Font cellFont = new Font(baseFont, 8);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase("CONVEYOR INSTALLER REPORT", headerFont), xPosition, yPosition, 0);
            xPosition = 40;
            yPosition = document.PageSize.Height - 80;
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("SALES ORDER NUMBER: 397788AA", headerFont), xPosition, yPosition, 0);

            headerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - document.TopMargin + headerTable.TotalHeight, writer.DirectContent);

            // Add footer
            PdfPTable footerTable = new PdfPTable(1);
            footerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            footerTable.DefaultCell.Border = 0;

            // Add your footer image here
            string advansyslogopath = new Uri(Path.Combine(UIConstants.ButtonIconsFolder, "advansyslogo.png"), UriKind.Absolute).AbsolutePath;
            Image footerImage = Image.GetInstance(advansyslogopath); // Replace with your logo path
            footerImage.ScaleAbsolute(100f, 50f); // Set the absolute size of the logo
            // Get the PDF content byte to add image
            //PdfContentByte contentByte = writer.DirectContent;
            // Calculate position for the image (bottom right)
            xPosition = document.PageSize.Width - footerImage.ScaledWidth - 10; // 10 is a margin from the right
            yPosition = 10; // 10 is a margin from the bottom
            // Add image to the specific position
            footerImage.SetAbsolutePosition(xPosition, yPosition);
            contentByte.AddImage(footerImage);
            PdfPCell footerCell = new PdfPCell(footerImage, true);
            footerCell.Border = 0;
            footerTable.AddCell(footerCell);

            footerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.BottomMargin - footerTable.TotalHeight, writer.DirectContent);
        }
    }

}
