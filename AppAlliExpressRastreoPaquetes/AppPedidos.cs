using AppAlliExpressRastreoPaquetes.AdministradoresArchivo;
using AppAlliExpressRastreoPaquetes.Interfaces;
using System;
using System.IO;

namespace AppAlliExpressRastreoPaquetes
{
    class AppPedidos
    {
        static void Main(string[] args)
        {
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(Directory.GetParent(workingDirectory).Parent.FullName).FullName;
            string fileName = "Pedidos.csv";
            IFileExistValidator fileExistValidator = new FileExistValidator();
            IFileDataReader fileDataReader = new FileDataReader();
            IClock clock = new Clock();

            ProcesadorArchivoPedidos procesadorArchivoPedidos =
                new ProcesadorArchivoPedidos(projectDirectory, fileName, fileExistValidator, fileDataReader, clock);
            procesadorArchivoPedidos.ProcesarArchivo();
        }

        
    }
}
