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
            return RatioToCoords(.8224, .9193);
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
            return RatioToCoords(.6798, .1628);
        }

        public int[] RankedButton()
        {
            return RatioToCoords(.7749, .1432);
        }

        public int[] QuestLogButton()
        {
            return RatioToCoords(.2778, .8685);
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
            double xpos = .1827 + (cardNum % 4) * .1279;
            double ypos = cardNum < 4 ? .3255 : .6510;
            return RatioToCoords(xpos, ypos);
        }

        public int[] ManaButton(int cardNum)
        {
            double xpos = .2135 + cardNum * .0263;
            return RatioToCoords(xpos, .9115);
        }

        public int[] FlipNext()
        {
            return RatioToCoords(.6579, .5013);
        }

        public int[] FlipBack()
        {
            return RatioToCoords(.1389, .5013);
        }

        public int[] CardInDeckList(int cardNum)
        {
            cardNum--;
            double ypos = .1237 + cardNum * .0378;
            return RatioToCoords(.7968, ypos);
        }

        public int[] CardInDeckListBottom(int cardNum)
        {
            //FIXME: this was a dumb hack that only worked with static positions
            //cardNum -= 22;
            double ypos = .5534 + cardNum * .0378;
            return RatioToCoords(.7968, ypos);
        }


        public int[] MyCollectionButton()
        {
            return RatioToCoords(.3771, .9074);
        }


        public int[] GameCancelButton()
        {
            return RatioToCoords(.5117, .9074);
        }

        public int[] GoldArenaAdmission()
        {
            return RatioToCoords(.6031, .6510);
        }

        public int[] CardListDragStart()
        {
            return RatioToCoords(.8575, .0482);
        }

        public int[] CardListDragEnd()
        {
            return RatioToCoords(.8575, .8555);
        }

        public int[] Emote(string words)
        {
            var emoteMap = new Dictionary<string, double[]>
            {
                ["thank you"] = new double[] { .4203, .6185 },
                ["sorry"] = new double[] { .5848, .6185 },
                ["well played"] = new double[] { .4203, .7044 },
                ["good game"] = new double[] { .4203, .7044 },
                ["oops"] = new double[] { .5848, .7044 },
                ["greetings"] = new double[] { .4203, .7956 },
                ["threaten"] = new double[] { .5848, .7956 },

            };
            double[] ratios = emoteMap[words];
            return RatioToCoords(ratios[0], ratios[1]);
        }

        public int[] Deck(int deckNum)
        {
            var deckMap = new Dictionary<int, double[]>
            {
                [1] = new double[] { .2558, .2799 },
                [2] = new double[] { .3655, .2799 },
                [3] = new double[] { .4751, .2799 },
                [4] = new double[] { .2558, .4948 },
                [5] = new double[] { .3655, .4948 },
                [6] = new double[] { .4751, .4948 },
                [7] = new double[] { .2558, .7031 },
                [8] = new double[] { .3655, .7031 },
                [9] = new double[] { .4751, .7031 },

            };
            double[] ratios = deckMap[deckNum];
            return RatioToCoords(ratios[0], ratios[1]);
        }

        public int[] SelectBuildDeck(int deckNum)
        {
            deckNum--;
            return RatioToCoords(.7675, .1302 + .0872 * deckNum);
        }

        public int[] CenterPosition()
        {
            return RatioToCoords(.5007, .5013);
        }
        public int[] HidePosition()
        {
            return RatioToCoords(0, .5);
        }

        public int[] ArenaOpenButton()
        {
            return RatioToCoords(.4934, .4492);
        }

        public int[] ArenaPlayButton()
        {
            return RatioToCoords(.5702, .7813);
        }

        public int[] TavernBrawlButton()
        {
            return RatioToCoords(.5044, .5143);
        }

        public int[] BuyPackButton()
        {
            return RatioToCoords(.7361, .7174);
        }

        public int[] CardBookTabs(int tabNum)
        {
            tabNum--;
            return RatioToCoords(.1520 + .0365 * tabNum, .0365);
        }

        public int[] ShowOnlyGoldenCards()
        {
            return RatioToCoords(.7376, .5534);
        }

        public int[] IncludeUncraftableCards()
        {
            return RatioToCoords(.7383, .6263);
        }

        public int[] CraftingButton()
        {
            return RatioToCoords(.6404, .9232);
        }

        public int[] DisenchantCard()
        {
            return RatioToCoords(.4174, .8398);
        }

        public int[] CreateCard()
        {
            return RatioToCoords(.5307, .8398);
        }

        public int[] CancelDisenchant()
        {
            return RatioToCoords(.5577, .5964);
        }

        public int[] ConfirmDisenchant()
        {
            return RatioToCoords(.4408, .5964);
        }

        public int[] ShopButton()
        {
            return RatioToCoords(.1806, .8763);
        }

        public int[] OpenPacksButton()
        {
            return RatioToCoords(.4000, .8359);
        }

        public int[][] CardPacks()
        {
            var ratios = new double[][] {
                new double[] { .7756, .3516 }, 
                new double[] { .6937, .7708 }, 
                new double[] { .4832, .7513 },
                new double[] { .4152, .3438 },
                new double[] { .5906, .2109 },
                new double[] { .5855, .5156 },
            };
            return ratios.Select(coords => new int[] { (int)coords[0], (int)coords[1] }).ToArray();
        }
    }
}
