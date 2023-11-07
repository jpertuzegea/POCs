

using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text;


// ----------------------------------------------------------------------------------
//     Para esta POC se utiliza la libreria iTextSharp Version 5.5.13.3 
// ----------------------------------------------------------------------------------
string PathOrigin = @"./PDFs"; // Se debe configuar los archivos copiar siemore en ruta cuando se compila (propiedades)
string PathDestinatary = @"./Procesados/";

string[] files = Directory.GetFiles(PathOrigin); // Obtener archivos 

foreach (string file in files)
{
    if (File.Exists(file))
    {
        // Lee PDF linea por lina y retorna un string
        string AllLinesDocument = ReadAllPDFDocumentLine_Line(file);

        // Mueve el archivo a la ruta Procesado
        var FileName = file.Replace(PathOrigin + "\\", "");
        File.Move(file, PathDestinatary + FileName);
    }
}


// ----------------------------------------------------------------------------------
// Lee todo el documento PDF en prosa
string ReadAllPDFDocumentLine_Line(string filePath)
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
