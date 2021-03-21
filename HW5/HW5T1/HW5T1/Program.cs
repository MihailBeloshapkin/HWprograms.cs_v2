using System;
using System.Threading.Tasks;
using System.IO;

namespace HW5T1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var nunit = new MyNUnit("../../../../SuccessTests");
                nunit.Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }
    }
}
