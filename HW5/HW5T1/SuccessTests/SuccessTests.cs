using Attributes;
using System;

namespace SuccessTests
{
    public class SuccessTests
    {
        [TestAttribute]
        public void BeforeClass()
        {
            int number = 30;
            number += 239;
        }
    }
}
