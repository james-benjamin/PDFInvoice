using System.Collections.Generic;

namespace PdfInvoice
{
    public class PdfBodyContent
    {
        public string Total;
        public string BillToContent;
        public string LicenseeContent;
        public string CommentContent;

        public List<PdfBodySubContent> BodySubContent = null;
    }

    public class PdfBodySubContent
    {
        public string ImagePath;
        public List<string> Text;
        public List<string> Price;
    }
}
