using System;
using System.Threading.Tasks;
using System.IO;

namespace HW5T1
{
    class Program
    {
        static async Task Main(string[] args)
        {/*
            if (args.Length != 1)
            {
                throw new ArgumentException();
            }*/
            try
            {
                var nunit = new MyNUnit("../../../../SuccessTests");
                nunit.Execute();
                nunit.DisplayResults();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }
    }
}
