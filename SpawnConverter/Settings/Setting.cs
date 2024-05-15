using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using SpawnConverter.Logs;

namespace SpawnConverter
{
    public class Settings : LogEvent
    {
        private const string config_name = "SpawnConverter.ltx";
        private static readonly string config_fpath = Path.Combine(Environment.CurrentDirectory, config_name);

        private static string sdk_path = string.Empty;
        private static string game_path = string.Empty;

        private static Dictionary<string, string> sections = new(0);

        internal static string SdkPath
        {
            get => sdk_path;
            private set => sdk_path = value;
        }
        internal static string GamePath
        {
            get => game_path;
            private set => game_path = value;
        }
        internal static Dictionary<string, string> Sections
        {
            get => sections;
            private set => sections = value;
        }


        internal static void Load()
        {
            if (!File.Exists(config_name))
            {
                CreateConfig();
            }

            ReadSettings();
        }

        private static void CreateConfig()
        {
            StringBuilder sb = new();

            sb.AppendLine("[general]");
            sb.AppendLine("game_path\t=\t");
            sb.AppendLine("sdk_path\t=\t");
            sb.AppendLine();
            sb.AppendLine("[sections]");
            sb.AppendLine(";animated_object\t=\tO_PHYS_S");

            File.WriteAllText(config_name, sb.ToString());
        }

        private static void ReadSettings()
        {
            SdkPath = Read_Value("general", "sdk_path");
            GamePath = Read_Value("general", "game_path");

            FillCustomSections();
        }

        private static void FillCustomSections()
        {
            byte[] buffer = new byte[2048];

            _ = GetPrivateProfileSection("sections", buffer, 2048, config_fpath);
            string[] tmp = Encoding.ASCII.GetString(buffer).Trim('\0').Replace("\0\0", ":").Replace("\0", "").Split(':');

            foreach(var str in tmp)
            {
                string[] pair = str.Split('=');

                if (pair.Length < 2)
                {
                    continue;
                }

                Sections.Add(pair[0], pair[1]);
            }
        }

        internal static string Read_Value(string section, string key)
        {
            StringBuilder sb = new(255);
            _ = GetPrivateProfileString(section, key, string.Empty, sb, 255, config_fpath);

            return sb.ToString();
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder sb, int size, string filepath);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileSection(string section, byte[] buffer, int size, string filepath);
    }
}
