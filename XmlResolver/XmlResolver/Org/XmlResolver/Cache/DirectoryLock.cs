using System;
using System.IO;
using System.Threading;

namespace Org.XmlResolver.Cache {
    public class DirectoryLock
    {
        public static int retryCount = 30; // ~15 seconds
        private FileStream lockStream = null;

        public DirectoryLock(string dir) {
            string lockfn = Path.Combine(dir, "lock");
            int count = retryCount;
            while (true) {
                try {
                    lockStream = File.Open(lockfn, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    return;
                }
                catch (IOException) {
                    count--;
                    if (count <= 0) {
                        throw;
                    }
                    Thread.Sleep(500);
                }
            }
        }

        ~DirectoryLock() {
            Release();
        }
        
        public void Release() {
            if (lockStream != null) {
                lockStream.Close();
                lockStream = null;
            }
        }
    }
}