using Attributes;
using System;

namespace SuccessTests
{
    public class SuccessTests
    {
        [Test]
        public void BeforeClass()
        {
            int number = 30;
            number += 239;
        }
    }
}
