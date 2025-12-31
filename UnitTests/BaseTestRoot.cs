using System;
using System.IO;
using XmlResolver.Utils;

namespace UnitTests {
    public class BaseTestRoot {
        public readonly string TEST_ROOT_PATH;
        public readonly Uri TEST_ROOT_DIRECTORY;

        public BaseTestRoot() {
            // FIXME: Deal with Windows paths
            var path = Environment.GetEnvironmentVariable("GITHUB_WORKSPACE");
            if (string.IsNullOrEmpty(path)) {
                path = Environment.GetEnvironmentVariable("CSHARP_XMLRESOLVER_ROOT");
                if (string.IsNullOrEmpty(path))
                {
                    path = "/tmp"; // It won't work, but it's somewhere...
                }
            }

            while (path.EndsWith("/")) {
                path = path.Substring(0, path.Length - 1);
            }

            TEST_ROOT_PATH = path;
            TEST_ROOT_DIRECTORY = UriUtils.NewUri(TEST_ROOT_PATH + "/");
        }
    }
}