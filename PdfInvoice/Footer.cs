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

            Disclosure(FooterTable, new Font(Bftimes, 10, Font.NORMAL, BaseColor.BLACK));
            AccountInfo(FooterTable);
            TermsAndCondictions(FooterTable, new Font(Bftimes, 6, Font.NORMAL, BaseColor.BLACK));

            FooterTable.WriteSelectedRows(0, -1, 26, _document.GetBottom(0) + FooterTable.TotalHeight, _writer.DirectContent);
        }

        private void Disclosure(PdfPTable table, Font font)
        {
            Phrase payPhrase = new Phrase(new Chunk("\n", font));
            payPhrase.Add(new Chunk("", font).setLineHeight(13));

            table.AddCell(AddCell(new CellRowSettings(1, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0), payPhrase, CellBackColorWhite));
        }

        private void AccountInfo(PdfPTable table)
        {
            table.AddCell(AddCellWithMultipleContent(new CellRowSettings(1, 0, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0), "", new List<string>() { "", "", "", "", "", "" }));
            table.AddCell(AddCellWithMultipleContent(new CellRowSettings(1, 0, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0), "", new List<string>() { "", "", "", "", "" }));
        }

        private void TermsAndCondictions(PdfPTable table, Font font)
        {
            Phrase termsPhrase = new Phrase(new Chunk("\n\n\n", font).setLineHeight(10));
            termsPhrase.Add(new Chunk("n", font).setLineHeight(10));
            termsPhrase.Add(new Chunk("\n\n", font).setLineHeight(10));
            termsPhrase.Add(new Chunk("\n", font).setLineHeight(10));
            termsPhrase.Add(new Chunk("", font).setLineHeight(10));
            table.AddCell(AddCell(new CellRowSettings(0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0), termsPhrase, CellBackColorWhite));
        }
    }
}
