using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

enum type
{
    STRING, INT, FLOAT, READ, WRITE, REPEAT, UNTIL, IF, ELSE, ELSEIF, THEN, RETURN, ENDL,
    ID,
    INT_NUMBER, FLOAT_NUMBER, STRING_LITERAL,
    ASSIGNMENT, PLUS, MINUS, MULTIPLY, DIVIDE,
    LESS_THAN, GREATER_THAN, EQUAL, NOT_EQUAL,
    AND, OR,
    SEMI_COLON, COMMA, LEFT_PARENTH, RIGHT_PARENTH, LEFT_BRACE, RIGHT_BRACE,
    COMMENT

}

namespace Compiler
{
    class Token
    {
        public static Dictionary<String, type> lexType = new Dictionary<string, type>(){
            {"string", type.STRING},
            {"int", type.INT},
            {"float", type.FLOAT},
            {"read", type.READ},
            {"write", type.WRITE},
            {"until", type.UNTIL},
            {"repeat", type.REPEAT},
            {"if", type.IF},
            {"else", type.ELSE},
            {"elseif", type.ELSEIF},
            {"then", type.THEN},
            {"return", type.RETURN},
            {"end", type.ENDL},
            {":=", type.ASSIGNMENT},
            {"+", type.PLUS},
            {"-", type.MINUS},
            {"*", type.MULTIPLY},
            {"/", type.DIVIDE},
            {"<", type.LESS_THAN},
            {">", type.GREATER_THAN},
            {"=", type.EQUAL},
            {"<>", type.NOT_EQUAL},
            {"&&", type.AND},
            {"||", type.OR},
            {";", type.SEMI_COLON},
            {",", type.COMMA},
            {"(", type.LEFT_PARENTH},
            {")", type.RIGHT_PARENTH},
            {"{", type.LEFT_BRACE},
            {"}", type.RIGHT_BRACE}

        };


        public string lexeme { get; set; }
        public type? tokenType { get; set; }



    };

}

