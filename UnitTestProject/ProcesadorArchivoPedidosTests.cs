using AppAlliExpressRastreoPaquetes;
using AppAlliExpressRastreoPaquetes.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;

namespace UnitTestProject
{
    [TestClass]
    public class ProcesadorArchivoPedidosTests
    {
        private Mock<IClock> _clockMock;
        private string _path;
        private string _fileName;
        private Mock<IFileExistValidator> _fileExistValidatorMock;
        private Mock<IFileDataReader> _fileDataReaderMock;
        private DateTime _tiempoApp;
        private DateTime _dateTimeNow;
        private string[] _filasPedidos;

        private ProcesadorArchivoPedidos _procesador;
        private Mock<ProcesadorArchivoPedidos> _procesadorMock;

        [TestInitialize]
        public void OnSetup()
        {
            DateTime.TryParse("06/02/2020 09:00", out _tiempoApp);
            _dateTimeNow = _tiempoApp;
            _filasPedidos = new string[1];
            _path = "f:\\somedirectory";
            _fileName = "xfile.csv";
            _clockMock = new Mock<IClock>();
            _clockMock.Setup(m => m.GetTime()).Returns(_dateTimeNow);
            _fileExistValidatorMock = new Mock<IFileExistValidator>();
            _fileExistValidatorMock.Setup(m => m.ValidateFileExist(It.IsAny<string>(), It.IsAny<string>())).Returns("");
            _fileDataReaderMock = new Mock<IFileDataReader>();
            _fileDataReaderMock.Setup(m => m.GetFileDataRows(It.IsAny<string>(), It.IsAny<string>())).Returns(_filasPedidos);

            _procesador = new ProcesadorArchivoPedidos(_path, _fileName, _fileExistValidatorMock.Object, _fileDataReaderMock.Object, _clockMock.Object);
            _procesadorMock = new Mock<ProcesadorArchivoPedidos>(_path, _fileName, _fileExistValidatorMock.Object, _fileDataReaderMock.Object, _clockMock.Object) { CallBase = true };
            _procesadorMock.Protected().Setup<bool>("LlamarMetodoConsoleMethods", ItExpr.IsAny<string>()).Returns(true);
            _procesadorMock.Protected().Setup<bool>("LlamarImprimirMensajesPedidoProcesadorPedido", ItExpr.IsAny<IProcesadorPedidos>()).Returns(true);            
        }

        [TestMethod]
        public void ProcesarArchivo_Method_Should_Call_LlamarMetodoConsoleMethods_Method_When_Exception_By_File_Validator_Ocurrs()
        {
            //Arrange
            _fileExistValidatorMock.Setup(m => m.ValidateFileExist(It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception("AlgoOcurrio"));

            //Act
            _procesadorMock.Object.ProcesarArchivo();

            //Assert
            _procesadorMock.Protected().Verify<bool>("LlamarMetodoConsoleMethods", Times.Once(),  ItExpr.IsAny<string>());
        }

        [TestMethod]
        public void ProcesarArchivo_Method_Should_Call_LlamarMetodoConsoleMethods_Method_When_Exception_By_DataReader_Ocurrs()
        {
            //Arrange
            _fileDataReaderMock.Setup(m => m.GetFileDataRows(It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception("AlgoOcurrio"));

            //Act
            _procesadorMock.Object.ProcesarArchivo();

            //Assert
            _procesadorMock.Protected().Verify<bool>("LlamarMetodoConsoleMethods", Times.Once(), ItExpr.IsAny<string>());
        }

        [TestMethod]
        public void ProcesarArchivo_Method_Should_Call_LlamarMetodoConsoleMethods_Method_When_Validate_File_Returns_Error()
        {
            //Arrange
            _fileExistValidatorMock.Setup(m => m.ValidateFileExist(It.IsAny<string>(), It.IsAny<string>())).Returns("El archivo o directorio no existen");

            //Act
            _procesadorMock.Object.ProcesarArchivo();

            //Assert
            _procesadorMock.Protected().Verify<bool>("LlamarMetodoConsoleMethods", Times.Once(), ItExpr.IsAny<string>());
        }

        [TestMethod]
        [DataRow("")]
        [DataRow("algo")]        
        [DataRow("algo,algo")]
        [DataRow("algo,algo,algo")]
        [DataRow("algo,algo,algo,algo")]
        [DataRow("algo,algo,algo,algo,algo")]
        [DataRow("algo,algo,algo,algo,algo,algo")]
        public void ProcesarArchivo_Method_Should_Call_LlamarMetodoConsoleMethods_Method_When_ObtenerParametrosPedido_Method_Throws_Error_By_Number_Columns(string lineaPedido)
        {
            //Arrange
            _filasPedidos[0] = lineaPedido;
            //Act
            _procesadorMock.Object.ProcesarArchivo();

            //Assert
            _procesadorMock.Protected().Verify<bool>("LlamarMetodoConsoleMethods", Times.Once(), ItExpr.IsAny<string>());            
        }

        [TestMethod]        
        [DataRow("algo,algo,algo,algo,algo,algo")]
        [DataRow("algo,algo,01/01/2020,algo,algo,algo")]
        public void ProcesarArchivo_Method_Should_Call_LlamarMetodoConsoleMethods_Method_When_ObtenerParametrosPedido_Method_Throws_Error_By_Distance_Value_Not_Valid(string lineaPedido)
        {
            //Arrange
            _filasPedidos[0] = lineaPedido;

            //Act
            _procesadorMock.Object.ProcesarArchivo();

            //Assert
            _procesadorMock.Protected().Verify<bool>("LlamarMetodoConsoleMethods", Times.Once(), ItExpr.IsAny<string>());
        }

        [TestMethod]
        [DataRow("algo,algo,0,algo,algo,algo")]
        [DataRow("algo,algo,-20,algo,algo,algo")]
        [DataRow("algo,algo,-20.20,algo,algo,algo")]
        public void ProcesarArchivo_Method_Should_Call_LlamarMetodoConsoleMethods_Method_When_ObtenerParametrosPedido_Method_Throws_Error_By_Distance_Value_LessEqual_Zero(string lineaPedido)
        {
            //Arrange
            _filasPedidos[0] = lineaPedido;

            //Act
            _procesadorMock.Object.ProcesarArchivo();

            //Assert
            _procesadorMock.Protected().Verify<bool>("LlamarMetodoConsoleMethods", Times.Once(), ItExpr.IsAny<string>());
        }

        [TestMethod]
        [DataRow("algo,algo,1,algo,algo,155/02/2020")]
        [DataRow("algo,algo,1,algo,algo,15/020/2020")]
        [DataRow("algo,algo,1,algo,algo,15/02/-2020")]
        [DataRow("algo,algo,0.025,algo,algo,15/02/2020 25:15:30")]
        [DataRow("algo,algo,200,algo,algo,15/02/2020 -00:00:30")]
        [DataRow("algo,algo,200,algo,algo,15/02/2020 -00:10:30")]
        [DataRow("algo,algo,200,algo,algo,15/02/2020 -21:10:30")]
        [DataRow("algo,algo,0.025,algo,algo,15/02/2020 21:21:30 AM")]
        [DataRow("algo,algo,0.025,algo,algo,15/02/2020 09:21:30 PM")]
        public void ProcesarArchivo_Method_Should_Call_LlamarMetodoConsoleMethods_Method_When_ObtenerParametrosPedido_Method_Throws_Error_By_Date_Value_Not_Valid(string lineaPedido)
        {
            //Arrange
            _filasPedidos[0] = lineaPedido;

            //Act
            _procesadorMock.Object.ProcesarArchivo();

            //Assert
            _procesadorMock.Protected().Verify<bool>("LlamarMetodoConsoleMethods", Times.Once(), ItExpr.IsAny<string>());
        }

        
        [TestMethod]
        //Esta prueba funciona con el codigo actual. Si se añaden nuevas empresas se debe actualizar la prueba para añadir la nueva empresa y ver que pase tambien la prueba.
        [DataRow("algo,algo,1,dhl,avion,14/02/2020")]
        [DataRow("algo,algo,1,DHL,Avion,14/02/2020 09:10:11")]
        [DataRow("algo,algo,1,Estafeta,Barco,14/02/2020 21:10:11")]
        [DataRow("algo,algo,1,estafeTA, barcO ,14/02/2020 09:10:11 AM")]
        [DataRow("algo,algo,1, feDeX,treN,14/02/2020 09:10:11 PM")]
        [DataRow("algo,algo,1,FeDeX , TreN,14/02/2020 09:10:11 PM")]
        public void ProcesarArchivo_Method_Should_Call_LlamarImprimirMensajesPedidoProcesadorPedido_Method_When_All_Data_Is_Correct(string lineaPedido)
        {
            //Arrange
            _filasPedidos[0] = lineaPedido;
            //Act
            _procesadorMock.Object.ProcesarArchivo();

            //Assert
            _procesadorMock.Protected().Verify<bool>("LlamarImprimirMensajesPedidoProcesadorPedido", Times.Once(), ItExpr.IsAny<IProcesadorPedidos>());
        }
    }
}
