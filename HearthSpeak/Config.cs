using System;
using System.IO;
using System.Text.RegularExpressions;

namespace HearthSpeak
{
    public static class Config
    {
        private readonly static string LogConfigDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                                                       + @"\Blizzard\Hearthstone";
        private readonly static string LogFilename = "log.config";

        public static string InitializeLogFile()
        {
            if (!Directory.Exists(LogConfigDirectory)) return "Unable to locate Hearthstone";
            string fullPath = Path.Combine(LogConfigDirectory, LogFilename);
            if (!File.Exists(fullPath)) File.Create(fullPath).Dispose();
            bool intheZone = false;
            bool logLevel1 = false;
            bool consolePrinting = false;
            var zoneRegex = new Regex(@"\[Zone\]");
            var logLevelRegex = new Regex(@"LogLevel *\= *1");
            var printingRegex = new Regex(@"ConsolePrinting *\= *true");
            var otherZoneRegex = new Regex(@"\[");


            using (StreamReader sr = File.OpenText(fullPath))
            {
                string line = String.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    var otherZoneMatch = otherZoneRegex.Match(line);
                    if (otherZoneMatch.Success && intheZone) break;
                    var zoneMatch = zoneRegex.Match(line);
                    if (zoneMatch.Success)
                    {
                        if (intheZone) break;
                        intheZone = true;
                        continue;
                    }
                    if (intheZone)
                    {
                        var levelMatch = logLevelRegex.Match(line);
                        if (levelMatch.Success)
                        {
                            logLevel1 = true;
                            continue;
                        }
                        var printingMatch = printingRegex.Match(line);
                        if (printingMatch.Success)
                        {
                            consolePrinting = true;
                        }
                    }
                }
            }
            if (intheZone && logLevel1 && consolePrinting) return "";
            using (var file = new System.IO.StreamWriter(fullPath, true))
            {
                file.WriteLine("[Zone]");
                file.WriteLine("LogLevel=1");
                file.WriteLine("ConsolePrinting=true");
            }
            Console.WriteLine("Updating log configuration. Please restart Hearthstone for this change to take effect.\n");
            return "";
        }

    }
}
