using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfInvoice
{
    public abstract class PdfSettings
    {
        public List<PdfFooterContent> PdfFooterContentList;

        public const float MarginLeft = 26;
        public const float MarginRight = 26;
        public const float MarginTop = 26;
        public const float MarginBottom = 26;
        public const float Padding = 30;
        public const int CellBorder = Rectangle.NO_BORDER;

        public static readonly Rectangle PageSize = iTextSharp.text.PageSize.LETTER;

        public static readonly BaseFont Bftimes = BaseFont.CreateFont(@"C:\Windows\Fonts\LSANS.TTF", BaseFont.CP1252, BaseFont.EMBEDDED);
        public static readonly BaseColor CellBackColorGrey = new BaseColor(231, 231, 231);
        public static readonly BaseColor CellBackColorWhite = new BaseColor(255, 255, 255);

        public PdfPTable TableSetUp(int rows)
        {
            PdfPTable table = new PdfPTable(rows)
            {
                SpacingBefore = Padding - 18,
                TotalWidth = PageSize.Width - 52,
                LockedWidth = true
            };
            return table;
        }

        public PdfPCell AddCell(CellRowSettings cellRowSettings, Phrase phrase, BaseColor cellColored)
        {
            PdfPCell cell = CellSettings(cellRowSettings, phrase);

            SetSpanAndCellHeight(cell, cellRowSettings);

            cell.BackgroundColor = cellColored;

            return cell;
        }

        public PdfPCell AddCell(CellRowSettings cellRowSettings, Image image)
        {
            PdfPCell cell = CellSettings(cellRowSettings, image);

            SetSpanAndCellHeight(cell, cellRowSettings);

            return cell;
        }

        public PdfPCell AddCell(CellRowSettings cellRowSettings, string text, BaseColor cellColored, int fontsize = 10)
        {
            PdfPCell cell = CellSettings(cellRowSettings, text, new Font(Bftimes, fontsize, Font.NORMAL, BaseColor.BLACK));

            SetSpanAndCellHeight(cell, cellRowSettings);

            cell.BackgroundColor = cellColored;

            return cell;
        }

        public PdfPCell AddTextForCellRow(CellRowSettings cellRowSettings, string text)
        {
            PdfPCell cell = CellSettings(cellRowSettings, text, new Font(Bftimes, 10, Font.NORMAL, BaseColor.BLACK));

            SetSpanAndCellHeight(cell, cellRowSettings);

            return cell;
        }

        public PdfPCell AddTextForCellRow(CellRowSettings cellRowSettings, string text, BaseColor cellColored)
        {
            PdfPCell cell = CellSettings(cellRowSettings, text, new Font(Bftimes, 10, Font.NORMAL, BaseColor.BLACK));

            SetSpanAndCellHeight(cell, cellRowSettings);

            cell.BackgroundColor = cellColored;

            return cell;
        }

        public PdfPCell AddCellWithMultipleContent(CellRowSettings cellRowSettings, string subText = "")
        {
            PdfPCell cell = new PdfPCell();
            SetSpanAndCellHeight(cell, cellRowSettings);

            AddCellElement(cell, cellRowSettings, subText);
            
            return cell;
        }

        public PdfPCell AddCellWithMultipleContent(CellRowSettings cellRowSettings, string subText,
            List<string> subtextList)
        {
            PdfPCell cell = new PdfPCell();
            SetSpanAndCellHeight(cell, cellRowSettings);

            foreach (var text in subtextList)
                AddCellElement(cell, cellRowSettings, text);

            return cell;
        }

        public PdfPCell AddCellWithMultipleContent(CellRowSettings cellRowSettings, Image image)
        {
            PdfPCell cell = new PdfPCell();
            SetSpanAndCellHeight(cell, cellRowSettings);

            cell.AddElement(image);
            cell.Border = CellBorder;

            return cell;
        }

        public PdfPCell CellSettings(CellRowSettings cellRowSettings, Image image)
        {
            PdfPCell cell = new PdfPCell(image)
            {
                Border = CellBorder,
                HorizontalAlignment = cellRowSettings.HorizontalAlign,
                VerticalAlignment = cellRowSettings.VerticalAlign
            };

            return cell;
        }

        public PdfPCell CellSettings(CellRowSettings cellRowSettings, Phrase phrase)
        {
            PdfPCell cell = new PdfPCell(phrase)
            {
                Border = CellBorder,
                HorizontalAlignment = cellRowSettings.HorizontalAlign,
                VerticalAlignment = cellRowSettings.VerticalAlign
            };

            return cell;
        }

        public PdfPCell CellSettings(CellRowSettings cellRowSettings, string text, Font font)
        {
            PdfPCell cell = new PdfPCell(CellContence(text, font))
            {
                Border = CellBorder,
                HorizontalAlignment = cellRowSettings.HorizontalAlign,
                VerticalAlignment = cellRowSettings.VerticalAlign
            };

            return cell;
        }

        public void AddCellElement(PdfPCell cell, CellRowSettings cellRowSettings, string text)
        {
            Paragraph paragraph = new Paragraph(CellContence(text, new Font(Bftimes, 10, Font.NORMAL, BaseColor.BLACK)))
            {
                Alignment = cellRowSettings.HorizontalAlign
            };
            cell.AddElement(paragraph);
            cell.Border = CellBorder;
        }

        public Phrase CellContence(string text, Font times)
        {
            Phrase phrase = new Phrase(text, times);

            return phrase;
        }

        public void SetSpanAndCellHeight(PdfPCell cell, CellRowSettings cellRowSettings)
        {
            if (cellRowSettings.Rowspan != 0)
                cell.Rowspan = cellRowSettings.Rowspan;

            if (cellRowSettings.Colspan != 0)
                cell.Colspan = cellRowSettings.Colspan;

            if (cellRowSettings.CellHeight != 0)
                cell.FixedHeight = cellRowSettings.CellHeight;
        }

        public static PdfPCell EditBorders(PdfPCell cell)
        {
            cell.Border = Rectangle.RIGHT_BORDER;
            cell.BorderColor = BaseColor.WHITE;
            return cell;
        }

        public struct CellRowSettings
        {
            public int Rowspan;
            public int Colspan;
            public int HorizontalAlign;
            public int VerticalAlign;
            public int CellHeight;

            public CellRowSettings(int rowspan, int colspan, int horizontalAlign, int verticalAlign, int cellHeight)
            {
                Rowspan = rowspan;
                Colspan = colspan;
                HorizontalAlign = horizontalAlign;
                VerticalAlign = verticalAlign;
                CellHeight = cellHeight;
            }
        }
    }
}
