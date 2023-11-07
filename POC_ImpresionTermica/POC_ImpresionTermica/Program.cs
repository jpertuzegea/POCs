
using System.Drawing;
using System.Drawing.Printing;


// ---------------------------------------------------
// Se debe instalar la libreria System.Drawing.Common de nugget
// Para poder crear el documento printDocument e imprimir en cualquier impresora que este instalada
// en este caso, es para imprimir con impresora de tiket o factura (Termica)
// ---------------------------------------------------


// *******************************
ImprimirDocument();
// *******************************



void ImprimirDocument()
{
    try
    {

        PrintDocument PrintDocument = new PrintDocument();
        PrinterSettings printerSettings = new PrinterSettings();

        // ************* Obtiene el nombre de todas las impresoras instaladas ************* 
        List<string> ListPrintersInstalled = ObtenerImpresorasInstaladas();
        // ************* Obtiene el nombre de todas las impresoras instaladas ************* 

        // ----------------------------------------
        // EL nombre de la impresora puede tomarse de la lista de impresoras instaladas (Linea Anterior)
        // "SLK-TS100" -- Impresota termica de tikets que dio Infotrans para pruebas
        // "Microsoft Print to PDF"
        // "Microsoft XPS Document Writer"
        printerSettings.PrinterName = "Microsoft Print to PDF";// "SLK-TS100";
        // ----------------------------------------

        PrintDocument.PrinterSettings = printerSettings;

        PrintDocument.PrintPage += ImprimirVector;
        PrintDocument.Print();

    }
    catch (Exception error)
    {

    }

}

void ImprimirVector(object sender, PrintPageEventArgs e)
{
    int cont_Y = 1;
    bool Cufe = false;

    // Create a StringFormat object with the each line of text, and the block
    // of text centered on the page.
    StringFormat stringFormat = new StringFormat();
    stringFormat.Alignment = StringAlignment.Center;
    stringFormat.LineAlignment = StringAlignment.Center;

    StringFormat stringFormatDerecha = new StringFormat();
    stringFormatDerecha.Alignment = StringAlignment.Far;
    stringFormatDerecha.LineAlignment = StringAlignment.Far;


    e.Graphics.DrawString("Nombre: Jorge Pertuz Egea",
               new System.Drawing.Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point),
               Brushes.Black, new System.Drawing.Rectangle(0, cont_Y, 300, 50), stringFormat);// stringFormat Centra el tetxto

    cont_Y += 35;

    e.Graphics.DrawString("Profesion: Ingeniero de Sistemas",
                new System.Drawing.Font("Arial", 8, FontStyle.Regular, GraphicsUnit.Point),
                Brushes.Black, new System.Drawing.Rectangle(0, cont_Y, 300, 50), stringFormat);// stringFormat Centra el tetxto
    cont_Y += 35;

    e.Graphics.DrawString("PosGrado: Ingenieria de Software",
        new System.Drawing.Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point),
        Brushes.Black, new System.Drawing.Rectangle(0, cont_Y, 300, 50));// ---- Sin stringFormat se coloca a la izquierda el texto

    cont_Y += 15;

    e.Graphics.DrawString("Año Graduacion: 2018",
              new System.Drawing.Font("Arial", 9, FontStyle.Regular, GraphicsUnit.Point),
              Brushes.Black, new System.Drawing.Rectangle(0, cont_Y, 300, 50));// ---- Sin stringFormat se coloca a la izquierda el texto

    cont_Y += 35;

}

List<string> ObtenerImpresorasInstaladas()
{
    string PkInstalatedPrinter;
    List<string> ListPrinters = new List<string>();

    string list = "";

    for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
    {
        PkInstalatedPrinter = PrinterSettings.InstalledPrinters[i];
        ListPrinters.Add(PkInstalatedPrinter);
        list = list + "[" + PkInstalatedPrinter + "] ";
    }
    return ListPrinters;
}


