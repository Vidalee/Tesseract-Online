using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tesseract_Online
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Init();
            Thread.Sleep(3000);
            //Logger.INFO("test");
            Console.ReadKey();
        }
    }
}
