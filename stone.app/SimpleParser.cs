using System;
using System.Collections.Generic;
using System.Text;

namespace stone.app
{
    public class SimpleParser
    {

        private SimpleASTNode AssignmentStatement(TokenReader tokens)
        {
            SimpleASTNode node = null;
            Token token = tokens.Peek();
            if(token != null && token.GetType() == TokenType.Identifier)
            {
                token = tokens.Read();
                node = new SimpleASTNode(ASTNodeType.AssignmentStmt, token.GetText());
                token = tokens.Peek();
                if(token != null && token.GetType() == TokenType.Assignment)
                {
                    tokens.Read();
                    SimpleASTNode child = Additive(tokens);
                    if(child == null)
                    {
                        throw new Exception("invalid assignment statement, expecting an expression");
                    }
                    else
                    {
                        node.AddChild(child);
                        token = tokens.Peek();
                        if(token != null && token.GetType() == TokenType.SemiColon)
                        {
                            tokens.Read();
                        }
                        else
                        {
                            throw new Exception("invalid statement, expecting semicolon");
                        }
                    }
                }
                else
                {
                    tokens.UnRead();
                    node = null;
                }
            }
            return node;
        }

        /// <summary>
        /// 声明变量
        /// int a
        /// int b = 2 * 3
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        private SimpleASTNode IntDeclare(TokenReader tokens)
        {
            SimpleASTNode node = null;
            Token token = tokens.Peek();
            if(token != null && token.GetType() == TokenType.Int)
            {
                token = tokens.Read();
                if(tokens.Peek().GetType() == TokenType.Identifier)
                {
                    token = tokens.Read();
                    node = new SimpleASTNode(ASTNodeType.IntDeclaration, token.GetText());
                    token = tokens.Peek();
                    if(token != null && token.GetType() == TokenType.Assignment)
                    {
                        tokens.Read();
                        SimpleASTNode child = Additive(tokens);
                        if(child == null)
                        {
                            throw new Exception("invalid variable iniialization, expecting an expression.");
                        }
                        else
                        {
                            node.AddChild(child);
                        }
                    }
                }
                else
                {
                    throw new Exception("Variable name expected.");
                }
            }
            if(node != null)
            {
                token = tokens.Peek();
                if(token != null && token.GetType() == TokenType.SemiColon)
                {
                    tokens.Read();
                }
                else
                {
                    throw new Exception("invalid statement, expecting semicolon");
                }
            }
            return node;
        }
        private SimpleASTNode Additive(TokenReader tokens)
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
                        tokens.Read();
                        SimpleASTNode child2 = Multiplicative(tokens);
                        if(child2 != null)
                        {
                            node = new SimpleASTNode(ASTNodeType.Additive, token.GetText());
                            node.AddChild(child1);
                            node.AddChild(child2);
                            child1 = node;
                        }
                        else
                        {
                            throw new Exception("invalid additive expression, expecting the right part.");
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return node;
        }

        private SimpleASTNode Multiplicative(TokenReader tokens)
        {
            SimpleASTNode child1 = Primary(tokens);
            SimpleASTNode node = child1;
          
            while (true)
            {
                Token token = tokens.Peek();
                if(token != null && (token.GetType() == TokenType.Star || token.GetType() == TokenType.Slash))
                {
                    token = tokens.Read();
                    SimpleASTNode child2 = Primary(tokens);
                    if(child2 != null)
                    {
                        node = new SimpleASTNode(ASTNodeType.Multiplicative, token.GetText());
                        node.AddChild(child1);
                        node.AddChild(child2);
                        child1 = node;
                    }
                    else
                    {
                        throw new Exception("invalid multiplicative expression, expecting the right part.");
                    }
                }
                else
                {
                    break;
                }
            }
            return node;
        }

        private SimpleASTNode Primary(TokenReader tokens)
        {
            SimpleASTNode node = null;
            Token token = tokens.Peek();
            if (token != null)
            {
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
                    node = Additive(tokens);
                    if(node != null)
                    {
                        token = tokens.Peek();
                        if(token != null && token.GetType() == TokenType.RightParen)
                        {
                            tokens.Read();
                        }
                        else
                        {
                            throw new Exception("expecting right parenthesis");
                        }
                    }
                    else
                    {
                        throw new Exception("expecting an additive expression inside parenthesis");
                    }
                }
            }
            return node;
        }

        private class SimpleASTNode : ASTNode
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

            ASTNodeType ASTNode.GetType()
            {
                return nodeType;
            }

            public void AddChild(SimpleASTNode child)
            {
                children.Add(child);
                child.parent = this;
            }
        }
        void dumpAST(ASTNode node, string indent)
        {
            Console.WriteLine(indent + node.GetType() + " " + node.GetText());
            foreach (var child in node.GetChildren())
            {
                dumpAST(child, indent + "\t");
            }
        }
    }
}
