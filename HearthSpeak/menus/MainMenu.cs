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
            //Add("Change Screen Resolution", AdjustResolution);
            Add("Quit", ExitApplication);
            Prompt = "Enter an option: ";
            Display();
            Console.ReadLine();
        }

        public void AdjustResolution()
        {
            
        }

        public void ExitApplication()
        {
            System.Environment.Exit(1);
        }

        public void StartListening()
        {
            var recognizer = new Recognizer();
            recognizer.ListenIO();
        }

    }
}
