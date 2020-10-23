using System;
using System.Collections.Generic;
using System.Text;

namespace stone.app
{
    public enum TokenType
    {
        Plus,   //+
        Minus,  //-
        Star,  //*
        Slash,  // /

        GE,  //>=
        GT,  //>
        EQ,  //==
        LE,  //<=
        LT,   //<

        SemiColon,  // ;
        LeftParen,  // (
        RightParen,  //)
        Assignment,  // =

        If,
        Else,

        Int,

        Identifier,  //标志符

        IntLiteral,  //整型字面量
        StringLiteral  //字符型字面量
    }
}
