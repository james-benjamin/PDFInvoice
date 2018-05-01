using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfInvoice
{
    public class Footer : PdfSettings
    {
        private static PdfPTable FooterTable { get; set; }
        
        public void CreateFooter(Document doc, PdfWriter writer)
        {
            doc.NewPage();
            FooterTable = TableSetUp(2);
            FooterTable.SpacingBefore = 0;
            FooterTable.HorizontalAlignment = Element.ALIGN_LEFT;
            FooterTable.SetWidths(new float[] { 15, 45 });

            Disclosure(FooterTable, new Font(Bftimes, 10, Font.NORMAL, BaseColor.BLACK));
            AccountInfo(FooterTable);
            TermsAndCondictions(FooterTable, new Font(Bftimes, 6, Font.NORMAL, BaseColor.BLACK));

            FooterTable.WriteSelectedRows(0, -1, 26, doc.GetBottom(0) + FooterTable.TotalHeight, writer.DirectContent);
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
