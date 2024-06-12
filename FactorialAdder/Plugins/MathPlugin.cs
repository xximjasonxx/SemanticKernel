using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace FactorialAdder.Plugins
{
    
    public sealed class MathPlugin
    {
        [KernelFunction("factorial"), Description("Calculates the factorial of a number")]
        public static int Factorial(
            [Description("The number to get the factorial of")]int number)
        {
            int result = 1;
            while (number > 1)
            {
                result *= number--;
            }

            return result;
        }

        [KernelFunction("add"), Description("Adds two numbers")]
        public static int Add(
            [Description("The first number to add")]int numberOne,
            [Description("The second number to add")]int numberTwo)
        {
            return numberOne + numberTwo;
        }

        [KernelFunction("subtract10"), Description("Subtracts 10 from a number")]
        public static int Subtract10(
            [Description("The number to subtract 10 from")]int number)
        {
            return number - 10;
        }
    }
}