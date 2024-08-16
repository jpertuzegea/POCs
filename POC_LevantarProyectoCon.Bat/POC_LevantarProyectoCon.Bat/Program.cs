/*
 * Para ejecutar un proyecto desde un .Bat, 
 * se debe crear un archivo .Bat y copiar el siguiente Codigo.
 
********************************************************************* 
:: Este bat permite tomar los fuentes de un proyecto c# de una ruta y copiarlos en otra ruta diferente
:: para luego ejecutar el aplicativo desde la ruta final.
:: este aplicativo se levanta a traves de la consola.
:: En caso de ser solicitado en la consola, presionar la letra D --> Directorio

title Levantar Proyecto con .Bat

:: Ruta Base Destino donde se copian los fuentes del proyecto
set BASE_DESTINATION_PATH=%userprofile%\documents\POCS\Jorge

:: Ruta Destino completa
set DESTINATION="%BASE_DESTINATION_PATH%\Prueba"

:: Ruta completa origen de los fuentes C#
set ORIGIN="C:\Users\Jorge.Pertuz\Documents\Proyectos - Jorge Pertuz Egea\POCs\POC_LevantarProyectoCon.Bat\POC_LevantarProyectoCon.Bat\bin\Debug\net8.0"

:: Elimina la ruta destino en caso de existir 
del /s /q %DESTINATION%

:: Copia los fuentes del proyecto de la carpeta origen a la carpeta destino
xcopy %ORIGIN% %DESTINATION% /e /y

:: se ubica en la ruta destino donde se encuentran ya los fuentes copiados 
cd %DESTINATION%

:: Se ejecuta el proyecto y se le da un puerto por el cual levantar en caso de necesitar un puerto 
call dotnet POC_LevantarProyectoCon.Bat.dll :: --urls "http://*:5005"
*********************************************************************
  
 */

Console.WriteLine("");
Console.WriteLine("");
Console.WriteLine("");

Console.WriteLine("***********************************************");
Console.WriteLine("Este programa se levanta desde un archivo .Bat ");
Console.WriteLine("***********************************************");
 
Console.WriteLine("");
Console.WriteLine("Presione una tecla para continuar...");

Console.ReadKey();



