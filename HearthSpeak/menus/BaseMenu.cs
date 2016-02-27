using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthSpeak.menus
{
    class BaseMenu
    {
        // Can't use Action with OrderedDictionary??
        public List<string> OptionNames;
        public List<Action> OptionFuncs;
        public string Prompt;

        public BaseMenu()
        {
            OptionNames = new List<string>();
            OptionFuncs = new List<Action>();
        }

        public void Add(string name, Action func)
        {
            OptionNames.Add(name);
            OptionFuncs.Add(func);
        }

        public void Display()
        {
            for (int i = 0; i < OptionNames.Count; i++)
            {
                Console.WriteLine((i+1) + ". " + OptionNames[i]);
            }
            GetInput();
        }

        void GetInput()
        {
            Console.WriteLine("");
            int inputNum = -1;
            while (true)
            {
                Console.WriteLine(Prompt);
                string input = Console.ReadLine();
                bool result = Int32.TryParse(input, out inputNum);
                if (result && inputNum > 0 && inputNum <=
                    OptionNames.Count) break;
                Console.WriteLine("\nInvalid Input");
            }
            Console.WriteLine("\n");
            OptionFuncs[inputNum - 1]();

        }

    }
}
