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
        private LogFileParser parser;
        private Locator locator;
        private int ClickDelay;

        public GameManager()
        {
            parser = new LogFileParser();
            parser.StartWatching();
            ClickDelay = 100;
            locator = new Locator(parser);
            ActionMap = new Dictionary<Regex, Action<List<string>>>
            {
                [new Regex(@"\Aposition")] = GetPosition,
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
                [new Regex(@"\Acancel")] = CancelGame,
                [new Regex(@"\Acasual")] = SelectCasual,
                [new Regex(@"\Aranked")] = SelectRanked,
                [new Regex(@"\Aconcede game")] = ConcedeGame,
                [new Regex(@"\Aquest log")] = QuestLog,
                [new Regex(@"\Ablue button")] = BlueButton,
                [new Regex(@"\Acenter mouse")] = CenterMouse,
                [new Regex(@"\Ahide mouse")] = HideMouse,
                [new Regex(@"\Acancel")] = GameCancel,
                [new Regex(@"\Amy collection")] = MyCollection,
                [new Regex(@"\Achoose [1-9]")] = ChooseDeck,
                [new Regex(@"\Adeck [1-9]")] = SelectBuildDeck,
                [new Regex(@"\Aopen pack")] = OpenPack,
                [new Regex(@"\A(up|right|down|left).+")] = MoveDirection,
                [new Regex(@"\A(thank you)|(sorry)|(well played)|(good game)|(oops)|(threaten)|(greetings)")] = Emote,
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

        public void GetPosition(List<string> words)
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
            InputControl.MouseClick(locator.CardInHand(Int32.Parse(words[1])), "left", ClickDelay / 2);
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
            InputControl.MouseClick(destCoords, "left", ClickDelay);
        }

        public void HeroPower(List<string> words)
        {
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

        public void CancelGame(List<string> words)
        {
            InputControl.MouseClick(locator.CancelButton());
        }

        public void SelectCasual(List<string> words)
        {
            InputControl.MouseClick(locator.CasualButton());
        }

        public void SelectRanked(List<string> words)
        {
            InputControl.MouseClick(locator.RankedButton());
        }

        public void QuestLog(List<string> words)
        {
            InputControl.MouseClick(locator.QuestLogButton());
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

        public void CenterMouse(List<string> words)
        {
            InputControl.SetCursorPosition(locator.CenterPosition());
        }

        public void HideMouse(List<string> words)
        {
            InputControl.SetCursorPosition(locator.HidePosition());
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

        public void ChooseDeck(List<string> words)
        {
            InputControl.MouseClick(locator.Deck(Int32.Parse(words[1])));
        }

        public void SelectBuildDeck(List<string> words)
        {
            InputControl.MouseClick(locator.SelectBuildDeck(Int32.Parse(words[1])));
        }

        public void OpenPack(List<string> words)
        {
            InputControl.TypeKeys(" ");
            Thread.Sleep(5000);
            List<int[]> points = locator.CardPacks().ToList();
            int[] doneButton = points.Last();
            points.RemoveAt(points.Count - 1);
            foreach (int[] cardPosition in points)
            {
                InputControl.MouseClick(cardPosition, "left", ClickDelay / 2);
            }
            Thread.Sleep(2500);
            InputControl.MouseClick(doneButton, "left", ClickDelay / 2);
        }


        public void MoveDirection(List<string> words)
        {
            string moveAmount = ""; double moveAmountDouble;
            for (var i = 1; i < words.Count; i++)
            {
                string word = words[i] == "point" ? "." : words[i];
                moveAmount += word;
            }
            bool result = Double.TryParse(moveAmount, out moveAmountDouble);
            if (!result) return;
            int moveAmountInt = (int)Math.Ceiling(moveAmountDouble * 200);
            int[] currentPos = InputControl.CursorPosition();
            switch (words[0])
            {
                case "up":
                    InputControl.SetCursorPosition(currentPos[0], currentPos[1] - moveAmountInt);
                    break;
                case "right":
                    InputControl.SetCursorPosition(currentPos[0] + moveAmountInt, currentPos[1]);
                    break;
                case "down":
                    InputControl.SetCursorPosition(currentPos[0], currentPos[1] + moveAmountInt);
                    break;
                case "left":
                    InputControl.SetCursorPosition(currentPos[0] - moveAmountInt, currentPos[1]);
                    break;

            }
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
