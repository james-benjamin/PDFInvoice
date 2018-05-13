using System.Collections.Generic;
using System.Drawing.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfInvoice
{
    public class Footer : PdfSettings
    {
        private Document _document;
        private PdfFooterContent _footerContent;
        private PdfWriter _writer;

        private static PdfPTable FooterTable { get; set; }

        public Footer(Document doc, PdfFooterContent footerContent, PdfWriter writer)
        {
            _document = doc;
            _footerContent = footerContent;
            _writer = writer;
        }

        public void CreateFooter()
        {
            _document.NewPage();
            FooterTable = TableSetUp(2);
            FooterTable.SpacingBefore = 0;
            FooterTable.HorizontalAlignment = Element.ALIGN_LEFT;
            FooterTable.SetWidths(new float[] { 15, 45 });

            FooterInfo(FooterTable);

            FooterTable.WriteSelectedRows(0, -1, 26, _document.GetBottom(0) + FooterTable.TotalHeight, _writer.DirectContent);
        }

        private void FooterInfo(PdfPTable table)
        {
            table.AddCell(AddCellWithMultipleContent(new CellRowSettings(1, 0, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0), "", _footerContent.TestList ));
        }
    }
}
