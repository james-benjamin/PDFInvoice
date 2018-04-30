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

        public PdfBodyContent(string imagePath, List<string> text, List<string> price)
        {
            ImagePath = imagePath;
            Text = text;
            Price = price;
        }
    }
}
