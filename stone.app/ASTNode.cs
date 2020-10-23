using System;
using System.Collections.Generic;
using System.Text;

namespace stone.app
{
    public interface ASTNode
    {
        public ASTNode GetParent();
        public List<ASTNode> GetChildren();
        public ASTNodeType GetType();
        public string GetText();
    }
}
