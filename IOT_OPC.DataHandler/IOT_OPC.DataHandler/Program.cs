using IIOT_OPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOT_OPC.DataHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            //polling on pieces number, call save but don't implement

            DatasHandler handler = new DatasHandler();

            //handler.DaServerMgt_DataChanged(1, true,true)
            handler.Connect();
            Console.ReadLine();
        }
    }
}
