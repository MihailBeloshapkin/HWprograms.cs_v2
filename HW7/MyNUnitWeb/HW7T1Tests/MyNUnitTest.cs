using NUnit.Framework;
using System.Collections.Generic;
using System;
using HW5T1;

namespace HW5T1Tests
{
    public class MyNUnitTest
    {
        private HW5T1.MyNUnit nunit;

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SetOfSuccessCaseTest()
        {
            nunit = new HW5T1.MyNUnit("../../../../SuccessTests");
            nunit.Execute();
            var allData = nunit.GetAllData();
            Assert.AreEqual(1, allData.Count);
            Assert.IsTrue(String.Equals(allData[0].Name, "RandomTest1") &&
                          String.Equals(allData[0].Result, "Success") &&
                          String.Equals(allData[0].WhyIgnored, null) &&
                          allData[0].TimeOfExecution == 0);
        }
    }
}