﻿using System;
using EasyAccess.Model.EDMs;
using EasyAccess.Model.VOs;
using EasyAccess.UnitTest.Bootstrap;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyAccess.UnitTest.TestInfrastructure.TestCustomTimestamp
{
    [TestClass]
    public class TestCustomTimestamp: SpringTestBase
    {
        [TestMethod]
        public void TestUpdateMode_Equal()
        {
            var account = Account.FindById(1);
            account.Age = 25;
            account.Sex = Sex.Female;
            account.RowVersion = 1;
            Account.Update(account);
            EasyAccessUnitOfWork.Commit();

            Assert.AreEqual(25, account.Age);
        }
    }
}
