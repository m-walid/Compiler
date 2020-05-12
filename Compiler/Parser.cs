using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    class Parser
    {
        private List<Token> tokens = new List<Token>();
        //private Node root = new Node();
        private Token currToken;
        private int index=1;
        public Parser(List<Token> tokens)
        {
                this.tokens = tokens;
                
        }

        void match(type tokenType)
        {
            if(currToken.tokenType == tokenType)
            {
                getNextToken();
            }
            else
            {
                Console.WriteLine("error");
            }
        }

        void getNextToken()
        {
            index++;
            if (index < this.tokens.Count())
            {
                currToken = this.tokens[index];
            }
            else
            {
                currToken = null;
            }
        }

        private void program()
        {
            stmt_seq();
        }

        private void stmt_seq()
        {
            stmt();
            while(currToken.tokenType == type.SEMI_COLON)
            {
                match(type.SEMI_COLON);
                stmt();
            }
        }

        private void stmt()
        {
            switch (currToken.tokenType)
            {
                case type.IF:
                    if_stmt();
                    break;
                case type.REPEAT:
                    repeat_stmt();
                    break;
                case type.ASSIGNMENT:
                    assign_stmt();
                    break;
                case type.READ:
                    read_stmt();
                    break;
                case type.WRITE:
                    write_stmt();
                    break;
            }
        }

        private void if_stmt()
        {
            match(type.IF);
            match(type.LEFT_BRACE);
            exp();
            match(type.RIGHT_BRACE);
            match(type.THEN);
            stmt_seq();
            if(currToken.tokenType == type.ELSE)
            {
                match(type.ELSE);
                stmt_seq();
               

            }
            match(type.ENDL);
        }

        private void repeat_stmt()
        {
            match(type.REPEAT);
            stmt_seq();
            match(type.UNTIL);
            exp();
        }

        private void assign_stmt()
        {
            match(type.ID);
            match(type.ASSIGNMENT);
            exp();
        }

        private void read_stmt()
        {
            match(type.READ);
            match(type.ID);
        }

        private void write_stmt()
        {
            match(type.WRITE);
            exp();
        }

        private void exp()
        {
            simple_exp();
            if(currToken.tokenType == ((currToken.tokenType == type.LESS_THAN) || (currToken.tokenType == type.GREATER_THAN) || (currToken.tokenType == type.EQUAL) ))
            {
               comp_op();
               simple_exp();
            }
        }

        private void comp_op()
        {
            if (currToken.tokenType == type.LESS_THAN)   match(type.LESS_THAN); 

            else if (currToken.tokenType == type.GREATER_THAN) match(type.GREATER_THAN);

            else if (currToken.tokenType == type.EQUAL) match(type.EQUAL);
            else Console.WriteLine("error");
        }

        private void simple_exp()
        {
            term();
            while ( currToken.tokenType == ((currToken.tokenType == type.PLUS) ||(currToken.tokenType == type.MINUS) )  )
            {
                add_op();
                term();
            }
        }

        private void add_op()
        {
            if (currToken.tokenType == type.PLUS)   match(type.PLUS); 

            else if (currToken.tokenType == type.MINUS) match(type.MINUS);
            else Console.WriteLine("error");
        }

        private void term()
        {
            factor();
            while (currToken.tokenType == (currToken.tokenType == type.MULTIPLY) ||(currToken.tokenType == type.DIVIDE) ) 
            {
                mul_op();
                factor();
            }
        }

        private void mul_op()
        {
            if (currToken.tokenType == type.MULYIPLY)  match(type.MULYIPLY);
            else if (currToken.tokenType == type.DIVIDE) match(type.DIVIDE);
            else Console.WriteLine("error");
            
        }

        private void factor()
        {
            if (currToken.tokenType == type.ID) match(type.ID);
            else if(currToken.tokenType == type.LEFT_PARENTH)
            {
                exp();
                match(type.RIGHT_PARENTH);
            }
            else if(currToken.tokenType == type.FLOAT) match(type.FLOAT);
            else if(currToken.tokenType == type.INT) match(type.INT);
        }

        




    }
}
