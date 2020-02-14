using AppAlliExpressRastreoPaquetes.Interfaces;
using System;
using System.IO;

namespace AppAlliExpressRastreoPaquetes.AdministradoresArchivo
{
    public class FileDataReader : IFileDataReader
    {
        /// <summary>
        /// Obtiene la información del archivo en el path dado y devuelve un arreglo con los valores de las filas dentro del archivo.
        /// </summary>
        /// <param name="path">La ruta o directorio del archivo.</param>
        /// <param name="fileName">El nombre del archivo.</param>
        /// <returns>Arreglo con los valores de las filas dentro del archivo.</returns>
        public virtual string[] GetFileDataRows(string path, string fileName)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new Exception("La variable 'path' no puede ser nula o vacía");
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new Exception("La variable 'fileName' no puede ser nula o vacía");
            }

            string dataFile = string.Format("{0}\\{1}", path, fileName);
            return CallReadAllLinesMethod(dataFile);
        }

        /// <summary>
        /// Método que llama al método estático de lectura de archivos para devolver el resultado del archivo.
        /// </summary>
        /// <param name="dataFile">La ruta del archivo.</param>
        /// <returns>Arreglo de cadenas con la información de las filas del archivo.</returns>
        protected virtual string[] CallReadAllLinesMethod(string dataFile)
        {
            //No se valida la existencia del archivo ya que eso lo debió hacer la clase FileEventDateValidator
            return File.ReadAllLines(dataFile);
        }
    }
}
