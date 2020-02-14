using AppAlliExpressRastreoPaquetes.AdministradoresArchivo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;
using System.Linq;

namespace UnitTestProject.AdministradoresArchivo
{
    [TestClass]
    public class FileDataReaderTests
    {
        private FileDataReader _fileDataReader;
        [TestInitialize]
        public void OnSetup()
        {
            _fileDataReader = new FileDataReader();
        }

        [TestMethod()]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("     ")]
        public void GetFileDataRows_Method_Should_Return_Path_Exception(string path)
        {
            //Arrange
            string fileName = "somefile.txt";

            //Act
            Exception exception = Assert.ThrowsException<Exception>(() => _fileDataReader.GetFileDataRows(path, fileName));

            //Assert
            Assert.AreEqual("La variable 'path' no puede ser nula o vacía", exception.Message);
        }

        [TestMethod()]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("     ")]
        public void GetFileDataRows_Method_Should_Return_FileName_Exception(string fileName)
        {
            //Arrange
            string path = "D:\\SomeDirectory";

            //Act
            Exception exception = Assert.ThrowsException<Exception>(() => _fileDataReader.GetFileDataRows(path, fileName));

            //Assert
            Assert.AreEqual("La variable 'fileName' no puede ser nula o vacía", exception.Message);
        }

        [TestMethod()]
        public void GetFileDataRows_Method_Should_Return_DataRows_Correctly_From_File()
        {
            //Arrange
            string path = "D:\\SomeDirectory";
            string fileName = "someFile.txt";
            string[] dataRows = new string[2];
            dataRows[0] = "value1";
            dataRows[1] = "value2";
            //Uso necesario del mock parcial debido a que se usa una clase estática que supone que debe leer el archivo y regresar sus datos.
            Mock<FileDataReader> FileDataReaderMock = new Mock<FileDataReader>() { CallBase = true };
            FileDataReaderMock.Protected().Setup<string[]>("CallReadAllLinesMethod", ItExpr.IsAny<string>())
                .Returns(dataRows);

            //Act
            string[] result = FileDataReaderMock.Object.GetFileDataRows(path, fileName);

            //Assert
            FileDataReaderMock.Protected().Verify<string[]>("CallReadAllLinesMethod", Times.Once(), ItExpr.IsAny<string>());
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(dataRows[0], result[0]);
            Assert.AreEqual(dataRows[1], result[1]);
        }
    }
}
