namespace ECLang.Internal
{
    using System.Collections.Generic;

    using ECLang.Internal.Attributes.Base;

    public class AttributeContainer
    {
        private static readonly List<Attribute> attributes = new List<Attribute>(); 

        public static bool Add<T>()
            where T : Attribute, new()
        {
            if(attributes.Contains(new T()))
            {
                return false;
            }
            attributes.Add(new T());
            return true;
        }

        public static Attribute[] ToArray()
        {
            return attributes.ToArray();
        }
    }
}
