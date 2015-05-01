using System.Collections.Generic;

namespace ECLang.Internal
{
    using ECLang.Internal.Binary.Modules;

    public class EcFileFormat
    {
        public string Version { get; set; }
        public FilesystemModule Filesystem { get; set; }
        public ResourcesModule Resources { get; set; }
        public List<byte[]> Dependencies { get; set; }

        public EcFileFormat()
        {
            Filesystem = new FilesystemModule();
            Dependencies = new List<byte[]>();
            Resources = new ResourcesModule();
        }
    }
}
