using AppAlliExpressRastreoPaquetes.Interfaces;
using System.IO;

namespace AppAlliExpressRastreoPaquetes.AdministradoresArchivo
{
    public class FileExistValidator : IFileExistValidator
    {
        public string ValidateFileExist(string path, string fileName)
        {
            string errorMessage = string.Empty;
            string fullPath;

            if (string.IsNullOrWhiteSpace(path))
            {
                errorMessage = "La ruta proporcionada del archivo no existe.";
            }
            else
            {
                fullPath = Path.GetFullPath(path);
                fileName = fileName != null ? fileName.Trim() : fileName;

                if (!CallDirectoryExistMethod(fullPath))
                {
                    errorMessage = string.Format("El directorio '{0}' no existe.", fullPath);
                }
                else if (string.IsNullOrWhiteSpace(fileName))
                {
                    errorMessage = "El nombre del archivo es incorrecto o vacío.";
                }
                else if (!CallFileExistMethod(string.Format("{0}\\{1}", fullPath, fileName.Trim())))
                {
                    errorMessage = string.Format("El archivo '{0}' no existe en la ruta '{1}'.", fileName, fullPath);
                }
            }

            return errorMessage;
        }


        /// <summary>
        /// Llama al método del directory del System.IO para verificar la existencia del directorio.
        /// </summary>
        /// <param name="fullPath">El directorio a verificar.</param>
        /// <returns>Valor true si existe o false si no existe.</returns>
        protected virtual bool CallDirectoryExistMethod(string fullPath)
        {
            return Directory.Exists(fullPath);
        }

        /// <summary>
        /// Llama al método del directory del System.IO para verificar la existencia del directorio.
        /// </summary>
        /// <param name="filePath">el archivo con la ruta del directorio a verificar.</param>
        /// <returns>Valor true si existe o false si no existe.</returns>
        protected virtual bool CallFileExistMethod(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}
