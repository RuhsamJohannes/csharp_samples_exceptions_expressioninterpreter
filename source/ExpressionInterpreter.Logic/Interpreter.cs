using System;
using System.Text;

namespace ExpressionInterpreter.Logic
{
    public class Interpreter
    {
        private double _operandLeft;
        private double _operandRight;
        private char _op;  // Operator                  

        /// <summary>
        /// Eingelesener Text
        /// </summary>
        public string ExpressionText { get; private set; }

        public double OperandLeft
        {
            get { return _operandLeft; }
        }

        public double OperandRight
        {
            get { return _operandRight; }
        }

        public char Op
        {
            get { return _op; }
        }


        public void Parse(string expressionText)
        {
            ExpressionText = expressionText;
            ParseExpressionStringToFields();
        }

        /// <summary>
        /// Wertet den Ausdruck aus und gibt das Ergebnis zurück.
        /// Fehlerhafte Operatoren und Division durch 0 werden über Exceptions zurückgemeldet
        /// </summary>
        public double Calculate()
        {
            if (Op == '+')
            {
                return OperandLeft + OperandRight;
            }
            else if (Op == '-')
            {
                return OperandLeft - OperandRight;
            }
            else if (Op == '*')
            {
                return OperandLeft * OperandRight;
            }
            else 
            {

                return OperandLeft / OperandRight;
            }
        }

        /// <summary>
        /// Expressionstring in seine Bestandteile zerlegen und in die Felder speichern.
        /// 
        ///     { }[-]{ }D{D}[,D{D}]{ }(+|-|*|/){ }[-]{ }D{D}[,D{D}]{ }
        ///     
        /// Syntax  OP = +-*/
        ///         Vorzeichen -
        ///         Zahlen double/int
        ///         Trennzeichen Leerzeichen zwischen OP, Vorzeichen und Zahlen
        /// </summary>
        public void ParseExpressionStringToFields()
        {
            int pos = 0;
            SkipBlanks(ref pos);
            _operandLeft = ScanNumber(ref pos);
            SkipBlanks(ref pos);
            _op = ExpressionText[pos];
            pos++;
            SkipBlanks(ref pos);
            _operandRight = ScanNumber(ref pos);
        }

        /// <summary>
        /// Ein Double muss mit einer Ziffer beginnen. Gibt es Nachkommastellen,
        /// müssen auch diese mit einer Ziffer beginnen.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        private double ScanNumber(ref int pos)
        {
            bool isNegative = false;
            bool containsComma = false;
            int comma = 0;
            double result = 0;
            double faktor = 1;

            if (ExpressionText[pos] == '-')
            {
                isNegative = true;
                pos++;
            }
            while (pos < ExpressionText.Length && 
                (char.IsDigit(ExpressionText[pos]) || ExpressionText[pos] == ','))
            {
                if(ExpressionText[pos] == ',')
                {
                    containsComma = true;
                    comma = pos;
                }
                pos++;
            }

            int endOfNum = pos;

            if (containsComma)
            {
                pos = comma - 1;

                while (pos >= 0 && char.IsDigit(ExpressionText[pos]))
                {
                    result += ScanInteger(ref pos) * faktor;
                    faktor *= 10;
                    pos--;
                }
                pos = comma + 1;
                faktor = 1;
                while (pos < ExpressionText.Length && char.IsDigit(ExpressionText[pos]))
                {
                    faktor /= 10;
                    result += ScanInteger(ref pos) * faktor;
                    pos++;
                }
            }
            else
            {
                pos = endOfNum - 1;
                
                while (pos >= 0 && char.IsDigit(ExpressionText[pos]))
                {
                    result += ScanInteger(ref pos) * faktor;
                    faktor *= 10;
                    pos--;
                }
            }
            if (isNegative)
            {
                result = result * -1;
            }
            if (pos != ExpressionText.Length)
            {
                pos = endOfNum;
            }

            return result;
        }

        /// <summary>
        /// Eine Ganzzahl muss mit einer Ziffer beginnen.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        private int ScanInteger(ref int pos)
        {
                return ExpressionText[pos] - '0';
        }

        /// <summary>
        /// Setzt die Position weiter, wenn Leerzeichen vorhanden sind
        /// </summary>
        /// <param name="pos"></param>
        private void SkipBlanks(ref int pos)
        {
            while (char.IsWhiteSpace(ExpressionText[pos]))
            {
                pos++;
            }
        }

        /// <summary>
        /// Exceptionmessage samt Innerexception-Texten ausgeben
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetExceptionTextWithInnerExceptions(Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
