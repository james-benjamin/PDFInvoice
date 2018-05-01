using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfInvoice
{
    public class Pdf : PdfSettings
    {
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
    }
}
