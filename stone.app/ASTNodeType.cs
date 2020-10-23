using System;
using System.Collections.Generic;
using System.Text;

namespace stone.app
{
    public enum ASTNodeType
    {
        Program, //程序入口，根节点

        IntDeclaration,  //整型变量声明
        ExpressionStmt,   //表达式变语句，即表达式后面跟个分号
        AssignmentStmt,  //赋值语句

        Primary,   //基础表达式
        Multiplicative,  //乘法表达式
        Additive,   //加法表达式

        Identifier,   //标志符
        IntLiteral   //整型字面量
    }
}
