using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfInvoice
{
    public class Pdf : PdfSettings
    {
        private readonly MemoryStream _stream = new MemoryStream();

        private PdfHeaderContent _headerContent;
        public PdfHeaderContent HeaderContent
        {
            get { return _headerContent; }
            set { _headerContent = value; }
        }

        private PdfBodyContent _bodyContent;
        public PdfBodyContent BodyContent
        {
            get { return _bodyContent; }
            set { _bodyContent = value; }
        }

        private PdfFooterContent _footerContent;
        public PdfFooterContent FooterContent
        {
            get { return _footerContent; }
            set { _footerContent = value; }
        }

        public Pdf(PdfHeaderContent headerContent, PdfBodyContent bodyContent, PdfFooterContent footerContent)
        {
            _headerContent = headerContent;
            _bodyContent = bodyContent;
            _footerContent = footerContent;
        }

        public byte[] CreatePdf()
        {
            using (Document doc = new Document(PageSize, MarginLeft, MarginRight, MarginTop, MarginBottom))
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, _stream);
                writer.CloseStream = false;

                doc.Open();

                new Header(doc, _headerContent).CreateHeader();
                new Body(doc, _bodyContent).CreateBody();
                new Footer(doc, _footerContent, writer).CreateFooter();
            }

            return _stream.ToArray();
        }
    }
}
