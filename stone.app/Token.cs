using System;
using System.Collections.Generic;
using System.Text;

namespace stone.app
{
    public interface  Token
    {
        public TokenType GetType();
        public string GetText();
    }
}
