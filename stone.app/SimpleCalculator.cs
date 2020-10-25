using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace stone.app
{
    public class SimpleCalculator 
    {
        public static void TextCalculator()
        {
            SimpleCalculator calculator = new SimpleCalculator();

            string script = "int a = b+3";
            Console.WriteLine("解析变量声明语句：" + script);
            SimpleLexer lexer = new SimpleLexer();
            TokenReader tokens = lexer.Tokenize(script);

            try
            {
                SimpleASTNode node = calculator.InitDeclare(tokens);
                calculator.DumpAST(node, "");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // script = "2 + 3*5";
            //Console.WriteLine("\n计算：" + script + ",看上去一切正常");
            script = "2 + 3 + 4";
            calculator.Evaluate(script);

        }

        public void Evaluate(string script)
        {
            try
            {
                ASTNode tree = parse(script);

                DumpAST(tree, "");
                Evaluate(tree, "");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void DumpAST(ASTNode node, string indent)
        {
            Console.WriteLine(indent + node.GetType() + " " + node.GetText());
            foreach(ASTNode child in node.GetChildren())
            {
                DumpAST(child, indent + "\t");
            }
        }

        /// <summary>
        /// 解析脚本，并返回根节点
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ASTNode parse(string code)
        {
            SimpleLexer lexer = new SimpleLexer();
            TokenReader tokens = lexer.Tokenize(code);

            ASTNode rootNode = Prog(tokens);
            return rootNode;
        }

        /// <summary>
        /// 对某个AST节点求值，并打印求值过程
        /// </summary>
        /// <param name="node"></param>
        /// <param name="indent">打印输出时的缩进量，用tab控制</param>
        /// <returns></returns>
        private int Evaluate(ASTNode node, string indent)
        {
            int result = 0;
            Console.WriteLine(indent + "Calculating:" + node.GetType());
            switch (node.GetType())
            {
                case ASTNodeType.Program:
                    foreach(ASTNode child in node.GetChildren())
                    {
                        result = Evaluate(child, indent + "\t");
                    }                
                    break;
                case ASTNodeType.Additive:
                    ASTNode child1 = node.GetChildren()[0];
                    int value1 = Evaluate(child1, indent + "\t");
                    ASTNode child2 = node.GetChildren()[1];
                    int value2 = Evaluate(child2, indent + "\t");
                    if (node.GetText().Equals("+"))
                    {
                        result = value1 + value2;
                    }
                    else
                    {
                        result = value1 - value2;
                    }
                    break;
                case ASTNodeType.Multiplicative:
                    child1 = node.GetChildren()[0];
                    value1 = Evaluate(child1, indent + "\t");
                    child2 = node.GetChildren()[1];
                    value2 = Evaluate(child2, indent + "\t");
                    if (node.GetText().Equals("*"))
                    {
                        result = value1 * value2;
                    }
                    else
                    {
                        result = value1 / value2;
                    }
                    break;
                case ASTNodeType.IntLiteral:
                    result = int.Parse(node.GetText());
                    break;
                default:
                    break;

            }
            Console.WriteLine(indent + "Result:" + result);
            return result;
        }

        /// <summary>
        /// 根节点
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        private SimpleASTNode Prog(TokenReader tokens)
        {
            SimpleASTNode node = new SimpleASTNode(ASTNodeType.Program, "Caculator");
            SimpleASTNode child = Additive2(tokens);

            if(child != null)
            {
                node.AddChild(child);
            }
            return node;
        }

        private SimpleASTNode InitDeclare(TokenReader tokens)
        {
            SimpleASTNode node = null;
            Token token = tokens.Peek();
            if(token != null && token.GetType() == TokenType.Int)
            {
                token = tokens.Read();
                if(tokens.Peek().GetType() == TokenType.Identifier)
                {
                    token = tokens.Read();
                    node = new SimpleASTNode(ASTNodeType.Identifier, token.GetText());
                    token = tokens.Peek();
                    if(token != null && token.GetType() == TokenType.Assignment)
                    {
                        tokens.Read();
                        SimpleASTNode child = Additive(tokens);
                        if(child == null)
                        {
                            throw new Exception("inalide varible inititalization, expecting an expression");
                        }
                        else
                        {
                            node.AddChild(child);
                        }
                    }
                    else
                    {
                        token = tokens.Peek();
                        if(token != null && token.GetType() == TokenType.SemiColon)
                        {
                            tokens.Read();
                        }
                        else
                        {
                            throw new Exception("invalid statement, exepecting semicolon");
                        }
                    }
                }
            }
            return node;
        }

        private SimpleASTNode Additive2(TokenReader tokens)
        {
            SimpleASTNode child1 = Multiplicative(tokens);
            SimpleASTNode node = child1;
            if(child1 != null)
            {
                while (true)
                {
                    Token token = tokens.Peek();
                    if(token != null && (token.GetType() == TokenType.Plus || token.GetType() == TokenType.Minus))
                    {
                        token = tokens.Read();
                        SimpleASTNode child2 = Multiplicative(tokens);
                        node = new SimpleASTNode(ASTNodeType.Additive, token.GetText());
                        node.AddChild(child1);
                        node.AddChild(child2);
                        child1 = node;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return node;
        }

        /// <summary>
        /// 解析加法表达式
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        private SimpleASTNode Additive(TokenReader tokens)
        {
            SimpleASTNode child1 = Multiplicative(tokens);
            SimpleASTNode node = child1;

            Token token = tokens.Peek();
            if(child1 != null && token != null)
            {
                if(token.GetType() == TokenType.Plus || token.GetType() == TokenType.Minus)
                {
                    token = tokens.Read();
                    SimpleASTNode child2 = Additive(tokens);
                    if(child2 != null)
                    {
                        node = new SimpleASTNode(ASTNodeType.Additive, token.GetText());
                        node.AddChild(child1);
                        node.AddChild(child2);
                    }
                    else
                    {
                        throw new Exception("invalid additive expression, expecting the right part.");
                    }
                }
            }
            return node;
        }

        /// <summary>
        /// 语法解析： 乘法表达式
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        private SimpleASTNode Multiplicative(TokenReader tokens)
        {
            SimpleASTNode child1 = Primary(tokens);
            SimpleASTNode node = child1;

            Token token = tokens.Peek();
            if(child1 != null && token != null)
            {
                if(token.GetType() == TokenType.Star || token.GetType() == TokenType.Slash)
                {
                    token = tokens.Read();
                    SimpleASTNode child2 = Multiplicative(tokens);
                    if(child2 != null)
                    {
                        node = new SimpleASTNode(ASTNodeType.Multiplicative, token.GetText());
                        node.AddChild(child1);
                        node.AddChild(child2);
                    }
                    else
                    {
                        throw new Exception("invalid multiplacative expression, expecting the right part.");
                    }
                }
            }
            return node;
        }


        /// <summary>
        /// 基础表达式
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        private SimpleASTNode Primary(TokenReader tokens)
        {
            SimpleASTNode node = null;
            Token token = tokens.Peek();
            if (token.GetType() == TokenType.IntLiteral)
            {
                token = tokens.Read();
                node = new SimpleASTNode(ASTNodeType.IntLiteral, token.GetText());
            }
            else if (token.GetType() == TokenType.Identifier)
            {
                token = tokens.Read();
                node = new SimpleASTNode(ASTNodeType.Identifier, token.GetText());
            }
            else if (token.GetType() == TokenType.LeftParen)
            {
                tokens.Read();
                node = Additive2(tokens);
                if(node != null)
                {
                    token = tokens.Peek();
                    if(token != null && token.GetType() == TokenType.RightParen)
                    {
                        tokens.Read();
                    }
                }
                else
                {
                    throw new Exception("expecting an additive expression inside parenthesis");
                }
            }
            return node;
        }
       
       
    }
    public class SimpleASTNode : ASTNode
    {
        SimpleASTNode parent = null;
        List<ASTNode> children = new List<ASTNode>();

        ASTNodeType nodeType;
        string text = null;

        public SimpleASTNode(ASTNodeType nodeType, string text)
        {
            this.nodeType = nodeType;
            this.text = text;
        }

        public List<ASTNode> GetChildren()
        {
            return children;
        }

        public ASTNode GetParent()
        {
            return parent;
        }

        public string GetText()
        {
            return text;
        }


        public void AddChild(SimpleASTNode child)
        {
            children.Add(child);
            child.parent = this;
        }

        ASTNodeType ASTNode.GetType()
        {
            return nodeType;
        }
    }
}
