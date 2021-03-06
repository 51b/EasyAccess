﻿using System.Dynamic;
using System.Linq;
using EasyAccess.Infrastructure.UnitOfWork;
using EasyAccess.Repository.Repositories;
using EasyAccess.UnitTest.Bootstrap;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spring.Context;
using Spring.Context.Support;
using Spring.Testing.Microsoft;

namespace EasyAccess.UnitTest.TestInfrastructure.TestUnitOfWork
{
    [TestClass]
    public class UnitOfWorkBaseTest : SpringTestBase
    {
        [TestMethod]
        public void TestIoc()
        {
            Assert.IsNotNull(EasyAccessUnitOfWork);
        }
    }
}
