using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Text;
using Org.XmlResolver;
using Org.XmlResolver.Cache;
using Org.XmlResolver.Utils;
using Version = XmlResolverData.Version;

namespace SampleApp {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello World!");

            string zipFile = "/Users/ndw/Projects/xmlresolver/resolver/src/test/resources/dir-sample.zip";
            /*
            ZipArchive zipRead = System.IO.Compression.ZipFile.OpenRead(zipFile);
            foreach (ZipArchiveEntry entry in zipRead.Entries) {
                Console.WriteLine(entry);
            }
            */
            
            XmlResolverConfiguration config = new();
            config.AddCatalog(zipFile);
            Resolver resolver = new Resolver(config);
            object o = resolver.GetEntity(new Uri("https://xmlresolver.org/ns/sample/sample.dtd"), null, null);
            Console.WriteLine(o);
            
            /*
            try {
                HttpWebRequest req = WebRequest.CreateHttp("https://nwalsh.com/");
                req.Method = "HEAD";
                HttpWebResponse resp = (HttpWebResponse) req.GetResponse();
                Console.WriteLine(resp);
            }
            catch (WebException ex) {
                Console.WriteLine(ex);
            }
            */

            //Console.WriteLine("Data Version {0}", Version.DataVersion);

            /*

            XmlResolverConfiguration config = new();

            var assembly = Assembly.GetExecutingAssembly();
            String[] files = assembly.GetManifestResourceNames();

            foreach (var file in files)
            {
                Console.WriteLine(file);
            }

            Uri loc = GetLocationUri("lookup2.xml");
            Console.WriteLine(loc);
            Stream stream = UriUtils.GetStream(loc, Assembly.GetExecutingAssembly());
            if (stream == null) {
                throw new NullReferenceException("Failed to read stream from " + loc);
            }
            StreamReader sr = new StreamReader(stream);
            String line;
            while ((line = sr.ReadLine()) != null) {
                Console.WriteLine(line);
            }
            */

            /*
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach(AssemblyName an in Assembly.GetExecutingAssembly().GetReferencedAssemblies()){                      
                Assembly asm = Assembly.Load(an.ToString());
                Console.WriteLine(asm);
                String[] afiles = asm.GetManifestResourceNames();
                foreach (var file in afiles)
                {
                    Console.WriteLine("\t"+file);
                }
            }
            */

            /*
            GetReferencedAssemblies.Demo();
            */

            /*
            List<Uri> props = new();
            props.Add(UriUtils.Resolve(UriUtils.Cwd(), "/Users/ndw/java/xmlresolver-appsettings.xml"));
            props.Add(UriUtils.Resolve(UriUtils.Cwd(), "/Users/ndw/java/xmlresolver-appsettings.json"));
            
            XmlResolverConfiguration config = new(props, null);
            Console.WriteLine(config);

            CatalogResolver resolver = new CatalogResolver();
            var rsrc = resolver.ResolveUri("bibliography.xml", "http://docbook.sourceforge.net/release/bibliography/");
            Console.WriteLine(rsrc.GetLocalUri());
            rsrc = resolver.ResolveEntity(null, "-//W3C//DTD SVG 1.1//EN",
                "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd", null);
            if (rsrc != null) {
                Console.WriteLine(rsrc.GetLocalUri());
            }
            else {
                Console.WriteLine("Failed to resolve");
            }

            rsrc = resolver.ResolveUri("http://example.com/example.xml", null);
            if (rsrc != null) {
                Console.WriteLine(rsrc.GetLocalUri());
            }
            else {
                Console.WriteLine("Failed to resolve");
            }
            */
        }

        public static Uri GetLocationUri(string resourcePath, Assembly assembly = null) {
            StringBuilder uribuilder = new StringBuilder();
            uribuilder.Append("application:///");

            if (assembly == null) {
                assembly = Assembly.GetExecutingAssembly();
            }

            // FIXME: the Microsoft documentation is inconsistent about the format of the path.
            // It says, the path must conform to AssemblyShortName{;Version]{;PublicKey];component/Path
            // but then goes on to show examples that have a leading "/" before the AssemblyShortName.
            // I can't work out how to construct paths of the latter form, see
            // https://stackoverflow.com/questions/68255149 so I'm just going to run with the former
            // for now.

            AssemblyName name = assembly.GetName();
            uribuilder.Append(name.Name);
            if (name.Version != null) {
                uribuilder.Append(";");
                uribuilder.Append(name.Version);
            }
            
            // FIXME: what about the public key?

            uribuilder.Append(";component");

            resourcePath = resourcePath.Replace('\\', '/');
            if (!resourcePath.StartsWith("/")) {
                resourcePath = "/" + resourcePath;
            }

            Uri appBase = new Uri(uribuilder.ToString(), UriKind.Absolute);
            Uri appPath = new Uri(resourcePath, UriKind.Relative);
            Uri packUri = PackUriHelper.Create(appBase, appPath);
            return packUri;
        }
    }
    

    /// <summary>
    ///     Intent: Get referenced assemblies, either recursively or flat. Not thread safe, if running in a multi
    ///     threaded environment must use locks.
    /// </summary>
    /// https://stackoverflow.com/questions/383686/how-do-you-loop-through-currently-loaded-assemblies
    public static class GetReferencedAssemblies {
        public static void Demo() {
            var referencedAssemblies = Assembly.GetEntryAssembly().MyGetReferencedAssembliesRecursive();
            var missingAssemblies = Assembly.GetEntryAssembly().MyGetMissingAssembliesRecursive();
            // Can use this within a class.
            //var referencedAssemblies = this.MyGetReferencedAssembliesRecursive();
        }

        public class MissingAssembly {
            public MissingAssembly(string missingAssemblyName, string missingAssemblyNameParent) {
                MissingAssemblyName = missingAssemblyName;
                MissingAssemblyNameParent = missingAssemblyNameParent;
            }

            public string MissingAssemblyName { get; set; }
            public string MissingAssemblyNameParent { get; set; }
        }

        private static Dictionary<string, Assembly> _dependentAssemblyList;
        private static List<MissingAssembly> _missingAssemblyList;

        /// <summary>
        ///     Intent: Get assemblies referenced by entry assembly. Not recursive.
        /// </summary>
        public static List<string> MyGetReferencedAssembliesFlat(this Type type) {
            var results = type.Assembly.GetReferencedAssemblies();
            return results.Select(o => o.FullName).OrderBy(o => o).ToList();
        }

        /// <summary>
        ///     Intent: Get assemblies currently dependent on entry assembly. Recursive.
        /// </summary>
        public static Dictionary<string, Assembly> MyGetReferencedAssembliesRecursive(this Assembly assembly) {
            _dependentAssemblyList = new Dictionary<string, Assembly>();
            _missingAssemblyList = new List<MissingAssembly>();

            InternalGetDependentAssembliesRecursive(assembly);

            /*
            // Only include assemblies that we wrote ourselves (ignore ones from GAC).
            var keysToRemove = _dependentAssemblyList.Values.Where(
                o => o.GlobalAssemblyCache == true).ToList();

            foreach (var k in keysToRemove) {
                _dependentAssemblyList.Remove(k.FullName.MyToName());
            }
            */
            
            return _dependentAssemblyList;
        }

        /// <summary>
        ///     Intent: Get missing assemblies.
        /// </summary>
        public static List<MissingAssembly> MyGetMissingAssembliesRecursive(this Assembly assembly) {
            _dependentAssemblyList = new Dictionary<string, Assembly>();
            _missingAssemblyList = new List<MissingAssembly>();
            InternalGetDependentAssembliesRecursive(assembly);

            return _missingAssemblyList;
        }

        /// <summary>
        ///     Intent: Internal recursive class to get all dependent assemblies, and all dependent assemblies of
        ///     dependent assemblies, etc.
        /// </summary>
        private static void InternalGetDependentAssembliesRecursive(Assembly assembly) {
            // Load assemblies with newest versions first. Omitting the ordering results in false positives on
            // _missingAssemblyList.
            var referencedAssemblies = assembly.GetReferencedAssemblies()
                .OrderByDescending(o => o.Version);

            foreach (var r in referencedAssemblies) {
                if (String.IsNullOrEmpty(assembly.FullName)) {
                    continue;
                }

                if (_dependentAssemblyList.ContainsKey(r.FullName.MyToName()) == false) {
                    try {
                        var a = Assembly.ReflectionOnlyLoad(r.FullName);
                        _dependentAssemblyList[a.FullName.MyToName()] = a;
                        InternalGetDependentAssembliesRecursive(a);
                    }
                    catch (Exception) {
                        _missingAssemblyList.Add(new MissingAssembly(r.FullName.Split(',')[0],
                            assembly.FullName.MyToName()));
                    }
                }
            }
        }

        private static string MyToName(this string fullName) {
            return fullName.Split(',')[0];
        }
    }
}