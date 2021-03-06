﻿using System.Linq;
using EasyAccess.Repository.Bootstrap.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyAccess.UnitTest.TestRepository
{
    [TestClass]
    public class EasyAccessInitializerTest
    {
        public static EasyAccessContext EasyAccessCtx = new EasyAccessContext();

        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            EasyAccessDatabaseInitializer.Initialize();
        }

        [TestMethod]
        public void TestSeed()
        {
            Assert.IsTrue(0 < EasyAccessCtx.Accounts.Count());
        }
    }
}
