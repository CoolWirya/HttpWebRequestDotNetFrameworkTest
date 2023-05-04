using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace apitest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string msisdn = "252659000707";
            api api = new api();
            api.HLRLogin("");
            api.HLRLocation(msisdn, "");
            string output = api.HLRLogout("");
            Console.WriteLine(output);
            Console.ReadLine();


           

        }


     


    }
}
