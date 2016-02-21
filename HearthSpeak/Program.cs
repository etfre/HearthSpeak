using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            AddGrammars(recognizer);
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
            System.Console.WriteLine("Ready!");
            while (true) Thread.Sleep(3000);
        }

        static Grammar BuildGrammar()
        {
            var hearthDictionary = new List<string> {
                "face", "play online", "solo adventures", "concede game", "cancel", "blue button",
                "position", "click", "finish", "power", "champion", "face", "go back", "well played",
                "thank you", "sorry", "my collection", "oops", "threaten", "greetings", "good game",
                "escape", "cancel", "casual", "ranked", "quest log", "center mouse", "hide mouse", "naxxramas"
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

        static void AddGrammars(SpeechRecognitionEngine recognizer)
        {
            Grammar dictationGrammar = BuildGrammar();
            Grammar mulliganGrammer = MakeRepeatedGrammar(new string[] { "mulligan" }, new string[] { "1", "2", "3", "4", "confirm" }, 99);
            Grammar moveGrammar = MakeMoveGrammar();
            recognizer.LoadGrammar(dictationGrammar);
            recognizer.LoadGrammar(mulliganGrammer);
            recognizer.LoadGrammar(moveGrammar);
        }

        private static Grammar MakeRepeatedGrammar(string[] firstWords, string[] choicesArr, int choicesMax=99)
        {
            var gb = new GrammarBuilder(new Choices(firstWords));
            var choices = new Choices(choicesArr);
            gb.Append(new GrammarBuilder(choices, 1, choicesMax));
            return new Grammar(gb);
        }

        private static Grammar MakeMoveGrammar()
        {
            var directions = new string[] { "up", "right", "down", "left" };
            var numberList = Enumerable.Range(0, 9).Select(i => i.ToString()).ToArray();
            var numberChoices = new Choices(numberList);
            var gb = new GrammarBuilder(new Choices(directions));
            gb.Append(numberChoices, 0, 4);
            gb.Append(new GrammarBuilder("point", 0, 1));
            gb.Append(new GrammarBuilder(numberChoices, 0, 4));
            return new Grammar(gb);
        }

    }
}
