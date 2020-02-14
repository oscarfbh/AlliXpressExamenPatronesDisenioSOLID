# AlliXpressExamenPatronesDiseñoSOLID
 Examen de patrones de diseño y SOLID


Se aplicaron los patrones:
Abstract Factory: 
Este se ve en la implementación de la interfaz IEmpresaAbstractFactory que se aplicó a las empresas que a su vez llaman a los medios de transporte en los metodos de la clase.  El cliente ClienteFabricas es el que usa la fabrica (empresa) para que calcule en base de los productos (Medios transporte).

Chain of Responsabilities:
Se definió la interfaz IHandlerTime que define el método de responsabilidad y el Setter para saber si existe un proximo para el calculo de las expresiones en base al rango de tiempo.


Observer:
Se definio la interfaz ISuscriberFabricas (se aplico también sobre ClientesFabricas) para definir que cuando ocurre el calculo del costo por la empresa y medio igual verifique en las otras (observers) que verifiquen si su costo es menor implementando un mismo método.


Para el caso de agregar más transportes es necesario agregarlo como una nueva clase dentro de la carpeta "MediosTransportes" y heredar de la clase abstracta AbstractMedio  y definir sus valores de costo por km y velocidad de entrega en hrs que son los datos necesarios para sus cálculos y en la interfaz IEmpresaAbstractFactory agregar el nuevo método para calcular por el nuevo medio e implementar dicho método en las clases que implementan IEmpresaAbstractFactory. En el metodo EfectuarCalculos de la clase ClienteFabricas que implementa IClienteFabricas se debe añadir el nuevo medio de transporte y que llame el nuevo método de la interfaz IEmpresaAbstractFactory.

Para el caso de agregar una nueva empresa es necesario generar la nueva clase en la carpeta Empresas y que implemente la interfaz IEmpresaAbstractFactory asó como las dependencias de su constructor y constantes usadas en ese tipo de clases. Adicional en la clase ProcesadorArchivoPedidos modificar los métodos privados GenerarClientesFabricas y GenerarProcesadoresPedidos para incluir la nueva empresa y que ahora esté disponible en el sistema.