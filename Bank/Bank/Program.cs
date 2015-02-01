using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using Common;

namespace Bank
{          
    class Program
    {
        static void Main(string[] args)
        {
            Uri uri = new Uri("net.tcp://localhost:2000/Bank");
            ServiceHost host = new ServiceHost(typeof(BankService), uri);
            try
            {
                NetTcpBinding tcpBinding = new NetTcpBinding();
                tcpBinding.Security.Mode = SecurityMode.None;

                host.AddServiceEndpoint(typeof(IBankService), tcpBinding, uri);

                ServiceMetadataBehavior smb = host.Description.Behaviors.Find<ServiceMetadataBehavior>();
                if (smb == null)
                {
                    smb = new ServiceMetadataBehavior();
                    host.Description.Behaviors.Add(smb);
                }
                smb.HttpGetEnabled = false;
                smb.HttpsGetEnabled = false;

                var mexBinding = MetadataExchangeBindings.CreateMexTcpBinding();
                host.AddServiceEndpoint(typeof(IMetadataExchange), mexBinding, "mex");

                // Open the service host to accept incoming calls
                host.Open();
                // The service can now be accessed.
                Console.WriteLine("The service is ready.");
                Console.WriteLine("Press <ENTER> to terminate service.");
                Console.WriteLine();
                Console.ReadLine();

                // Close the ServiceHostBase to shutdown the service.
                host.Close();
            }
            catch (CommunicationException commProblem)
            {
                Console.WriteLine("There was a communication problem. " + commProblem.Message);
                Console.Read();
            }
        }
    }
}
