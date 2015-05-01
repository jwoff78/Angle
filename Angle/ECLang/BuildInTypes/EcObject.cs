using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ECLang.BuildInTypes.Base;

namespace ECLang.BuildInTypes
{
   

    public class EcObject : EcBuiltInType
    {
        public object value = null;
        public static bool IsValid(string s)
        {
            return true;
        }
        public EcObject(object v)
        {
            try
            {

                value = Activator.CreateInstance((v as Type));
            }
            catch
            {
                value = v;
            }
        }
        public override object call(string data, List<object> perams)
        {
              List<Type> ls = new List<Type>();
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
             
              if (segmints.Length  > 2)
              {
                  tmp = value.GetType().GetProperty(segmints[1]).GetValue(value, null);
                  for (int index = 0; index < segmints.Length ; index++)
                  {
                      string i = segmints[index];
                      if (index != 0 && index != 1 && index != segmints.Length - 1)
                      {
                          tmp = tmp.GetType().GetProperty(i).GetValue(perams[0].GetType(), null);
                      }
                      if(index == segmints.Length - 1)
                      {
                          try
                          {
                              var b = tmp.GetType().GetMethod(i);
                              object[] args = perams.ToArray();
                              var v = b.Invoke(tmp, args);
                              tmp = v;
                          }
                          catch(Exception ee)
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
                  var rettt = value.GetType();
                  if(value is EcObject)
                  {
                      rettt = (value as EcObject).value.GetType();
                  }
                  else
                  {
                      rettt = value.GetType();
                  }
                  var rett = rettt.GetMethod(nn,ls.ToArray());
                  object res = null;
                  try
                  {
                      if (value is EcObject)
                      {
                          res = rett.Invoke((value as EcObject).value, perams.ToArray());
                      }
                      else
                      {
                          res = rett.Invoke(value, perams.ToArray());
                      }
                  }
                  catch
                  {
                      try
                      {
                          if (value is EcObject)
                          {
                              rett.Invoke((value as EcObject).value, perams.ToArray());
                          }
                          else
                          {
                              rett.Invoke(value, perams.ToArray());
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
        public override string ToString()
        {
            return value.ToString();
        }
    }
}
