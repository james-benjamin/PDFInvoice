using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfInvoice
{
    public class Pdf : PdfSettings
    {
        private readonly MemoryStream _stream = new MemoryStream();

        public PdfHeaderContent HeaderContent = null;
        public List<PdfBodyContent> BodyContentsList = null;
        public PdfFooterContent FooterContent = null;

        public Pdf(PdfHeaderContent headerContent, List<PdfBodyContent> bodyContentsList, PdfFooterContent footerContent)
        {
            HeaderContent = headerContent;
            BodyContentsList = bodyContentsList;
            FooterContent = footerContent;
        }

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
    }
}
