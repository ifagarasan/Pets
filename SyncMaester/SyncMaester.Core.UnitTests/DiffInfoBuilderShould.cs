using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kore.IO.Scanners;
using Kore.IO.Sync;
using Kore.IO.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace SyncMaester.Core.UnitTests
{
    [TestClass]
    public class DiffInfoBuilderShould
    {
        [TestMethod]
        public void LeaveSourceAndDestinationUnalteredWhenLevelIsFlat()
        {
            IDiffInfoBuilder diffInfoBuilder = new DiffInfoBuilder();
        }
    }
}
