using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBlackTree
{
    class Program
    {
        static int comparer(string valueOne, string valueTwo)
        {
            if(int.TryParse(valueOne, out int resultOne) && int.TryParse(valueTwo, out int resultTwo))
            {
                return resultOne.CompareTo(resultTwo);
            }

            return valueOne.CompareTo(valueTwo);
        }

        static void Main(string[] args)
        {
           
            Tree<string> tree = new Tree<string>(Comparer<string>.Create((a, b) => comparer(a, b)));
            tree.Add("0");
            tree.Add("1");
            tree.Add("2");
            tree.Add("3");
            tree.Add("4");
            tree.Add("5");
            tree.Add("6");
            tree.Add("7");
            tree.Add("8");
            tree.Add("9");
            
            tree.Display();
            Console.ReadKey();
        }
    }
}
