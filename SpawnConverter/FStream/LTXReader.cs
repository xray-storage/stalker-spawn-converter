using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SpawnConverter.FStream
{
    public partial class LTXReader : FileReader
    {
        private readonly string FileDir = string.Empty;
        private readonly string FileName = string.Empty;

        private readonly Dictionary<string, string> Dic;
        private readonly StringBuilder buffer = new();
        private readonly DBFile dbfile;

        private static bool on_read_keys = false;

        public LTXReader(string fname)
        {
            Dic = new(0);

            FileDir = FilePath.GAME.CONFIGS;
            FileName = fname;

            dbfile = new(FilePath.DB.CONFIGS);

            Read(FileDir, FileName);
        }

        public string ReadString(string section, string key) => ReadValue(section, key);
        public List<string> GetKeys() => Dic.Keys.ToList();

        private void Read(string fdir, string fname)
        {
            Stream stream;
            string fullpath = Path.Combine(fdir, fname);

            if (!Directory.Exists(FileDir) || !File.Exists(fullpath))
            {
                var data = dbfile.SearchFile(fullpath);

                if (data is null)
                {
                    LogError(CODES.NOT_FILE);
                    return;
                }

                stream = dbfile.Unpack(data);
            }
            else
            {
                stream = File.OpenRead(Path.Combine(fdir, fname));
            }

            string section = string.Empty;
            using StreamReader reader = new(stream);
            
            void ResetReadKey()
            {
                on_read_keys = false;

                if (Dic.TryGetValue(section, out _))
                {
                    Dic[section] = Dic.ToString();
                }
                else
                {
                    Dic.Add(section, buffer.ToString());
                }

                buffer.Clear();
            }

            while (!reader.EndOfStream)
            {
                string line = Regex.Replace(reader.ReadLine(), REGULAR.CLEAR, "");

                if (on_read_keys)
                {
                    if (!Regex.IsMatch(line, REGULAR.SECTION))
                    {
                        if (Regex.IsMatch(line, REGULAR.KEY))
                        {
                            string key = Regex.Match(line, REGULAR.KEY).Groups["key"].Value;
                            string value = Regex.Match(line, REGULAR.KEY).Groups["value"].Value;

                            if (!string.IsNullOrEmpty(key))
                            {
                                buffer.AppendLine($"{key}={value}");
                            }
                        }

                        continue;
                    }

                    ResetReadKey();
                }

                if (Regex.IsMatch(line, REGULAR.INCLUDE))
                {
                    string result = Regex.Match(line, REGULAR.INCLUDE).Groups["pattern"].Value;

                    if (string.IsNullOrEmpty(result))
                    {
                        continue;
                    }

                    string newdir = Path.Combine(fdir, Path.GetDirectoryName(result));
                    string pattern = Path.GetFileName(result);

                    if (Directory.Exists(newdir) && File.Exists(Path.Combine(newdir, pattern)))
                    {
                        foreach (string file in Directory.GetFiles(newdir, pattern))
                        {
                            Read(newdir, Path.GetFileName(file));
                        }
                    }
                    else
                    {
                        var files = dbfile.SearchFilesByPattern(Path.Combine(newdir, pattern));

                        foreach(var data in files)
                        {
                            Read(newdir, Path.GetFileName(data.Name));
                        }
                    }
                }
                else if (Regex.IsMatch(line, REGULAR.SECTION))
                {
                    section = Regex.Match(line, REGULAR.SECTION).Groups["section"].Value.ToLower();
                    string parent = Regex.Match(line, REGULAR.SECTION).Groups["parent"].Value.ToLower();

                    buffer.AppendLine($"[{section}]:{parent}");
                    on_read_keys = true;
                }
            }

            if (on_read_keys)
            {
                ResetReadKey();
            }
        }

        private string GetParentValue(string buffer, string key, string def)
        {
            string result = def;

            if (Regex.IsMatch(buffer, REGULAR.PARENT, RegexOptions.Multiline))
            {
                IList<string> list = Regex.Match(buffer, REGULAR.PARENT, 
                    RegexOptions.Multiline).Groups["parent"].Value.Split(',').ToList();

                foreach (string parent in list)
                {
                    string temp = ReadValue(parent, key, def);
                    result = temp != def ? temp : result;
                }
            }

            return result;
        }

        private string ReadValue(string section, string key, string def = "")
        {
            string result = def;

            if (Dic.TryGetValue(section, out string buff))
            {
                foreach (Match m in Regex.Matches(buff, REGULAR.KEY, RegexOptions.Multiline))
                {
                    if (key.CompareTo(m.Groups["key"].Value) == 0)
                    {
                        return m.Groups["value"].Value != string.Empty ? m.Groups["value"].Value : " ";
                    }
                }

                result = GetParentValue(buff, key, def);
            }

            return result;
        }
    }
}
