using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthSpeak
{
    class GameManager
    {
        public Dictionary<Regex, Action<List<string>>> ActionMap;
        public Dictionary<string, int> CardinalMap;
        private string Numbers;
        private LogFileParser parser;
        private Locator locator;
        private int ClickDelay;

        public GameManager()
        {
            parser = new LogFileParser();
            parser.StartWatching();
            ClickDelay = 600;
            locator = new Locator(parser);
            ActionMap = new Dictionary<Regex, Action<List<string>>>
            {
                [new Regex(@"\Apoint")] = GetPoint,
                [new Regex(@"\Aclick")] = Click,
                [new Regex(@"\Afinish")] = EndTurn,
                [new Regex(@"\Aplay online")] = PlayGame,
                [new Regex(@"\Asolo adventures")] = SoloAdventures,
                [new Regex(@"\Ago back")] = GoBack,
                [new Regex(@"\Acard (10|[1-9])")] = HandCard,
                [new Regex(@"\Aplay (10|[1-9])")] = PlayCard,
                [new Regex(@"\Afriendly [1-9]")] = FriendlyBoard,
                [new Regex(@"\Aenemy [1-9]")] = OpposingBoard,
                [new Regex(@"\Apower")] = HeroPower,
                [new Regex(@"\Achampion")] = FriendlyPortrait,
                [new Regex(@"\Aface")] = OpposingPortrait,
                [new Regex(@"\Amulligan( [1-4]| confirm)+")] = Mulligan,
                [new Regex(@"\Aselect [1-4]")] = SelectCard,
                [new Regex(@"\escape")] = Escape,
                [new Regex(@"\Aconcede game")] = ConcedeGame,
                [new Regex(@"\Ablue button")] = BlueButton,
                [new Regex(@"\Acancel")] = GameCancel,
                [new Regex(@"\Amy collection")] = MyCollection,
                [new Regex(@"\Adeck [1-9]")] = Deck,
                [new Regex(@"\A(thank you)|(sorry)|(well played)|(good game)|(oops)|(threaten)|(greetings)")] = Emote,
            };
            CardinalMap = new Dictionary<string, int>
            {
                ["first"] = 1,
                ["second"] = 2,
                ["third"] = 3,
                ["fourth"] = 4,
                ["fifth"] = 5,
                ["sixth"] = 6,
                ["seventh"] = 7,
                ["eighth"] = 8,
                ["ninth"] = 9,
                ["tenth"] = 10,
            };
        }

        public void PlayGame(List<string> words)
        {
            InputControl.MouseClick(locator.PlayButton());
        }

        public void SoloAdventures(List<string> words)
        {
            InputControl.MouseClick(locator.SoloAdventuresButton());
        }

        public void GoBack(List<string> words)
        {
            InputControl.MouseClick(locator.BackButton());
        }

        public void EndTurn(List<string> words)
        {
            InputControl.MouseClick(locator.EndTurnButton());
        }

        public void GetPoint(List<string> words)
        {
            var pos = InputControl.CursorPosition();
            System.Console.WriteLine(pos[0].ToString() + ", " + pos[1].ToString());
        }

        public void Click(List<string> words)
        {
            InputControl.MouseClick();
        }

        public void HandCard(List<string> words)
        {
            InputControl.MouseClick(-1, -1, "right");
            Thread.Sleep(ClickDelay / 2);
            if (parser.SetAsideCount > 0)
            {
                InputControl.MouseClick(locator.FaceCard(Int32.Parse(words[1])));
            }
            else
            {
                InputControl.MouseClick(locator.CardInHand(Int32.Parse(words[1])), "left", ClickDelay / 2);
            }
        }

        public void FriendlyBoard(List<string> words)
        {
            BoardAction(words, locator.FriendlyBoard);
        }

        public void OpposingBoard(List<string> words)
        {
            BoardAction(words, locator.OpposingBoard);
        }

        private void BoardAction(List<string> words, Func<int, int[]> func)
        {
            int pos = Int32.Parse(words[1]);
            int[] destCoords = func(pos);
            InputControl.SetCursorPosition(destCoords[0], 570);
            Thread.Sleep(ClickDelay);
            InputControl.MouseClick(destCoords);
        }

        public void HeroPower(List<string> words)
        {
            Thread.Sleep(ClickDelay);
            InputControl.MouseClick(locator.HeroPower());
        }

        public void FriendlyPortrait(List<string> words)
        {
            int[] destCoords = locator.FriendlyPortrait();
            Thread.Sleep(ClickDelay);
            InputControl.SetCursorPosition(destCoords[0], 570);
            Thread.Sleep(ClickDelay);
            InputControl.MouseClick(destCoords);
        }

        public void OpposingPortrait(List<string> words)
        {
            int[] destCoords = locator.OpposingPortrait();
            Thread.Sleep(ClickDelay);
            InputControl.SetCursorPosition(destCoords[0], 570);
            Thread.Sleep(ClickDelay);
            InputControl.MouseClick(destCoords);
        }

        public void Mulligan(List<string> words)
        {
            words.RemoveAt(0);
            foreach (string word in words)
            {
                Thread.Sleep(ClickDelay);
                InputControl.MouseClick(locator.Mulligan(word));
            }
        }

        public void Escape(List<string> words)
        {
            InputControl.TypeKeys("{ESC}");
        }

        public void ConcedeGame(List<string> words)
        {
            InputControl.TypeKeys("{ESC}");
            Thread.Sleep(ClickDelay);
            InputControl.MouseClick(locator.ConcedeButton());
        }

        public void BlueButton(List<string> words)
        {
            Thread.Sleep(ClickDelay);
            InputControl.MouseClick(locator.BlueButton());
        }

        public void MyCollection(List<string> words)
        {
            Thread.Sleep(ClickDelay);
            InputControl.MouseClick(locator.MyCollectionButton());
        }


        public void GameCancel(List<string> words)
        {
            InputControl.MouseClick(locator.GameCancelButton());
        }

        public void SelectCard(List<string> words)
        {
            Thread.Sleep(ClickDelay);
            InputControl.MouseClick(locator.FaceCard(Int32.Parse(words[1])));
        }

        public void PlayCard(List<string> words)
        {
            HandCard(words);
            Thread.Sleep(ClickDelay);
            BoardAction(new string[] { "friendly", "1" }.ToList(), locator.FriendlyBoard);
        }

        public void Deck(List<string> words)
        {
            InputControl.MouseClick(locator.Deck(Int32.Parse(words[1])));
        }

        public void Emote(List<string> words)
        {
            string joinedWords = String.Join(" ", words);
            InputControl.MouseClick(locator.FriendlyPortrait(), "right");
            Thread.Sleep(250);
            InputControl.MouseClick(locator.Emote(joinedWords));

        }

    }
}
