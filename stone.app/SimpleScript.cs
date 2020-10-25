using System;
using System.Collections.Generic;
using System.Text;

namespace stone.app
{
    public class SimpleScript
    {
        public Dictionary<string, int> variables = new Dictionary<string, int>();
        private static bool verbose = false;

        private int Evaluate(ASTNode node, string indent)
        {
            int result = 0;
            if (verbose)
            {
                Console.WriteLine(indent + "Calculatings:" + node.GetType());
            }
            switch (node.GetType())
            {
                case ASTNodeType.Program:
                    foreach(var child in node.GetChildren())
                    {
                        result = Evaluate(child, indent);
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
                case ASTNodeType.Identifier:
                    string varName = node.GetText();
                    if (variables.ContainsKey(varName))
                    {
                        int value;
                        if(!variables.TryGetValue(varName, out value))
                        {
                            throw new Exception("variables " + varName + "has not been set any value");
                        }
                    }
                    else
                    {
                        throw new Exception("unknow variable:" + varName);
                    }
                    break;
                case ASTNodeType.AssignmentStmt:
                    varName = node.GetText();
                    if (!variables.ContainsKey(varName))
                    {

                    }
            }
        }
    }
}
