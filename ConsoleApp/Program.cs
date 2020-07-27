using System;
using System.Linq;

namespace ConsoleApp
{
    public static class Program
    {
        private static void Main()
        {
            var workload = new Workload.Workload();
            workload.Start();

            while (true)
            {
                Console.Write("Bit mask (i.e. 1001) or empty string to exit: ");
                var maskStr = Console.ReadLine();

                if (string.IsNullOrEmpty(maskStr))
                    break;

                workload.SetMask(maskStr.Select(ch => ch == '1'));
            }
        }
    }
}
