using System.Collections.Generic;
using System.Windows.Markup;

namespace ECLang.Internal.Binary.Modules
{
    using System.IO;
    using System.Xaml;

    public class ResourcesModule : IModule
    {
        public List<File> Files { get; set; }

        public ResourcesModule()
        {
            Files = new List<File>();
        }

        public void Write(BinaryWriter s)
        {
            var ms = new MemoryStream();
            XamlServices.Save(ms, Files);

            s.Write(ms.ToArray().Length);
            s.Write(ms.ToArray());
        }

        public void Read(BinaryReader s)
        {
            var count = s.ReadInt32();

            var ms = new MemoryStream(s.ReadBytes(count));

            Files = (List<File>)XamlServices.Load(ms);
        }
    }
}