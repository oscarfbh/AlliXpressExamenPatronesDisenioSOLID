using AppAlliExpressRastreoPaquetes.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;

namespace UnitTestProject
{
    [TestClass]
    public class ClockTests
    {
        private IClock _clock;
        private DateTime _dateTimeEvent;

        [TestInitialize]
        public void OnSetup()
        {
            DateTime.TryParse("06/02/2020 09:00", out _dateTimeEvent);
            _clock = new AppAlliExpressRastreoPaquetes.Clock();

        }

        [TestMethod]
        public void GetTime_Method_Should_Return_DateTimeNow_Greater_Than_DateTime_Provided()
        {
            //Arrange
            string dateTimeInPastCharacter = "2005-05-05 22:12 PM";
            DateTime dateTime = DateTime.ParseExact(dateTimeInPastCharacter, "yyyy-MM-dd HH:mm tt", CultureInfo.InvariantCulture);

            //Act
            DateTime clockResult = _clock.GetTime();

            //Assert
            Assert.IsTrue(dateTime < clockResult);
        }

    }
}
