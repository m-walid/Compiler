using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compiler
{
    class InvalidExpectedToken : Exception
    {
        private string msg;
        public InvalidExpectedToken(string s) 
        {
            msg = s;

        }
        public override string Message{
            get{return msg;}
        }
    }
    class Parser
    {
        private Token[] tokens;
        public List<TreeNode> root_nodes { get; set; }
        private Token currToken;
        private int index = -1;
        private bool finishedFlag=false;
        private bool ifFlag = false;
        private bool repeatFlag = false;


        public Parser(List<Token> tokens)
        {
            this.tokens = tokens.ToArray().Where(token => token.tokenType != type.COMMENT).ToArray();
            getNextToken();
            root_nodes = program();

        }

        void match(type tokenType)
        {
            if (currToken.tokenType == tokenType)
            {
                getNextToken();
            }
            else
            {
                if (finishedFlag) throw new InvalidExpectedToken("Error : missing expected token " + tokenType);

                throw new InvalidExpectedToken("Error : Unexpected token " + currToken.lexeme + " of type " + currToken.tokenType + " , expected token is " + tokenType + " at token index "+index);
            }
        }

        void getNextToken()
        {
            index++;
            if (index < this.tokens.Length)
            {
                currToken = this.tokens[index];
            }
            else
            {
                finishedFlag = true;

            }
            Console.WriteLine(this.tokens.Length);
        }

        private List<TreeNode> program()
        {
            return stmt_seq();
        }

        private List<TreeNode> stmt_seq()
        {
            List<TreeNode> node_list = new List<TreeNode>();
            TreeNode test = stmt();
            node_list.Add(test);
            while (currToken.tokenType == type.SEMI_COLON)
            {
                match(type.SEMI_COLON);
                node_list.Add(stmt());
            }
            if (!ifFlag && !repeatFlag && !finishedFlag ) throw new InvalidExpectedToken("Error : Unexpected token " + currToken.lexeme + " of type " + currToken.tokenType + " , expected token is " + type.SEMI_COLON + " at token index " + index );
            return node_list;
        }

        private TreeNode stmt()
        {
            switch (currToken.tokenType)
            {
                case type.IF:
                    return if_stmt();
                case type.REPEAT:
                    return repeat_stmt();

                case type.READ:
                    return read_stmt();
                case type.WRITE:
                    return write_stmt();
                case type.ID:
                    return assign_stmt();
                default:
                    throw new InvalidExpectedToken("Error : Expected a statment");

            }
        }

        private TreeNode if_stmt()
        {
            match(type.IF);
            TreeNode if_node = new TreeNode("if");
            if_node.Nodes.Add(exp());
            match(type.THEN);
            ifFlag = true;
            if_node.Nodes.AddRange(stmt_seq().ToArray());
            ifFlag = false;
            if (currToken.tokenType == type.ELSE)
            {
                match(type.ELSE);
                ifFlag = true;
                if_node.Nodes.AddRange(stmt_seq().ToArray());
                ifFlag = false;

            }
            match(type.ENDL);
            return if_node;
        }

        private TreeNode repeat_stmt()
        {
            match(type.REPEAT);
            TreeNode repeat_node = new TreeNode("repeat");
            repeatFlag = true;
            repeat_node.Nodes.AddRange(stmt_seq().ToArray());
            repeatFlag = false;
            match(type.UNTIL);
            repeat_node.Nodes.Add(exp());
            return repeat_node;
        }

        private TreeNode assign_stmt()
        {
            TreeNode assign_node = new TreeNode("Assign (" + currToken.lexeme + ")");
            match(type.ID);
            match(type.ASSIGNMENT);

            assign_node.Nodes.Add(exp());
            return assign_node;
        }

        private TreeNode read_stmt()
        {
            match(type.READ);
            TreeNode read_node = new TreeNode("read (" + currToken.lexeme + ")");
            match(type.ID);
            return read_node;
        }

        private TreeNode write_stmt()
        {
            match(type.WRITE);
            TreeNode write_node = new TreeNode("write");
            write_node.Nodes.Add(exp());
            return write_node;
        }

        private TreeNode exp()
        {
            TreeNode simple_exp_node1 = simple_exp();
            if (currToken.tokenType == type.LESS_THAN || currToken.tokenType == type.GREATER_THAN || currToken.tokenType == type.EQUAL)
            {
                TreeNode op_node = comp_op();
                TreeNode simple_exp_node2 = simple_exp();
                op_node.Nodes.Add(simple_exp_node1);
                op_node.Nodes.Add(simple_exp_node2);
                return op_node;
            }
            return simple_exp_node1;
        }

        private TreeNode comp_op()
        {
            TreeNode comp_node = new TreeNode("op (" + currToken.lexeme + ")");
            if (currToken.tokenType == type.LESS_THAN)
            {
                match(type.LESS_THAN);
            }
            else if (currToken.tokenType == type.GREATER_THAN)
            {
                match(type.GREATER_THAN);
            }
            else if (currToken.tokenType == type.EQUAL)
            {
                match(type.EQUAL);
            }
            else
            {
                throw new InvalidExpectedToken("Error : Invalid operation " + currToken.lexeme);
            }
            return comp_node;
        }

        private TreeNode simple_exp()
        {
            TreeNode term_node1 = term();
            TreeNode add_op_node = null;
            TreeNode temp = null;
            while (currToken.tokenType == type.PLUS || currToken.tokenType == type.MINUS)
            {
                temp = add_op_node;
                add_op_node = add_op();
                if (temp != null) term_node1 = temp;
                TreeNode term_node2 = term();
                add_op_node.Nodes.Add(term_node1);
                add_op_node.Nodes.Add(term_node2);

            }
            if (add_op_node != null) return add_op_node;
            return term_node1;
        }

        private TreeNode add_op()
        {
            TreeNode add_node = new TreeNode("op (" + currToken.lexeme + ")");
            if (currToken.tokenType == type.PLUS) match(type.PLUS);
            else if (currToken.tokenType == type.MINUS) match(type.MINUS);
            else
            {
                throw new InvalidExpectedToken("Error : Invalid operation " + currToken.lexeme);
            }
            return add_node;
        }

        private TreeNode term()
        {
            TreeNode f_node1 = factor();
            TreeNode mul_op_node = null;
            TreeNode temp = null;
            while (currToken.tokenType == type.MULTIPLY || currToken.tokenType == type.DIVIDE)
            {
                temp = mul_op_node;
                mul_op_node = mul_op();
                if (temp != null) f_node1 = temp;
                TreeNode f_node2 = factor();
                mul_op_node.Nodes.Add(f_node1);
                mul_op_node.Nodes.Add(f_node2);

            }
            if (mul_op_node != null) return mul_op_node;
            return f_node1;
        }

        private TreeNode mul_op()
        {
            TreeNode mul_node = new TreeNode("op (" + currToken.lexeme + ")");
            if (currToken.tokenType == type.MULTIPLY) match(type.MULTIPLY);
            else if (currToken.tokenType == type.DIVIDE) match(type.DIVIDE);
            else
            {
                throw new InvalidExpectedToken("Error : Invalid operation " + currToken.lexeme);
            }
            return mul_node;
        }

        private TreeNode factor()
        {
            if (currToken.tokenType == type.ID)
            {
                TreeNode id_node = new TreeNode(currToken.tokenType + "(" + currToken.lexeme + ")");
                match(type.ID);
                return id_node;
            }
            else if (currToken.tokenType == type.LEFT_PARENTH)
            {
                match(type.LEFT_PARENTH);
                TreeNode t = exp();
                match(type.RIGHT_PARENTH);
                return t;
            }
            else if (currToken.tokenType == type.FLOAT_NUMBER)
            {
                TreeNode number = new TreeNode(currToken.tokenType + "(" + currToken.lexeme + ")");
                match(type.FLOAT_NUMBER);
                return number;
            }
            else if (currToken.tokenType == type.INT_NUMBER)
            {
                TreeNode number = new TreeNode(currToken.tokenType + "(" + currToken.lexeme + ")");
                match(type.INT_NUMBER);
                return number;
            }
            else
            {

                throw new InvalidExpectedToken("Error : Expected factor");
            }
        }
    }
}
