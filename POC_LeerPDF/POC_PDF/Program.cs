// See https://aka.ms/new-console-template for more information

using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text;
 

// ----------------------------------------------------------------------------------
string ruta = @"C:\Users\jpertuz\Desktop\PDFs\Impresion 000000000006945.pdf";
string Result = parsePDFDocument(ruta);
LeerPDFYGuardarLineaPorLineaEnTXT(ruta);
// ----------------------------------------------------------------------------------

 
// Lee documento PDF linea por linea y lo guarda en un txt
void LeerPDFYGuardarLineaPorLineaEnTXT(string ruta)
{
    List<string> lineas = new List<string>();
    PdfReader reader = new PdfReader(ruta);

    int intPageNum = reader.NumberOfPages;
    string[] words;
    string linex;
    string text;

    for (int i = 1; i <= intPageNum; i++)
    {
        text = PdfTextExtractor.GetTextFromPage(reader, i, new LocationTextExtractionStrategy());

        words = text.Split('\n');

        for (int j = 0, len = words.Length; j < len; j++)
        {
            linex = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(words[j]));
            lineas.Add(linex);
        }
    }

    File.WriteAllLines(@"C:\Users\jpertuz\Desktop\PDFs\Temp\DatosSalida.txt", lineas);

}

// Lee todo el documento PDF en prosa
string parsePDFDocument(string filePath)
{
    using (PdfReader read = new PdfReader(filePath))
    {
        StringBuilder convertedText = new StringBuilder();

        for (int p = 1; p <= read.NumberOfPages; p++)
        {
            convertedText.Append(PdfTextExtractor.GetTextFromPage(read, p));
        }

        return convertedText.ToString();
    }
}
