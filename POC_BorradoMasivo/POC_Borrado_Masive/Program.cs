using Microsoft.EntityFrameworkCore;
using POC_BorradoMasivo;
using POC_BorradoMasivo.Entity;
using System.Diagnostics;

List<Departament> ListDepartament = new List<Departament>();

int TotalInsertMasive = 2;
int cont = 0;

try
{
    Stopwatch stopWatch = new Stopwatch();
    stopWatch.Start();

    // _____________________________________________________
    ArmarBD();
    // BorradoMasivo();
    // _____________________________________________________

    stopWatch.Stop();

    TimeSpan ts = stopWatch.Elapsed;
    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

    // _____________________________________________________
    Console.WriteLine("_____________________________________________________");
    Console.WriteLine();
    Console.WriteLine("Tiempo Total : " + elapsedTime);
    Console.WriteLine("_____________________________________________________");
    // _____________________________________________________
}
catch (Exception)
{
    throw;
}


void ArmarBD()
{
    LeerLocal();

    for (int i = 0; i < TotalInsertMasive; i++)
    {
        InsertarMasivoLocal();
    }
}

void LeerLocal()
{
    var ContextDB = new ContextDB();
    ListDepartament = ContextDB.Departament.ToList();
}

void BorradoMasivo()
{
    var ContextDB = new ContextDB();
    ContextDB.Departament.Take(1000).ExecuteDelete();
}





void InsertarMasivoLocal()
{
    Stopwatch stopWatch = new Stopwatch();
    stopWatch.Start();

    ListDepartament.AsParallel()
            .WithDegreeOfParallelism(10) // Establecemos el número de hilos que ejecutarán las tareas en paralelo
            .ForAll(x =>
            {
                // Creamos un nuevo contexto para cada hilo para evitar problemas de concurrencia
                using (var dbContext = new ContextDB())
                {
                    Departament Departament = new Departament();
                    Departament.Name = x.Name;
                    Departament.State = x.State;

                    dbContext.Departament.Add(Departament);
                    dbContext.SaveChanges(); // Guardamos en la base de datos de manera individual por hilo
                    cont = cont + 1;
                    Console.WriteLine($"Registro Guardado: {cont}");
                }
            });

    stopWatch.Stop();

    TimeSpan ts = stopWatch.Elapsed;
    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

    // _____________________________________________________
    Console.WriteLine("_____________________________________________________");
    Console.WriteLine();
    Console.WriteLine("Tiempo Total : " + elapsedTime);
    Console.WriteLine("_____________________________________________________");


    //foreach (var item in ListDepartament)
    //{
    //    Departament itemDepartament = new Departament();
    //    itemDepartament.Name = item.Name;
    //    itemDepartament.State = item.State;

    //    ContextDB.Departament.Add(itemDepartament);
    //    ContextDB.SaveChanges();
    //    cont = cont + 1;
    //    Console.WriteLine("Registros Guardados : " + cont);
    //}

}
