using System;
using System.Collections.Generic;
using System.Globalization;
using iTextSharp.text;

namespace PdfInvoice
{
    public class Body : PdfSettings
    {
        private static decimal _totalPrice;

        private Document _document;
        private PdfBodyContent _bodyContent;

        public Body(Document doc, PdfBodyContent bodyContent)
        {
            _document = doc;
            _bodyContent = bodyContent;
        }

        public void CreateBody()
        {
            var table = TableSetUp(3);
            table.SpacingBefore = 30;
            table.SetWidths(new float[] { 33, 10, 59 });
            table.KeepTogether = true;

            table.AddCell(AddCell(new CellRowSettings(1, 0, Element.ALIGN_LEFT, Element.ALIGN_TOP, 16), "Bill To:", CellBackColorGrey));
            table.AddCell(AddCell(new CellRowSettings(1, 0, Element.ALIGN_MIDDLE, Element.ALIGN_MIDDLE, 16), "", CellBackColorWhite));
            table.AddCell(AddCell(new CellRowSettings(1, 0, Element.ALIGN_LEFT, Element.ALIGN_TOP, 16), "Company", CellBackColorGrey));

            table.AddCell(AddCellWithMultipleContent(new CellRowSettings(1, 0, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0), _bodyContent.BillToContent));
            table.AddCell(AddCellWithMultipleContent(new CellRowSettings(1, 0, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0)));
            table.AddCell(AddCellWithMultipleContent(new CellRowSettings(1, 0, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0), _bodyContent.LicenseeContent));
            _document.Add(table);

            BodyReferenceTable(_document);

            foreach (var item in _bodyContent.BodySubContent)
                BodyItemsTable(_document, item.ImagePath, item.Text, item.Price);

            BodyTotalTable(_document, _totalPrice);
        }

        private void BodyReferenceTable(Document doc)
        {
            var table = TableSetUp(1);
            table.SetWidths(new float[] { 100 });
            table.KeepTogether = true;

            table.AddCell(EditBorders(AddCell(new CellRowSettings(1, 0, Element.ALIGN_LEFT, Element.ALIGN_TOP, 16), "Comment", CellBackColorGrey)));

            table.AddCell(AddCellWithMultipleContent(new CellRowSettings(1, 0, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0), _bodyContent.CommentContent));
            table.AddCell(AddCellWithMultipleContent(new CellRowSettings(1, 0, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0)));
            doc.Add(table);
        }

        private void BodyItemsTable(Document doc, string imagePath, List<string> testList, List<string> price)
        {
            Image image = Image.GetInstance(imagePath);
            image.ScaleToFitLineWhenOverflow = true;

            var table = TableSetUp(3);
            table.SetWidths(new float[] { 24, 58, 18 });
            table.KeepTogether = true;

            table.AddCell(AddCell(new CellRowSettings(1, 0, Element.ALIGN_LEFT, Element.ALIGN_TOP, 16), "Image", CellBackColorGrey));
            table.AddCell(AddCell(new CellRowSettings(1, 0, Element.ALIGN_LEFT, Element.ALIGN_TOP, 16), "Description", CellBackColorGrey));
            table.AddCell(AddCell(new CellRowSettings(1, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 16), "Amount", CellBackColorGrey));

            table.AddCell(AddCellWithMultipleContent(new CellRowSettings(2, 0, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0), image));

            foreach (var item in price)
                _totalPrice += Convert.ToDecimal(item.Remove(0, 1));

            table.AddCell(AddTextForCellRow(new CellRowSettings(1, 0, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0), testList[0]));
            table.AddCell(AddTextForCellRow(new CellRowSettings(1, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0), price[0]));

            if (price.Count > 1)
            {
                table.AddCell(AddTextForCellRow(
                    new CellRowSettings(1, 0, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0), testList[1]));
                table.AddCell(AddTextForCellRow(
                    new CellRowSettings(1, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0), price[1]));
            }

            doc.Add(table);
        }

        private void BodyTotalTable(Document doc, decimal totalPrice)
        {
            var table = TableSetUp(2);
            table.TotalWidth = (PageSize.Width - 52) / 4;
            table.SetWidths(new float[] { 15, 15 });
            table.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.KeepTogether = true;

            table.AddCell(AddCell(new CellRowSettings(1, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP, 16), "Total", CellBackColorGrey));
            table.AddCell(AddCell(new CellRowSettings(1, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP, 16), "$" + totalPrice.ToString(CultureInfo.InvariantCulture), CellBackColorGrey));
            doc.Add(table);
        }
    }
}
