using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfInvoice
{
    public abstract class PdfSettings
    {
        public string Date;
        public string InvoiceNo;
        public string SalesRef;
        public string Total;
        public string BillToContent;
        public string LicenseeContent;
        public string CommentContent;
        public List<PdfHeaderContent> PdfHeaderContentList;
        public List<PdfBodyContent> PdfBodyContentList;
        public List<PdfFooterContent> PdfFooterContentList;
        public string LogoPath;

        internal const float MarginLeft = 26;
        internal const float MarginRight = 26;
        internal const float MarginTop = 26;
        internal const float MarginBottom = 26;
        internal const float Padding = 30;
        internal const int CellBorder = Rectangle.NO_BORDER;

        internal static readonly Rectangle PageSize = iTextSharp.text.PageSize.LETTER;

        internal static readonly BaseFont Bftimes = BaseFont.CreateFont(@"C:\Windows\Fonts\LSANS.TTF", BaseFont.CP1252, BaseFont.EMBEDDED);
        internal static readonly BaseColor CellBackColorGrey = new BaseColor(231, 231, 231);
        internal static readonly BaseColor CellBackColorWhite = new BaseColor(255, 255, 255);

        private readonly MemoryStream _stream = new MemoryStream();

        public byte[] CreatePdf()
        {
            using (Document doc = new Document(PageSize, MarginLeft, MarginRight, MarginTop, MarginBottom))
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, _stream);
                writer.CloseStream = false;

                doc.Open();

                //Header.CreateHeader(doc, this);
                //Body.CreateBody(doc, this);
                //Footer.CreateFooter(doc, writer, this);
            }

            return _stream.ToArray();
        }

        internal PdfPTable TableSetUp(int rows)
        {
            PdfPTable table = new PdfPTable(rows)
            {
                SpacingBefore = Padding - 18,
                TotalWidth = PageSize.Width - 52,
                LockedWidth = true
            };
            return table;
        }

        internal PdfPCell AddCell(CellRowSettings cellRowSettings, Phrase phrase, BaseColor cellColored)
        {
            PdfPCell cell = CellSettings(cellRowSettings, phrase);

            SetSpanAndCellHeight(cell, cellRowSettings);

            cell.BackgroundColor = cellColored;

            return cell;
        }

        internal PdfPCell AddCell(CellRowSettings cellRowSettings, iTextSharp.text.Image image)
        {
            PdfPCell cell = CellSettings(cellRowSettings, image);

            SetSpanAndCellHeight(cell, cellRowSettings);

            return cell;
        }

        internal PdfPCell AddCell(CellRowSettings cellRowSettings, string text, BaseColor cellColored, int fontsize = 10)
        {
            PdfPCell cell = CellSettings(cellRowSettings, text, new Font(Bftimes, fontsize, Font.NORMAL, BaseColor.BLACK));

            SetSpanAndCellHeight(cell, cellRowSettings);

            cell.BackgroundColor = cellColored;

            return cell;
        }

        internal PdfPCell AddTextForCellRow(CellRowSettings cellRowSettings, string text)
        {
            PdfPCell cell = CellSettings(cellRowSettings, text, new Font(Bftimes, 10, Font.NORMAL, BaseColor.BLACK));

            SetSpanAndCellHeight(cell, cellRowSettings);

            return cell;
        }

        internal PdfPCell AddTextForCellRow(CellRowSettings cellRowSettings, string text, BaseColor cellColored)
        {
            PdfPCell cell = CellSettings(cellRowSettings, text, new Font(Bftimes, 10, Font.NORMAL, BaseColor.BLACK));

            SetSpanAndCellHeight(cell, cellRowSettings);

            cell.BackgroundColor = cellColored;

            return cell;
        }

        internal PdfPCell AddCellWithMultipleContent(CellRowSettings cellRowSettings, string subText = "", List<string> subtextList = null)
        {
            PdfPCell cell = new PdfPCell();
            SetSpanAndCellHeight(cell, cellRowSettings);

            if (subtextList == null)
                AddCellElement(cell, cellRowSettings, subText);
            else
            {
                for (int i = 0; i < subtextList.Count; i++)
                    AddCellElement(cell, cellRowSettings, subtextList[i]);
            }

            return cell;
        }

        internal PdfPCell AddCellWithMultipleContent(CellRowSettings cellRowSettings, iTextSharp.text.Image image)
        {
            PdfPCell cell = new PdfPCell();
            SetSpanAndCellHeight(cell, cellRowSettings);

            cell.AddElement(image);
            cell.Border = CellBorder;

            return cell;
        }

        internal PdfPCell CellSettings(CellRowSettings cellRowSettings, iTextSharp.text.Image image)
        {
            PdfPCell cell = new PdfPCell(image);
            cell.Border = CellBorder;
            cell.HorizontalAlignment = cellRowSettings.horizontalAlign;
            cell.VerticalAlignment = cellRowSettings.verticalAlign;

            return cell;
        }

        internal PdfPCell CellSettings(CellRowSettings cellRowSettings, Phrase phrase)
        {
            PdfPCell cell = new PdfPCell(phrase);
            cell.Border = CellBorder;
            cell.HorizontalAlignment = cellRowSettings.horizontalAlign;
            cell.VerticalAlignment = cellRowSettings.verticalAlign;

            return cell;
        }

        internal PdfPCell CellSettings(CellRowSettings cellRowSettings, string text, Font font)
        {
            PdfPCell cell = new PdfPCell(CellContence(text, font));
            cell.Border = CellBorder;
            cell.HorizontalAlignment = cellRowSettings.horizontalAlign;
            cell.VerticalAlignment = cellRowSettings.verticalAlign;

            return cell;
        }

        internal void AddCellElement(PdfPCell cell, CellRowSettings cellRowSettings, string text)
        {
            Paragraph paragraph = new Paragraph(CellContence(text, new Font(Bftimes, 10, Font.NORMAL, BaseColor.BLACK)));
            paragraph.Alignment = cellRowSettings.horizontalAlign;
            cell.AddElement(paragraph);
            cell.Border = CellBorder;
        }

        internal Phrase CellContence(string text, Font times)
        {
            Phrase phrase = new Phrase(text, times);

            return phrase;
        }

        internal void SetSpanAndCellHeight(PdfPCell cell, CellRowSettings cellRowSettings)
        {
            if (cellRowSettings.rowspan != 0)
                cell.Rowspan = cellRowSettings.rowspan;

            if (cellRowSettings.colspan != 0)
                cell.Colspan = cellRowSettings.colspan;

            if (cellRowSettings.cellHeight != 0)
                cell.FixedHeight = cellRowSettings.cellHeight;
        }

        internal static PdfPCell EditBorders(PdfPCell cell)
        {
            cell.Border = Rectangle.RIGHT_BORDER;
            cell.BorderColor = BaseColor.WHITE;
            return cell;
        }

        internal struct CellRowSettings
        {
            public int rowspan;
            public int colspan;
            public int horizontalAlign;
            public int verticalAlign;
            public int cellHeight;

            public CellRowSettings(int rowspan, int colspan, int horizontalAlign, int verticalAlign, int cellHeight)
            {
                this.rowspan = rowspan;
                this.colspan = colspan;
                this.horizontalAlign = horizontalAlign;
                this.verticalAlign = verticalAlign;
                this.cellHeight = cellHeight;
            }
        }
    }
}
