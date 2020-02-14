namespace AppAlliExpressRastreoPaquetes.Interfaces
{
    public interface IFileDataReader
    {
        /// <summary>
        /// Obtiene la información del archivo en el path dado y devuelve un arreglo con los valores de las filas dentro del archivo.
        /// </summary>
        /// <param name="path">La ruta o directorio del archivo.</param>
        /// <param name="fileName">El nombre del archivo.</param>
        string[] GetFileDataRows(string path, string fileName);
    }
}
