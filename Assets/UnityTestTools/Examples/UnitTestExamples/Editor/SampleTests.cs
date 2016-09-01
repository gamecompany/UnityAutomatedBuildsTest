using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;

namespace UnityTest
{
    [TestFixture]
    [Category("Sample Tests")]
    internal class SampleTests
    {
        [Test]
        public void PassingTest()
        {
            Assert.Pass();
        }

        [Test]
        public void ParameterizedTest([Values(1, 2, 3)] int a)
        {
            Assert.Pass();
        }

        [Test]
        public void RangeTest([NUnit.Framework.Range(1, 10, 3)] int x)
        {
            Assert.Pass();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "expected message")]
        public void ExpectedExceptionTest()
        {
            throw new ArgumentException("expected message");
        }
    }
}
