using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;


enum state
{
    START, DONE, ERROR, IN_NUM, IN_ID, IN_ASSIGN, IN_SLASH, IN_COMMENT, LEAVE_COMMENT, IN_NOT_EQUAL, IN_AND, IN_OR, IN_STRING
}


namespace Compiler
{
    class Scanner
    {
        const string WHITE_SPACE = @"\s";
        const string LETTER = @"[A-Za-z]";
        const string VALID_ID = @"[a-zA-Z0-9]";
        const string DIGIT = @"\d";


        public List<Token> tokens = new List<Token>();
        private string input;
        public int index = -1;
        private char? currentChar = null;
        private state currentState = state.START;
        private Token currentToken;
        public bool flag = false;
        public string errorMsg;

        public Scanner(string input)
        {
            this.input = input;
            getNext();
            getTokens();
            

        }
        private void getNext()
        {
            index++;
            if (index < input.Length)
            {
                currentChar = input[index];
            }
            else
            {
                currentChar = null;
            }
        }


        


        private void handleStart()
        {
            currentToken.lexeme += currentChar;
            //for loop 3la el dict
            if (currentChar == ':') currentState = state.IN_ASSIGN;
            else if (currentChar == '/') currentState = state.IN_SLASH;
            else if (currentChar == '<') currentState = state.IN_NOT_EQUAL;
            else if (currentChar == '&') currentState = state.IN_AND;
            else if (currentChar == '|') currentState = state.IN_OR;
            else if (currentChar == '"') currentState = state.IN_STRING;
            else if (new Regex(DIGIT).IsMatch(currentChar + "")) currentState = state.IN_NUM;
            else if (new Regex(LETTER).IsMatch(currentChar + "")) currentState = state.IN_ID;
            else if (new Regex(WHITE_SPACE).IsMatch(currentChar + "")) { currentState = state.START; currentToken.lexeme = ""; }
            else if (Token.lexType.ContainsKey(currentToken.lexeme)) currentToken.tokenType = Token.lexType[currentToken.lexeme];
            else currentState = state.ERROR;
            if (currentToken.tokenType != null)
            {
                currentState = state.DONE;
                getNext();
            }
        }

        private void handleString()
        {
            currentToken.lexeme += currentChar;
            if (currentChar == '"')
            {

                currentState = state.DONE;
                currentToken.tokenType = type.STRING_LITERAL;
                getNext();
            }
        }
        private void handleNumber()
        {
            if (new Regex(DIGIT).IsMatch(currentChar + ""))
            {
                currentToken.lexeme += currentChar;
            }
            else if (currentChar == '.')
            {
                if (currentToken.lexeme.Contains('.'))
                {
                    currentState = state.ERROR;
                }
                else
                {
                    currentToken.lexeme += currentChar;
                }
            }
            else if (new Regex(LETTER).IsMatch(currentChar + ""))
            {
                currentState = state.ERROR;
            }
            else
            {
                if (currentToken.lexeme[currentToken.lexeme.Length - 1] == '.') currentState = state.ERROR;
                else if (currentToken.lexeme.Contains('.'))
                {
                    currentToken.tokenType = type.FLOAT_NUMBER;
                    currentState = state.DONE;
                }
                else
                {
                    currentToken.tokenType = type.INT_NUMBER;
                    currentState = state.DONE;
                }

            }
        }
        private void handleInComment()
        {
                currentToken.lexeme += currentChar;
            if (currentChar == '*')
            {
                getNext();
                    handleLeaveComment();
            }   
        }
        private void handleLeaveComment()
        {
            if (currentChar == '/')
            {
                
                currentToken.lexeme += currentChar;
                currentToken.tokenType = type.COMMENT;
                currentState = state.DONE;
                getNext();

            }
        }
        private void handleError()
        {
            flag = true;
            if (currentChar == ' ')
            {
               errorMsg = ($"Error occurred at \"" + currentToken.lexeme + "\" at index \"" + (index + 1) + "\" , please review code.");

            }
            else
            {
                errorMsg = ($"Error occurred at \"" + currentChar + "\" at index \"" + (index + 1) + "\" , please review code.");


            }
            currentChar = null;
        }

        private void handleAnd()
        {
            if (currentChar == '&')
            {
                currentToken.lexeme += currentChar;
                currentToken.tokenType = type.AND;
                currentState = state.DONE;
                getNext();
            }
            else
            {
                currentState = state.ERROR;
            }
        }
            private void handleOr()
        {
            if (currentChar == '|')
            {
                currentToken.lexeme += currentChar;
                currentToken.tokenType = type.OR;
                currentState = state.DONE;
                getNext();
            }
            else
            {
                currentState = state.ERROR;
            }
        }
        private void handleID()
        {
            if (new Regex(VALID_ID).IsMatch(currentChar + "") == true)
            {
                currentToken.lexeme += currentChar;

            }
            else if (Token.lexType.ContainsKey(currentToken.lexeme))
            {
                currentToken.tokenType = Token.lexType[currentToken.lexeme];
                currentState = state.DONE;
            }
            else
            {
                currentToken.tokenType = type.ID;
                currentState = state.DONE;
            }

        }




        private void handleSlash()
        {
            if (currentChar == '*')
            {
                currentToken.lexeme += currentChar;
                currentState = state.IN_COMMENT;
            }
            else
            {
                currentToken.tokenType = type.DIVIDE;
                currentState = state.DONE;
            }
        }

        private void handleAssign()
        {
            if (currentChar == '=')
            {
                currentToken.lexeme += currentChar;
                currentToken.tokenType = type.ASSIGNMENT;
                currentState = state.DONE;
                getNext(); //because w don't need to check this char anymore

            }
            else
            {
                currentState = state.ERROR;
            }
        }


        private void getTokens()
        {
            while (currentChar != null)
            {
                currentState = state.START;
                currentToken = new Token();
                currentToken.lexeme = "";

                while (currentState != state.DONE && currentChar != null)
                {
                    switch (currentState)
                    {
                        case state.START:
                            handleStart();
                            break;
                        case state.IN_NUM:
                            handleNumber();
                            break;
                        case state.IN_ID:
                            handleID();
                            break;
                        case state.IN_ASSIGN:
                            handleAssign();
                            break;
                        case state.IN_SLASH:
                            handleSlash();
                            break;
                        case state.IN_COMMENT:
                            handleInComment();
                            break;
                        case state.LEAVE_COMMENT:
                            handleLeaveComment();
                            break;
                        case state.IN_STRING:
                            handleString();
                            break;
                        case state.IN_NOT_EQUAL:
                            if (currentChar == '>')
                            {
                                currentToken.tokenType = type.NOT_EQUAL;
                                currentToken.lexeme += currentChar;
                                getNext();
                            }
                            else
                            {
                                currentToken.tokenType = type.LESS_THAN;

                            }
                            currentState = state.DONE;
                            break;
                        case state.IN_AND:
                            handleAnd();
                            break;
                        case state.IN_OR:
                            handleOr();
                            break;
                        case state.ERROR:
                            handleError();
                            break;
                    }
                    if (currentState != state.DONE && currentState != state.ERROR) getNext(); //because if done we need to check the last char before we advance
                }
                tokens.Add(currentToken); 
            }
        }





    }




}
