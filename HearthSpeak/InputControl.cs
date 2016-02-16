using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;

namespace HearthSpeak
{
    class InputControl
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;
        public const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        public const int MOUSEEVENTF_RIGHTUP = 0x10;


        public static void MouseClick(int xpos=-1, int ypos=-1, string button="left", int delay = 0)
        {
            xpos = xpos == -1 ? Cursor.Position.X : xpos;
            ypos = ypos == -1 ? Cursor.Position.Y : ypos;
            Thread.Sleep(delay);
            Click(xpos, ypos, button);
        }

        public static void MouseClick(int[] coords, string button="left", int delay = 0)
        {
            Thread.Sleep(delay);
            Click(coords[0], coords[1], button);
        }

        public static void SetCursorPosition(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
        }

        public static int[] CursorPosition()
        {
            return new int[] { Cursor.Position.X, Cursor.Position.Y };
        }

        private static void Click(int xpos, int ypos, string button="left")
        {
            SetCursorPos(xpos, ypos);
            int downInt = button == "left" ? MOUSEEVENTF_LEFTDOWN : MOUSEEVENTF_RIGHTDOWN;
            int upInt = button == "left" ? MOUSEEVENTF_LEFTUP : MOUSEEVENTF_RIGHTUP;
            Thread.Sleep(200);
            mouse_event(downInt, xpos, ypos, 0, 0);
            mouse_event(upInt, xpos, ypos, 0, 0);
            Thread.Sleep(200);
        }

        public static void TypeKeys(string keys)
        {
            SendKeys.SendWait(keys);
        }
    }
}
