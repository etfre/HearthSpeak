using System;
using System.Speech.Recognition;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthSpeak.menus
{
    class MainMenu : BaseMenu
    {
        public MainMenu()
        {
            Add("Start Listening", StartListening);
            Add("Change Screen Resolution", AdjustResolution);
            Add("Quit", AdjustResolution);
            Prompt = "Enter an option: ";
            Display();
            Console.ReadLine();
        }

        public void AdjustResolution()
        {

        }

        public void StartListening()
        {
            var recognizer = new Recognizer();
            System.Console.WriteLine("Listening for input. Press Enter to stop listening.");
            Console.ReadLine();
            recognizer.Engine.RecognizeAsyncCancel();
            new MainMenu();
        }

    }
}
