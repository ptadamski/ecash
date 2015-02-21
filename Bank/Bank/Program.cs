using Bank.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Bank
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                                                                                   
                System.IO.StreamReader prv = new System.IO.StreamReader("rsa.prv");
                Bank.Model.Bank.PrivateKey = prv.ReadToEnd().ToRsaPrivateKey();

                System.IO.StreamReader pub = new System.IO.StreamReader("rsa.pub");  
                Bank.Model.Bank.PublicKey = pub.ReadToEnd().ToRsaPublicKey();

                ServiceHost host = new ServiceHost(typeof(BankService));
                host.Open();
                foreach (var item in host.BaseAddresses)
                {                                    
                    Console.WriteLine(item);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.ReadLine();
        }
    }
}
