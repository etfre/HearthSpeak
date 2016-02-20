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
        static private GameManager gameManager;

        static void Main(string[] args)
        {
            gameManager = new GameManager();
            var recognizer = new SpeechRecognitionEngine();
            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);
            recognizer.SetInputToDefaultAudioDevice();
            Grammar dictationGrammar = BuildGrammar();
            Grammar mulliganGrammer = BuildMulliganGrammar();
            recognizer.LoadGrammar(dictationGrammar);
            recognizer.LoadGrammar(mulliganGrammer);
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
            System.Console.WriteLine("Ready!");
            while (true) { }
        }

        static Grammar BuildGrammar()
        {
            var hearthDictionary = new List<string> {
                "face", "play online", "solo adventures", "concede game", "cancel", "blue button",
                "point", "click", "finish", "power", "champion", "face", "go back", "well played",
                "thank you", "sorry", "my collection", "oops", "threaten", "greetings", "good game",
                "escape", "cancel", "casual", "ranked", "quest log", "naxxramas"
            };
            foreach (string desc in new string[] { "friendly", "enemy", "card", "deck", "play", "choose" })
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
        static void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result == null || e.Result.Confidence <= .85) return;
            List<string> words = e.Result.Text.Split(' ').ToList();
            System.Console.WriteLine("Got input: " + String.Join(" ", words));
            while (words.Count > 0)
            {
                string wordsText = String.Join(" ", words.ToArray());
                string matchedText = "";
                foreach (var item in gameManager.ActionMap)
                {
                    Match match = item.Key.Match(wordsText);
                    if (!match.Success) continue;
                    matchedText = match.Groups[0].Value;
                    item.Value(matchedText.Split(' ').ToList());
                    words = wordsText.Remove(0, matchedText.Length).Split(' ').ToList();
                    break;
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
