using System.Collections.Generic;
using System.Windows.Markup;

namespace ECLang.Internal.Binary.Modules
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Text;
    using System.Xaml;

    public class FilesystemModule : IModule
    {
        public List<IFSElement> Elements { get; set; }

        public Folder GetFolder(string name)
        {
            foreach (var fsElement in Elements)
            {
                if (fsElement is Folder)
                {
                    var f = fsElement as Folder;
                    if (f.Name == name) return f;
                }
            }
            return null;
        }

        public void AddFile(string name, byte[] content)
        {
            var f = new File();
            f.Name = name;
            f.Content = content;

            Elements.Add(f);
        }
        public void AddFile<TType>(string name, TType content)
        {
            if (content is string)
                AddFile(name, Encoding.ASCII.GetBytes(content as string));
            else if (content is Image)
            {
                var ms = new MemoryStream();
                var img = content as Image;
                img.Save(ms, ImageFormat.Jpeg);

                AddFile(name, ms.ToArray());
            }
        }

        public File GetFile(string name)
        {
            foreach (var c in Elements)
            {
                var child = c as File; 
                if (child.Name == name)
                {
                    return child;
                }
            }
            return null;
        }
        public TType GetFile<TType>(string name)
            where TType : class
        {
            if (typeof(TType).Name == typeof(string).Name)
            {
                return Encoding.ASCII.GetString(GetFile(name).Content) as TType;
            }
            if (typeof(TType).Name == typeof(Image).Name)
            {
                var ms = new MemoryStream(this.GetFile(name).Content);
                return Image.FromStream(ms) as TType;
            }
            return null;
        }

        public FilesystemModule()
        {
            Elements = new List<IFSElement>();
        }

        public void Write(BinaryWriter s)
        {
            var ms = new MemoryStream();
            XamlServices.Save(ms, Elements);

            s.Write(ms.ToArray().Length);
            s.Write(ms.ToArray());
        }

        public void Read(BinaryReader s)
        {
            var count = s.ReadInt32();

            var ms = new MemoryStream(s.ReadBytes(count));

            Elements = (List<IFSElement>)XamlServices.Load(ms);
        }
    }

    public interface IFSElement { }

    [ContentProperty("Childs")]
    public class Folder : IFSElement
    {
        public List<File> Childs { get; set; }
        public string Name { get; set; }

        public void AddFile(string name, byte[] content)
        {
            var f = new File();
            f.Name = name;
            f.Content = content;

            Childs.Add(f);
        }
        public void AddFile<TType>(string name, TType content)
        {
            if (content is string)
                AddFile(name, Encoding.ASCII.GetBytes(content as string));
            else if (content is Image)
            {
                var ms = new MemoryStream();
                var img = content as Image;
                img.Save(ms, ImageFormat.Jpeg);

                AddFile(name, ms.ToArray());
            }
        }

        public File GetFile(string name)
        {
            foreach (var child in Childs)
            {
                if (child.Name == name)
                {
                    return child;
                }
            }
            return null;
        }
        public TType GetFile<TType>(string name)
            where TType : class
        {
            if (typeof(TType).Name == typeof(string).Name)
            {
                return Encoding.ASCII.GetString(GetFile(name).Content) as TType;
            }
            if (typeof(TType).Name == typeof(Image).Name)
            {
                var ms = new MemoryStream(this.GetFile(name).Content);
                return Image.FromStream(ms) as TType;
            }
            return null;
        }
    }

    public class File : IFSElement
    {
        public string Name { get; set; }
        public byte[] Content { get; set; }
    }
}