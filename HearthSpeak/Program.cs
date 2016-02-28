using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using HearthSpeak.menus;
using System.Speech.Recognition;

namespace HearthSpeak
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to HearthSpeak!\n");
            if (Properties.Settings.Default.StartListeningAtLaunch)
            {
                var recognizer = new Recognizer();
                recognizer.ListenIO();
            }
            else new MainMenu();
        }
    }
}
