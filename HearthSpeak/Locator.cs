using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthSpeak
{
    class Locator
    {

        private LogFileParser parser;
        private double CreatureSep;
        private double screenWidth;
        private double screenHeight;
        public Locator(LogFileParser parser)
        {
            this.parser = parser;
            this.screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            CreatureSep = .0731;
        }

        private int[] RatioToCoords(double rx, double ry) {
            int x = (int)(rx * this.screenWidth);
            int y = (int)(ry * this.screenHeight);
            return new int[] { x, y };
        }

        public int[] EndTurnButton()
        {
            return RatioToCoords( .8129, .4518);
        }

        public int[] PlayButton()
        {
            return RatioToCoords(.497, .306);
        }

        public int[] SoloAdventuresButton()
        {
            return RatioToCoords(.4825, .3776);
        }

        public int[] BackButton()
        {
            return new int[] { 1125, 706 };
        }

        public int[] CardInHand(int cardNum)
        {
            Console.WriteLine(cardNum);
            double xpos = -1;
            double ypos = .93;
            cardNum--;
            switch (parser.FriendlyHandCount)
            {
                case 1:
                    xpos = .475;
                    break;
                case 2:
                    xpos = .475 + cardNum * .073;
                    break;
                case 3:
                    xpos = .387 + cardNum * .088;
                    break;
                case 4:
                    xpos = .3304 + cardNum * .0768;
                    break;
                case 5:
                    xpos = .350 + cardNum * .06;
                    break;
                case 6:
                    xpos = .353 + cardNum * .045;
                    break;
                case 7:
                    xpos = .314 + cardNum * .05;
                    break;
                case 8:
                    xpos = .311 + cardNum * .042;
                    break;
                case 9:
                    xpos = .307 + cardNum * .037;
                    break;
                case 10:
                    xpos = .307 + cardNum * .032;
                    ypos = .9635;
                    break;
            }
            return RatioToCoords(xpos, ypos);
        }

        public int[] FriendlyBoard(int pos)
        {
            double xpos = CreateXPos(pos, parser.FriendlyPlayCount);
            double ypos = .546875;
            return RatioToCoords(xpos, ypos);
        }

        public int[] OpposingBoard(int pos)
        {
            double ypos = .3255;
            double xpos = CreateXPos(pos, parser.OpposingPlayCount);
            return RatioToCoords(xpos, ypos);
        }

        private double CreateXPos(int pos, int cardCount)
        {
            pos--;
            double firstLeft = .5066 - .0358 * cardCount;
            double xpos = firstLeft + pos * CreatureSep;
            return xpos;
        }

        public int[] FriendlyPortrait()
        {
            return RatioToCoords(.5044, .7017);
        }

        public int[] OpposingPortrait()
        {
            return RatioToCoords(.5044, .1953);
        }

        public int[] HeroPower()
        {
            return RatioToCoords(.5958, .7552);
        }

        public int[] Mulligan(string word)
        {
            double ypos = .4557;
            var mulliganPositions = new Dictionary<string, double[]>
            {
                ["1"] = new double[] { .3289, ypos },
                ["2"] = new double[] { .4605, ypos },
                ["3"] = new double[] { .6213, ypos },
                ["4"] = new double[] { .7054, ypos },
                ["confirm"] = new double[] { .4971, .78125 }
            }[word];
            return RatioToCoords(mulliganPositions[0], mulliganPositions[1]);
        }

        public int[] CasualButton()
        {
            return new int[] { 930, 125 };
        }

        public int[] RankedButton()
        {
            return new int[] { 1060, 110 };
        }

        public int[] QuestLogButton()
        {
            return new int[] { 380, 667 };
        }

        public int[] ConcedeButton()
        {
            return RatioToCoords(.5, .3659);
        }

        public int[] BlueButton()
        {
            return RatioToCoords(.7028, .8216);
        }

        public int[] ConstructCard(int cardNum)
        {
            cardNum--;
            int xpos = 250 + (cardNum % 4) * 175;
            int ypos = cardNum < 4 ? 250 : 500;
            return new int[] { xpos, ypos };
        }

        public int[] ManaButton(int cardNum)
        {
            int xpos = 292 + cardNum * 36;
            return new int[] { xpos, 700 };
        }

        public int[] FlipNext()
        {
            return new int[] { 900, 385 };
        }

        public int[] FlipBack()
        {
            return new int[] { 190, 385 };
        }

        public int[] CardInDeckList(int cardNum)
        {
            cardNum--;
            int ypos = 95 + cardNum * 29;
            return new int[] { 1090, ypos };
        }

        public int[] CardInDeckListBottom(int cardNum)
        {
            cardNum -= 22;
            int ypos = 425 + cardNum * 29;
            return new int[] { 1090, ypos };
        }


        public int[] MyCollectionButton()
        {
            return RatioToCoords(.3771, .9074);
        }


        public int[] GameCancelButton()
        {
            return new int[] { 700, 645 };
        }

        public int[] GoldArenaAdmission()
        {
            return new int[] { 825, 500 };
        }

        public int[] CardListDragStart()
        {
            return new int[] { 1173, 37 };
        }

        public int[] CardListDragEnd()
        {
            return new int[] { 1173, 657 };
        }

        public int[] Emote(string words)
        {
            var emoteMap = new Dictionary<string, int[]>
            {
                ["thank you"] = new int[] { 575, 475 },
                ["sorry"] = new int[] { 800, 475 },
                ["well played"] = new int[] { 575, 541 },
                ["good game"] = new int[] { 575, 541 },
                ["oops"] = new int[] { 800, 541 },
                ["greetings"] = new int[] { 575, 611 },
                ["threaten"] = new int[] { 800, 611 },

            };
            return emoteMap[words];
        }

        public int[] Deck(int deckNum)
        {
            var deckMap = new Dictionary<int, int[]>
            {
                [1] = new int[] { 350, 215 },
                [2] = new int[] { 500, 215 },
                [3] = new int[] { 650, 215 },
                [4] = new int[] { 350, 380 },
                [5] = new int[] { 500, 380 },
                [6] = new int[] { 650, 380 },
                [7] = new int[] { 350, 540 },
                [8] = new int[] { 500, 540 },
                [9] = new int[] { 650, 540 },

            };
            return deckMap[deckNum];
        }

        public int[] SelectBuildDeck(int deckNum)
        {
            deckNum--;
            return new int[] { 1050, 100 + 67 * deckNum };
        }

        public int[] CenterPosition()
        {
            return new int[] { 685, 358 };
        }
        public int[] HidePosition()
        {
            return new int[] { 0, 767 };
        }

        public int[] ArenaOpenButton()
        {
            return new int[] { 675, 345 };
        }

        public int[] ArenaPlayButton()
        {
            return new int[] { 780, 600 };
        }

        public int[] TavernBrawlButton()
        {
            return new int[] { 690, 395 };
        }

        public int[] BuyPackButton()
        {
            return new int[] { 1007, 551 };
        }

        public int[] CardBookTabs(int tabNum)
        {
            tabNum--;
            return new int[] { 208 + 50 * tabNum, 28 };
        }

        public int[] ShowOnlyGoldenCards()
        {
            return new int[] { 1009, 425 };
        }

        public int[] IncludeUncraftableCards()
        {
            return new int[] { 1010, 481 };
        }

        public int[] CraftingButton()
        {
            return new int[] { 876, 709 };
        }

        public int[] DisenchantCard()
        {
            return new int[] { 571, 645 };
        }

        public int[] CreateCard()
        {
            return new int[] { 726, 645 };
        }

        public int[] CancelDisenchant()
        {
            return new int[] { 763, 458 };
        }

        public int[] ConfirmDisenchant()
        {
            return new int[] { 603, 458 };
        }

        public int[] ShopButton()
        {
            return new int[] { 247, 673 };
        }

        public int[] OpenPacksButton()
        {
            return new int[] { 547, 642 };
        }

        public int[][] CardPacks()
        {
            return new int[][] {
                new int[] { 1061, 270 }, 
                new int[] { 949, 592 }, 
                new int[] { 661, 577 },
                new int[] { 568, 264 },
                new int[] { 808, 162 },
                new int[] { 801, 396 },
            };
        }
    }
}
