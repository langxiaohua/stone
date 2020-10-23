using System;
using System.Collections.Generic;
using System.Text;

namespace stone.app
{
    public class SimpleTokenReader : TokenReader
    {
        List<Token> tokens = null;
        int pos = 0;

        public SimpleTokenReader(List<Token> tokens)
        {
            this.tokens = tokens;
        }
        public int GetPosition()
        {
            return pos;
        }

        public Token Peek()
        {
            if(pos < tokens.Count)
            {
                return tokens[pos];
            }
            return null;
        }

        public Token Read()
        {
            if(pos < tokens.Count)
            {
                return tokens[pos++];
            }
            return null;
        }

        public void SetPosition(int position)
        {
            if(position >= 0 && position < tokens.Count)
            {
                pos = position;
            }
        }

        public void UnRead()
        {
            if(pos > 0)
            {
                pos--;
            }
        }
    }
}
