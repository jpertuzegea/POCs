// See https://aka.ms/new-console-template for more information
using Microsoft.Data.SqlClient;

Console.WriteLine("Hello, World!");

RealiceBackUpDataBase();



void RealiceBackUpDataBase()
{
    string CopyName = (DateTime.UtcNow).AddHours(-5).ToString("yyyy-MM-dd") + "qqqq.bak";
    string DataBase = "Jorge";
    string DestinationPath = "C:\\Jorge\\";

    var ConectionString = $"Server=localhost;Database={DataBase};Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
    SqlConnection conection = new SqlConnection(ConectionString);

    try
    {
        var Query = $"BACKUP DATABASE [{DataBase}] TO  DISK = N'{DestinationPath}{CopyName}' WITH NOFORMAT, NOINIT,  NAME = N'{DataBase}-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";

        SqlCommand cmd = new SqlCommand(Query, conection);

        conection.Open();
        cmd.ExecuteNonQuery();
        Console.WriteLine("====================================================");
        Console.WriteLine("BackUp Realizado con Exito");
        Console.WriteLine("====================================================");

    }
    catch (Exception error)
    {
        Console.WriteLine("****************************************************");
        Console.WriteLine("Error AL Realizar BackUp \n\n");
        Console.WriteLine("Error ==> " + error.ToString());
        Console.WriteLine("****************************************************");
    }
    finally
    {
        conection.Close();
        conection.Dispose();
    }
}