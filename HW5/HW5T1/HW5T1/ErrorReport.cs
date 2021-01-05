using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Text;

namespace HW5T1
{
    public static class ErrorReport
    {
        public static async Task WriteErrors(IEnumerable<InvalidTestClassReport> reports, TextWriter writer)
        {
            foreach (var report in reports)
            {
                await writer.WriteLineAsync($"{report.Name}");
                
                foreach (var method in report.InvalidMethods)
                {
                    await writer.WriteLineAsync($"{method.Name}:");

                    foreach (var error in method.Errors)
                    {
                        await writer.WriteLineAsync($"{error}");
                    }
                }
            }
        }
    }
}
