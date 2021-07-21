using System;
using System.IO;

namespace UnitTests {
    public class CacheManager : ResolverTest {
        public FileInfo ClearCache(string relpath) {
            string cache = Path.Combine(Environment.CurrentDirectory, relpath);
            if (File.Exists(cache)) {
                Directory.Delete(cache, true);
            }

            return new FileInfo(cache);
        }
    }
}