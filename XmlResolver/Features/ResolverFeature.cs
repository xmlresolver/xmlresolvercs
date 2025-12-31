using System;
using System.Collections.Generic;

namespace XmlResolver.Features {
    public abstract class ResolverFeature {
        private static Dictionary<string,ResolverFeature> _index = new ();
        private readonly string _name;
        private readonly Type _type;

        public ResolverFeature(string name, Type type) {
            _name = name;
            _type = type;
        }

        public String GetFeatureName() {
            return _name;
        }

        public Type GetFeatureType() {
            return _type;
        }

        public ResolverFeature ByName(string name) {
            return _index[name];
        }

        public List<string> GetNames() {
            return new(_index.Keys);
        }
        
        private static ListOfStringResolverFeature register(ListOfStringResolverFeature resolverFeature) {
            _index.Add(resolverFeature.GetFeatureName(), resolverFeature);
            return resolverFeature;
        }

        private static BoolResolverFeature register(BoolResolverFeature resolverFeature) {
            _index.Add(resolverFeature.GetFeatureName(), resolverFeature);
            return resolverFeature;
        }
        
        private static StringResolverFeature register(StringResolverFeature resolverFeature) {
            _index.Add(resolverFeature.GetFeatureName(), resolverFeature);
            return resolverFeature;
        }
        
        private static CatalogManagerResolverFeature register(CatalogManagerResolverFeature resolverFeature) {
            _index.Add(resolverFeature.GetFeatureName(), resolverFeature);
            return resolverFeature;
        }
        
        public static readonly ListOfStringResolverFeature CATALOG_FILES = register(new ListOfStringResolverFeature(
            "http://xmlresolver.org/feature/catalog-files", new()));

        public static readonly ListOfStringResolverFeature CATALOG_ADDITIONS = register(new ListOfStringResolverFeature(
            "http://xmlresolver.org/feature/catalog-additions", new()));

        public static readonly BoolResolverFeature PREFER_PUBLIC = register(new BoolResolverFeature(
            "http://xmlresolver.org/feature/prefer-public", true));

        public static readonly BoolResolverFeature PREFER_PROPERTY_FILE = register(new BoolResolverFeature(
            "http://xmlresolver.org/feature/prefer-property-file", false));

        public static readonly BoolResolverFeature ALLOW_CATALOG_PI = register(new BoolResolverFeature(
            "http://xmlresolver.org/feature/allow-catalog-pi", true));

        public static readonly CatalogManagerResolverFeature CATALOG_MANAGER = register(new CatalogManagerResolverFeature(
            "http://xmlresolver.org/feature/catalog-manager", null));

        public static readonly BoolResolverFeature URI_FOR_SYSTEM = register(new BoolResolverFeature(
            "http://xmlresolver.org/feature/uri-for-system", true));

        public static readonly BoolResolverFeature MERGE_HTTPS = register(new BoolResolverFeature(
            "http://xmlresolver.org/feature/merge-https", true));
        
        public static readonly StringResolverFeature CATALOG_LOADER_CLASS = register(new StringResolverFeature(
            "http://xmlresolver.org/feature/catalog-loader-class", "XmlResolver.Loaders.XmlLoader"));

        public static readonly BoolResolverFeature PARSE_RDDL = register(new BoolResolverFeature(
            "http://xmlresolver.org/feature/parse-rddl", true));

        public static readonly ListOfStringResolverFeature ASSEMBLY_CATALOGS = register(new ListOfStringResolverFeature(
            "http://xmlresolver.org/feature/assembly-catalog", new List<string> ()));

        public static readonly BoolResolverFeature ARCHIVED_CATALOGS = register(new BoolResolverFeature(
            "http://xmlresolver.org/feature/archived-catalogs", true));

        public static readonly BoolResolverFeature USE_DATA_ASSEMBLY = register(new BoolResolverFeature(
            "http://xmlresolver.org/feature/use-data-assembly", true));
            
        public static readonly BoolResolverFeature FIX_WINDOWS_SYSTEM_IDENTIFIERS = register(new BoolResolverFeature(
            "http://xmlresolver.org/feature/fix-windows-system-identifiers", false));

        public static readonly ListOfStringResolverFeature ACCESS_EXTERNAL_DOCUMENT = register(new ListOfStringResolverFeature(
            "http://xmlresolver.org/feature/access-external-document", null)); // null == all

        public static readonly ListOfStringResolverFeature ACCESS_EXTERNAL_ENTITY = register(new ListOfStringResolverFeature(
            "http://xmlresolver.org/feature/access-external-entity", null)); // null == all
    }
}