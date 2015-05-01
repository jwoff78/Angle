namespace ECLang
{
    using System;
    using System.Collections.Generic;

    using ECLang.AST;
    using ECLang.AST.Statements;
    using ECLang.Internal;
    using ECLang.Internal.AST;
    using ECLang.Internal.AST.Statements;
    using ECLang.Properties;

    public class Parser
    {
        #region Static Fields

        public static List<CodeBlock> CodeBlocks = new List<CodeBlock>();

        public static Grammar Grammar;

        public static List<MultilineStatement> MultilineStatementHandler = new List<MultilineStatement>();

        public static List<Statement> Nodes = new List<Statement>();

        public static List<Statement> StatementHandlers = new List<Statement>();

        #endregion

        #region Public Methods and Operators

        public static void Execute(string src)
        {
            string ClassLevelBlock = "";
            string temp = "";
            string aname = "";
            string returns = "";
            string aheader = "";
            string perams = "";
            bool inblock = false;
            string[] srcSplt = src.Replace("\r\n", "\n").Replace("\t", "").Split(new[] { '\n', ';' });
            for (int index = 0; index < srcSplt.Length; index++)
            {
                string line1 = srcSplt[index];
                string line = line1.Trim(' ');
                if (Grammar.GetPattern("defstart").IsValid(line))
                {
                    inblock = true;
                    aname = Grammar.GetPattern("defstart").Match(line).Groups["funcName"].Value;
                    aheader = line;
                    perams = Grammar.GetPattern("defstart").Match(line).Groups["Perams"].Value;
                }
                else if (Grammar.GetPattern("defend").IsValid(line))
                {
                    inblock = false;

                    CodeBlocks.Add(
                        new CodeBlock
                        {
                            Parameters = ParseCodeBlockParams(perams),
                            Name = aname,
                            Nodes = ParseCodeBlock(temp, aheader).Nodes,
                            Returns = returns
                        });
                    temp = "";
                    aname = "";
                }
                else if (inblock)
                {
                    if (Grammar.GetPattern("return").IsValid(line))
                    {
                        returns = Grammar.GetPattern("return").Match(line).Groups["Value"].Value;
                    }
                    else
                    {
                        temp += line + ";\n";
                    }
                }
                else if (!inblock)
                {
                    ClassLevelBlock += line + ";\n";
                }
            }
            CodeBlocks.Add(new CodeBlock { Name = "ClassCode", Nodes = ParseCodeBlock(ClassLevelBlock, "").Nodes });

            /*
            string[] srcSplt = src.Replace("\r\n", "\n").Split(new[] {'\n', ';'}, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in srcSplt)
            {
                foreach (Statement statementHandler in StatementHandlers)
                {
                    if (statementHandler.IsStatement(line))
                    {
                        Nodes.Add(statementHandler.Interprete(line));
                        break;
                    }
                }
            }
             * */
        }

        public static void Init()
        {
            Grammar = new Grammar(Resources.Grammar);

            //singleline statments
            StatementHandlers.Add(new DeIncreaseStmt());
            StatementHandlers.Add(new AttributeStmt());
            StatementHandlers.Add(new DecStmt());
            StatementHandlers.Add(new DecSetStmt());
            StatementHandlers.Add(new CommentStmt());
            StatementHandlers.Add(new CallStmt());
            StatementHandlers.Add(new ObjCallStmt());
            StatementHandlers.Add(new ImportStmt());
            StatementHandlers.Add(new DelStmt());
            StatementHandlers.Add(new ModeStmt());
            StatementHandlers.Add(new ThrowStmt());

            //MultilineStatements
            MultilineStatementHandler.Add(new IfStmt());
            MultilineStatementHandler.Add(new WhileStmt());
            MultilineStatementHandler.Add(new ForStmt());
            MultilineStatementHandler.Add(new TryCatchStmt());
            MultilineStatementHandler.Add(new SwitchStmt());
            MultilineStatementHandler.Add(new ForEachStmt());
        }

        public static CodeBlock ParseCodeBlock(string block, string header)
        {
            var cb = new CodeBlock();
            string[] srcSplt = block.Replace("\r\n", "\n")
                .Split(new[] { '\n', ';' }, StringSplitOptions.RemoveEmptyEntries);
            Boolean inMultilineStatementloop = false;
            string temp = "";
            MultilineStatement currentStmt = null;
            Boolean IsInAttributeLoop = false;
            var aAttributeStmt = new List<AttributeStmt>();
            for (int index = 0; index < srcSplt.Length; index++)
            {
                string line = srcSplt[index];
                Int32 currentline = 0;
               
                
                if (!inMultilineStatementloop)
                {
                    foreach (Statement statementHandler in StatementHandlers)
                    {
                        if (statementHandler.IsStatement(line))
                        {
                            if (new AttributeStmt().IsStatement(line))
                            {
                                aAttributeStmt.Add(new AttributeStmt().Interprete(line, currentline) as AttributeStmt);
                                IsInAttributeLoop = true;
                            }
                            else
                            {
                                if (IsInAttributeLoop)
                                {
                                    if (statementHandler is DecStmt)
                                    {
                                        var dec = statementHandler.Interprete(line, currentline) as DecStmt;
                                        dec.Attributes = aAttributeStmt;
                                        cb.Nodes.Add(dec);
                                        aAttributeStmt = new List<AttributeStmt>();
                                        IsInAttributeLoop = false;
                                    }
                                    else
                                    {
                                        cb.Nodes.Add(statementHandler.Interprete(line, currentline));
                                    }
                                }
                                else
                                {
                                    cb.Nodes.Add(statementHandler.Interprete(line, currentline));
                                }
                            }
                            break;
                        }
                    }

                    foreach (MultilineStatement stmt in MultilineStatementHandler)
                    {
                        if (stmt.StartIsMatch(line))
                        {
                            temp += line + "\n";

                            inMultilineStatementloop = true;

                            currentStmt = stmt;

                            stmt.Clear();
                            break;
                        }
                    }
                }
                else
                {
                    if (currentStmt.EndIsMatch(line))
                    {
                        temp += line + "\n";
                        inMultilineStatementloop = false;

                        cb.Nodes.Add(currentStmt.Interprete(temp));
                        currentStmt.Clear();
                        currentStmt = null;
                        temp = "";
                    }
                    else
                    {
                        temp += line + ";\n";
                    }
                }
            }
            return cb;
        }

        public static List<string> ParseCodeBlockParams(string src)
        {
            var rets = new List<string>();
            foreach (string i in src.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string name = i.Split(':')[0].TrimStart(' ').TrimEnd(' ');
                rets.Add(name);
            }

            return rets;
        }

        public static List<IAst> ParserSingleLines(string block)
        {
            var aNodes = new List<IAst>();
            string[] srcSplt = block.Replace("\r\n", "\n")
                .Split(new[] { '\n', ';' }, StringSplitOptions.RemoveEmptyEntries);
            for (int index = 0; index < srcSplt.Length; index++)
            {
                string line = srcSplt[index];
                foreach (Statement statementHandler in StatementHandlers)
                {
                    if (statementHandler.IsStatement(line))
                    {
                        aNodes.Add(statementHandler.Interprete(line, index + 1));
                        break;
                    }
                }
            }
            return aNodes;
        }

        #endregion
    }
}