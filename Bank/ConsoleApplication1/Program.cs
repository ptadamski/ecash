﻿using Common;
using Sklep.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Sklep
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(ShopService));
            host.Open();
            foreach (var item in host.BaseAddresses)
            {
                Console.WriteLine(item);
            }
        }
    }
}
