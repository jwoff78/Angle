using System.IO;

namespace ECLang.Internal.Binary
{
    using System.IO.Compression;

    public class Writer
    {
        public static void Save(Stream strm, EcFileFormat efFormat)
        {
            var bw = new BinaryWriter(new GZipStream(strm, CompressionMode.Compress));

            bw.Write(efFormat.Version);
            bw.Write(efFormat.Dependencies.Count);

            bw.Flush();

            efFormat.Filesystem.Write(bw);
            efFormat.Resources.Write(bw);

            bw.Flush();

            foreach (var dep in efFormat.Dependencies)
            {
                bw.Write(dep.Length);
                bw.Write(dep);
            }


            bw.Flush();
            bw.Close();
        }
    }
}
