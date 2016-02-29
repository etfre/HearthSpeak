using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniParser;

namespace HearthSpeak
{
    public static class Config
    {
        private readonly static string LogConfigPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                                                       + @"\Blizzard\Hearthstone\log.config";

        public static void InitializeLogFile()
        {

        }

    }
}
