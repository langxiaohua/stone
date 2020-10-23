using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace stone.app
{
    public class SimpleLexer
    {

        private StringBuilder _tokenText = null;  //临时保存token
        private List<Token> _tokens = null;  //保存解析出来的token
        private SimpleToken _token = null;  //当前正在解析的token

        private bool IsAlpha(int ch)
        {
            return (ch >= 'a' && ch <= 'z' || ch >= 'A' && ch <= 'Z');
        }
        private bool IsDigit(int ch)
        {
            return ch >= '0' && ch <= '9';
        }
        private bool IsBlank(int ch)
        {
            return ch == ' ' || ch == '\t' || ch == '\n';
        }

        private DfaState InitToken(char ch)
        {
            if(_tokenText != null && _tokenText.Length > 0)
            {
                _token.Text = _tokenText.ToString();
                _tokens.Add(_token);

                _tokenText = new StringBuilder();
                _token = new SimpleToken();
            }

            DfaState newState = DfaState.Initial;
            if (IsAlpha(ch))
            {
                if(ch == 'i')
                {
                    newState = DfaState.Id_int1;
                }
                else
                {
                    newState = DfaState.Id;
                }
                _token.Type = TokenType.Identifier;
                _tokenText.Append(ch);
            }
            else if(IsDigit(ch))
            {
                newState = DfaState.IntLiteral;
                _token.Type = TokenType.IntLiteral;
                _tokenText.Append(ch);
            }
            else if(ch == '>')
            {
                newState = DfaState.GT;
                _token.Type = TokenType.GT;
                _tokenText.Append(ch);
            }
            else if(ch == '+')
            {
                newState = DfaState.Plus;
                _token.Type = TokenType.Plus;
                _tokenText.Append(ch);
            }
            else if(ch == '-')
            {
                newState = DfaState.Minus;
                _token.Type = TokenType.Minus;
                _tokenText.Append(ch);
            }
            else if(ch == '*')
            {
                newState = DfaState.Star;
                _token.Type = TokenType.Star;
                _tokenText.Append(ch);
            }
            else if(ch == '/')
            {
                newState = DfaState.Slash;
                _token.Type = TokenType.Slash;
                _tokenText.Append(ch);
            }
            else if(ch == ';')
            {
                newState = DfaState.SemiColon;
                _token.Type = TokenType.SemiColon;
                _tokenText.Append(ch);
            }
            else if(ch == '(')
            {
                newState = DfaState.LeftParen;
                _token.Type = TokenType.LeftParen;
                _tokenText.Append(ch);
            }
            else if(ch == ')')
            {
                newState = DfaState.RightParen;
                _token.Type = TokenType.RightParen;
                _tokenText.Append(ch);
            }
            else if(ch == '=')
            {
                newState = DfaState.Assignment;
                _token.Type = TokenType.Assignment;
                _tokenText.Append(ch);
            }
            else
            {
                newState = DfaState.Initial;
            }
            return newState;
        }

        public SimpleTokenReader Tokenize(string code)
        {
            _tokens = new List<Token>();
            StringReader reader = new StringReader(code);
            _tokenText = new StringBuilder();
            _token = new SimpleToken();
            int ich = 0;
            char ch = '0';

            DfaState state = DfaState.Initial;
            try
            {
                while((ich = reader.Read()) != -1)
                {
                    ch = (char)ich;
                    switch (state)
                    {
                        case DfaState.Initial:
                            state = InitToken(ch);
                            break;
                        case DfaState.Id:
                            if(IsAlpha(ch) || IsDigit(ch))
                            {
                                _tokenText.Append(ch);
                            }
                            else
                            {
                                state = InitToken(ch);
                            }
                            break;
                        case DfaState.GT:
                            if(ch == '=')
                            {
                                _token.Type = TokenType.GE;
                                state = DfaState.GE;
                                _tokenText.Append(ch);
                            }
                            else
                            {
                                state = InitToken(ch);
                            }
                            break;
                        case DfaState.GE:
                        case DfaState.Assignment:
                        case DfaState.Plus:
                        case DfaState.Minus:
                        case DfaState.Star:
                        case DfaState.Slash:
                        case DfaState.SemiColon:
                        case DfaState.LeftParen:
                        case DfaState.RightParen:
                            state = InitToken(ch);
                            break;
                        case DfaState.IntLiteral:
                            if (IsDigit(ch))
                            {
                                _tokenText.Append(ch);
                            }
                            else
                            {
                                InitToken(ch);
                            }
                            break;
                        case DfaState.Id_int1:
                            if(ch == 'n')
                            {
                                state = DfaState.Id_int2;
                                _tokenText.Append(ch);
                            }
                            else if(IsDigit(ch) || IsAlpha(ch))
                            {
                                state = DfaState.Id;
                                _tokenText.Append(ch);
                            }
                            else
                            {
                                state = InitToken(ch);
                            }
                            break;
                        case DfaState.Id_int2:
                            if(ch == 't')
                            {
                                state = DfaState.Id_int3;
                                _tokenText.Append(ch);
                            }
                            else if(IsDigit(ch) || IsAlpha(ch))
                            {
                                state = DfaState.Id;
                                _tokenText.Append(ch);
                            }
                            else
                            {
                                state = InitToken(ch);
                            }
                            break;
                        case DfaState.Id_int3:
                            if (IsBlank(ch))
                            {
                                _token.Type = TokenType.Int;
                                state = InitToken(ch);
                            }
                            else
                            {
                                state = DfaState.Id;
                                _tokenText.Append(ch);
                            }
                            break;
                        default:
                            break;
                    }
                }

                if(_tokenText.Length > 0)
                {
                    InitToken(ch);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new SimpleTokenReader(_tokens);
        }

        private enum DfaState
        {
            Initial,

            If, Id_if1, Id_if2, Else, Id_else1, Id_else2, Id_else3, Id_else4, Int, Id_int1, Id_int2, Id_int3, Id, GT, GE,

            Assignment,

            Plus, Minus, Star, Slash,

            SemiColon,
            LeftParen,
            RightParen,

            IntLiteral
        }
    }
}
