using Attributes;
using System;

namespace SuccessTests
{
    public class SuccessTests
    {
        /*
        [BeforeClass]
        public static void BeforeClassTest()
        {
            int number = 30;
            number /= 30;
        }

        [Before]
        public void BeforeTest()
        {
            int i = 0;
            i *= 30;
        }
        */

        [Test]
        public static void RandomTest1()
        {
        //    var array = new int[30];
        //    array[30]++;
            int number = 30;
            number += 239;
        }

        /*
        [After]
        public void AfterTest()
        {
            int i = 30;
            i /= 30;
        }

        [AfterClass]
        public static void AfterClassTest()
        {
            int i = 8;
            i += 7;
        }
        */
    }
}
