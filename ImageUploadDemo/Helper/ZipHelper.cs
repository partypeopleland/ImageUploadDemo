using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace ImageUploadDemo.Helper
{
    public static class ZipHelper
    {
        // 黑暗執行續：程式範例：byte[] 不落地壓縮 ZIP 檔
        // ref:https://blog.darkthread.net/blog/zip-byte-array/
        public static byte[] ZipData(Dictionary<string, byte[]> data)
        {
            using (var zipStream = new MemoryStream())
            {
                using (var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Update))
                {
                    foreach (var fileName in data.Keys)
                    {
                        var entry = zipArchive.CreateEntry(fileName);
                        using (var entryStream = entry.Open())
                        {
                            var buff = data[fileName];
                            entryStream.Write(buff, 0, buff.Length);
                        }
                    }
                }
                return zipStream.ToArray();
            }
        }

        public static Dictionary<string, byte[]> UnzipData(byte[] zip)
        {
            var dict = new Dictionary<string, byte[]>();
            using (var msZip = new MemoryStream(zip))
            {
                using (var archive = new ZipArchive(msZip, ZipArchiveMode.Read))
                {
                    archive.Entries.ToList().ForEach(entry =>
                    {
                        //e.FullName可取得完整路徑
                        if (string.IsNullOrEmpty(entry.Name)) return;
                        using (var entryStream = entry.Open())
                        {
                            using (var msEntry = new MemoryStream())
                            {
                                entryStream.CopyTo(msEntry);
                                dict.Add(entry.Name, msEntry.ToArray());
                            }
                        }
                    });
                }
            }
            return dict;
        }
    }
}