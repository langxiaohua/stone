using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace stone.app
{
    public class SimpleCalculator 
    {


     

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
                node = Additive(tokens);
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
                return children ;
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
}
