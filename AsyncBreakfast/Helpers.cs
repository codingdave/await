using System;
using System.Diagnostics;

namespace AsyncBreakfast
{
    internal static class Helpers
    {
        internal static void WriteLine(string t)
        {
            Console.WriteLine(t);
            Debug.WriteLine(t);
        }
    }
}
