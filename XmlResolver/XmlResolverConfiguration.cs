using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.Extensions.Configuration;
using NLog;
using XmlResolver.Features;
using XmlResolver.Utils;

namespace XmlResolver;

/// <summary>
/// The XML resolver configuration.
/// </summary>
/// <para>This class is the implementation of the <see cref="IResolverConfiguration"/> for the
/// <see cref="XmlResolver"/>.</para>
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

public class XmlResolverConfiguration : IResolverConfiguration
{
    private readonly object _syncLock = new();

    public const string AssemblyUriPrefix = "https://xmlresolver.org/assembly/";
    private const string _xmlResolverDataAssembly = "XmlResolverData, Culture=neutral, PublicKeyToken=null";
    private const string _newCatrsrc = "XmlResolver.catalog.xml";
    private const string _oldCatrsrc = "Org.XmlResolver.catalog.xml";

    protected static ResolverLogger logger = new(LogManager.GetCurrentClassLogger());

    private static List<ResolverFeature> knownFeatures = new()
    {
        ResolverFeature.CATALOG_FILES, ResolverFeature.PREFER_PUBLIC, ResolverFeature.PREFER_PROPERTY_FILE,
        ResolverFeature.ALLOW_CATALOG_PI, ResolverFeature.CATALOG_ADDITIONS,
        ResolverFeature.MERGE_HTTPS, ResolverFeature.MASK_PACK_URIS,
        ResolverFeature.CATALOG_MANAGER, ResolverFeature.URI_FOR_SYSTEM, ResolverFeature.CATALOG_LOADER_CLASS,
        ResolverFeature.PARSE_RDDL, ResolverFeature.ASSEMBLY_CATALOGS, ResolverFeature.ARCHIVED_CATALOGS,
        ResolverFeature.USE_DATA_ASSEMBLY, ResolverFeature.FIX_WINDOWS_SYSTEM_IDENTIFIERS,
        ResolverFeature.ACCESS_EXTERNAL_ENTITY, ResolverFeature.ACCESS_EXTERNAL_DOCUMENT
    };

    // private static List<string> classpathCatalogList = null;
    private List<string> _catalogs = new();
    private List<string> _additionalCatalogs = new();
    private List<string> _assemblyCatalogs = new();
    private List<string> _builtinAssemblyCatalogs = new();
    private string _accessExternalEntity = "all";
    private string _accessExternalDocument = "all";
    private Dictionary<string, string> _assemblyCache = new();

    private bool _preferPublic = ResolverFeature.PREFER_PUBLIC.GetDefaultValue();
    private bool _preferPropertyFile = ResolverFeature.PREFER_PROPERTY_FILE.GetDefaultValue();
    private bool _allowCatalogPi = ResolverFeature.ALLOW_CATALOG_PI.GetDefaultValue();
    private CatalogManager? _manager = ResolverFeature.CATALOG_MANAGER.GetDefaultValue(); // also null
    private bool _uriForSystem = ResolverFeature.URI_FOR_SYSTEM.GetDefaultValue();
    private bool _mergeHttps = ResolverFeature.MERGE_HTTPS.GetDefaultValue();
    private bool _maskPackUris = ResolverFeature.MASK_PACK_URIS.GetDefaultValue();
    private string _catalogLoader = ResolverFeature.CATALOG_LOADER_CLASS.GetDefaultValue();
    private bool _parseRddl = ResolverFeature.PARSE_RDDL.GetDefaultValue();
    private bool _archivedCatalogs = ResolverFeature.ARCHIVED_CATALOGS.GetDefaultValue();
    private bool _useDataAssembly = ResolverFeature.USE_DATA_ASSEMBLY.GetDefaultValue();
    private bool _fixWindowsSystemIdentifiers = ResolverFeature.FIX_WINDOWS_SYSTEM_IDENTIFIERS.GetDefaultValue();
    private bool _showConfigChanges = false; // make the config process a bit less chatty

    /// <summary>
    /// Create a new configuration from defaults.
    /// </summary>
    public XmlResolverConfiguration() : this(null, null)
    {
        // nop
    }

    /// <summary>
    /// Create a new configuration using the specified catalog files.
    /// </summary>
    /// <para>For historical reasons, the list of catalogs must be semicolon separated in this string.</para>
    /// <para>This constructor uses the default properties.</para>
    /// <param name="catalogFiles">The list of catalog files.</param>
    public XmlResolverConfiguration(string catalogFiles)
        : this(null, new List<string>(Regex.Split(catalogFiles, @"\s*;\s*")))
    {
        // nop
    }

    /// <summary>
    /// Create a new configuration using the specified catalog files.
    /// </summary>
    /// <para>This constructor uses the default properties.</para>
    /// <param name="catalogFiles">The list of catalog files.</param>
    public XmlResolverConfiguration(List<string> catalogFiles) : this(null, catalogFiles)
    {
        // nop
    }

    /// <summary>
    /// Create a new configuration using the specified list of property files and list of catalog files.
    /// </summary>
    /// <para>The specified list of property files will be searched in order. The first property
    /// file that can be opened successfully will be used and the rest will be ignored.</para>
    /// <param name="propertyFiles"></param>
    /// <param name="catalogFiles"></param>
    public XmlResolverConfiguration(List<Uri>? propertyFiles, List<string>? catalogFiles)
    {
        var assembly = Assembly.GetExecutingAssembly();
        logger.Log(ResolverLogger.Config, "Assembly: {0}", assembly.FullName ?? "null");
        _showConfigChanges = false;
        _catalogs.Clear();
        _additionalCatalogs.Clear();
        _assemblyCatalogs.Clear();
        _builtinAssemblyCatalogs.Clear();
        _assemblyCache.Clear();
        LoadConfiguration(propertyFiles, catalogFiles);
        _showConfigChanges = true;
    }

    /// <summary>
    /// Create a new configuration by copying an existing one.
    /// </summary>
    /// <param name="current">The configuration to copy.</param>
    public XmlResolverConfiguration(XmlResolverConfiguration current)
    {
        _catalogs = new List<string>(current._catalogs);
        _additionalCatalogs = new List<string>(current._additionalCatalogs);
        _assemblyCatalogs = new List<string>(current._assemblyCatalogs);
        _builtinAssemblyCatalogs = new List<string>(current._builtinAssemblyCatalogs);
        _preferPublic = current._preferPublic;
        _preferPropertyFile = current._preferPropertyFile;
        _allowCatalogPi = current._allowCatalogPi;
        _manager = current._manager == null ? null : new CatalogManager(current._manager, this);

        _uriForSystem = current._uriForSystem;
        _mergeHttps = current._mergeHttps;
        _maskPackUris = current._maskPackUris;
        _catalogLoader = current._catalogLoader;
        _parseRddl = current._parseRddl;
        _archivedCatalogs = current._archivedCatalogs;
        _useDataAssembly = current._useDataAssembly;
        _showConfigChanges = current._showConfigChanges;
        _fixWindowsSystemIdentifiers = current._fixWindowsSystemIdentifiers;
        _accessExternalEntity = current._accessExternalEntity;
        _accessExternalDocument = current._accessExternalDocument;
    }

    private void LoadConfiguration(List<Uri>? propertyFiles, List<string>? catalogFiles)
    {
        LoadSystemPropertiesConfiguration();

        List<Uri> propertyFilesList = new();
        if (propertyFiles == null)
        {
            var propfn = Environment.GetEnvironmentVariable("XMLRESOLVER_APPSETTINGS");

            if (propfn is null or "")
            {
                // FIXME: load the system one
                // Maybe https://stackoverflow.com/questions/474055/c-sharp-equivalent-of-getclassloader-getresourceasstream
            }
            else
            {
                foreach (var fn in propfn.Split(";"))
                {
                    propertyFilesList.Add(UriUtils.Resolve(UriUtils.Cwd(), fn.Trim()));
                }
            }
        }
        else
        {
            propertyFilesList.AddRange(propertyFiles);
        }

        Uri? loaded = null;
        IConfigurationSection? section = null;
        foreach (var propFile in propertyFilesList)
        {
            if (loaded == null && "file".Equals(propFile.Scheme))
            {
                if (File.Exists(propFile.AbsolutePath))
                {
                    var cfgBuilder = new ConfigurationBuilder();
                    cfgBuilder.AddXmlFile(propFile.AbsolutePath);
                    try
                    {
                        var config = cfgBuilder.Build();
                        loaded = propFile;
                        section = config.GetSection("xmlResolver");
                    }
                    catch (Exception ex)
                    {
                        if (ex is InvalidDataException or XmlException)
                        {
                            // Maybe it's a JSON file?
                            cfgBuilder = new ConfigurationBuilder();
                            cfgBuilder.AddJsonFile(propFile.AbsolutePath);
                            try
                            {
                                var config = cfgBuilder.Build();
                                section = config.GetSection("XmlResolver");
                                loaded = propFile;
                            }
                            catch (FormatException)
                            {
                                logger.Log(ResolverLogger.Config, "Failed to read {0}", propFile.AbsolutePath);
                            }
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
            else
            {
                logger.Log(ResolverLogger.Error, "Property URI is not a file: URI: {0}", propFile.ToString());
            }
        }

        if (loaded != null && section != null)
        {
            LoadPropertiesConfiguration(loaded, section);
        }

        if (catalogFiles == null)
        {
            if (_catalogs.Count == 0)
            {
                _catalogs.Add("./catalog.xml");
            }
        }
        else
        {
            _catalogs.Clear();
            foreach (var fn in catalogFiles)
            {
                if (!"".Equals(fn.Trim()))
                {
                    _catalogs.Add(fn.Trim());
                }
            }
        }

        if (_useDataAssembly)
        {
            _builtinAssemblyCatalogs.Add(_xmlResolverDataAssembly);
        }
    }

    private void LoadSystemPropertiesConfiguration()
    {
        // C# doesn't have "system properties". Environment variables seem like a plausible workaround...
        string? property = Environment.GetEnvironmentVariable("XML_CATALOG_FILES");
        if (property != null)
        {
            if (_showConfigChanges)
            {
                logger.Log(ResolverLogger.Config, "Catalog list cleared");
            }

            _catalogs.Clear();
            foreach (var token in property.Split(";"))
            {
                if (!"".Equals(token.Trim()))
                {
                    if (_showConfigChanges)
                    {
                        logger.Log(ResolverLogger.Config, "Catalog: {0}", token);
                    }

                    _catalogs.Add(token);
                }

            }
        }

        property = Environment.GetEnvironmentVariable("XML_CATALOG_ADDITIONS");
        if (property != null)
        {
            foreach (var token in property.Split(";"))
            {
                if (!"".Equals(token.Trim()))
                {
                    if (_showConfigChanges)
                    {
                        logger.Log(ResolverLogger.Config, "Catalog: {0}", token);
                    }

                    _additionalCatalogs.Add(token);
                }
            }
        }

        property = Environment.GetEnvironmentVariable("XML_CATALOG_PREFER");
        if (property != null)
        {
            if (_showConfigChanges)
            {
                logger.Log(ResolverLogger.Config, "Prefer public: {0}", property);
            }

            _preferPublic = "public".Equals(property.ToLower());
        }

        property = Environment.GetEnvironmentVariable("XML_CATALOG_ACCESS_EXTERNAL_ENTITY");
        if (property != null)
        {
            if (_showConfigChanges)
            {
                logger.Log(ResolverLogger.Config, "Access external entities: {0}", property);
            }

            _accessExternalEntity = property;
        }

        property = Environment.GetEnvironmentVariable("XML_CATALOG_ACCESS_EXTERNAL_DOCUMENT");
        if (property != null)
        {
            if (_showConfigChanges)
            {
                logger.Log(ResolverLogger.Config, "Access external documents: {0}", property);
            }

            _accessExternalDocument = property;
        }

        SetBoolean("XML_CATALOG_PREFER_PROPERTY_FILE", "Prefer propertyFile: {0}", ref _preferPropertyFile);
        SetBoolean("XML_CATALOG_ALLOW_PI", "Allow catalog PI: {0}", ref _allowCatalogPi);

        SetBoolean("XML_CATALOG_URI_FOR_SYSTEM", "URI-for-system: {0}", ref _uriForSystem);
        SetBoolean("XML_CATALOG_MERGE_HTTPS", "Merge https: {0}", ref _mergeHttps);
        SetBoolean("XML_CATALOG_MASK_PACK_URIS", "Mask-pack-URIs: {0}", ref _maskPackUris);
        SetBoolean("XML_CATALOG_FIX_WINDOWS_SYSTEM_IDENTIFIERS", "Fix Windows system identifiers: {0}",
            ref _fixWindowsSystemIdentifiers);

        property = Environment.GetEnvironmentVariable("XML_CATALOG_LOADER_CLASS");
        if (property != null)
        {
            if (_showConfigChanges)
            {
                logger.Log(ResolverLogger.Config, "Catalog loader: {0}", property);
            }

            _catalogLoader = property;
        }

        SetBoolean("XML_CATALOG_PARSE_RDDL", "Use RDDL: {0}", ref _parseRddl);
        SetBoolean("XML_CATALOG_ARCHIVED_CATALOGS", "Use archived catalogs: {0}", ref _archivedCatalogs);
        SetBoolean("XML_CATALOG_USE_DATA_ASSEMBLY", "Use data assembly: {0}", ref _useDataAssembly);
    }

    private void SetBoolean(string name, string desc, ref bool value)
    {
        var property = Environment.GetEnvironmentVariable(name);
        if (property == null)
        {
            return;
        }

        if (_showConfigChanges)
        {
            logger.Log(ResolverLogger.Config, desc, property);
        }

        value = IsTrue(property);
    }

    private bool IsTrue(string? value)
    {
        if (value == null)
        {
            return false;
        }
        return "true".Equals(value) || "yes".Equals(value) || "1".Equals(value);
    }

    private void LoadPropertiesConfiguration(Uri propertyFile, IConfigurationSection section)
    {
        // In Java, we set a system property to communicate with the logger.
        // We can't do that in C# so I'm not sure what to do here...
        var property = section.GetSection("catalogLogging");
        if (property.Value != null)
        {
            ResolverLogger.CatalogLogging = property.Value;
        }

        bool relative = true;
        property = section.GetSection("relativeCatalogs");
        if (property.Value != null)
        {
            relative = IsTrue(property.Value);
        }

        if (_showConfigChanges)
        {
            logger.Log(ResolverLogger.Config, "Relative catalogs: {0}", relative.ToString());
        }

        property = section.GetSection("catalogs");
        if (property.Value != null)
        {
            string[] tokens = property.Value.Split(";");
            _catalogs.Clear();
            if (_showConfigChanges)
            {
                logger.Log(ResolverLogger.Config, "Catalog list cleared");
            }

            foreach (var token in tokens)
            {
                if (!"".Equals(token))
                {
                    string caturi = token;
                    if (relative && propertyFile != null)
                    {
                        caturi = UriUtils.Resolve(propertyFile, caturi).ToString();
                    }

                    if (_showConfigChanges)
                    {
                        logger.Log(ResolverLogger.Config, "Catalog: {0}", caturi);
                    }

                    _catalogs.Add(caturi);
                }
            }
        }

        property = section.GetSection("catalogAdditions");
        if (property.Value != null)
        {
            string[] tokens = property.Value.Split(";");
            foreach (var token in tokens)
            {
                if (!"".Equals(token))
                {
                    string caturi = token;
                    if (relative && propertyFile != null)
                    {
                        caturi = UriUtils.Resolve(propertyFile, caturi).ToString();
                    }

                    if (_showConfigChanges)
                    {
                        logger.Log(ResolverLogger.Config, "Catalog: {0}", caturi);
                    }

                    _additionalCatalogs.Add(caturi);
                }
            }
        }

        property = section.GetSection("prefer");
        if (property.Value != null)
        {
            if (_showConfigChanges)
            {
                logger.Log(ResolverLogger.Config, "Prefer public: {0}", property.Value);
            }

            _preferPublic = "public".Equals(property.Value.ToLower());
        }

        property = section.GetSection("accessExternalEntity");
        if (property.Value != null)
        {
            if (_showConfigChanges)
            {
                logger.Log(ResolverLogger.Config, "Access external entities: {0}", property.Value);
            }

            _accessExternalEntity = property.Value;
        }

        property = section.GetSection("accessExternalDocument");
        if (property.Value != null)
        {
            if (_showConfigChanges)
            {
                logger.Log(ResolverLogger.Config, "Access external documents: {0}", property.Value);
            }

            _accessExternalDocument = property.Value;
        }

        SetPropertyBoolean(section.GetSection("preferPropertyFile"), "Prefer propertyFile: {0}",
            ref _preferPropertyFile);
        SetPropertyBoolean(section.GetSection("allowOasisXmlCatalogPi"), "Allow catalog PI: {0}", ref _allowCatalogPi);

        SetPropertyBoolean(section.GetSection("uriForSystem"), "URI-for-system: {0}", ref _uriForSystem);
        SetPropertyBoolean(section.GetSection("mergeHttps"), "Merge https: {0}", ref _mergeHttps);
        SetPropertyBoolean(section.GetSection("maskPackUris"), "Mask-pack-URIs: {0}", ref _maskPackUris);
        SetPropertyBoolean(section.GetSection("fixWindowsSystemIdentifiers"), "Fix Windows system identifiers: {0}",
            ref _fixWindowsSystemIdentifiers);

        property = section.GetSection("catalogLoaderClass");
        if (property.Value != null)
        {
            if (_showConfigChanges)
            {
                logger.Log(ResolverLogger.Config, "Catalog loader: {0}", property.Value);
            }

            _catalogLoader = property.Value;
        }

        SetPropertyBoolean(section.GetSection("parseRddl"), "Use RDDL: {0}", ref _parseRddl);
        SetPropertyBoolean(section.GetSection("archivedCatalogs"), "Use archived catalogs: {0}", ref _archivedCatalogs);
        SetPropertyBoolean(section.GetSection("useDataAssembly"), "Use data assembly: {0}", ref _useDataAssembly);
    }

    private void SetPropertyBoolean(IConfigurationSection section, string desc, ref bool value)
    {
        var property = section.Value;
        if (property == null)
        {
            return;
        }

        if (_showConfigChanges)
        {
            logger.Log(ResolverLogger.Config, desc, property);
        }

        value = IsTrue(property);
    }

    public void AddCatalog(string? catalog)
    {
        if (catalog == null)
        {
            return;
        }
        
        lock (_syncLock)
        {
            _catalogs.Add(catalog);
        }
    }

    public void AddAssemblyCatalog(string path)
    {
        AddAssemblyCatalog(path, Assembly.GetExecutingAssembly());
    }

    public void AddAssemblyCatalog(string path, Assembly asm)
    {
        var cat = UriUtils.GetLocationUri(path, asm);
        AddCatalog(cat.ToString());
    }

    public void AddCatalog(Uri catalog, Stream data)
    {
        if (catalog == null)
        {
            throw new ArgumentNullException(nameof(catalog));
        }

        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        var uri = UriUtils.Resolve(UriUtils.Cwd(), catalog);
        lock (_syncLock)
        {
            _catalogs.Add(uri.ToString());
            _manager ??= (CatalogManager?)GetFeature(ResolverFeature.CATALOG_MANAGER);
            _manager?.LoadCatalog(uri, data);
        }
    }

    public void RemoveCatalog(string? catalog)
    {
        if (catalog == null)
        {
            return;
        }

        lock (_syncLock)
        {
            _catalogs.Remove(catalog);
        }
    }

    private List<string> UriSchemesAsList(string? value)
    {
        if (value is null or "all")
        {
            return new List<string> { "all" };
        }

        var elements = Regex.Split(value, @"\s+");
        return elements.Contains("all") ? new List<string> { "all" } : new List<string>(elements);
    }

    public void SetFeature(ResolverFeature feature, object? value)
    {
        if (feature == ResolverFeature.CATALOG_FILES)
        {
            lock (_syncLock)
            {
                _catalogs.Clear();
                if (value != null)
                {
                    _catalogs.AddRange((List<string>)value);
                }
            }

            return;
        }

        if (feature == ResolverFeature.CATALOG_ADDITIONS)
        {
            lock (_syncLock)
            {
                _additionalCatalogs.Clear();
                if (value != null)
                {
                    _additionalCatalogs.AddRange((List<string>)value);
                }
            }

            return;
        }

        if (feature == ResolverFeature.ASSEMBLY_CATALOGS)
        {
            switch (value)
            {
                case null:
                    _assemblyCatalogs.Clear();
                    _assemblyCache.Clear();
                    break;
                case string s:
                    _assemblyCatalogs.Add(s);
                    break;
                case List<string> list:
                    _assemblyCatalogs.Clear();
                    _assemblyCatalogs.AddRange(list);
                    break;
                default:
                    throw new ArgumentException("Invalid value for ASSEMBLY_CATALOGS feature");
            }

            return;
        }

        if (feature == ResolverFeature.ACCESS_EXTERNAL_ENTITY)
        {
            if (value == null)
            {
                _accessExternalEntity = "all";
            }
            else
            {
                _accessExternalEntity = (string)value;
            }
            return;
        }

        if (feature == ResolverFeature.ACCESS_EXTERNAL_DOCUMENT)
        {
            if (value == null)
            {
                _accessExternalDocument = "all";
            }
            else
            {
                _accessExternalDocument = (string)value;
            }
            return;
        }

        if (value == null)
        {
            throw new NullReferenceException(feature.GetFeatureName() + " must not be null");
        }

        if (feature == ResolverFeature.PREFER_PUBLIC)
        {
            _preferPublic = (bool)value;
        }
        else if (feature == ResolverFeature.PREFER_PROPERTY_FILE)
        {
            _preferPropertyFile = (bool)value;
        }
        else if (feature == ResolverFeature.ALLOW_CATALOG_PI)
        {
            _allowCatalogPi = (bool)value;
        }
        else if (feature == ResolverFeature.CATALOG_MANAGER)
        {
            _manager = (CatalogManager)value;
        }
        else if (feature == ResolverFeature.URI_FOR_SYSTEM)
        {
            _uriForSystem = (bool)value;
        }
        else if (feature == ResolverFeature.MERGE_HTTPS)
        {
            _mergeHttps = (bool)value;
        }
        else if (feature == ResolverFeature.MASK_PACK_URIS)
        {
            _maskPackUris = (bool)value;
        }
        else if (feature == ResolverFeature.CATALOG_LOADER_CLASS)
        {
            _catalogLoader = (string)value;
        }
        else if (feature == ResolverFeature.PARSE_RDDL)
        {
            _parseRddl = (bool)value;
        }
        else if (feature == ResolverFeature.ARCHIVED_CATALOGS)
        {
            _archivedCatalogs = (bool)value;
        }
        else if (feature == ResolverFeature.USE_DATA_ASSEMBLY)
        {
            _useDataAssembly = (bool)value;
            // Remove the XmlResolverData assembly if it's in the list
            List<string> updatedCatalogs = new();
            foreach (string catalog in _builtinAssemblyCatalogs)
            {
                if (catalog != _xmlResolverDataAssembly)
                {
                    updatedCatalogs.Add(catalog);
                }
            }

            _builtinAssemblyCatalogs.Clear();
            _builtinAssemblyCatalogs.AddRange(updatedCatalogs);
            // Put the XmlResolverData assembly in the list if it's enabled
            if (_useDataAssembly)
            {
                _builtinAssemblyCatalogs.Add(_xmlResolverDataAssembly);
            }
        }
        else if (feature == ResolverFeature.FIX_WINDOWS_SYSTEM_IDENTIFIERS)
        {
            _fixWindowsSystemIdentifiers = (bool)value;
        }
        else
        {
            logger.Log(ResolverLogger.Error, "Ignoring unknown feature: %s", feature.GetFeatureName());
        }
    }

    public object? GetFeature(ResolverFeature feature)
    {
        if (feature == ResolverFeature.CATALOG_MANAGER)
        {
            // Delay construction of the default catalog manager until it's
            // requested. If the user sets it before asking for it, we'll
            // never have to construct it.
            if (_manager == null)
            {
                _manager = new CatalogManager(this);
            }

            return _manager;
        }

        if (feature == ResolverFeature.CATALOG_FILES)
        {
            List<string>? cats = null;
            lock (_syncLock)
            {
                // Let's avoid returning duplicates...
                cats = new();
                foreach (string cat in _catalogs)
                {
                    if (!cats.Contains(cat))
                    {
                        cats.Add(cat);
                    }
                }

                foreach (string cat in _additionalCatalogs)
                {
                    if (!cats.Contains(cat))
                    {
                        cats.Add(cat);
                    }
                }

                foreach (string asm in _assemblyCatalogs)
                {
                    string? cat = LoadAssemblyCatalog(asm);
                    if (cat != null && !cats.Contains(cat))
                    {
                        cats.Add(cat);
                    }
                }

                foreach (string asm in _builtinAssemblyCatalogs)
                {
                    string? cat = LoadAssemblyCatalog(asm);
                    if (cat != null && !cats.Contains(cat))
                    {
                        cats.Add(cat);
                    }
                }
            }

            return cats;
        }

        if (feature == ResolverFeature.CATALOG_ADDITIONS)
        {
            List<string>? cats = null;
            lock (_syncLock)
            {
                cats = new(_additionalCatalogs);
            }

            return cats;
        }

        if (feature == ResolverFeature.PREFER_PUBLIC)
        {
            return _preferPublic;
        }

        if (feature == ResolverFeature.PREFER_PROPERTY_FILE)
        {
            return _preferPropertyFile;
        }

        if (feature == ResolverFeature.ALLOW_CATALOG_PI)
        {
            return _allowCatalogPi;
        }

        if (feature == ResolverFeature.URI_FOR_SYSTEM)
        {
            return _uriForSystem;
        }

        if (feature == ResolverFeature.MERGE_HTTPS)
        {
            return _mergeHttps;
        }

        if (feature == ResolverFeature.MASK_PACK_URIS)
        {
            return _maskPackUris;
        }

        if (feature == ResolverFeature.CATALOG_LOADER_CLASS)
        {
            return _catalogLoader;
        }

        if (feature == ResolverFeature.PARSE_RDDL)
        {
            return _parseRddl;
        }

        if (feature == ResolverFeature.ARCHIVED_CATALOGS)
        {
            return _archivedCatalogs;
        }

        if (feature == ResolverFeature.ASSEMBLY_CATALOGS)
        {
            return _assemblyCatalogs;
        }

        if (feature == ResolverFeature.USE_DATA_ASSEMBLY)
        {
            return _useDataAssembly;
        }

        if (feature == ResolverFeature.FIX_WINDOWS_SYSTEM_IDENTIFIERS)
        {
            return _fixWindowsSystemIdentifiers;
        }

        if (feature == ResolverFeature.ACCESS_EXTERNAL_ENTITY)
        {
            return _accessExternalEntity;
        }

        if (feature == ResolverFeature.ACCESS_EXTERNAL_DOCUMENT)
        {
            return _accessExternalDocument;
        }

        logger.Log(ResolverLogger.Error, "Ignoring unknown feature: %s", feature.GetFeatureName());
        return null;
    }

    public List<ResolverFeature> GetFeatures()
    {
        return new(knownFeatures);
    }

    private string? LoadAssemblyCatalog(string asm)
    {
        var asmLoc = AssemblyUriPrefix + asm;
        if (_assemblyCache.TryGetValue(asmLoc, out var catalog))
        {
            return catalog;
        }

        try
        {
            var asmName = new AssemblyName(asm);
            var xAssembly = Assembly.Load(asmName);

            foreach (var file in xAssembly.GetManifestResourceNames())
            {
                var catRsrc = _newCatrsrc == file ? _newCatrsrc : _oldCatrsrc;
                if (catRsrc != file)
                {
                    continue;
                }
                var dllLoc = UriUtils.GetLocationUri(catRsrc, xAssembly).ToString();
                _assemblyCache.Add(asmLoc, dllLoc);
                return dllLoc;
            }

            logger.Log(ResolverLogger.Config, "Failed to load catalog from assembly: {0}", asm);
            return null;
        }
        catch (Exception)
        {
            logger.Log(ResolverLogger.Config, "Failed to load assembly: {0}", asm);
            return null;
        }
    }
}