namespace ravin.cal.math
{
    using System;
    using System.Collections.Generic;
    public class Calculator<T> : ICalculator where T : new()
    {
        #region variables
        protected Stack<T>  Operand;
        protected Stack<char> Operator;
        protected Stack<T>  carryoverOperand;
        protected Stack<char> carryoverOperator;
        protected ICalculatorCore<T> calculatorCore;


        public Calculator(ICalculatorCore<T> core)
        {
            Operand = new Stack<T>();
            Operator = new Stack<char>();
            carryoverOperand = new Stack<T>();
            carryoverOperator = new Stack<char>();
            calculatorCore = core;
        }
        #endregion 
        public IEnumerable<string> GetOperators()
        { 
            return new List<string>() {
                                        "+ : to add to numbers",
                                        "- : to subtract to numbers",
                                        "* : to multiply to numbers",
                                        "/ : to divide to numbers",
                                        "/ and _ : to present mix number example 3_1/2 i.e. 3 and half"};
            }
        public string Calculate(string input)
        {
            //clear operator and operands stacks
            Operand.Clear();
            Operator.Clear();
            carryoverOperand.Clear();
            carryoverOperator.Clear();

            Parse(input);
            return Process();
        }

        #region private

        protected string  Process()
        {
            while( Operator.Count != 0 || carryoverOperator.Count !=0 )
            {
               //if ( Operator.Count == 0)
               while(carryoverOperator.Count != 0)
                {
                    Operator.Push(carryoverOperator.Pop());
                    Operand.Push(carryoverOperand.Pop());
                }

                ProcessEx();
            }

            if (Operand.Count > 1) throw new Exception("Something is not right");

            return calculatorCore.Read(Operand.Pop());
        }
        protected void ProcessEx()
        {
            if (Operator.Count == 0) return;

            var topOperator = Operator.Pop();
            if (Operator.Count == 0)
            {
                if (Operand.Count <= 1)
                    throw new ApplicationException("Invalid math equation with excess operators.");

                    var result = DoOperation(topOperator, Operand.Pop(), Operand.Pop());
                    Operand.Push(result);
            }
            else
            {
 
                    var secondtopOperator = Operator.Peek();
                    if (IsAllowed(topOperator, secondtopOperator))
                    {

                        if (Operand.Count <= 1)
                            throw new ApplicationException("Invalid math equation with excess operators.");
                        var result = DoOperation(topOperator, Operand.Pop(), Operand.Pop());
                        Operand.Push(result);
                    }
                    else
                    {
                        if (Operand.Count == 0)
                            throw new ApplicationException("Invalid math equation with excess operators.");
                        carryoverOperand.Push(Operand.Pop());
                        carryoverOperator.Push(topOperator);
                        var result = DoOperation(Operator.Pop(), Operand.Pop(), Operand.Pop());
                        Operand.Push(result);
                }
                
            }


        }
        
        protected bool IsAllowed(Char oa, Char ob)
        {
            return GetPrecedenceOrder(oa) >= GetPrecedenceOrder(ob);
        }

        protected int GetPrecedenceOrder(char c) 
        {
            switch (c)
            {
                case '+': return 5;
                case '-': return 5;
                case '*': return 9;
                case '/': return 10;
                default: throw new NotSupportedException($"operator <{c}> doesn't have precedence order.");
            }
        }

        protected T DoOperation(char c, T op1, T op2)
        {
            switch (c)
            {
                case '+': return calculatorCore.Plus(op2 ,op1);
                case '-': return calculatorCore.Minus(op2, op1);
                case '*': return calculatorCore.Multiply(op2, op1);
                case '/': return calculatorCore.Divide(op2, op1);
                default: throw new NotSupportedException($"operator <{c}> doesn't have calculation formulae.");
            }
        }
        protected void Parse(string input)
        {
            char[] delimeter = { '-', '+', '*', '/', '_' };
            string[] tokens = input.Split(delimeter);
            int charIndex = -1;
            int tokenIndex = 0;

            do
            {
                charIndex++;
                charIndex = input.Substring(charIndex).IndexOfAny(delimeter) != -1 ? input.Substring(charIndex).IndexOfAny(delimeter) + charIndex : -1;

                if (charIndex != -1 && input[charIndex] == '_')
                {
                    charIndex++;
                    charIndex = input.Substring(charIndex).IndexOfAny(delimeter) != -1 ? input.Substring(charIndex).IndexOfAny(delimeter) + charIndex : -1;
                    if (charIndex != -1 && input[charIndex] != '/')
                    {
                      throw new Exception("Invalid math equation");
                    }
                    Operand.Push(calculatorCore.CreateNew(Convert.ToInt32(tokens[tokenIndex++].Trim()), Convert.ToInt32(tokens[tokenIndex++].Trim()), Convert.ToInt32(tokens[tokenIndex++].Trim())));
                    charIndex++;
                    charIndex = input.Substring(charIndex).IndexOfAny(delimeter) != -1 ? input.Substring(charIndex).IndexOfAny(delimeter) + charIndex : -1;
                    if (charIndex != -1) Operator.Push(input[charIndex]);
                }else
                {
                    if( charIndex != -1)  Operator.Push(input[charIndex]);
                    Operand.Push(calculatorCore.CreateNew(Convert.ToInt32(tokens[tokenIndex++].Trim()), 0, 1));
                }
            } while (charIndex != -1 &&  tokenIndex < tokens.Length);
        }
        #endregion

    }//ravin.cal.math.Calculator
}//ravin.cal.math 
