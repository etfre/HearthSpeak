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
                [new Regex(@"\Aescape")] = Escape,
                [new Regex(@"\Acasual")] = SelectCasual,
                [new Regex(@"\Aranked")] = SelectRanked,
                [new Regex(@"\Aconcede game")] = ConcedeGame,
                [new Regex(@"\Aquest log")] = QuestLog,
                [new Regex(@"\Ablue button")] = BlueButton,
                [new Regex(@"\Acenter mouse")] = CenterMouse,
                [new Regex(@"\Ahide mouse")] = HideMouse,
                [new Regex(@"\Acancel search")] = GameCancel,
                [new Regex(@"\Amy collection")] = MyCollection,
                [new Regex(@"\Achoose [1-9]")] = ChooseDeck,
                [new Regex(@"\Adeck [1-9]")] = SelectBuildDeck,
                [new Regex(@"\Aselect [1-8]")] = ConstructCard,
                [new Regex(@"\Afilter [0-7]")] = FilterByMana,
                [new Regex(@"\Atoggle (10|[1-9])")] = CardBookTabs,
                [new Regex(@"\Ax marks the spot")] = OpenPack,
                [new Regex(@"\Aflip next")] = FlipNext,
                [new Regex(@"\Aflip back")] = FlipBack,
                [new Regex(@"\Athe arena")] = ArenaOpenButton,
                [new Regex(@"\Aarena play")] = ArenaPlayButton,
                [new Regex(@"\Aabuy pack")] = BuyPack,
                [new Regex(@"\Ashow only golden cards")] = ShowOnlyGoldenCards,
                [new Regex(@"\Ainclude uncraftable cards")] = IncludeUncraftableCards,
                [new Regex(@"\Atavern brawl")] = TavernBrawlButton,
                [new Regex(@"\Adisenchant card")] = DisenchantCard,
                [new Regex(@"\Acreate card")] = CreateCard,
                [new Regex(@"\Aconfirm disenchant")] = ConfirmDisenchant,
                [new Regex(@"\Acancel disenchant")] = CancelDisenchant,
                [new Regex(@"\Acrafting")] = CraftingButton,
                [new Regex(@"\Aopen packs")] = OpenPacksButton,
                [new Regex(@"\Ashop for cards")] = ShopButton,

                [new Regex(@"\Aremove \d( \d)?")] = RemoveCardInDeckList,
                [new Regex(@"\Ascroll up")] = ScrollCardListUp,
                [new Regex(@"\Ascroll down")] = ScrollCardListDown,

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

        public void ConstructCard(List<string> words)
        {
            InputControl.MouseClick(locator.ConstructCard(Int32.Parse(words[1])));
        }

        public void FilterByMana(List<string> words)
        {
            InputControl.MouseClick(locator.ManaButton(Int32.Parse(words[1])));
        }

        public void CardBookTabs(List<string> words)
        {
            InputControl.MouseClick(locator.CardBookTabs(Int32.Parse(words[1])));
        }

        public void FlipNext(List<string> words)
        {
            InputControl.MouseClick(locator.FlipNext());
        }

        public void FlipBack(List<string> words)
        {
            InputControl.MouseClick(locator.FlipBack());
        }

        public void ArenaOpenButton(List<string> words)
        {
            InputControl.MouseClick(locator.ArenaOpenButton());
        }

        public void ArenaPlayButton(List<string> words)
        {
            InputControl.MouseClick(locator.ArenaPlayButton());
        }

        public void TavernBrawlButton(List<string> words)
        {
            InputControl.MouseClick(locator.TavernBrawlButton());
        }


        public void BuyPack(List<string> words)
        {
            InputControl.MouseClick(locator.BuyPackButton());
        }


        public void ShowOnlyGoldenCards(List<string> words)
        {
            Console.WriteLine("fll");
            InputControl.MouseClick(locator.ShowOnlyGoldenCards());
        }


        public void IncludeUncraftableCards(List<string> words)
        {
            InputControl.MouseClick(locator.IncludeUncraftableCards());
        }


        public void DisenchantCard(List<string> words)
        {
            InputControl.MouseClick(locator.DisenchantCard());
        }


        public void CreateCard(List<string> words)
        {
            InputControl.MouseClick(locator.CreateCard());
        }


        public void ConfirmDisenchant(List<string> words)
        {
            InputControl.MouseClick(locator.ConfirmDisenchant());
        }


        public void CancelDisenchant(List<string> words)
        {
            InputControl.MouseClick(locator.CancelDisenchant());
        }

        public void CraftingButton(List<string> words)
        {
            InputControl.MouseClick(locator.CraftingButton());
        }

        public void OpenPacksButton(List<string> words)
        {
            InputControl.MouseClick(locator.OpenPacksButton());
        }

        public void ShopButton(List<string> words)
        {
            InputControl.MouseClick(locator.ShopButton());
        }

        public void RemoveCardInDeckList(List<string> words)
        {
            InputControl.MouseClick(locator.CardListDragStart());
            int num;
            if (words.Count == 2) num = Int32.Parse(words[1]);
            else num = Int32.Parse(words[1] + words[2]);
            Console.WriteLine(num);
            if (num < 22) InputControl.MouseClick(locator.CardInDeckList(num));
            else
            {
                InputControl.MouseClick(locator.CardListDragEnd());
                InputControl.MouseClick(locator.CardInDeckListBottom(num));
            }
        }

        public void ScrollCardListDown(List<string> words)
        {
            InputControl.MouseClick(locator.CardListDragEnd());
        }

        public void ScrollCardListUp(List<string> words)
        {
            InputControl.MouseClick(locator.CardListDragStart());
        }

    }
}
