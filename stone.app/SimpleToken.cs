using System;
using System.Collections.Generic;
using System.Text;

namespace stone.app
{
    public class SimpleToken : Token
    {
        private TokenType _type;
        public TokenType Type
        {
            get { return _type; }
            set { _type = value; }
        }
        private string _text = null;
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public string GetText()
        {
            return _text;
        }

        TokenType Token.GetType()
        {
            return _type;
        }
    }
}
