using System.Collections.Generic;
using System.Linq;
using System;

public class BinomialCalculator
{
    public static void Main()
    {
        Console.WriteLine("Enter binomial surrounded by () followed by a power, or type exit (ex: (-5x+9)^4)");
        string expr = Console.ReadLine();
        while (expr != "exit")
        {
            Console.WriteLine(Expand(expr));
            Console.WriteLine("Write another expression, or type exit");
            expr = Console.ReadLine();
        }
        
    }

    public static string Expand(string expr)
    {
        char[] Signs = "-+".ToCharArray();
        string ax = expr.Substring(1, expr.IndexOfAny(Signs, 2) - 1);
        string Sign = expr[expr.IndexOfAny(Signs, 2)].ToString();
        double b = double.Parse(expr.Substring(ax.Length + 1, expr.IndexOf(')') - ax.Length - 1));
        int OrigExponent = int.Parse(expr.Substring(expr.IndexOf('^') + 1));

        if (OrigExponent == 0) return 1.ToString();
        else if (OrigExponent == 1) return ax + Sign + Math.Abs(b);
        else if (b == 0) return CustomExponent(ax, OrigExponent);

        int aMultiple = OrigExponent, bMultiple = 0, ExpHalf = OrigExponent / 2 + 2, nTerm = 1; //Returns Floored value 5 >> 2
        string Result = "";
        List<int> PascalNumbers = new List<int>();

        while (nTerm != ExpHalf)
        {
            int inMultiply;
            if (nTerm == 1) inMultiply = 1;
            else inMultiply = Enumerable.Range(OrigExponent - nTerm + 2, OrigExponent - (OrigExponent - nTerm + 1)).Aggregate(1, (x, y) => x * y) / Enumerable.Range(2, nTerm - 2).Aggregate(1, (x, y) => x * y);
            PascalNumbers.Add(inMultiply);
            nTerm++;
        }
        if (OrigExponent % 2 == 0) PascalNumbers.AddRange(PascalNumbers.Where(x => x != PascalNumbers.Max()).OrderByDescending(x => x));
        else PascalNumbers.AddRange(PascalNumbers.OrderByDescending(x => x));

        nTerm = 0;
        while(nTerm != OrigExponent + 1)
        {
            Result += CustomMultiply(PascalNumbers[nTerm], CustomExponent(ax, aMultiple), Math.Pow(b, bMultiple));
            if (nTerm == 0 && Result[0] == '+') Result = Result.Substring(1);
            aMultiple--;
            bMultiple++;
            nTerm++;
        }
        return Result;
    }

    private static string CustomExponent(string a, int Exponent)
    {
        if (Exponent == 0) return 1.ToString();
        else if (Exponent == 1) return a.ToString();
        char[] Numbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        char Prefix = a.FirstOrDefault(char.IsLetter);
        double Num;
        int FirstNumIndex = a.IndexOfAny(Numbers);
        if (FirstNumIndex != -1 && double.TryParse(a.Substring(FirstNumIndex, a.LastIndexOfAny(Numbers) - FirstNumIndex + 1), out double Res)) Num = Res;
        else Num = 1;
        int Sign = a.IndexOf('-') == -1 ? 1 : -1;
        return ((Sign == -1 ? (Exponent % 2 == 0 ? 1 : -1) : 1) * Math.Pow(Num, Exponent)).ToString() + (char.IsLetter(Prefix) ? $"{Prefix}^{Exponent}" : "");
    }

    private static string CustomMultiply(int Pasc, string a, double b)
    {
        char[] Numbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        double Num = Pasc * b, FirstNumIndex = a.IndexOfAny(Numbers);
        int Sign = a.ToCharArray().Contains('-') ^ b < 0 ? -1 : 1;
        int IndexOfFirstLetter = a.IndexOf(a.FirstOrDefault(char.IsLetter));
        string Func = IndexOfFirstLetter > -1 ? a.Substring(IndexOfFirstLetter) : "";
        if (double.TryParse(new string(a.Where(c => c != '-').TakeWhile(char.IsDigit).ToArray()), out double Res)) Num *= Res;
        if (Num == 1 && !string.IsNullOrEmpty(Func)) return (Sign == -1 ? "-" : "+") + Func;
        else if (Num == 0) return "";
        else return (Sign == 1 ? "+" : "-") + Math.Abs(Num) + Func;
    }
}