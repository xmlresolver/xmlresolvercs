using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.Extensions.Configuration;
using NLog;
using XmlResolver.Features;
using XmlResolver.Utils;

#nullable enable

namespace XmlResolver;
    
    /// <summary>
    /// The XML resolver configuration.
    /// </summary>
    /// <para>This class is the implementation of the <see cref="IResolverConfiguration"/> for the
    /// <see cref="CatalogResolver"/>.</para>
    /// <para>The default value for property files is taken from the
    /// <c>XMLRESOLVER_APPSETTINGS</c> environment variable. If this identifies a property file, it will
    /// be used to configure the resolver.</para>
    /// <para>Property files are read with the <see cref="IConfigurationSection"/> API. If the file is XML,
    /// properties are loaded from a section named <c>xmlResolver</c>. If the file is JSON,
    /// properties are loaded from an object named <c>XmlResolver</c>.</para>
    /// <para>In addition to a property file, individual environent variables can be used to specify properties:</para>
    /// <list type="table">
    /// <listheader>
    ///   <term>Env. Variable</term>
    ///   <description>Configuration property</description>
    /// </listheader>
    /// <item>
    ///    <term>XML_CATALOG_ADDITIONS</term>
    ///    <description>Additions to the catalog <c>ResolverFeature.CATALOG_ADDITIONS</c>.</description>
    /// </item>
    /// <item>
    ///    <term>XML_CATALOG_ALLOW_PI</term>
    ///    <description>Allow the OASIS XML Catalog PI to influence resolution, <c>ResolverFeature.ALLOW_CATALOG_PI</c>.</description>
    /// </item>
    /// <item>
    ///    <term>XML_CATALOG_ARCHIVED_CATALOGS</term>
    ///    <description>Allow ZIP files to be specified as catalogs, <c>ResolverFeature.ARCHIVED_CATALOGS</c>.</description>
    /// </item>
    /// <item>
    ///    <term>XML_CATALOG_CACHE</term>
    ///    <description>Enable caching, <c>ResolverFeature.CACHE</c>.</description>
    /// </item>
    /// <item>
    ///    <term>XML_CATALOG_CACHE_UNDER_HOME</term>
    ///    <description>Use <c>xmlresolver.org/cache</c> in the users home directory for caching, <c>ResolverFeature.CACHE_UNDER_HOME</c>.</description>
    /// </item>
    /// <item>
    ///    <term>XML_CATALOG_CACHE_ENABLED</term>
    ///    <description>Enable or disable all use of the cache.</description>
    /// </item>
    /// <item>
    ///    <term>XML_CATALOG_FILES</term>
    ///    <description>A list of XML Catalog files, <c>ResolverFeature.CATALOG_FILES</c>.</description>
    /// </item>
    /// <item>
    ///    <term>XML_CATALOG_LOADER_CLASS</term>
    ///    <description>The fully qualified name of the class to instantiate to load catalog files, <c>ResolverFeature.CATALOG_LOADER_CLASS</c>.</description>
    /// </item>
    /// <item>
    ///    <term>XML_CATALOG_MASK_PACK_URIS</term>
    ///    <description>Mask <c>pack:</c> URIs, <c>ResolverFeature.MASK_PACK_URIS</c>.</description>
    /// </item>
    /// <item>
    ///    <term>XML_CATALOG_MERGE_HTTPS</term>
    ///    <description>Merge http: and https: URIs for comparisons, <c>ResolverFeature.MERGE_HTTPS</c>.</description>
    /// </item>
    /// <item>
    ///    <term>XML_CATALOG_PARSE_RDDL</term>
    ///    <description>Attempt to parse RDDL files if located by resolution, <c>ResolverFeature.PARSE_RDDL</c>.</description>
    /// </item>
    /// <item>
    ///    <term>XML_CATALOG_PREFER</term>
    ///    <description>Prefer public or system entries, <c>ResolverFeature.PREFER_PUBLIC</c>.</description>
    /// </item>
    /// <item>
    ///    <term>XML_CATALOG_PREFER_PROPERTY_FILE</term>
    ///    <description>Prefer property file values over environment variables, <c>ResolverFeature.PREFER_PROPERTY_FILE</c>.</description>
    /// </item>
    /// <item>
    ///    <term>XML_CATALOG_URI_FOR_SYSTEM</term>
    ///    <description>Use uri entries for system identifier lookup, <c>ResolverFeature.URI_FOR_SYSTEM</c>.</description>
    /// </item>
    /// <item>
    ///    <term>XML_CATALOG_USE_DATA_ASSEMBLY</term>
    ///    <description>Use the XmlResolverData assembly for resolution (if it's available).</description>
    /// </item>
    /// </list>
    /// 
    public class XmlResolverConfiguration : IResolverConfiguration {
        private readonly object _syncLock = new object();

        public readonly string AssemblyUriPrefix = "https://xmlresolver.org/assembly/";
        private readonly string xmlResolverDataAssembly = "XmlResolverData, Culture=neutral, PublicKeyToken=null";
        private readonly string catrsrc = "XmlResolver.catalog.xml";

        protected static ResolverLogger logger = new(LogManager.GetCurrentClassLogger());

        private static readonly List<ResolverFeature> KnownFeatures = [
            ResolverFeature.CATALOG_FILES, ResolverFeature.PREFER_PUBLIC, ResolverFeature.PREFER_PROPERTY_FILE,
            ResolverFeature.ALLOW_CATALOG_PI, ResolverFeature.CATALOG_ADDITIONS,
            ResolverFeature.MERGE_HTTPS,
            ResolverFeature.CATALOG_MANAGER, ResolverFeature.URI_FOR_SYSTEM, ResolverFeature.CATALOG_LOADER_CLASS,
            ResolverFeature.PARSE_RDDL, ResolverFeature.ASSEMBLY_CATALOGS, ResolverFeature.ARCHIVED_CATALOGS,
            ResolverFeature.USE_DATA_ASSEMBLY, ResolverFeature.FIX_WINDOWS_SYSTEM_IDENTIFIERS,
            ResolverFeature.ACCESS_EXTERNAL_ENTITY, ResolverFeature.ACCESS_EXTERNAL_DOCUMENT
        ];

        // private static List<string> classpathCatalogList = null;
        private List<string> catalogs = new();
        private List<string> additionalCatalogs = new();
        private List<string> assemblyCatalogs = new();
        private List<string> builtinAssemblyCatalogs = new();
        private List<string> accessExternalEntity = new();
        private List<string> accessExternalDocument = new();
        private Dictionary<string,string> assemblyCache = new();

        private bool preferPublic = ResolverFeature.PREFER_PUBLIC.GetDefaultValue();
        private bool preferPropertyFile = ResolverFeature.PREFER_PROPERTY_FILE.GetDefaultValue();
        private bool allowCatalogPi = ResolverFeature.ALLOW_CATALOG_PI.GetDefaultValue();
        private CatalogManager? manager = ResolverFeature.CATALOG_MANAGER.GetDefaultValue(); // also null
        private bool uriForSystem = ResolverFeature.URI_FOR_SYSTEM.GetDefaultValue();
        private bool mergeHttps = ResolverFeature.MERGE_HTTPS.GetDefaultValue();
        private string catalogLoader = ResolverFeature.CATALOG_LOADER_CLASS.GetDefaultValue();
        private bool parseRddl = ResolverFeature.PARSE_RDDL.GetDefaultValue();
        private bool archivedCatalogs = ResolverFeature.ARCHIVED_CATALOGS.GetDefaultValue();
        private bool useDataAssembly = ResolverFeature.USE_DATA_ASSEMBLY.GetDefaultValue();
        private bool fixWindowsSystemIdentifiers = ResolverFeature.FIX_WINDOWS_SYSTEM_IDENTIFIERS.GetDefaultValue();
        private bool showConfigChanges = false; // make the config process a bit less chatty
        
        /// <summary>
        /// Create a new configuration from defaults.
        /// </summary>
        public XmlResolverConfiguration(): this(null, null) {
            // nop
        }

        /// <summary>
        /// Create a new configuration using the specified catalog files.
        /// </summary>
        /// <para>For historical reasons, the list of catalogs must be semicolon separated in this string.</para>
        /// <para>This constructor uses the default properties.</para>
        /// <param name="catalogFiles">The list of catalog files.</param>
        public XmlResolverConfiguration(string catalogFiles)
            : this(null, new List<string>(Regex.Split(catalogFiles, @"\s*;\s*"))) {
            // nop
        }

        /// <summary>
        /// Create a new configuration using the specified catalog files.
        /// </summary>
        /// <para>This constructor uses the default properties.</para>
        /// <param name="catalogFiles">The list of catalog files.</param>
        public XmlResolverConfiguration(List<string> catalogFiles) : this(null, catalogFiles) {
            // nop
        }
        
        /// <summary>
        /// Create a new configuration using the specified list of property files and list of catalog files.
        /// </summary>
        /// <para>The specified list of property files will be searched in order. The first property
        /// file that can be opened successfully will be used and the rest will be ignored.</para>
        /// <param name="propertyFiles"></param>
        /// <param name="catalogFiles"></param>
        public XmlResolverConfiguration(List<Uri>? propertyFiles, List<string>? catalogFiles) {
            Assembly assembly = Assembly.GetExecutingAssembly();
            logger.Debug(assembly.FullName);
            showConfigChanges = false;
            catalogs.Clear();
            additionalCatalogs.Clear();
            assemblyCatalogs.Clear();
            builtinAssemblyCatalogs.Clear();
            assemblyCache.Clear();
            LoadConfiguration(propertyFiles, catalogFiles);
            showConfigChanges = true;

            if (ResolverFeature.ACCESS_EXTERNAL_ENTITY.GetDefaultValue() == null)
            {
                accessExternalEntity.Add("all");
            }
            else
            {
                accessExternalEntity = ResolverFeature.ACCESS_EXTERNAL_ENTITY.GetDefaultValue();
            }

            if (ResolverFeature.ACCESS_EXTERNAL_DOCUMENT.GetDefaultValue() == null)
            {
                accessExternalDocument.Add("all");
            }
            else
            {
                accessExternalDocument = ResolverFeature.ACCESS_EXTERNAL_DOCUMENT.GetDefaultValue();
            }
        }

        /// <summary>
        /// Create a new configuration by copying an existing one.
        /// </summary>
        /// <param name="current">The configuration to copy.</param>
        public XmlResolverConfiguration(XmlResolverConfiguration current) {
            catalogs = new List<string>(current.catalogs);
            additionalCatalogs = new List<string>(current.additionalCatalogs);
            assemblyCatalogs = new List<string>(current.assemblyCatalogs);
            builtinAssemblyCatalogs = new List<string>(current.builtinAssemblyCatalogs);
            preferPublic = current.preferPublic;
            preferPropertyFile = current.preferPropertyFile;
            allowCatalogPi = current.allowCatalogPi;
            if (current.manager == null) {
                manager = null;
            } else {
                manager = new CatalogManager(current.manager, this);
            }
            uriForSystem = current.uriForSystem;
            mergeHttps = current.mergeHttps;
            catalogLoader = current.catalogLoader;
            parseRddl = current.parseRddl;
            archivedCatalogs = current.archivedCatalogs;
            useDataAssembly = current.useDataAssembly;
            showConfigChanges = current.showConfigChanges;
            fixWindowsSystemIdentifiers = current.fixWindowsSystemIdentifiers;
            accessExternalEntity = current.accessExternalEntity;
            accessExternalDocument = current.accessExternalDocument;
        }

        private void LoadConfiguration(List<Uri>? propertyFiles, List<string>? catalogFiles) {
            LoadSystemPropertiesConfiguration();

            List<Uri> propertyFilesList = new();
            if (propertyFiles == null) {
                var propfn = Environment.GetEnvironmentVariable("XMLRESOLVER_APPSETTINGS");

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

            Uri? loaded = null;
            IConfigurationSection? section = null;
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
                                logger.Debug("Failed to read {0}", pfile.AbsolutePath);
                            }
                        }
                    }
                } else {
                    logger.Debug("Property URI is not a file: URI: {0}", pfile.ToString());
                }
            }

            if (loaded != null && section != null) {
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

            if (useDataAssembly) {
                builtinAssemblyCatalogs.Add(xmlResolverDataAssembly);
            }
        }

        private void LoadSystemPropertiesConfiguration() {
            // C# doesn't have "system properties". Environment variables seem like a plausible workaround...
            var property = Environment.GetEnvironmentVariable("XML_CATALOG_FILES");
            if (property != null) {
                if (showConfigChanges) {
                    logger.Debug("Catalog list cleared");
                }
                catalogs.Clear();
                foreach (var token in property.Split(";")) {
                    if (!"".Equals(token.Trim())) {
                        if (showConfigChanges) {
                            logger.Debug("Catalog: {0}", token);
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
                            logger.Debug("Catalog: {0}", token);
                        }
                        additionalCatalogs.Add(token);
                    }
                }
            }

            property = Environment.GetEnvironmentVariable("XML_CATALOG_PREFER");
            if (property != null) {
                if (showConfigChanges) {
                    logger.Debug("Prefer public: {0}", property);
                }
                preferPublic = "public".Equals(property.ToLower());
            }

            property = Environment.GetEnvironmentVariable("XML_CATALOG_ACCESS_EXTERNAL_ENTITY");
            if (property != null)
            {
                if (showConfigChanges)
                {
                    logger.Debug("Access external entities: {0}", property);
                }
                accessExternalEntity = UriSchemesAsList(property);
            }
            
            property = Environment.GetEnvironmentVariable("XML_CATALOG_ACCESS_EXTERNAL_DOCUMENT");
            if (property != null)
            {
                if (showConfigChanges)
                {
                    logger.Debug("Access external documents: {0}", property);
                }
                accessExternalDocument = UriSchemesAsList(property);
            }
            
            SetBoolean("XML_CATALOG_PREFER_PROPERTY_FILE", "Prefer propertyFile: {0}", ref preferPropertyFile);
            SetBoolean("XML_CATALOG_ALLOW_PI", "Allow catalog PI: {0}", ref allowCatalogPi);

            SetBoolean("XML_CATALOG_URI_FOR_SYSTEM", "URI-for-system: {0}", ref uriForSystem);
            SetBoolean("XML_CATALOG_MERGE_HTTPS", "Merge https: {0}", ref mergeHttps);
            SetBoolean("XML_CATALOG_FIX_WINDOWS_SYSTEM_IDENTIFIERS", "Fix Windows system identifiers: {0}", ref fixWindowsSystemIdentifiers);
            
            property = Environment.GetEnvironmentVariable("XML_CATALOG_LOADER_CLASS");
            if (property != null) {
                if (showConfigChanges) {
                    logger.Debug("Catalog loader: {0}", property);
                }
                catalogLoader = property;
            }

            SetBoolean("XML_CATALOG_PARSE_RDDL", "Use RDDL: {0}", ref parseRddl);
            SetBoolean("XML_CATALOG_ARCHIVED_CATALOGS", "Use archived catalogs: {0}", ref archivedCatalogs);
            SetBoolean("XML_CATALOG_USE_DATA_ASSEMBLY", "Use data assembly: {0}", ref useDataAssembly);
        }

        private void SetBoolean(string name, string desc, ref bool value) {
            var property = Environment.GetEnvironmentVariable(name);
            if (property != null) {
                if (showConfigChanges) {
                    logger.Debug(desc, property);
                }

                value = IsTrue(property);
            }
        }

        private bool IsTrue(string? value) {
            return "true".Equals(value) || "yes".Equals(value) || "1".Equals(value);
        }

        private void LoadPropertiesConfiguration(Uri propertyFile, IConfigurationSection section) {
            bool relative = true;
            var property = section.GetSection("relativeCatalogs");
            if (property.Value != null) {
                relative = IsTrue(property.Value);
            }
            if (showConfigChanges) {
                logger.Debug("Relative catalogs: {0}", relative.ToString());
            }

            property = section.GetSection("catalogs");
            if (property.Value != null) {
                var tokens = property.Value.Split(";");
                catalogs.Clear();
                if (showConfigChanges) {
                    logger.Debug("Catalog list cleared");
                }

                foreach (var token in tokens) {
                    if (!"".Equals(token)) {
                        string caturi = token;
                        if (relative && propertyFile != null) {
                            caturi = UriUtils.Resolve(propertyFile, caturi).ToString();
                        }
                        if (showConfigChanges) {
                            logger.Debug("Catalog: {0}", caturi);
                        }
                        catalogs.Add(caturi);
                    }
                }
            }
            
            property = section.GetSection("catalogAdditions");
            if (property.Value != null) {
                var tokens = property.Value.Split(";");
                foreach (var token in tokens) {
                    if (!"".Equals(token)) {
                        string caturi = token;
                        if (relative && propertyFile != null) {
                            caturi = UriUtils.Resolve(propertyFile, caturi).ToString();
                        }
                        if (showConfigChanges) {
                            logger.Debug("Catalog: {0}", caturi);
                        }
                        additionalCatalogs.Add(caturi);
                    }
                }
            }

            property = section.GetSection("prefer");
            if (property.Value != null) {
                if (showConfigChanges) {
                    logger.Debug("Prefer public: {0}", property.Value);
                }
                preferPublic = "public".Equals(property.Value.ToLower());
            }

            property = section.GetSection("accessExternalEntity");
            if (property.Value != null)
            {
                if (showConfigChanges)
                {
                    logger.Debug("Access external entities: {0}", property.Value);
                }
                accessExternalEntity = UriSchemesAsList(property.Value);
            }

            property = section.GetSection("accessExternalDocument");
            if (property.Value != null)
            {
                if (showConfigChanges)
                {
                    logger.Debug("Access external documents: {0}", property.Value);
                }
                accessExternalDocument = UriSchemesAsList(property.Value);
            }

            SetPropertyBoolean(section.GetSection("preferPropertyFile"), "Prefer propertyFile: {0}", ref preferPropertyFile);
            SetPropertyBoolean(section.GetSection("allowOasisXmlCatalogPi"), "Allow catalog PI: {0}", ref allowCatalogPi);

            SetPropertyBoolean(section.GetSection("uriForSystem"), "URI-for-system: {0}", ref uriForSystem);
            SetPropertyBoolean(section.GetSection("mergeHttps"), "Merge https: {0}", ref mergeHttps);
            SetPropertyBoolean(section.GetSection("fixWindowsSystemIdentifiers"), "Fix Windows system identifiers: {0}", ref fixWindowsSystemIdentifiers);
            
            property = section.GetSection("catalogLoaderClass");
            if (property.Value != null) {
                if (showConfigChanges) {
                    logger.Debug("Catalog loader: {0}", property.Value);
                }
                catalogLoader = property.Value;
            }

            SetPropertyBoolean(section.GetSection("parseRddl"),"Use RDDL: {0}", ref parseRddl);
            SetPropertyBoolean(section.GetSection("archivedCatalogs"),"Use archived catalogs: {0}", ref archivedCatalogs);
            SetPropertyBoolean(section.GetSection("useDataAssembly"), "Use data assembly: {0}", ref useDataAssembly);
        }

        private void SetPropertyBoolean(IConfigurationSection section, string desc, ref bool value) {
            var property = section.Value;
            if (property != null) {
                if (showConfigChanges) {
                    logger.Debug(desc, property);
                }

                value = IsTrue(property);
            }
        }
        
        public void AddCatalog(string? catalog) {
            if (catalog != null) {
                lock (_syncLock) {
                    catalogs.Add(catalog);
                }
            }
        }

        public void AddAssemblyCatalog(string path) {
            AddAssemblyCatalog(path, Assembly.GetExecutingAssembly());
        }

        public void AddAssemblyCatalog(string path, Assembly asm) {
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
                    manager = (CatalogManager) GetFeature(ResolverFeature.CATALOG_MANAGER)!;
                }

                manager.LoadCatalog(uri, data);
            }

        }
        
        public void RemoveCatalog(string catalog) {
            if (catalog != null) {
                lock (_syncLock) {
                    catalogs.Remove(catalog);
                }
            }
        }

        private List<string> UriSchemesAsList(string? value)
        {
            if (value is null or "all")
            {
                return new List<string> { "all" };
            }

            String[] elements = Regex.Split(value, @"\s+");
            if (elements.Contains("all"))
            {
                return new List<string> { "all" };
            }
            return new List<string>(elements);
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
            }
            
            if (feature == ResolverFeature.CATALOG_ADDITIONS) {
                lock (_syncLock) {
                    additionalCatalogs.Clear();
                    if (value != null) {
                        additionalCatalogs.AddRange((List<string>) value);
                    }
                }
                return;
            }

            if (feature == ResolverFeature.ASSEMBLY_CATALOGS) {
                if (value == null) {
                    assemblyCatalogs.Clear();
                    assemblyCache.Clear();
                }
                else {
                    if (value is string) {
                        assemblyCatalogs.Add((string)value);
                    } else if (value is List<string>) {
                        assemblyCatalogs.Clear();
                        assemblyCatalogs.AddRange((List<string>) value);
                    } else {
                        throw new ArgumentException("Invalid value for ASSEMBLY_CATALOGS feature");
                    }
                }
                return;
            }

            if (feature == ResolverFeature.ACCESS_EXTERNAL_ENTITY)
            {
                accessExternalEntity = UriSchemesAsList((string)value);
                return;
            }

            if (feature == ResolverFeature.ACCESS_EXTERNAL_DOCUMENT)
            {
                accessExternalDocument = UriSchemesAsList((string)value);
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
                allowCatalogPi = (Boolean) value;
            } else if (feature == ResolverFeature.CATALOG_MANAGER) {
                manager = (CatalogManager) value;
            } else if (feature == ResolverFeature.URI_FOR_SYSTEM) {
                uriForSystem = (Boolean) value;
            } else if (feature == ResolverFeature.MERGE_HTTPS) {
                mergeHttps = (Boolean) value;
            } else if (feature == ResolverFeature.CATALOG_LOADER_CLASS) {
                catalogLoader = (string) value;
            } else if (feature == ResolverFeature.PARSE_RDDL) {
                parseRddl = (Boolean) value;
            } else if (feature == ResolverFeature.ARCHIVED_CATALOGS) {
                archivedCatalogs = (Boolean) value;
            } else if (feature == ResolverFeature.USE_DATA_ASSEMBLY) {
                useDataAssembly = (Boolean) value;
                // Remove the XmlResolverData assembly if it's in the list
                List<string> updatedCatalogs = new();
                foreach (string catalog in builtinAssemblyCatalogs)
                {
                    if (catalog != xmlResolverDataAssembly) {
                        updatedCatalogs.Add(catalog);
                    }
                }
                builtinAssemblyCatalogs.Clear();
                builtinAssemblyCatalogs.AddRange(updatedCatalogs);
                // Put the XmlResolverData assembly in the list if it's enabled
                if (useDataAssembly) {
                    builtinAssemblyCatalogs.Add(xmlResolverDataAssembly);
                }
            } else if (feature == ResolverFeature.FIX_WINDOWS_SYSTEM_IDENTIFIERS) {
                fixWindowsSystemIdentifiers = (Boolean) value;
            } else {
                logger.Debug("Ignoring unknown feature: %s", feature.GetFeatureName());
            }        
        }

        public object? GetFeature(ResolverFeature feature) {
            if (feature == ResolverFeature.CATALOG_MANAGER) {
                // Delay construction of the default catalog manager until it's
                // requested. If the user sets it before asking for it, we'll
                // never have to construct it.
                if (manager == null) {
                    manager = new CatalogManager(this);
                }
                return manager;
            } 
            
            if (feature == ResolverFeature.CATALOG_FILES) {
                List<string>? cats = null;
                lock (_syncLock) {
                    // Let's avoid returning duplicates...
                    cats = [];
                    foreach (string cat in catalogs)
                    {
                        if (!cats.Contains(cat)) {
                            cats.Add(cat);
                        }
                    }
                    foreach (string cat in additionalCatalogs)
                    {
                        if (!cats.Contains(cat)) {
                            cats.Add(cat);
                        }
                    }
                    foreach (string asm in assemblyCatalogs)
                    {
                        var cat = LoadAssemblyCatalog(asm);
                        if (cat != null && !cats.Contains(cat)) {
                            cats.Add(cat);
                        }
                    }
                    foreach (string asm in builtinAssemblyCatalogs)
                    {
                        var cat = LoadAssemblyCatalog(asm);
                        if (cat != null && !cats.Contains(cat)) {
                            cats.Add(cat);
                        }
                    }
                }
                return cats;
            }
            
            if (feature == ResolverFeature.CATALOG_ADDITIONS) {
                lock (_syncLock)
                {
                    List<string> cats = [];
                    cats.AddRange(additionalCatalogs);
                    return cats;
                }
            }
            
            if (feature == ResolverFeature.PREFER_PUBLIC) {
                return preferPublic;
            }
            
            if (feature == ResolverFeature.PREFER_PROPERTY_FILE) {
                return preferPropertyFile;
            } 
            
            if (feature == ResolverFeature.ALLOW_CATALOG_PI) {
                return allowCatalogPi;
            } 
            
            if (feature == ResolverFeature.URI_FOR_SYSTEM) {
                return uriForSystem;
            } 
            
            if (feature == ResolverFeature.MERGE_HTTPS) {
                return mergeHttps;
            } 
            
            if (feature == ResolverFeature.CATALOG_LOADER_CLASS) {
                return catalogLoader;
            } 
            
            if (feature == ResolverFeature.PARSE_RDDL) {
                return parseRddl;
            }
            
            if (feature == ResolverFeature.ARCHIVED_CATALOGS) {
                return archivedCatalogs;
            } 
            
            if (feature == ResolverFeature.ASSEMBLY_CATALOGS) {
                return assemblyCatalogs;
            }
            
            if (feature == ResolverFeature.USE_DATA_ASSEMBLY) {
                return useDataAssembly;
            }
            
            if (feature == ResolverFeature.FIX_WINDOWS_SYSTEM_IDENTIFIERS) {
                return fixWindowsSystemIdentifiers;
            }

            if (feature == ResolverFeature.ACCESS_EXTERNAL_ENTITY)
            {
                return new List<string>(accessExternalEntity);
            }
            
            if (feature == ResolverFeature.ACCESS_EXTERNAL_DOCUMENT)
            {
                return new List<string>(accessExternalDocument);
            }
            
            logger.Debug("Ignoring unknown feature: %s", feature.GetFeatureName());
            return null;
        }

        public List<ResolverFeature> GetFeatures() {
            return new(KnownFeatures);
        }

        private string? LoadAssemblyCatalog(string asm)
        {
            String asmloc = AssemblyUriPrefix + asm;
            if (assemblyCache.ContainsKey(asmloc))
            {
                return assemblyCache[asmloc];
            }
            
            try
            {
                AssemblyName asmname = new AssemblyName(asm);
                Assembly xassembly = Assembly.Load(asmname);

                foreach (var file in xassembly.GetManifestResourceNames()) {
                    if (catrsrc.Equals(file)) {
                        String DllLoc = UriUtils.GetLocationUri(catrsrc, xassembly).ToString();
                        assemblyCache.Add(asmloc, DllLoc);
                        return DllLoc;
                    }
                }

                logger.Debug("Failed to load catalog from assembly: {0}", asm);
                return null; 
            }
            catch (Exception)
            {
                logger.Debug("Failed to load assembly: {0}", asm);
                return null;
            }
        }
        
        
        public IResourceResponse GetResource(IResourceRequest request)
        {
            return ResourceAccess.GetResource(request);
        }

        public IResourceResponse GetResource(IResourceResponse request)
        {
            return ResourceAccess.GetResource(request);
        }
    }