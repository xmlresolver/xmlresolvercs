using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.Extensions.Configuration;
using NLog;
using Org.XmlResolver.Cache;
using Org.XmlResolver.Features;
using Org.XmlResolver.Utils;

namespace Org.XmlResolver {
    public class XmlResolverConfiguration : ResolverConfiguration {
        private readonly object _syncLock = new object();

        protected static ResolverLogger logger = new(LogManager.GetCurrentClassLogger());

        private static List<ResolverFeature> knownFeatures = new() {
            ResolverFeature.CATALOG_FILES, ResolverFeature.PREFER_PUBLIC, ResolverFeature.PREFER_PROPERTY_FILE,
            ResolverFeature.ALLOW_CATALOG_PI, ResolverFeature.CATALOG_ADDITIONS, ResolverFeature.CACHE_DIRECTORY,
            ResolverFeature.CACHE_UNDER_HOME, ResolverFeature.CACHE, ResolverFeature.MERGE_HTTPS, ResolverFeature.MASK_PACK_URIS,
            ResolverFeature.CATALOG_MANAGER, ResolverFeature.URI_FOR_SYSTEM, ResolverFeature.CATALOG_LOADER_CLASS,
            ResolverFeature.PARSE_RDDL, ResolverFeature.CLASSPATH_CATALOGS
        };

        // private static List<string> classpathCatalogList = null;
        private List<string> catalogs = new();

        private bool preferPublic = ResolverFeature.PREFER_PUBLIC.GetDefaultValue();
        private bool preferPropertyFile = ResolverFeature.PREFER_PROPERTY_FILE.GetDefaultValue();
        private bool allowCatalogPI = ResolverFeature.ALLOW_CATALOG_PI.GetDefaultValue();
        private String cacheDirectory = ResolverFeature.CACHE_DIRECTORY.GetDefaultValue();
        private bool cacheUnderHome = ResolverFeature.CACHE_UNDER_HOME.GetDefaultValue();
        private ResourceCache cache = ResolverFeature.CACHE.GetDefaultValue(); // null
        private CatalogManager manager = ResolverFeature.CATALOG_MANAGER.GetDefaultValue(); // also null
        private bool uriForSystem = ResolverFeature.URI_FOR_SYSTEM.GetDefaultValue();
        private bool mergeHttps = ResolverFeature.MERGE_HTTPS.GetDefaultValue();
        private bool maskPackUris = ResolverFeature.MASK_PACK_URIS.GetDefaultValue();
        private String catalogLoader = ResolverFeature.CATALOG_LOADER_CLASS.GetDefaultValue();
        private bool parseRddl = ResolverFeature.PARSE_RDDL.GetDefaultValue();
        private bool classpathCatalogs = ResolverFeature.CLASSPATH_CATALOGS.GetDefaultValue();
        private bool showConfigChanges = false; // make the config process a bit less chatty
        
        public XmlResolverConfiguration(): this(null, null) {
            // nop
        }

        public XmlResolverConfiguration(string catalogFiles)
            : this(null, new List<string>(Regex.Split(catalogFiles, @"\s*;\s*"))) {
            // nop
        }

        public XmlResolverConfiguration(List<string> catalogFiles) : this(null, catalogFiles) {
            // nop
        }
        
        public XmlResolverConfiguration(List<Uri> propertyFiles, List<string> catalogFiles) {
            logger.Log(ResolverLogger.CONFIG, "XMLResolver version FIXME:");
            showConfigChanges = false;
            catalogs.Clear();
            LoadConfiguration(propertyFiles, catalogFiles);
            showConfigChanges = true;
        }

        public XmlResolverConfiguration(XmlResolverConfiguration current) {
            catalogs = new List<string>(current.catalogs);
            preferPublic = current.preferPublic;
            preferPropertyFile = current.preferPropertyFile;
            allowCatalogPI = current.allowCatalogPI;
            cacheDirectory = current.cacheDirectory;
            cacheUnderHome = current.cacheUnderHome;
            cache = current.cache;
            if (current.manager == null) {
                manager = null;
            } else {
                manager = new CatalogManager(current.manager, this);
            }
            uriForSystem = current.uriForSystem;
            mergeHttps = current.mergeHttps;
            maskPackUris = current.maskPackUris;
            catalogLoader = current.catalogLoader;
            parseRddl = current.parseRddl;
            classpathCatalogs = current.classpathCatalogs;
            showConfigChanges = current.showConfigChanges;
        }

        private void LoadConfiguration(List<Uri> propertyFiles, List<string> catalogFiles) {
            LoadSystemPropertiesConfiguration();

            List<Uri> propertyFilesList = new();
            if (propertyFiles == null) {
                string propfn = Environment.GetEnvironmentVariable("XMLRESOLVER_PROPERTIES");

                if (propfn == null || "".Equals(propfn)) {
                    // FIXME: load the system one
                    // Maybe https://stackoverflow.com/questions/474055/c-sharp-equivalent-of-getclassloader-getresourceasstream
                } else {
                    foreach (var fn in propfn.Split(";")) {
                        propertyFilesList.Add(UriUtils.Resolve(UriUtils.Cwd(), fn.Trim()));
                    }
                }
            } else {
                propertyFilesList.AddRange(propertyFiles);
            }

            Uri loaded = null;
            IConfigurationSection section = null;
            foreach (var pfile in propertyFilesList) {
                if (loaded == null && "file".Equals(pfile.Scheme)) {
                    if (File.Exists(pfile.AbsolutePath)) {
                        var cfgBuilder = new ConfigurationBuilder();
                        cfgBuilder.AddXmlFile(pfile.AbsolutePath);
                        try {
                            var config = cfgBuilder.Build();
                            loaded = pfile;
                            section = config.GetSection("xmlResolver");
                        }
                        catch (XmlException) {
                            // Maybe it's a JSON file?
                            cfgBuilder = new ConfigurationBuilder();
                            cfgBuilder.AddJsonFile(pfile.AbsolutePath);
                            try {
                                var config = cfgBuilder.Build();
                                section = config.GetSection("XmlResolver");
                                loaded = pfile;
                            }
                            catch (FormatException) {
                                logger.Log(ResolverLogger.CONFIG, "Failed to read {0}", pfile.AbsolutePath);
                            }
                        }
                    }
                } else {
                    logger.Log(ResolverLogger.ERROR, "Property URI is not a file: URI: {0}", pfile.ToString());
                }
            }

            if (section != null) {
                LoadPropertiesConfiguration(loaded, section);
            }

            if (catalogFiles == null) {
                if (catalogs.Count == 0) {
                    catalogs.Add("./catalog.xml");
                }
            }
            else {
                catalogs.Clear();
                foreach (var fn in catalogFiles) {
                    if (!"".Equals(fn.Trim())) {
                        catalogs.Add(fn.Trim());
                    }
                }
            }
        }

        private void LoadSystemPropertiesConfiguration() {
            // C# doesn't have "system properties". Environment variables seem like a plausible workaround...
            string property = Environment.GetEnvironmentVariable("XML_CATALOG_FILES");
            if (property != null) {
                if (showConfigChanges) {
                    logger.Log(ResolverLogger.CONFIG, "Catalog list cleared");
                }
                catalogs.Clear();
                foreach (var token in property.Split(";")) {
                    if (!"".Equals(token.Trim())) {
                        if (showConfigChanges) {
                            logger.Log(ResolverLogger.CONFIG, "Catalog: {0}", token);
                        }
                        catalogs.Add(token);
                    }
                    
                }
            }

            property = Environment.GetEnvironmentVariable("XML_CATALOG_ADDITIONS");
            if (property != null) {
                foreach (var token in property.Split(";")) {
                    if (!"".Equals(token.Trim())) {
                        if (showConfigChanges) {
                            logger.Log(ResolverLogger.CONFIG, "Catalog: {0}", token);
                        }
                        catalogs.Add(token);
                    }
                }
            }

            property = Environment.GetEnvironmentVariable("XML_CATALOG_PREFER");
            if (property != null) {
                if (showConfigChanges) {
                    logger.Log(ResolverLogger.CONFIG, "Prefer public: {0}", property);
                }
                preferPublic = "public".Equals(property.ToLower());
            }
            
            SetBoolean("XML_CATALOG_PREFER_PROPERTY_FILE", "Prefer propertyFile: {0}", ref preferPropertyFile);
            SetBoolean("XML_CATALOG_ALLOW_PI", "Allow catalog PI: {0}", ref allowCatalogPI);

            property = Environment.GetEnvironmentVariable("XML_CATALOG_CACHE");
            if (property != null) {
                if (showConfigChanges) {
                    logger.Log(ResolverLogger.CONFIG, "Cache directory: {0}", property);
                }
                cacheDirectory = property;
            }

            SetBoolean("XML_CATALOG_CACHE_UNDER_HOME", "Cache under home: {0}", ref cacheUnderHome);
            SetBoolean("XML_CATALOG_URI_FOR_SYSTEM", "URI-for-system: {0}", ref uriForSystem);
            SetBoolean("XML_CATALOG_MERGE_HTTPS", "Merge https: {0}", ref mergeHttps);
            SetBoolean("XML_CATALOG_MASK_JAR_URIS", "Mask-jar-URIs: {0}", ref maskPackUris);
            
            property = Environment.GetEnvironmentVariable("XML_CATALOG_LOADER_CLASS");
            if (property != null) {
                if (showConfigChanges) {
                    logger.Log(ResolverLogger.CONFIG, "Catalog loader: {0}", property);
                }
                catalogLoader = property;
            }

            SetBoolean("XML_CATALOG_PARSE_RDDL", "Use RDDL: {0}", ref parseRddl);
            SetBoolean("XML_CATALOG_CLASSPATH_CATALOGS", "Classpath catalogs: {0}", ref classpathCatalogs);
        }

        private void SetBoolean(string name, string desc, ref bool value) {
            string property = Environment.GetEnvironmentVariable(name);
            if (property != null) {
                if (showConfigChanges) {
                    logger.Log(ResolverLogger.CONFIG, desc, property);
                }

                value = IsTrue(property);
            }
        }

        private bool IsTrue(string value) {
            if (value == null) {
                return false;
            } else {
                return "true".Equals(value) || "yes".Equals(value) || "1".Equals(value);
            }
        }

        private void LoadPropertiesConfiguration(Uri propertyFile, IConfigurationSection section) {
            // In Java, we set a system property to communicate with the logger.
            // We can't do that in C# so I'm not sure what to do here...
            var property = section.GetSection("catalogLogging");
            if (property.Value != null) {
                // ???
            }

            bool relative = true;
            property = section.GetSection("relativeCatalogs");
            if (property.Value != null) {
                relative = IsTrue(property.Value);
            }
            if (showConfigChanges) {
                logger.Log(ResolverLogger.CONFIG, "Relative catalogs: {0}", relative.ToString());
            }

            property = section.GetSection("catalogs");
            if (property.Value != null) {
                String[] tokens = property.Value.Split(";");
                catalogs.Clear();
                if (showConfigChanges) {
                    logger.Log(ResolverLogger.CONFIG, "Catalog list cleared");
                }

                foreach (var token in tokens) {
                    if (!"".Equals(token)) {
                        string caturi = token;
                        if (relative && propertyFile != null) {
                            caturi = UriUtils.Resolve(propertyFile, caturi).ToString();
                        }
                        if (showConfigChanges) {
                            logger.Log(ResolverLogger.CONFIG, "Catalog: {0}", caturi);
                        }
                        catalogs.Add(caturi);
                    }
                }
            }
            
            property = section.GetSection("catalogAdditions");
            if (property.Value != null) {
                String[] tokens = property.Value.Split(";");
                foreach (var token in tokens) {
                    if (!"".Equals(token)) {
                        string caturi = token;
                        if (relative && propertyFile != null) {
                            caturi = UriUtils.Resolve(propertyFile, caturi).ToString();
                        }
                        if (showConfigChanges) {
                            logger.Log(ResolverLogger.CONFIG, "Catalog: {0}", caturi);
                        }
                        catalogs.Add(caturi);
                    }
                }
            }

            property = section.GetSection("prefer");
            if (property.Value != null) {
                if (showConfigChanges) {
                    logger.Log(ResolverLogger.CONFIG, "Prefer public: {0}", property.Value);
                }
                preferPublic = "public".Equals(property.Value.ToLower());
            }

            SetPropertyBoolean(section.GetSection("preferPropertyFile"), "Prefer propertyFile: {0}", ref preferPropertyFile);
            SetPropertyBoolean(section.GetSection("allowOasisXmlCatalogPi"), "Allow catalog PI: {0}", ref allowCatalogPI);

            property = section.GetSection("catalogCache");
            if (property.Value != null) {
                if (showConfigChanges) {
                    logger.Log(ResolverLogger.CONFIG, "Cache directory: {0}", property.Value);
                }
                cacheDirectory = property.Value;
            }

            SetPropertyBoolean(section.GetSection("cacheUnderHome"), "Cache under home: {0}", ref cacheUnderHome);
            SetPropertyBoolean(section.GetSection("uriForSystem"), "URI-for-system: {0}", ref uriForSystem);
            SetPropertyBoolean(section.GetSection("mergeHttps"), "Merge https: {0}", ref mergeHttps);
            SetPropertyBoolean(section.GetSection("maskJarUris"), "Mask-jar-URIs: {0}", ref maskPackUris);
            
            property = section.GetSection("catalogLoaderClass");
            if (property.Value != null) {
                if (showConfigChanges) {
                    logger.Log(ResolverLogger.CONFIG, "Catalog loader: {0}", property.Value);
                }
                catalogLoader = property.Value;
            }

            SetPropertyBoolean(section.GetSection("parseRddl"),"Use RDDL: {0}", ref parseRddl);
            SetPropertyBoolean(section.GetSection("classpathCatalogs"), "Classpath catalogs: {0}", ref classpathCatalogs);
        }

        private void SetPropertyBoolean(IConfigurationSection section, string desc, ref bool value) {
            string property = section.Value;
            if (property != null) {
                if (showConfigChanges) {
                    logger.Log(ResolverLogger.CONFIG, desc, property);
                }

                value = IsTrue(property);
            }
        }
        
        public void AddCatalog(String catalog) {
            if (catalog != null) {
                lock (_syncLock) {
                    catalogs.Add(catalog);
                }
            }
        }

        public void AddAssemblyCatalog(String path) {
            AddAssemblyCatalog(path, Assembly.GetExecutingAssembly());
        }

        public void AddAssemblyCatalog(String path, Assembly asm) {
            Uri cat = UriUtils.GetLocationUri(path, asm);
            AddCatalog(cat.ToString());
        }

        public void AddCatalog(Uri catalog, Stream data) {
            if (catalog == null) {
                throw new ArgumentNullException(nameof(catalog));
            }

            if (data == null) {
                throw new ArgumentNullException(nameof(data));
            }

            Uri uri = UriUtils.Resolve(UriUtils.Cwd(), catalog);
            lock (_syncLock) {
                catalogs.Add(uri.ToString());
                if (manager == null) {
                    manager = (CatalogManager) GetFeature(ResolverFeature.CATALOG_MANAGER);
                }

                manager.LoadCatalog(uri, data);
            }

        }
        
        public void RemoveCatalog(String catalog) {
            if (catalog != null) {
                lock (_syncLock) {
                    catalogs.Remove(catalog);
                }
            }
        }
        
        public void SetFeature(ResolverFeature feature, object value) {
            if (feature == ResolverFeature.CATALOG_FILES) {
                lock (_syncLock) {
                    catalogs.Clear();
                    if (value != null) {
                        catalogs.AddRange((List<string>) value);
                    }
                }
                return;
            } else if (feature == ResolverFeature.CACHE_DIRECTORY) {
                cacheDirectory = (string) value;
                cache = null;
                return;
            } else if (feature == ResolverFeature.CACHE) {
                cache = (ResourceCache) value;
                return;
            }

            if (value == null) {
                throw new NullReferenceException(feature.GetFeatureName() + " must not be null");
            }
            
            if (feature == ResolverFeature.PREFER_PUBLIC) {
                preferPublic = (Boolean) value;
            } else if (feature == ResolverFeature.PREFER_PROPERTY_FILE) {
                preferPropertyFile = (Boolean) value;
            } else if (feature == ResolverFeature.ALLOW_CATALOG_PI) {
                allowCatalogPI = (Boolean) value;
            } else if (feature == ResolverFeature.CACHE_UNDER_HOME) {
                cacheUnderHome = (Boolean) value;
                cache = null;
            } else if (feature == ResolverFeature.CATALOG_MANAGER) {
                manager = (CatalogManager) value;
            } else if (feature == ResolverFeature.URI_FOR_SYSTEM) {
                uriForSystem = (Boolean) value;
            } else if (feature == ResolverFeature.MERGE_HTTPS) {
                mergeHttps = (Boolean) value;
            } else if (feature == ResolverFeature.MASK_PACK_URIS) {
                maskPackUris = (Boolean) value;
            } else if (feature == ResolverFeature.CATALOG_LOADER_CLASS) {
                catalogLoader = (String) value;
            } else if (feature == ResolverFeature.PARSE_RDDL) {
                parseRddl = (Boolean) value;
            } else if (feature == ResolverFeature.CLASSPATH_CATALOGS) {
                classpathCatalogs = (Boolean) value;
            } else {
                logger.Log(ResolverLogger.ERROR, "Ignoring unknown feature: %s", feature.GetFeatureName());
            }        
        }

        public object GetFeature(ResolverFeature feature) {
            if (feature == ResolverFeature.CATALOG_MANAGER) {
                // Delay construction of the default catalog manager until it's
                // requested. If the user sets it before asking for it, we'll
                // never have to construct it.
                if (manager == null) {
                    manager = new CatalogManager(this);
                }
                return manager;
            } else if (feature == ResolverFeature.CATALOG_FILES) {
                List<string> cats = new(catalogs);
                if (classpathCatalogs) {
                    // FIXME: find classpath catalog files
                }
                return cats;
            } else if (feature == ResolverFeature.PREFER_PUBLIC) {
                return preferPublic;
            } else if (feature == ResolverFeature.PREFER_PROPERTY_FILE) {
                return preferPropertyFile;
            } else if (feature == ResolverFeature.ALLOW_CATALOG_PI) {
                return allowCatalogPI;
            } else if (feature == ResolverFeature.CACHE_DIRECTORY) {
                return cacheDirectory;
            } else if (feature == ResolverFeature.URI_FOR_SYSTEM) {
                return uriForSystem;
            } else if (feature == ResolverFeature.MERGE_HTTPS) {
                return mergeHttps;
            } else if (feature == ResolverFeature.MASK_PACK_URIS) {
                return maskPackUris;
            } else if (feature == ResolverFeature.CATALOG_LOADER_CLASS) {
                return catalogLoader;
            } else if (feature == ResolverFeature.PARSE_RDDL) {
                return parseRddl;
            } else if (feature == ResolverFeature.CLASSPATH_CATALOGS) {
                return classpathCatalogs;
            } else if (feature == ResolverFeature.CACHE) {
                if (cache == null) {
                    cache = new ResourceCache(this);
                }
                return cache;
            } else if (feature == ResolverFeature.CACHE_UNDER_HOME) {
                return cacheUnderHome;
            } else {
                logger.Log(ResolverLogger.ERROR, "Ignoring unknown feature: %s", feature.GetFeatureName());
                return null;
            }
        }

        public List<ResolverFeature> GetFeatures() {
            return new(knownFeatures);
        }
    }
}