using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using NLog;
using Org.XmlResolver;
using Org.XmlResolver.Features;
using Org.XmlResolver.Utils;

namespace SampleApp {
    class Program {
        protected static readonly ResolverLogger logger = new(LogManager.GetCurrentClassLogger());

        static void Main(string[] args) {
            XmlResolverConfiguration config = new XmlResolverConfiguration();
            // The SampleApp refers to the XmlResolverData NuGet, so I can assume all of
            // the common resources in that assembly will be available.
            config.SetFeature(ResolverFeature.ASSEMBLY_CATALOGS, "XmlResolverData.dll");

            string document = null;
            
            // Crude parse of the command line arguments
            int pos = 0;
            while (pos < args.Length) {
                string arg = args[pos];

                if ("-c".Equals(arg)) {
                    config.AddCatalog(args[pos + 1]);
                    pos += 1;
                } else if (arg.StartsWith("-c")) {
                    config.AddCatalog(arg.Substring(2));
                }
                else {
                    document = arg;
                }

                pos += 1;
            }

            if (document != null) {
                Console.WriteLine("Parsing " + document);
                List<string> catalogs = (List<String>)config.GetFeature(ResolverFeature.CATALOG_FILES);
                Console.WriteLine("Using catalogs: " + String.Join(", ", catalogs));

                Resolver resolver = new Resolver(config);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Async = false;
                settings.DtdProcessing = DtdProcessing.Parse;
                settings.ValidationType = ValidationType.DTD;
                settings.XmlResolver = resolver;

                int count = 0;
                try {
                    using FileStream fs = File.OpenRead(document);
                    using (XmlReader reader = XmlReader.Create(fs, settings)) {
                        while (reader.Read()) {
                            count += 1;
                        }
                    }
                    Console.WriteLine("Counted " + count + " items in the document.");
                }
                catch (Exception ex) {
                    Console.WriteLine("Well, that didn't work like we expected, did it?");
                    Console.WriteLine(ex.Message);
                }
            }
            else {
                Console.WriteLine("Usage: SampleApp [-c catalog]* document.xml");
            }
        }
    }
}