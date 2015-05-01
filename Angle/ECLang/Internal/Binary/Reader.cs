using System.IO;

namespace ECLang.Internal.Binary
{
    using System.IO.Compression;

    public class Reader
    {
        public static void Load(Stream strm, ref EcFileFormat ecf)
        {
            var br = new BinaryReader(new GZipStream(strm, CompressionMode.Decompress));
            ecf.Version = br.ReadString();

            var depC = br.ReadInt32();

            ecf.Filesystem.Read(br);
            ecf.Resources.Read(br);

            for (int i = 0; i < depC; i++)
            {
                var bC = br.ReadInt32();

                ecf.Dependencies.Add(br.ReadBytes(bC));
            }

            br.Close();
        }
    }
}
