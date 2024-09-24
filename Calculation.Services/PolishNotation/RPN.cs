using Calculation.Domain.Error;

namespace Calculation.Services.PolishNotation;

public class RPN
{
    static public bool IsDelimeter(char c)
    {
        if ((" =".IndexOf(c) != -1))
            return true;
        return false;
    }

    static public bool IsOperator(char с)
    {
        if (("+-/*^()".IndexOf(с) != -1))
            return true;
        return false;
    }

    static public byte GetPriority(char s)
    {
        switch (s)
        {
            case '(': return 0;
            case ')': return 1;
            case '+': return 2;
            case '-': return 3;
            case '*': return 4;
            case '/': return 4;
            case '^': return 5;
            default: return 6;
        }
    }

    static public (double? result, Error? error) Calculate(string input)
        {
            var (output, error) = GetExpression(input);
            if (error != null)
                return (null, error);

            return Counting(output);
        }

        static private (string? output, Error? error) GetExpression(string input)
        {
            string output = string.Empty;
            Stack<char> operStack = new Stack<char>();

            try
            {
                for (int i = 0; i < input.Length; i++)
                {
                    if (IsDelimeter(input[i]))
                        continue;

                    if (Char.IsDigit(input[i]))
                    {
                        while (!IsDelimeter(input[i]) && !IsOperator(input[i]))
                        {
                            output += input[i];
                            i++;

                            if (i == input.Length) break;
                        }

                        output += " ";
                        i--;
                    }

                    if (IsOperator(input[i]))
                    {
                        if (input[i] == '(')
                            operStack.Push(input[i]);
                        else if (input[i] == ')')
                        {
                            if (operStack.Count == 0)
                                return (null, new Error("Несбалансированные скобки", 400));

                            char s = operStack.Pop();
                            while (s != '(')
                            {
                                output += s.ToString() + ' ';
                                if (operStack.Count == 0)
                                    return (null, new Error("Несбалансированные скобки", 400));
                                s = operStack.Pop();
                            }
                        }
                        else
                        {
                            while (operStack.Count > 0 && GetPriority(input[i]) <= GetPriority(operStack.Peek()))
                                output += operStack.Pop().ToString() + " ";

                            operStack.Push(input[i]);
                        }
                    }
                }

                while (operStack.Count > 0)
                {
                    char op = operStack.Pop();
                    if (op == '(')
                        return (null, new Error("Несбалансированные скобки", 400));
                    output += op + " ";
                }

                return (output, null); 
            }
            catch (Exception ex)
            {
                return (null, new Error($"Ошибка при разборе выражения: {ex.Message}", 500));
            }
        }

        static private (double? result, Error? error) Counting(string input)
        {
            double result = 0;
            Stack<double> temp = new Stack<double>();

            try
            {
                for (int i = 0; i < input.Length; i++)
                {
                    if (Char.IsDigit(input[i]))
                    {
                        string a = string.Empty;

                        while (!IsDelimeter(input[i]) && !IsOperator(input[i]))
                        {
                            a += input[i];
                            i++;
                            if (i == input.Length) break;
                        }

                        if (!double.TryParse(a, out double number))
                            return (null, new Error($"Не удалось преобразовать строку в число: {a}", 400));

                        temp.Push(number);
                        i--;
                    }
                    else if (IsOperator(input[i]))
                    {
                        if (temp.Count < 2)
                            return (null, new Error("Недостаточно операндов для операции", 400));

                        double a = temp.Pop();
                        double b = temp.Pop();

                        switch (input[i])
                        {
                            case '+':
                                result = b + a;
                                break;
                            case '-':
                                result = b - a;
                                break;
                            case '*':
                                result = b * a;
                                break;
                            case '/':
                                if (a == 0)
                                    return (null, new Error("Деление на ноль невозможно", 400));
                                result = b / a;
                                break;
                            case '^':
                                result = Math.Pow(b, a);
                                break;
                            default:
                                return (null, new Error($"Неизвестный оператор: {input[i]}", 400));
                        }

                        temp.Push(result);
                    }
                }

                if (temp.Count != 1)
                    return (null, new Error("Некорректное выражение", 400));

                return (temp.Pop(), null); 
            }
            catch (Exception ex)
            {
                return (null, new Error($"Ошибка при вычислении: {ex.Message}", 500));
            }
        }
}