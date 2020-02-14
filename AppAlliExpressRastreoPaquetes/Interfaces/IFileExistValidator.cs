namespace AppAlliExpressRastreoPaquetes.Interfaces
{
    public interface IFileExistValidator
    {
        /// <summary>
        /// Valida que exista el archivo en el directorio proporcionado. En caso de ser correcto regresa cadena vacía.
        /// En caso de algún error, regresa el mensaje del error detectado.
        /// </summary>
        /// <param name="path">Cadena representando la ruta del directorio donde se encuentra el archivo.</param>
        /// <param name="file">El nombre del archivo.</param>
        /// <returns>Cadena con el error detectado o en su caso cadena vacía si el directorio y archivo son correctos.</returns>
        string ValidateFileExist(string path, string fileName);
    }

}
