using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compiler
{
    
    class Parser
    {
        private Token[] tokens;
        public List<TreeNode> rootNodes { get; set; }
        private Token currToken;
        private int index = -1;
        private bool finishedFlag=false;
        private bool ifFlag = false;
        private bool repeatFlag = false;


        public Parser(List<Token> tokens)
        {
            this.tokens = tokens.ToArray().Where(token => token.tokenType != type.COMMENT).ToArray();
            GetNextToken();
            rootNodes = Program();

        }

        void Match(type tokenType)
        {
            if (currToken.tokenType == tokenType)
            {
                GetNextToken();
            }
            else
            {
                if (finishedFlag) throw new InvalidExpectedToken("Error : missing Expected token " + tokenType);

                throw new InvalidExpectedToken("Error : UnExpected token " + currToken.lexeme + " of type " + currToken.tokenType + " , Expected token is " + tokenType + " at token index "+index);
            }
        }

        void GetNextToken()
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
        }

        private List<TreeNode> Program()
        {
            return StmtSeq();
        }

        private List<TreeNode> StmtSeq()
        {
            List<TreeNode> nodeList = new List<TreeNode>();
            nodeList.Add(Stmt());
            while (currToken.tokenType == type.SEMI_COLON)
            {
                Match(type.SEMI_COLON);
                nodeList.Add(Stmt());
            }
            if (!ifFlag && !repeatFlag && !finishedFlag ) throw new InvalidExpectedToken("Error : Unexpected token " + currToken.lexeme + " of type " + currToken.tokenType + " , Expected token is " + type.SEMI_COLON + " at token index " + index );
            return nodeList;
        }

        private TreeNode Stmt()
        {
            switch (currToken.tokenType)
            {
                case type.IF:
                    return IfStmt();
                case type.REPEAT:
                    return RepeatStmt();

                case type.READ:
                    return ReadStmt();
                case type.WRITE:
                    return WriteStmt();
                case type.ID:
                    return AssignStmt();
                default:
                    throw new InvalidExpectedToken("Error : Expected a statment");

            }
        }

        private TreeNode IfStmt()
        {
            Match(type.IF);
            TreeNode ifNode = new TreeNode("if");
            ifNode.Nodes.Add(Exp());
            Match(type.THEN);
            ifFlag = true;
            ifNode.Nodes.AddRange(StmtSeq().ToArray());
            ifFlag = false;
            if (currToken.tokenType == type.ELSE)
            {
                Match(type.ELSE);
                ifFlag = true;
                ifNode.Nodes.AddRange(StmtSeq().ToArray());
                ifFlag = false;

            }
            Match(type.ENDL);
            return ifNode;
        }

        private TreeNode RepeatStmt()
        {
            Match(type.REPEAT);
            TreeNode repeatNode = new TreeNode("repeat");
            repeatFlag = true;
            repeatNode.Nodes.AddRange(StmtSeq().ToArray());
            repeatFlag = false;
            Match(type.UNTIL);
            repeatNode.Nodes.Add(Exp());
            return repeatNode;
        }

        private TreeNode AssignStmt()
        {
            TreeNode assignNode = new TreeNode("assign (" + currToken.lexeme + ")");
            Match(type.ID);
            Match(type.ASSIGNMENT);

            assignNode.Nodes.Add(Exp());
            return assignNode;
        }

        private TreeNode ReadStmt()
        {
            Match(type.READ);
            TreeNode readNode = new TreeNode("read (" + currToken.lexeme + ")");
            Match(type.ID);
            return readNode;
        }

        private TreeNode WriteStmt()
        {
            Match(type.WRITE);
            TreeNode writeNode = new TreeNode("write");
            writeNode.Nodes.Add(Exp());
            return writeNode;
        }

        private TreeNode Exp()
        {
            TreeNode simpleExpNode1 = SimpleExp();
            if (currToken.tokenType == type.LESS_THAN || currToken.tokenType == type.GREATER_THAN || currToken.tokenType == type.EQUAL)
            {
                TreeNode opNode = CompOp();
                TreeNode simpleExpNode2 = SimpleExp();
                opNode.Nodes.Add(simpleExpNode1);
                opNode.Nodes.Add(simpleExpNode2);
                return opNode;
            }
            return simpleExpNode1;
        }

        private TreeNode CompOp()
        {
            TreeNode compNode = new TreeNode("op (" + currToken.lexeme + ")");
            if (currToken.tokenType == type.LESS_THAN)
            {
                Match(type.LESS_THAN);
            }
            else if (currToken.tokenType == type.GREATER_THAN)
            {
                Match(type.GREATER_THAN);
            }
            else if (currToken.tokenType == type.EQUAL)
            {
                Match(type.EQUAL);
            }
            else
            {
                throw new InvalidExpectedToken("Error : Invalid operation " + currToken.lexeme);
            }
            return compNode;
        }

        private TreeNode SimpleExp()
        {
            TreeNode termNode1 = Term();
            TreeNode addOpNode = null;
            TreeNode temp = null;
            while (currToken.tokenType == type.PLUS || currToken.tokenType == type.MINUS)
            {
                temp = addOpNode;
                addOpNode = AddOp();
                if (temp != null) termNode1 = temp;
                TreeNode Term_node2 = Term();
                addOpNode.Nodes.Add(termNode1);
                addOpNode.Nodes.Add(Term_node2);

            }
            if (addOpNode != null) return addOpNode;
            return termNode1;
        }

        private TreeNode AddOp()
        {
            TreeNode addNode = new TreeNode("op (" + currToken.lexeme + ")");
            if (currToken.tokenType == type.PLUS) Match(type.PLUS);
            else if (currToken.tokenType == type.MINUS) Match(type.MINUS);
            else
            {
                throw new InvalidExpectedToken("Error : Invalid operation " + currToken.lexeme);
            }
            return addNode;
        }

        private TreeNode Term()
        {
            TreeNode fNode1 = Factor();
            TreeNode mulOpNode = null;
            TreeNode temp = null;
            while (currToken.tokenType == type.MULTIPLY || currToken.tokenType == type.DIVIDE)
            {
                temp = mulOpNode;
                mulOpNode = MulOp();
                if (temp != null) fNode1 = temp;
                TreeNode fNode2 = Factor();
                mulOpNode.Nodes.Add(fNode1);
                mulOpNode.Nodes.Add(fNode2);

            }
            if (mulOpNode != null) return mulOpNode;
            return fNode1;
        }

        private TreeNode MulOp()
        {
            TreeNode mulNode = new TreeNode("op (" + currToken.lexeme + ")");
            if (currToken.tokenType == type.MULTIPLY) Match(type.MULTIPLY);
            else if (currToken.tokenType == type.DIVIDE) Match(type.DIVIDE);
            else
            {
                throw new InvalidExpectedToken("Error : Invalid operation " + currToken.lexeme);
            }
            return mulNode;
        }

        private TreeNode Factor()
        {
            if (currToken.tokenType == type.ID)
            {
                TreeNode idNode = new TreeNode("id" + "(" + currToken.lexeme + ")");
                Match(type.ID);
                return idNode;
            }
            else if (currToken.tokenType == type.LEFT_PARENTH)
            {
                Match(type.LEFT_PARENTH);
                TreeNode exp = Exp();
                Match(type.RIGHT_PARENTH);
                return exp;
            }
            else if (currToken.tokenType == type.FLOAT_NUMBER)
            {
                TreeNode number = new TreeNode("const" + "(" + currToken.lexeme + ")");
                Match(type.FLOAT_NUMBER);
                return number;
            }
            else if (currToken.tokenType == type.INT_NUMBER)
            {
                TreeNode number = new TreeNode("const" + "(" + currToken.lexeme + ")");
                Match(type.INT_NUMBER);
                return number;
            }
            else
            {

                throw new InvalidExpectedToken("Error : Expected Factor");
            }
        }
    }
}
