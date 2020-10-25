using System;
using System.Text.RegularExpressions;

namespace stone.app
{
    class Program
    {
        static void Main(string[] args)
        {

            //string input = "1851 1999 1950 1905 2003";
            //string pattern = @"(?<=19)\d{2}\b";

            //string pattan2 = "\\s*((//.*)|([0-9]+)|(\"(\\\\\"|\\\\\\\\|\\\\n|[^\"])*\")"
            //    + "|[A-Z_a-z]|[A-Z_a-z0-9]*|==|<=|>=|&&|\\|\\||\\p{P})?";
            //string pattern = "(helloloq)*(world)*";


            //foreach (Match match in Regex.Matches(input, pattern))
            //    Console.WriteLine(match.Value);
            SimpleCalculator.TextCalculator();

            Console.ReadKey();
        }

      
        private static void TestLexer()
        {
            SimpleLexer lexer = new SimpleLexer();
            string script = "int age = 45;";
            SimpleTokenReader tokenReader = lexer.Tokenize(script);
            Dump(tokenReader);

            script = "inta age = 45;";
            tokenReader = lexer.Tokenize(script);
            Dump(tokenReader);
            Console.ReadKey();
        }
        static void Dump(SimpleTokenReader tokenReader)
        {
            Console.WriteLine("text\ttype");
            Token token = null;
            while((token = tokenReader.Read()) != null)
            {
                Console.WriteLine(token.GetText() + "\t\t" + token.GetType());
            }
        }
    }
}
