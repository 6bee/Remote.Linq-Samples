﻿// Copyright (c) Christof Senn. All rights reserved. See license.txt in the project root for license information.

namespace DemoStartUp
{
    using System;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Description;

    class Program
    {
        static void Main(string[] args)
        {
            const string url = "net.pipe://localhost/8080/query";

            Console.WriteLine("Starting WCF service...");

            using (var wcfServiceHost = new ServiceHost(typeof(Server.QueryService)/*, new Uri("http://localhost:8090/query")*/))
            {
                wcfServiceHost.Description.Behaviors.OfType<ServiceDebugBehavior>().Single().IncludeExceptionDetailInFaults = true;
                wcfServiceHost.AddServiceEndpoint(typeof(Common.ServiceContracts.IQueryService), new NetNamedPipeBinding(), url);

                //var serviceMetadataBehavior = wcfServiceHost.Description.Behaviors.Find<ServiceMetadataBehavior>() ?? new ServiceMetadataBehavior();
                //serviceMetadataBehavior.HttpGetEnabled = true;
                //serviceMetadataBehavior.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                //wcfServiceHost.Description.Behaviors.Add(serviceMetadataBehavior);
                //wcfServiceHost.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexHttpBinding(), "mex");

                //wcfServiceHost.AddServiceEndpoint(typeof(Common.ServiceContracts.IQueryService), new WSHttpBinding(), "");

                wcfServiceHost.Open();

                Console.WriteLine("Started query service.");

                //Console.ReadLine();

                Console.WriteLine("Staring client demo...");
                Console.WriteLine("-------------------------------------------------");


                new Client.Demo(url).Run();


                Console.WriteLine();
                Console.WriteLine("-------------------------------------------------");

                Console.WriteLine("Done.");
            }

            Console.WriteLine("Terminated WCF service. Hit enter to exit");
            Console.ReadLine();
        }
    }
}
