using System;
using Demo.UpdateVersion.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        const String Filepath = @"D:\Projects\Magelia\99 Versioning\Demos.Update\Demoe.UpdateVersion\Demoe.UpdateVersion\Calendar.xamlx";

        [TestMethod]
        public void MarkToUpdateTest()
        {
            Updater.MarkToUpdate(Filepath);

        }
    }
}
