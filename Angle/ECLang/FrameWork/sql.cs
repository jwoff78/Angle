namespace ECLang.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using SharpHsql;

    public class sql
    {
        private Database db;

        private string errormsg;

        public void select_db(string name)
        {
            this.db = new Database(name);
        }

        public Channel connect(string username, string password)
        {
            return this.db.Connect(username, password);
        }

        public Result query(Channel myChannel, string query)
        {
            var r = this.db.Execute(query, myChannel);

            this.errormsg = r.Error;
            return r;
        }

        public void close(Channel myChannel)
        {
            this.db.Execute("shutdown", myChannel);
            myChannel.Disconnect();
        }

        public void auto_commit(Channel mc, bool b)
        {
            mc.SetAutoCommit(b);
        }

        public void rollback(Channel mc)
        {
            mc.Rollback();
        }

        public void commit(Channel myChannel)
        {
            myChannel.Commit();
        }

        public List<dynamic> fetch_object(Result rs)
        {
            var ret = new List<dynamic>();
            foreach (var kvp in this.fetch_array(rs))
            {
                var eo = new ExpandoObject();
                var eoColl = (ICollection<KeyValuePair<string, object>>)eo;
                foreach (var o in kvp)
                {
                    eoColl.Add(o);
                }
                ret.Add(eo);
            }
            return ret;
        }

        public List<Dictionary<string, object>> fetch_array(Result rs)
        {
            var ret = new List<Dictionary<string, object>>();
            if (rs.Root != null)
            {
                // Get the first one
                Record r = rs.Root;

                while (r != null)
                {
                    var x = new Dictionary<string, object>();

                    for (int index = 0; index < rs.Label.Length; index++)
                    {
                        var column = this.readableString(rs.Label[index]);
                        x[column] = r.Data[index];
                    }
                    
                    ret.Add(x);

                    r = r.Next;
                }
            }
            return ret;
        }

        public int num_rows(Result rs)
        {
            return rs.Root.Data.Length;
        }

        public int num_fields(Result rs)
        {
            return rs.Label.Length;
        }

        public string[] list_fields(Result rs)
        {
            return rs.Label.Select(this.readableString).ToArray();
        }

        public string escape_string(string s)
        {
            return Regex.Escape(s);
        }

        public string error()
        {
            return this.errormsg;
        }

        private string readableString(string src)
        {
            var s = src.ToLower();
            var sa = s.ToCharArray();
            sa[0] = Convert.ToChar(sa[0].ToString().ToUpper());

            return new string(sa);
        }
    }
}
