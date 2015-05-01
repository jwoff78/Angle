namespace ECLang
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Windows.Forms;
    using System.Xaml;

    using ECLang.AST;
    using ECLang.AST.Statements;
    using ECLang.Framework;
    using ECLang.Internal;
    using ECLang.Internal.AST.Statements;
    using ECLang.Internal.Attributes;
    using ECLang.Internal.Primitives;
    using ECLang.Internal.Primitives.Base;
    using ECLang.Internal.Tables;

    using SharpHsql;

    using MathParser = ECLang.Internal.MathParser;

    public class Engine
    {
        #region Static Fields

        public static Dictionary<string, Delegate> ClrFunctions = new Dictionary<string, Delegate>();

        public static Dictionary<string, Type> ClrTypes = new Dictionary<string, Type>();

        public static Dictionary<string, object> ClrVariables = new Dictionary<string, object>();

        public static List<string> NameSpaceList = new List<string>();

        public static List<Assembly> includdlls = new List<Assembly>();

        public static string[] tokens = "b03f5f7f11d50a3a~b77a5c561934e089".Split('~');

        private static readonly MathParser mp = new MathParser();

        #endregion

        #region Fields

        public static  List<Method> EventHandlers = new List<Method>();

        public ExecutanFlags Flag = ExecutanFlags.Normal;

        public static bool StopExecutionFlag;

        #endregion

        #region Constructors and Destructors

        public Engine()
        {
            PrimitivesManager.LoadPrimitives();
        }

        #endregion

        #region Enums

        [Flags]
        public enum ExecutanFlags
        {
            RamOptimized,

            Strict,

            Normal
        }

        #endregion

        #region Public Properties

        public List<string> InloopVars = new List<string>(); 
        public bool InLoop = false;

        public Error[] Errors
        {
            get
            {
                return ErrorTable.ToArray();
            }
        }

        #endregion

        public static void InitDefaults()
        {
            ClrTypes.Add("WindowBuilder", typeof(WindowBuilder));

            var sql = new sql();

            ClrFunctions.Add("sql_connect", new Func<string, string, Channel>(sql.connect));
            ClrFunctions.Add("sql_select_db", new Action<string>(sql.select_db));
            ClrFunctions.Add("sql_disconnect", new Action<Channel>(sql.close));
            ClrFunctions.Add("sql_query", new Func<Channel, string, Result>(sql.query));

            ClrFunctions.Add("sql_fetch_object", new Func<Result, List<dynamic>>(sql.fetch_object));
            ClrFunctions.Add("sql_fetch_array", new Func<Result, List<Dictionary<string, object>>>(sql.fetch_array));

            ClrFunctions.Add("save_object", new Action<string, object>(XamlServices.Save));
            ClrFunctions.Add("restore_object", new Func<string, object>(XamlServices.Load));
        }

        #region Public Methods and Operators

        public static object CallDotNet(string myclass1, String mymethod, object[] perams)
        {
            Type mymethoda = null;
            string myclass = myclass1;
            string myclass11 = myclass1;
            string np1 = "";
            try
            {
                Boolean donpttry = true;
                foreach (string i in myclass1.Split('.'))
                {
                    foreach (string ii in myclass1.Split('.'))
                    {
                        if (NameSpaceList.Contains(i + "." + ii))
                        {
                            donpttry = false;
                            np1 = i + "." + ii;
                            break;
                        }
                    }
                }
                foreach (string np in NameSpaceList)
                {
                    myclass = myclass11;
                    if (!donpttry)
                    {
                        break;
                    }

                    //  myclass;
                    foreach (string i in tokens)
                    {
                        if (mymethoda == null)
                        {
                            mymethoda =
                                Type.GetType(
                                    np + "." + myclass1 + ", " + np
                                    + ", Version=4.0.0.0, Culture=neutral, PublicKeyToken=" + i);
                        }
                    }
                    if (mymethoda == null)
                    {
                        mymethoda = Type.GetType(np + "." + myclass1);
                    }
                    if (mymethoda == null)
                    {
                        foreach (Assembly i in includdlls)
                        {
                            if (mymethoda == null)
                            {
                                mymethoda = i.GetType(np + "." + myclass1);
                            }
                        }
                    }
                    if (mymethoda == null)
                    {
                        myclass = np + "." + myclass;
                        mymethoda = typeof(int).Assembly.GetType(myclass.Split('.')[0] + "." + myclass.Split('.')[1]);
                    }
                    if (mymethoda != null)
                    {
                        break;
                    }
                }
                if (!donpttry)
                {
                    mymethoda =
                        Type.GetType(
                            myclass1 + ", " + np1
                            + ", Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");

                    if (mymethoda == null)
                    {
                        mymethoda = Type.GetType(myclass1);
                    }

                    if (mymethoda == null)
                    {
                        myclass = myclass;
                        mymethoda = typeof(int).Assembly.GetType(myclass.Split('.')[0] + "." + myclass.Split('.')[1]);
                    }
                }
                if (mymethoda == null)
                {
                    foreach (Assembly i in includdlls)
                    {
                        if (mymethoda == null)
                        {
                            mymethoda = i.GetType(myclass1);
                        }
                    }
                }
                if (mymethoda != null)
                {
                    object tmp = "";
                    object ret = null;
                    var ls = new List<Type>();
                    if (perams.Length != 0)
                    {
                        foreach (object i in perams)
                        {
                            ls.Add(i.GetType());
                        }
                    }
                    string[] segmints = myclass.Split('.');
                    if (segmints.Length > 2)
                    {
                        for (int index = 0; index < segmints.Length; index++)
                        {
                            string i = segmints[index];
                            if (index != 0 && index != 1)
                            {
                                tmp = mymethoda.GetProperty(i).GetValue(Activator.CreateInstance(mymethoda), null);
                            }
                        }
                        ret = new EcObject(Convert.ToString(tmp));
                    }
                    else
                    {
                        object rett =
                            mymethoda.GetMethod(
                                mymethod, BindingFlags.Static | BindingFlags.Public, null, ls.ToArray(), null).Invoke(
                                    mymethoda, perams);
                        ret = new EcObject(Convert.ToString(rett));
                    }
                    return ret;
                }
            }
            catch
            {
            }

            return null;
        }

        public static void CallEventHandler(string n)
        {
            try
            {
                Program.e.ResolveTreeNode(EventHandlers[Convert.ToInt32(n)].Nodes);
            }
            catch (Exception ee)
            {
                StopExecutionFlag = true;
                Console.WriteLine("Event Handler Error : " + ee.Message);
            }
        }

        public static List<object> ConvertEcToCs(Primitive[] vars)
        {
            var retursn = new List<object>();

            foreach (Primitive i in vars)
            {
                if (SymbolTable.Contains((Convert.ToString(i).TrimEnd('"').TrimStart('"'))))
                {
                    retursn.Add((SymbolTable.Get(Convert.ToString(i).TrimEnd('"').TrimStart('"')) as EcObject).Value);
                }
                else
                {
                    try
                    {
                        if (i is EcString)
                        {
                            retursn.Add(i.ToString());
                        }
                        if (i is EcObject)
                        {
                            if ((i as EcObject).Value is Method)
                            {
                                retursn.Add(((i as EcObject).Value as Method));
                            }
                            else
                            {
                                retursn.Add((i as EcObject).Value);
                            }
                        }
                        if (i is EcNumber)
                        {
                            retursn.Add(Convert.ToInt32(i.ToString()));
                        }
                        if (i is EcBool)
                        {
                            retursn.Add(Convert.ToBoolean((i as EcBool).Value));
                        }
                            /*   if (Char.IsNumber(i.ToString()[0]) && !i.ToString().Contains(" "))
                    {
                        retursn.Add(Convert.ToInt32(i.ToString()));
                    }*/
                    }
                    catch
                    {
                    }
                }
            }

            return retursn;
        }

        public static Type GetObject(string name)
        {
            name = name.Split('(')[0];
            foreach (string np in NameSpaceList)
            {
                try
                {
                    string myclass = name;

                    //  myclass;
                    Type mymethoda = null;
                    foreach (string i in tokens)
                    {
                        if (mymethoda == null)
                        {
                            mymethoda =
                                Type.GetType(
                                    np + "." + name + ", " + np + ", Version=4.0.0.0, Culture=neutral, PublicKeyToken="
                                    + i);
                        }
                    }

                    if (mymethoda == null)
                    {
                        mymethoda = Type.GetType(np + "." + name);
                    }
                    if (mymethoda == null)
                    {
                        myclass = np + "." + myclass;
                        mymethoda = typeof(int).Assembly.GetType(myclass.Split('.')[0] + "." + myclass.Split('.')[1]);
                    }
                    if (mymethoda == null)
                    {
                        foreach (Assembly i in includdlls)
                        {
                            if (mymethoda == null)
                            {
                                mymethoda = i.GetType(np + "." + name);
                            }
                        }
                    }
                    if (mymethoda != null)
                    {
                        return mymethoda;
                    }
                }
                catch
                {
                }
            }
            return null;
        }

        public static Primitive HandleOneParameter(Primitive value)
        {
            //TODO: this code need a shit load of clean up and bug fixing after primitivemanager implemt
            Primitive dec = value;

            

            string val = dec.ToString();
            if ((dec is EcString) || (value is EcObject))
            {
                string name = "";
                if ((value is EcObject))
                {
                    name = (value as EcObject).Value as string;
                }
                if ((value is EcString))
                {
                    val = (value as EcString).ToString();
                }

                if (SymbolTable.Contains(name))
                {
                    return SymbolTable.Get(name);
                }
                if (SymbolTable.Contains(val))
                {
                    return SymbolTable.Get(val);
                }
                try
                {
                    //Convert.ToString(SymbolTable.Get(m.Value))
                    if (!val.Contains("NOMATH "))
                    {
                        val = MathsExspretionHandler.ParseValue(val);
                    }
                }
                catch
                {
                }
                try
                {
                    if (!val.Contains("NOMATH "))
                    {
                        return new EcNumber().Parse(Convert.ToString(mp.Parse(val)));
                    }
                    else
                    {
                        val = val.Replace("NOMATH ", "");
                        name = name.Replace("NOMATH ", "");
                       new  EcNumber().Parse(Convert.ToString(mp.Parse("eclang")));
                        dec = new EcString(name.Replace("NOMATH ", ""), false);
                        return new EcNumber().Parse(Convert.ToString(mp.Parse("eclang")));
                    }
                }
                catch
                {
                    if (Parser.Grammar.GetPattern("name").IsValid(name) && !name.StartsWith("\"")
                        && !name.ToLower().StartsWith("new") && !name.Contains("."))
                    {
                        if (SymbolTable.Contains(name))
                        {
                            return new EcObject((SymbolTable.Get(name) as EcObject).Value);
                        }
                        if (FunctionTable.Contains(name))
                        {
                            Method valuee = FunctionTable.Get(name);
                            return new EcObject(valuee);
                        }
                    }
                    if (name.ToLower().StartsWith("new"))
                    {
                        return
                            new EcObject(
                                Activator.CreateInstance(
                                    GetObject(val.Replace("new", "").Replace("New", "").TrimEnd(' ').TrimStart(' '))));
                    }
                    if (name.ToLower().Contains(".") && !name.ToLower().Contains(",") && !name.ToLower().Contains("("))
                    {
                        //handle all cases
                        return
                            new EcObject(
                                (SymbolTable.Get(name.Split('.')[0]) as EcObject).Value.GetType().GetProperty(
                                    name.Split('.')[1]).GetValue(
                                        (SymbolTable.Get(name.Split('.')[0]) as EcObject).Value, new object[] { }));
                    }
                    if (!name.StartsWith("\"") && Parser.Grammar.GetPattern("objcl").IsValid(name) || name.Split('(')[0].Split('.').Length >= 2)
                    {
                        var dec1 = (new ObjCallStmt().Interprete(value.ToString(), 0) as ObjCallStmt);
                        dec1.Target = dec1.Target.Replace("NOMATH ", "");
                        if (SymbolTable.Contains(dec1.Target))
                        {
                            var obj = SymbolTable.Get(dec1.Target);
                            var callvalue = obj.call(
                                        dec1.Name, ConvertEcToCs(dec1.Paramaters.ToArray()));
                            return
                                new EcObject(callvalue);

                        }
                        if (ClrVariables.ContainsKey(dec1.Target))
                        {
                            if (ClrVariables[dec1.Target] is EcCustomClass)
                            {
                                var m = Activator.CreateInstance(ClrVariables[dec1.Target].GetType()) as EcCustomClass;

                                if (dec1.Paramaters.Count == 1)
                                {
                                    return
                                        new EcObject(
                                            Convert.ToString(m.Call(dec1.Name, HandleOneParameter(dec1.Paramaters[0]))));
                                }
                                if (dec1.Paramaters.Count == 0)
                                {
                                    return new EcObject(Convert.ToString(m.Call(dec1.Name, new Primitive[] { })));
                                }
                                else if (dec1.Paramaters.Count != 0)
                                {
                                    return
                                        new EcObject(
                                            Convert.ToString(
                                                m.Call(dec1.Name, HandleParametersArray(dec1.Paramaters).ToArray())));
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                if (dec1.Paramaters.Count == 1)
                                {
                                    return
                                        new EcObject(
                                            Convert.ToString(
                                                CallDotNet(
                                                    dec1.Target,
                                                    dec1.Name,
                                                    ConvertEcToCs(new[] { HandleOneParameter(dec1.Paramaters[0]) }).
                                                        ToArray())));
                                }
                                if (dec1.Paramaters.Count == 0)
                                {
                                    return
                                        new EcObject(
                                            Convert.ToString(CallDotNet(dec1.Target, dec1.Name, new object[] { })));
                                }
                                else if (dec1.Paramaters.Count != 0)
                                {
                                    return
                                        new EcObject(
                                            Convert.ToString(
                                                CallDotNet(
                                                    dec1.Target,
                                                    dec1.Name,
                                                    ConvertEcToCs(HandleParametersArray(dec1.Paramaters).ToArray()).
                                                        ToArray())));
                                }
                            }
                            catch (Exception)
                            {
                                //  throw new Exception("Function expected");
                            }
                        }
                        if (SymbolTable.Contains(name.Split('.')[0]))
                        {
                            return new EcObject((SymbolTable.Get(name.Split('.')[0]) as EcObject).Value);
                        }
                    }

                    return new EcString(val.TrimEnd('\n').TrimStart('\n'), false);
                }
            }

            else
            {
                if (dec is Primitive)
                {
                    return dec;
                }
            }
            return null;
        }

        public static List<Primitive> HandleParametersArray(List<Primitive> lst)
        {
            List<Primitive> Params = lst;
            for (int x = 0; x < Params.Count; x++)
            {
                Params[x] = HandleOneParameter(Params[x]);
            }
            return Params;
        }

        public void AddItem<TValue>(string name, TValue value)
        {
            if (value is Delegate)
            {
                this.AddFunction(name, value as Delegate);
            }
            else if (value is Type)
            {
                this.AddType(name, value as Type);
            }
            else
            {
                this.AddVar(name, value);
            }
        }

        public object Call(string methname, params object[] args)
        {
            return null;
        }

        public void Accept(IVisitor visitor)
        {
            foreach (var item in Parser.Nodes)
            {
                item.Accept(visitor);
            }
        }

        public object Evaluate(string code)
        {
         
           // this.StopExecutionFlag = false;
            Parser.Init();
            Parser.Execute(code);

            foreach (CodeBlock i in Parser.CodeBlocks)
            {
                FunctionTable.Store(i.Name, new Method { Nodes = i.Nodes, returns = i.Returns });
            }

            FunctionTable.Get("ClassCode").Run();
            // return ResolveTreeNode(Parser.Nodes);
            if (this.Flag.Has(ExecutanFlags.RamOptimized))
            {
                this.Reset();
            }
            
         
                return null;
        }

        public void Execute(string code)
        {
            this.Evaluate(code);
        }

        public string GetEcTypeValue(object type)
        {
            if (type is EcString)
            {
                return (type as EcString).ToString();
            }
            if (type is EcObject)
            {
                return (type as EcObject).ToString();
            }
            if (type is EcNumber)
            {
                return (type as EcNumber).ToString();
            }
            if (type is EcBool)
            {
                return (type as EcBool).ToString();
            }
            return null;
        }

        public Boolean HandleIF(dynamic va, dynamic vb, string op)
        {
            if (op == "==")
            {
                if ((va is Primitive) && !(vb is Primitive))
                {
                    if (this.GetEcTypeValue(va) == vb)
                    {
                        return true;
                    }
                }
                if (!(va is Primitive) && (vb is Primitive))
                {
                    if (va == this.GetEcTypeValue(vb))
                    {
                        return true;
                    }
                }
                if (!(va is Primitive) && !(vb is Primitive))
                {
                    if (va == vb)
                    {
                        return true;
                    }
                }
                if ((va is Primitive) && (vb is Primitive))
                {
                    var v = va as Primitive;

                    if (v.CanOvveride.Contains(new Equals()))
                    {
                        var b = v.Equals(va, vb);
                        return b;
                    }
                    else
                    {
                        if (this.GetEcTypeValue(va) == this.GetEcTypeValue(vb))
                        {
                            return true;
                        }
                    }
                }
            }

            if (op == "!=")
            {
                if ((va is Primitive) && !(vb is Primitive))
                {
                    if (this.GetEcTypeValue(va) != vb)
                    {
                        return true;
                    }
                }
                if (!(va is Primitive) && (vb is Primitive))
                {
                    if (va != this.GetEcTypeValue(vb))
                    {
                        return true;
                    }
                }
                if (!(va is Primitive) && !(vb is Primitive))
                {
                    if (va != vb)
                    {
                        return true;
                    }
                }
                if ((va is Primitive) && (vb is Primitive))
                {
                    var v = va as Primitive;

                    if (v.CanOvveride.Contains(new NotEquals()))
                    {
                        var b = v.NotEquals(va, vb);
                        return b;
                    }
                    else
                    {
                        if (this.GetEcTypeValue(va) != this.GetEcTypeValue(vb))
                        {
                            return true;
                        }
                    }
                }
            }

            if (op == "|")
            {
                if ((va is Primitive) && !(vb is Primitive))
                {
                    if (this.GetEcTypeValue(va) || vb)
                    {
                        return true;
                    }
                }
                if (!(va is Primitive) && (vb is Primitive))
                {
                    if (va || this.GetEcTypeValue(vb))
                    {
                        return true;
                    }
                }
                if (!(va is Primitive) && !(vb is Primitive))
                {
                    if (va || vb)
                    {
                        return true;
                    }
                }
                if ((va is Primitive) && (vb is Primitive))
                {
                    if (this.GetEcTypeValue(va) || this.GetEcTypeValue(vb))
                    {
                        return true;
                    }
                }
            }

            if (op == "<")
            {
                if ((va is Primitive) && !(vb is Primitive))
                {
                    if (int.Parse(this.GetEcTypeValue(va) < vb))
                    {
                        return true;
                    }
                }
                if (!(va is Primitive) && (vb is Primitive))
                {
                    if (va < int.Parse(this.GetEcTypeValue(vb)))
                    {
                        return true;
                    }
                }
                if (!(va is Primitive) && !(vb is Primitive))
                {
                    if (va < vb)
                    {
                        return true;
                    }
                }
                if ((va is Primitive) && (vb is Primitive))
                {
                    var v = va as Primitive;

                    if (v.CanOvveride.Contains(new LessThan()))
                    {
                        var b = v.LessThan(va, vb);
                        return b;
                    }
                    else
                    {
                        if (this.GetEcTypeValue(va) < this.GetEcTypeValue(vb))
                        {
                            return true;
                        }
                    }
                }
            }

            if (op == ">")
            {
                if ((va is Primitive) && !(vb is Primitive))
                {
                    if (int.Parse(this.GetEcTypeValue(va)) > vb)
                    {
                        return true;
                    }
                }
                if (!(va is Primitive) && (vb is Primitive))
                {
                    if (va > int.Parse(this.GetEcTypeValue(vb)))
                    {
                        return true;
                    }
                }
                if (!(va is Primitive) && !(vb is Primitive))
                {
                    if (va > vb)
                    {
                        return true;
                    }
                }
                if ((va is Primitive) && (vb is Primitive))
                {
                    var v = va as Primitive;

                    if (v.CanOvveride.Contains(new GreaterThan()))
                    {
                        var b = v.GreaterThan(va, vb);
                        return b;
                    }
                    else
                    {
                        if (this.GetEcTypeValue(va) > this.GetEcTypeValue(vb))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public Primitive ParserEcVar(string value, string Type)
        {
            Primitive Value = null;
            if (new EcNumber().Validate(value) && Type == "number")
            {
                Value = new EcNumber().Parse(value);
            }
            else if (new EcBool().Validate(value) && Type == "bool")
            {
                Value = new EcBool().Parse(value);
            }
            else if (Type == "string")
            {
                if (value.StartsWith('"'.ToString()) && value.EndsWith('"'.ToString()))
                {
                    Value = new EcString((value).TrimStart('"').TrimEnd('"'));
                }
                else
                {
                    if (Parser.Grammar.GetPattern("name").IsValid(value))
                    {
                        Value = new EcString(value);
                    }
                }
                return null;
            }
            return Value;
        }

        public object ResolveTreeNode(List<IAst> stmList)
        {
            if (StopExecutionFlag)
            {
                return null;
            }
            //Console.WriteLine(stmList.Count);
            try
            {
                //Application.DoEvents();
            }
            catch
            {
            }
            foreach (object statement in stmList)
            {
                if (statement is DecStmt)
                {
                    var dec = statement as DecStmt;
                    if (InLoop)
                    {
                        InloopVars.Add(dec.Name);
                    }
                    SymbolTable.Store(dec.Name, HandleOneParameter(dec.Value), dec.Attributes);
                }
                if (statement is ImportStmt)
                {
                    var dec = statement as ImportStmt;
                    string exstention = dec.Name.Split('.')[dec.Name.Split('.').Length - 1];
                    if (exstention != "ec" && exstention != "dll" && exstention != "exe")
                    {
                        NameSpaceList.Add(dec.Name);
                    }
                    else if (dec.Name.Split('.')[dec.Name.Split('.').Length - 1] == "ec")
                    {
                        Parser.CodeBlocks.Clear();
                        Parser.Init();
                        Parser.Execute(File.ReadAllText(dec.Name));

                        foreach (CodeBlock i in Parser.CodeBlocks)
                        {
                            if (i.Name == "ClassCode")
                            {
                                FunctionTable.Store(
                                    new FileInfo(dec.Name).Name.Replace(".ec", ""),
                                    new Method {Name = i.Name, Nodes = i.Nodes, returns = i.Returns });
                            }
                            else
                            {
                                FunctionTable.Store(i.Name, new Method { Name = i.Name, Nodes = i.Nodes, returns = i.Returns });
                            }
                        }
                    }
                    else if (dec.Name.Split('.')[dec.Name.Split('.').Length - 1] == "dll")
                    {
                        Assembly DLL = Assembly.LoadFile(dec.Name);
                        includdlls.Add(DLL);
                    }
                }

                if (statement is DecSetStmt)
                {
                    var dec = statement as DecSetStmt;
                    string callname = "";
                    if (SymbolTable.Contains(dec.Name))
                    {
                        callname = dec.Name;
                    }
                    else
                    {
                        callname = dec.Name.Split('.')[0];
                    }

                    if (AttributeManager.ContainsAttribute("@readonly", SymbolTable.GetAttributes(callname)))
                    {
                        ErrorTable.Add(0, "Could not set the value of " + dec.Name + " becuase it is readonly");
                    }
                    else
                    {
                        if (!dec.Name.Contains("."))
                        {
                            var va = SymbolTable.Get(dec.Name);
                            if (va != null)
                            {
                                // do += -= here
                                if (dec.Operator == "=")
                                {
                                    SymbolTable.Set(dec.Name, HandleOneParameter(dec.Value));
                                }

                                if (va.CanOvveride.Contains(new PlusEquals()))
                                {
                                    if (dec.Operator == "+=")
                                    {
                                        SymbolTable.Set(
                                            dec.Name,
                                            va.PlusEquals(SymbolTable.Get(dec.Name), HandleOneParameter(dec.Value)));
                                    }
                                }
                                if (va.CanOvveride.Contains(new MinusEquals()))
                                {
                                    if (dec.Operator == "-=")
                                    {
                                        SymbolTable.Set(
                                            dec.Name,
                                            va.MinusEquals(SymbolTable.Get(dec.Name), HandleOneParameter(dec.Value)));
                                    }
                                }
                            }
                        }
                        else
                        {
                            object tmp = null;
                            try
                            {
                                tmp = (SymbolTable.Get(dec.Name.Split('.')[0]) as EcObject).Value;
                            }
                            catch
                            {
                            }

                            if (tmp is EcObject)
                            {
                                var ecObject = SymbolTable.Get(dec.Name.Split('.')[0]) as EcObject;
                                if (ecObject != null)
                                {
                                    tmp = (ecObject.Value as EcObject).Value;
                                }
                            }
                            object tmp1 = tmp.GetType().GetProperty(dec.Name.Split('.')[1]);
                            if (tmp1 == null)
                            {
                                tmp1 = tmp.GetType().GetEvent(dec.Name.Split('.')[1]);
                            }
                            string[] segs = dec.Name.Split('.');
                            for (int index = 0; index < segs.Length; index++)
                            {
                                string i = segs[index];
                                if (index == segs.Length - 1)
                                {
                                    object value = ConvertEcToCs(new[] { (HandleOneParameter(dec.Value)) })[0];
                                    try
                                    {
                                        (tmp1 as PropertyInfo).SetValue(tmp, value, new object[] { });
                                    }
                                    catch
                                    {
                                        try
                                        {
                                            dynamic handler = tmp;
                                            Type tExForm = tmp.GetType();
                                            Object exFormAsObj = tmp;

                                            EventInfo evClick =
                                                tExForm.GetEvent(dec.Name.Split('.')[dec.Name.Split('.').Length - 1]);
                                            Type tDelegate = evClick.EventHandlerType;

                                            MethodInfo addHandler = evClick.GetAddMethod();
                                            Type t = evClick.EventHandlerType;
                                            EventHandlers.Add((value as Method));
                                            var DM = new DynamicMethod(
                                                "handler", null, this.GetDelegateParameterTypes(tDelegate));
                                            MethodInfo mtinfo = typeof(Engine).GetMethod("CallEventHandle");
                                            ILGenerator IL = DM.GetILGenerator();
                                            IL.Emit(OpCodes.Ldstr, Convert.ToString(EventHandlers.Count - 1));
                                            IL.Emit(OpCodes.Call, mtinfo);
                                            //  IL.Emit(OpCodes.Pop);
                                            IL.Emit(OpCodes.Ret);

                                            Object[] addHandlerArgs = { DM.CreateDelegate(t) };

                                            evClick.GetAddMethod().Invoke(tmp, addHandlerArgs);
                                        }
                                        catch
                                        {
                                        }
                                    }
                                }
                                else if (index != 0)
                                {
                                    tmp1 = tmp.GetType().GetProperty(i).GetValue(tmp.GetType(), null);
                                }
                            }
                        }
                    }
                }
                if (statement is CallStmt)
                {
                    var dec = statement as CallStmt;
                    if (ClrFunctions.ContainsKey(dec.Name))
                    {
                        if (dec.Paramaters.Count != 1)
                        {
                            ClrFunctions[dec.Name].DynamicInvoke(HandleParametersArray(dec.Paramaters).ToArray());
                        }
                        else
                        {
                            ClrFunctions[dec.Name].DynamicInvoke(
                                new EcString(Convert.ToString(HandleOneParameter(dec.Paramaters[0]).ToString())));
                        }
                    }
                    else if (FunctionTable.Get(dec.Name) != null)
                    {
                        FunctionTable.Get(dec.Name).Run(); //HandleParametersArray(dec.Paramaters).ToArray());
                    }
                    else
                    {

                        StopExecutionFlag = true;
                        ErrorTable.Add(0, "function expected &&[" + dec.Name + "]&&" );
                       // throw new Exception("Function expected");
                    }
                }
                if (statement is ObjCallStmt)
                {
                    var dec = statement as ObjCallStmt;
                    if (ClrVariables.ContainsKey(dec.Target))
                    {
                        if (ClrVariables[dec.Target] is EcCustomClass)
                        {
                            var m = Activator.CreateInstance(ClrVariables[dec.Target].GetType()) as EcCustomClass;

                            if (dec.Paramaters.Count == 1)
                            {
                                m.Call(dec.Name, HandleOneParameter(dec.Paramaters[0]));
                            }
                            else
                            {
                                m.Call(dec.Name, HandleParametersArray(dec.Paramaters).ToArray());
                            }
                        }
                    }
                    else
                    {
                        if (SymbolTable.Contains(dec.Target.Split('.')[0]))
                        {
                            Primitive m = SymbolTable.Get(dec.Target.Split('.')[0]);

                            if (dec.Paramaters.Count == 1)
                            {
                                m.call(
                                    dec.Target + "." + dec.Name,
                                    ConvertEcToCs(new[] { HandleOneParameter(dec.Paramaters[0]) }));
                            }
                            else
                            {
                                m.call(
                                    dec.Target + "." + dec.Name,
                                    ConvertEcToCs(HandleParametersArray(dec.Paramaters).ToArray()));
                            }
                        }
                        else
                        {
                            try
                            {
                                if (dec.Paramaters.Count == 1)
                                {
                                    CallDotNet(
                                        dec.Target,
                                        dec.Name,
                                        ConvertEcToCs(new[] { HandleOneParameter(dec.Paramaters[0]) }).ToArray());
                                }
                                else if (dec.Paramaters.Count != 0)
                                {
                                    CallDotNet(
                                        dec.Target,
                                        dec.Name,
                                        ConvertEcToCs(HandleParametersArray(dec.Paramaters).ToArray()).ToArray());
                                }
                                else
                                {
                                    CallDotNet(dec.Target, dec.Name, new object[] { });
                                }
                            }
                            catch (Exception ee)
                            {
                                StopExecutionFlag = true;
                                 ErrorTable.Add(0,"Function expected");
                            }
                        }
                    }
                }
                if (statement is IfStmt)
                {
                    var ifs = statement as IfStmt;
                    ifs.ParserHeader(ifs.Header);
                    dynamic va = HandleOneParameter(ifs.Left);
                    dynamic vb = HandleOneParameter(ifs.Right);
                    string op = ifs.op;
                    bool v = HandleIF(va, vb, op);
                    if (v)
                    {
                        Application.DoEvents();
                        if (StopExecutionFlag)
                        {
                            return null;
                        }
                        this.ResolveTreeNode(ifs.Statements);
                    }
                    else
                    {
                        Application.DoEvents();
                        if (StopExecutionFlag)
                        {
                            return null;
                        }
                        this.ResolveTreeNode(ifs.ElseStatements);
                    }
                    ifs = null;
                }
                if (statement is WhileStmt)
                {
                    var WhileStmt1 = statement as WhileStmt;
                    WhileStmt1.ParserHeader(WhileStmt1.Header);
                    dynamic va = HandleOneParameter(WhileStmt1.Left);
                    dynamic vb = HandleOneParameter(WhileStmt1.Right);
                    string op = WhileStmt1.op;
                    InLoop = true;
                    while (this.HandleIF(va, vb, op))
                    {
                        Application.DoEvents();
                        if (StopExecutionFlag)
                        {
                            break;
                        }
                        this.ResolveTreeNode(WhileStmt1.Nodes);
                        foreach (string inloopVar in InloopVars)
                        {
                            SymbolTable.Remove(inloopVar);
                        }
                    }
                    InloopVars.Clear();
                    InLoop = false;
                    
                }
                if (statement is ForStmt)
                {
                    var ForStmt1 = statement as ForStmt;
                    ForStmt1.ParserHeader(ForStmt1.Header);
                    SymbolTable.Store(ForStmt1.Value, this.ParserEcVar(ForStmt1.setValue, "number"));
                    int i = int.Parse(ForStmt1.setValue);
                    dynamic vb = HandleOneParameter(ForStmt1.Right);
                    string op = ForStmt1.op;
                    string forop = ForStmt1.forop;
                    InLoop = true;
                    while (this.HandleIF(i, vb, op))
                    {
                        Application.DoEvents();
                        if (StopExecutionFlag)
                        {
                            break;
                        }
                        this.ResolveTreeNode(ForStmt1.Statements);
                        if (forop == "++")
                        {
                            if (ForStmt1.Step != null) i = i + ForStmt1.Step.Every;
                            else i++;
                            SymbolTable.Set(ForStmt1.Value, this.ParserEcVar(i.ToString(), "number"));
                        }
                        if (forop == "--")
                        {
                            i--;
                            SymbolTable.Set(ForStmt1.Value, this.ParserEcVar(i.ToString(), "number"));
                        }
                        foreach (string inloopVar in InloopVars)
                        {
                            SymbolTable.Remove(inloopVar);
                        }
                    }
                    SymbolTable.Remove(ForStmt1.Value);
                    InloopVars.Clear();
                    InLoop = false;
                }
                if (statement is ForEachStmt)
                {
                    var dec = statement as ForEachStmt;
                    dec.ParserHeader(dec.Header);
                    var ar = (SymbolTable.Get(dec.Array) as EcArray).data; //for debugging
                    Primitive saveold = null;
                    if (SymbolTable.Contains(dec.ScopeName))
                    {
                        saveold = SymbolTable.Get(dec.ScopeName);
                        SymbolTable.Remove(dec.ScopeName);
                    }
                    if (ar != null)
                    {
                        foreach (var item in ar)
                        {
                            if (dec.Choice.Type != "")
                            {


                                var n = ConvertEcToCs(new Primitive[] {(item as Primitive)})[0].GetType().Name.ToLower();
                                // for debuugin
                                if (n == dec.Choice.Type)
                                {
                                    SymbolTable.Store(dec.ScopeName, (item as Primitive));
                                    ResolveTreeNode(dec.Nodes);
                                    SymbolTable.Remove(dec.ScopeName);
                                }


                            }
                            else
                            {


                                SymbolTable.Store(dec.ScopeName, (item as Primitive));
                                ResolveTreeNode(dec.Nodes);
                                SymbolTable.Remove(dec.ScopeName);
                            }
                        }
                    }
                    else
                    {
                        ErrorTable.Add(0, dec.Array + "is not an array");
                    }
                    if (saveold != null)
                    {
                        SymbolTable.Store(dec.ScopeName, saveold);
                    }

                }
                if (statement is ModeStmt)
                {
                    var mod = (statement as ModeStmt);
                    if (mod.Type.ToLower() == "strict")
                    {
                        Flag = Flag.Add(ExecutanFlags.Strict);
                    }
                    else if (mod.Type.ToLower() == "ramoptimized")
                    {
                        Flag = Flag.Add(ExecutanFlags.RamOptimized);
                    }
                    else if (mod.Type.ToLower() == "normal")
                    {
                        Flag = Flag.Add(ExecutanFlags.Normal);
                    }
                    else
                    {
                        ErrorTable.Add(0, "mode '" + mod.Type + "' does not exist. Use strict, ramoptimized or normal");
                    }
                }
                if(statement is ThrowStmt)
                {
                    var thr = statement as ThrowStmt;

                    Primitive obj;
                    if(SymbolTable.Contains(thr.Data))
                    {
                        obj = SymbolTable.Get(thr.Data);
                    }
                    else
                    {
                       
                            obj = new EcObject(thr.Data.TrimStart('"').TrimEnd('"'));
                      
                        
                    }

                    throw new ThrowException(obj);
                }
                if (statement is DelStmt)
                {
                    var del = (statement as DelStmt);

                    if (SymbolTable.Contains(del.Name))
                    {
                        if (SymbolTable.DeclarationAttributes[del.Name] != null)
                        {
                            if (!AttributeManager.ContainsAttribute("@lock", SymbolTable.DeclarationAttributes[del.Name]))
                            {
                                SymbolTable.Remove(del.Name);
                            }
                            else
                            {
                                ErrorTable.Add(
                                    0, "Could not delete \"" + del.Name + "\" from declarations because it is locked.");
                            }
                        }
                    }
                    else
                    {
                        //TODO: add func remove here later
                        ErrorTable.Add(
                            0, "Could not delete \"" + del.Name + "\" from declarations because it does not exist.");
                        StopExecutionFlag = true;
                    }
                }
                if (statement is TryCatchStmt)
                {
                    var tc = (statement as TryCatchStmt);

                    if (tc.CatchNodes.Count == 0 && tc.FinallyNodes.Count == 0)
                    {
                        ErrorTable.Add(0, "This Try Statment does not have a Catch or Finally.");
                    }

                    Primitive tmp = null;
                    try
                    {
                        Program.e.ResolveTreeNode(tc.TryNodes);
                    }
                    catch (ThrowException ex)
                    {
                        if (tc.CatchNodes.Count != 0 || tc.FinallyNodes.Count != 0)
                        {
                            if (SymbolTable.Contains(tc.catchheader.Split(' ')[1]))
                            {
                                tmp = SymbolTable.Get(tc.catchheader.Split(' ')[1]);
                                SymbolTable.Set(tc.catchheader.Split(' ')[1], ex.Object);
                            }
                            else
                            {
                                SymbolTable.Store(tc.catchheader.Split(' ')[1], ex.Object);
                            }

                            Program.e.ResolveTreeNode(tc.CatchNodes);

                            if (tmp != null)
                            {
                                //retore var so that it becomes like a local varible and not global
                                SymbolTable.Set(tc.catchheader.Split(' ')[1], tmp);
                            }
                            else
                            {
                                SymbolTable.Remove(tc.catchheader.Split(' ')[1]);
                            }
                        }
                    }
                    catch (Exception ee)
                    {
                        if (tc.CatchNodes.Count != 0 || tc.FinallyNodes.Count != 0)
                        {
                            if (SymbolTable.Contains(tc.catchheader.Split(' ')[1]))
                            {
                                tmp = SymbolTable.Get(tc.catchheader.Split(' ')[1]);
                                SymbolTable.Set(tc.catchheader.Split(' ')[1], new EcObject(ee));
                            }
                            else
                            {
                                SymbolTable.Store(tc.catchheader.Split(' ')[1], new EcObject(ee));
                            }

                            Program.e.ResolveTreeNode(tc.CatchNodes);

                            if (tmp != null)
                            {
                                //retore var so that it becomes like a local varible and not global
                                SymbolTable.Set(tc.catchheader.Split(' ')[1], tmp);
                            }
                            else
                            {
                                SymbolTable.Remove(tc.catchheader.Split(' ')[1]);
                            }
                        }
                    }
                        finally
                        {
                            if (tc.CatchNodes.Count != 0 || tc.FinallyNodes.Count != 0)
                            {
                                Program.e.ResolveTreeNode(tc.FinallyNodes);
                            }
                        }
                    
                }
                if (statement is DeIncreaseStmt)
                {
                    var di = (statement as DeIncreaseStmt);

                    if (SymbolTable.Contains(di.Variable))
                    {
                       // if (SymbolTable.Get(di.Variable) is EcNumber)
                        {
                            if (di.Operation == DeIncreaseOperation.Increase)
                            {
                                var v = SymbolTable.Get(di.Variable) as Primitive;
                                if (v.CanOvveride.Contains(new Increae()))
                                {
                                    SymbolTable.Remove(di.Variable);
                                    v =  v.Increase();
                                    SymbolTable.Store(di.Variable, v);
                                }
                            }
                            else
                            {

                                var v = SymbolTable.Get(di.Variable) as Primitive;
                                SymbolTable.Remove(di.Variable);
                                v = v.Decrease();
                                SymbolTable.Store(di.Variable, v);
                            }
                        }
                        
                    }
                    else
                    {
                        ErrorTable.Add(
                            0, "Could not increase or decrease \"" + di.Variable + "\" because it does not exist.");
                        StopExecutionFlag = true;
                    }
                }
                if (statement is SwitchStmt)
                {
                    var stmt = (statement as SwitchStmt);
                    stmt.ParserHeader(stmt.Header);
                    bool faild = false;
                    foreach (KeyValuePair<Primitive, List<IAst>> @case in stmt.Cases)
                    {
                        if ((@case.Key.Value as string) == (stmt.Parent.Value as string) )
                        {
                            ResolveTreeNode(@case.Value);
                            faild = false;
                        }
                        else
                        {
                            if(!faild)
                                faild = true;
                        }
                    }

                    if (faild)
                    {
                        ResolveTreeNode(stmt.Default);
                    }
                }
            }

            return null;
        }

        public void Stop()
        {
            StopExecutionFlag = true;
        }

        #endregion

        #region Methods

        private void AddFunction(string name, Delegate d)
        {
            ClrFunctions.Add(name, d);
        }

        private void AddType(string name, Type t)
        {
            ClrTypes.Add(name, t);
        }

        private void AddVar(string name, object o)
        {
            ClrVariables.Add(name, o);
        }

        private Type[] GetDelegateParameterTypes(Type d)
        {
            if (d.BaseType != typeof(MulticastDelegate))
            {
                StopExecutionFlag = true;
                ErrorTable.Add(0, "Not a delegate.");
              //  throw new ApplicationException("Not a delegate.");
            }

            MethodInfo invoke = d.GetMethod("Invoke");
            if (invoke == null)
            {

                StopExecutionFlag = true;
                ErrorTable.Add(0, "Not a delegate.");
               // throw new ApplicationException("Not a delegate.");
            }

            ParameterInfo[] parameters = invoke.GetParameters();
            var typeParameters = new Type[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                typeParameters[i] = parameters[i].ParameterType;
            }
            return typeParameters;
        }

        private Type GetDelegateReturnType(Type d)
        {
            if (d.BaseType != typeof(MulticastDelegate))
            {

                StopExecutionFlag = true;
                ErrorTable.Add(0, "Not a delegate.");
              //  throw new ApplicationException("Not a delegate.");
            }

            MethodInfo invoke = d.GetMethod("Invoke");
            if (invoke == null)
            {

                StopExecutionFlag = true;
                ErrorTable.Add(0, "Not a delegate.");
              //  throw new ApplicationException("Not a delegate.");
            }

            return invoke.ReturnType;
        }

        private void Reset()
        {
            Parser.CodeBlocks.Clear();
            StopExecutionFlag = false;
            SymbolTable.Clear();
            FunctionTable.Clear();
            ClrFunctions.Clear();
            ClrTypes.Clear();
            ClrVariables.Clear();
            includdlls = new List<Assembly>();
            NameSpaceList = new List<string>();
        }

        #endregion
    }
}