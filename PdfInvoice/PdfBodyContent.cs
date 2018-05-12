using System.Collections.Generic;

namespace PdfInvoice
{
    public class PdfBodyContent
    {
        public string ImagePath;
        public List<string> Text;
        public List<string> Price;

        public PdfBodyContent()
        { }
    }
}
