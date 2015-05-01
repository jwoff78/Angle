namespace ECLang.Internal.Binary
{
    using System.IO;

    public interface IModule
    {
        void Write(BinaryWriter s);

        void Read(BinaryReader s);
    }
}