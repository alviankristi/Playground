using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PG.Common.Extensions;

namespace PG.CommonTest
{
    [TestClass]
    public class ExceptionTest
    {
        private const string InnerExceptionMessage = "Inner Exception";
        private const string ExceptionMessage = "Exception Message";

        [TestMethod]
        public void GetLastInnerExceptionMessage_ReturnInnerException()
        {
            //arrange

            //act
            var exceptionModel = new System.Exception(ExceptionMessage, new System.Exception(InnerExceptionMessage)).GetLastInnerExceptionMessage();

            //assert
            Assert.AreEqual(exceptionModel, InnerExceptionMessage);
        }

        [TestMethod]
        public void GetLastInnerExceptionMessage_InnerExceptionNull_ReturnInnerException()
        {
            //arrange

            //act
            var exceptionModel = new ArgumentException(ExceptionMessage).GetLastInnerExceptionMessage();

            //assert
            Assert.AreEqual(exceptionModel, ExceptionMessage);
        }
    }
}
