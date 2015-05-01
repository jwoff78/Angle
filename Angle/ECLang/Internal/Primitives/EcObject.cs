namespace ECLang.Internal.Primitives
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using ECLang.Internal.Primitives.Base;

    public class EcObject : Primitive
    {
        #region Constructors and Destructors

        public EcObject(object value = null)
        {
            this.Name = "object";
            this.Value = value;
        }

        public EcObject()
        {
        }

        #endregion

        #region Public Methods and Operators

        public override Primitive Parse(object src)
        {
            return new EcObject(src);
        }

        public override bool Validate(object src)
        {
            return true;
        }

        public override object call(string data, List<object> perams)
        {
            var ls = new List<Type>();
            if (perams.Count != 0)
            {
                foreach (object i in perams)
                {
                    ls.Add(i.GetType());
                }
            }
            object tmp = null;
            object ret = null;

            string[] segmints = data.Split('.');

            if (segmints.Length > 2)
            {
                tmp = this.Value.GetType().GetProperty(segmints[1]).GetValue(this.Value, null);
                for (int index = 0; index < segmints.Length; index++)
                {
                    string i = segmints[index];
                    if (index != 0 && index != 1 && index != segmints.Length - 1)
                    {
                        tmp = tmp.GetType().GetProperty(i).GetValue(perams[0].GetType(), null);
                    }
                    if (index == segmints.Length - 1)
                    {
                        try
                        {
                            MethodInfo b = tmp.GetType().GetMethod(i);
                            object[] args = perams.ToArray();
                            object v = b.Invoke(tmp, args);
                            tmp = v;
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.InnerException.Message + " at " + data);
                        }
                    }
                }
                ret = new EcObject(tmp);
            }
            else
            {
                string nn = data.Split('.')[data.Split('.').Length - 1];
                Type rettt = this.Value.GetType();
                if (this.Value is EcObject)
                {
                    rettt = (this.Value as EcObject).Value.GetType();
                }
                else
                {
                    rettt = this.Value.GetType();
                }
                MethodInfo rett = rettt.GetMethod(nn, ls.ToArray());
                object res = null;
                try
                {
                    if (this.Value is EcObject)
                    {
                        res = rett.Invoke((this.Value as EcObject).Value, perams.ToArray());
                    }
                    else
                    {
                        res = rett.Invoke(this.Value, perams.ToArray());
                    }
                }
                catch
                {
                    try
                    {
                        if (this.Value is EcObject)
                        {
                            rett.Invoke((this.Value as EcObject).Value, perams.ToArray());
                        }
                        else
                        {
                            rett.Invoke(this.Value, perams.ToArray());
                        }
                    }
                    catch
                    {
                    }
                }
                if (res != null)
                {
                    if (res.GetType().Name != "Void")
                    {
                        ret = new EcObject(res);
                    }
                }
            }
            return ret;
        }

        #endregion
    }
}