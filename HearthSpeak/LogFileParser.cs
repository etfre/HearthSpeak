using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;


namespace HearthSpeak
{
    class LogFileParser
    {
        public Thread PollThread;
        private string LogDirectory;
        private Dictionary<string, Regex> LinePatterns;
        public int FriendlyHandCount;
        public int FriendlyPlayCount;
        public int OpposingPlayCount;
        public int SetAsideCount;
        public int NewFriendlyHandCount;
        public int NewFriendlyPlayCount;
        public int NewOpposingPlayCount;
        public int NewSetAsideCount;

        public void StartWatching()
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            LogDirectory = Path.Combine(localAppData, @"Blizzard\Hearthstone\Logs");
            FriendlyHandCount = 0;
            FriendlyPlayCount = 0;
            OpposingPlayCount = 0;
            SetAsideCount = 0;
            NewFriendlyHandCount = 0;
            NewFriendlyPlayCount = 0;
            NewOpposingPlayCount = 0;
            NewSetAsideCount = 0;

            LinePatterns = new Dictionary<string, Regex>
            {
                ["new game"] = new Regex(@"taskListId=1 changeListId=1 taskStart=0"),
                ["decrement friendly hand"] = new Regex(@"from FRIENDLY HAND"),
                ["increment friendly hand"] = new Regex(@"to FRIENDLY HAND"),
                ["increment friendly play"] = new Regex(@"to FRIENDLY PLAY$"),
                // avoid end of game/weapon leaving play
                ["decrement friendly play"] = new Regex(@"from FRIENDLY PLAY ->"),
                ["increment opposing play"] = new Regex(@"to OPPOSING PLAY$"),
                ["decrement opposing play"] = new Regex(@"from OPPOSING PLAY ->"),
                ["increment setaside"] = new Regex(@"TRANSITIONING card .+ zone=SETASIDE"),
                ["reset setaside"] = new Regex(@"AddServerZoneChanges"),

            };
            PollThread = new Thread(new ThreadStart(ParseLogs));
            PollThread.Start();
        }

        public void ParseLogs()
        {
            while (true)
            {
                var filesToParse = FilesToParse();
                foreach (var path in filesToParse)
                {
                    ParseFile(path);
                }
                ArchiveNewestFile(filesToParse);
                FriendlyHandCount = NewFriendlyHandCount;
                FriendlyPlayCount = NewFriendlyPlayCount;
                OpposingPlayCount = NewOpposingPlayCount;
                SetAsideCount = NewSetAsideCount;
                Thread.Sleep(200);
            }
        }

        private void ParseFile(string fileName)
        {
            string line;
            using (var fs = new FileStream(fileName, FileMode.Open,
               FileAccess.Read, FileShare.ReadWrite))
            using (var streamReader = new StreamReader(fs, Encoding.Default))
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    ParseLine(line);
                }
            }
        }

        private void ArchiveNewestFile(List<string> filesToParse)
        {
            if (filesToParse.Count == 0) return;
            string newestPath = filesToParse[filesToParse.Count - 1];
            string tempPath = newestPath + ".tmp";
            try
            {
                using (var fs = new FileStream(newestPath, FileMode.Open,
                                               FileAccess.Write))
                    if (fs.Length <= 4000000 || newestPath.Contains("archive")) return;
                System.IO.File.Move(newestPath, tempPath);
            }
            catch
            {
                return;
            }
            System.IO.File.Move(tempPath, GetReplacePath(newestPath));
        }

        private string GetReplacePath(string path)
        {
            string fname = path;
            string extension = "";
            int idx = path.LastIndexOf('.');
            if (idx > -1)
            {
                fname = path.Substring(0, idx);
                extension = path.Substring(idx);
            }
            fname += "_archive";
            int archiveCount = 2;
            string replacePath = fname + extension;
            while (File.Exists(replacePath))
            {
                replacePath = fname + archiveCount++.ToString() + extension;
            }
            return replacePath;
        }

        private List<string> FilesToParse()
        {
            var filesToParse = new List<string>();
            var directoryInfo = new DirectoryInfo(LogDirectory);
            List<FileInfo> fileList = directoryInfo.GetFiles().OrderByDescending(p => p.LastWriteTime).ToList();
            foreach (var file in fileList)
            {
                filesToParse.Add(file.FullName);
                if (containsNewGame(file.FullName))
                {
                    filesToParse.Reverse();
                    return filesToParse;
                }
            }
            return new List<string>();
        }

        private bool containsNewGame(string path)
        {
            string line;
            using (var fs = new FileStream(path, FileMode.Open,
                           FileAccess.Read, FileShare.ReadWrite))
            using (var streamReader = new StreamReader(fs, Encoding.Default))
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    Match match = LinePatterns["new game"].Match(line);
                    if (match.Success) return true;
                }
            }
            return false;
        }

        private void ParseLine(string line)
        {
            foreach (var item in LinePatterns)
            {
                Match match = item.Value.Match(line);
                if (!match.Success) continue;
                switch (item.Key)
                {
                    case "new game":
                        NewFriendlyHandCount = 0;
                        NewFriendlyPlayCount = 0;
                        NewOpposingPlayCount = 0;
                        NewSetAsideCount = 0;
                        break;
                    case "increment friendly hand":
                        NewFriendlyHandCount++;
                        break;
                    case "decrement friendly hand":
                        NewFriendlyHandCount--;
                        break;
                    case "increment friendly play":
                        NewFriendlyPlayCount++;
                        break;
                    case "decrement friendly play":
                        NewFriendlyPlayCount--;
                        break;
                    case "increment opposing play":
                        NewOpposingPlayCount++;
                        break;
                    case "decrement opposing play":
                        NewOpposingPlayCount--;
                        break;
                    case "increment setaside":
                        NewSetAsideCount++;
                        break;
                    case "reset setaside":
                        NewSetAsideCount = 0;
                        break;
                }
            }
        }

    }
}
