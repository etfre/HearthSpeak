using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

using System.Speech.Recognition;

namespace HearthSpeak
{
    class Program
    {
        static void Main(string[] args)
        {
            SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine();
            Grammar dictationGrammar = BuildGrammar();
            Grammar mulliganGrammer = BuildMulliganGrammar();
            recognizer.LoadGrammar(dictationGrammar);
            recognizer.LoadGrammar(mulliganGrammer);
            var gameManager = new GameManager();
            recognizer.SetInputToDefaultAudioDevice();
            System.Console.WriteLine("Ready!");
            try
            {
                while (true)
                {
                    if (args.Contains("--debug"))
                    {
                        Console.Write("-> ");
                        string userInput = Console.ReadLine();
                        Thread.Sleep(4000);
                        Program.RunInput(gameManager, new List<string>(userInput.Split(' ')));
                        continue;
                    }
                    RecognitionResult result = recognizer.Recognize();
                    if (result != null && result.Confidence > 0)
                    {
                        Thread actionThread = new Thread(() => Program.RunInput(gameManager, new List<string>(result.Text.Split(' '))));
                        actionThread.Start();
                    }
                }
            }
            finally
            {
                recognizer.UnloadAllGrammars();
            }
        }
        static Grammar BuildGrammar()
        {
            var hearthDictionary = new List<string> {
                "face", "play online", "solo adventures", "concede game", "cancel", "blue button",
                "point", "click", "finish", "power", "champion", "face", "go back", "well played",
                "thank you", "sorry", "my collection", "oops", "threaten", "greetings", "good game",
                "escape", "cancel", "naxxramas"
            };
            foreach (string desc in new string[] { "friendly", "enemy", "card", "deck", "play" })
            {
                for (int i = 0; i < 11; i++)
                {
                    hearthDictionary.Add(desc + " " + i.ToString());
                }
            }
            Choices choices = new Choices(hearthDictionary.ToArray());
            GrammarBuilder gb = new GrammarBuilder(choices, 1, 99);
            Grammar grammar = new Grammar(gb);
            return grammar;
        }
        public static void RunInput(GameManager manager, List<string> words)
        {
            System.Console.WriteLine("Got input: " + String.Join(" ", words));
            while (words.Count > 0)
            {
                string wordsText = String.Join(" ", words.ToArray());
                string matchedText = "";
                foreach (var item in manager.ActionMap)
                {
                    Match match = item.Key.Match(wordsText);
                    if (match.Success)
                    {
                        matchedText = match.Groups[0].Value;
                        item.Value(matchedText.Split(' ').ToList());
                        words = wordsText.Remove(0, matchedText.Length).Split(' ').ToList();
                        break;
                    }
                }
                if (matchedText == "")
                {
                    words.RemoveAt(0);
                }
            }
        }
        static Grammar BuildMulliganGrammar()
        {
            Choices toppings = new Choices(new string[] {
                "1", "2", "3", "4", "confirm"
            });
            GrammarBuilder gb = new GrammarBuilder("mulligan");
            gb.Append(toppings);
            gb.Append(new GrammarBuilder(toppings, 0, 4));
            Grammar grammar = new Grammar(gb);
            return grammar;
        }
    }
}
