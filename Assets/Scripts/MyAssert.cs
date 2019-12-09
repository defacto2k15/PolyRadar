using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace Assets.Scripts
{
    public static class MyAssert
    {
        public static void IsTrue(bool value)
        {
            if (!value)
            {
                ThrowException();
            }
        }

        private static void ThrowException()
        {
            throw new AssertionException("Assertion failure. Value was false", "");
        }
    }
}
