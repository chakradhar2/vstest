﻿
namespace TestPlatform.CrossPlatEngine.UnitTests.DataCollection
{
    using System;

    using Castle.DynamicProxy.Generators.Emitters;

    using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TestPlatform.CrossPlatEngine.UnitTests.Discovery;

    [TestClass]
    public class InProcDataCollectionSinkTests
    {
        private IDataCollectionSink dataCollectionSink;

        private DataCollectionContext dataCollectionContext;

        private TestCase testCase;

        [TestInitialize]
        public void InitializeTest()
        {
            this.dataCollectionSink = new InProcDataCollectionSink();
            this.testCase = new TestCase("DummyNS.DummyC.DummyM", new Uri("executor://mstest/v1"), "Dummy.dll");
            this.dataCollectionContext = new DataCollectionContext(this.testCase);
        }

        [TestMethod]
        public void SendDataShouldAddKeyValueToDictionaryInSink()
        {
            this.testCase.SetPropertyValue(TestCaseProperties.Id, Guid.NewGuid());
            this.dataCollectionSink.SendData(this.dataCollectionContext, "DummyKey", "DummyValue");            

            var dict = ((InProcDataCollectionSink)this.dataCollectionSink).GetDataCollectionDataSetForTestCase(this.testCase.Id);

            Assert.AreEqual<string>(dict["DummyKey"].ToString(), "DummyValue");
        }

        [TestMethod]
        
        public void SendDataShouldThrowArgumentExceptionIfKeyIsNull()
        {
            this.testCase.SetPropertyValue(TestCaseProperties.Id, Guid.NewGuid());

            Assert.ThrowsException<ArgumentNullException>(
                () => this.dataCollectionSink.SendData(this.dataCollectionContext, null, "DummyValue"));
        }

        [TestMethod]
        public void SendDataShouldThrowArgumentExceptionIfValueIsNull()
        {
            this.testCase.SetPropertyValue(TestCaseProperties.Id, Guid.NewGuid());

            Assert.ThrowsException<ArgumentNullException>(
                () => this.dataCollectionSink.SendData(this.dataCollectionContext, "DummyKey", null));
        }

        [TestMethod]
        [Ignore]

        // TODO : Currently this code hits when test case id is null for core projects. For that we don't have algorithm to generate the guid. It's not implemented exception now (Source Code : EqtHash.cs).        
        public void SendDataShouldThrowArgumentExceptionIfTestCaseIdIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => this.dataCollectionSink.SendData(this.dataCollectionContext, "DummyKey", "DummyValue"));
        }

        [TestMethod]
        public void GetDataCollectionDataSetForTestCaseShouldRemoveTestCaseAfterRetunredTheData()
        {
            this.testCase.SetPropertyValue(TestCaseProperties.Id, Guid.NewGuid());
            this.dataCollectionSink.SendData(this.dataCollectionContext, "DummyKey", "DummyValue");

            var dict = ((InProcDataCollectionSink)this.dataCollectionSink).GetDataCollectionDataSetForTestCase(this.testCase.Id);
            Assert.AreEqual<string>(dict["DummyKey"].ToString(), "DummyValue");

            var emptyDict = ((InProcDataCollectionSink)this.dataCollectionSink).GetDataCollectionDataSetForTestCase(this.testCase.Id);
            Assert.AreEqual<int>(emptyDict.Keys.Count, 0);
        }
    }
}