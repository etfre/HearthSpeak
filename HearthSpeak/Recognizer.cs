using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text.RegularExpressions;
using HearthSpeak.menus;
using System.Text;
using System.Threading.Tasks;

namespace HearthSpeak
{
    class Recognizer
    {

        GameManager gameManager;
        public SpeechRecognitionEngine Engine;

        public Recognizer()
        {
            gameManager = new GameManager();
            Engine = new SpeechRecognitionEngine();
            Engine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);
            Engine.SetInputToDefaultAudioDevice();
            AddGrammars(Engine);
            Engine.RecognizeAsync(RecognizeMode.Multiple);
        }
        Grammar BuildGrammar()
        {
            var hearthDictionary = new List<string> {
                "face", "play online", "solo adventures", "concede game", "cancel", "blue button",
                "position", "click", "finish", "power", "champion", "face", "go back", "well played",
                "thank you", "sorry", "my collection", "oops", "threaten", "greetings", "good game",
                "escape", "cancel", "casual", "ranked", "quest log", "center mouse", "hide mouse", "naxxramas",
                "open pack", "flip next", "flip back"
            };
            foreach (string desc in new string[] { "friendly", "enemy", "card", "deck", "play", "choose", "select",
                                                   "filter" })
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
        void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
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

        void AddGrammars(SpeechRecognitionEngine recognizer)
        {
            Grammar dictationGrammar = BuildGrammar();
            Grammar mulliganGrammer = MakeRepeatedGrammar(new string[] { "mulligan" }, new string[] { "1", "2", "3", "4", "confirm" }, 99);
            Grammar moveGrammar = MakeMoveGrammar();
            recognizer.LoadGrammar(dictationGrammar);
            recognizer.LoadGrammar(mulliganGrammer);
            recognizer.LoadGrammar(moveGrammar);
        }

        Grammar MakeRepeatedGrammar(string[] firstWords, string[] choicesArr, int choicesMax = 99)
        {
            var gb = new GrammarBuilder(new Choices(firstWords));
            var choices = new Choices(choicesArr);
            gb.Append(new GrammarBuilder(choices, 1, choicesMax));
            return new Grammar(gb);
        }

        Grammar MakeMoveGrammar()
        {
            var directions = new string[] { "up", "right", "down", "left" };
            var numberList = Enumerable.Range(0, 9).Select(i => i.ToString()).ToArray();
            var numberChoices = new Choices(numberList);
            var min1Number = new GrammarBuilder(numberChoices, 1, 4);
            var numInFront = new GrammarBuilder();
            numInFront.Append(min1Number);
            numInFront.Append(new GrammarBuilder(PointGrammar(numberChoices), 0, 1));
            var gb = new GrammarBuilder(new Choices(directions));
            gb.Append(new Choices(new GrammarBuilder[] { numInFront, PointGrammar(numberChoices) }));
            return new Grammar(gb);
        }

        GrammarBuilder PointGrammar(Choices numberChoices)
        {
            var pointGb = new GrammarBuilder("point");
            pointGb.Append(new GrammarBuilder(numberChoices), 1, 4);
            return pointGb;
        }

        public void ListenIO()
        {
            System.Console.WriteLine("Listening for input. Press Enter to stop listening.");
            Console.ReadLine();
            Engine.RecognizeAsyncCancel();
            new MainMenu();
        }

    }
}
