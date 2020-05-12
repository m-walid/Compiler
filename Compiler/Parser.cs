using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    class Parser
    {
        private Queue<Token> tokens = new Queue<Token>();
        private Node root = new Node();
        public Parser(Queue<Token> tokens)
        {
                this.tokens = tokens;
        }


        private Node program()
        {



            return stmt_sequence();
        }

        private Node stmt_sequence()
        {



            return stmt_sequence();
        }








    }
}
