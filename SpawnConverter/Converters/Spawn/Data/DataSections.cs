using System.Collections.Generic;

using SpawnConverter.Logs;
using SpawnConverter.FStream;
using SpawnConverter.Converters.Spawns.Interfaces;
using SpawnConverter.Converters.Spawns.Clsid;

namespace SpawnConverter.Converters.Spawns
{
    public class DataSections : LogEvent 
    {
        LTXReader ltx;
        public List<ISection> Sections { get; set; } = new(0);

        public bool SetData(XrFileReader reader)
        {
            if (reader.FindChunk(CHUNK.ALIFE.COUNT) == 0)
            {
                LogError(CODES.NOT_CHUNK);
                return false;
            }

            uint count = reader.ReadUInt32();
            Log($"* Found all section: {count}");

            if (reader.FindChunk(CHUNK.ALIFE.SEC_LIST) == 0)
            {
                LogError(CODES.NOT_CHUNK);
                return false;
            }

            for (uint i = 0; i < count; i++)
            {
                uint size;

                if ((size = reader.FindChunk(i)) == 0)
                {
                    LogError(CODES.NOT_CHUNK);
                    return false;
                }

                long reset_positon = reader.Position;

                if (reader.FindChunk(CHUNK.ALIFE.SECTION) == 0)
                {
                    LogError(CODES.NOT_CHUNK);
                    return false;
                }

                reader.Position += 12;
                string section = reader.ReadStringZ();
                
                _ = reader.ReadStringZ();

                reader.Position += 48;
                ushort gvid = reader.ReadUInt16();

                reader.Position = reset_positon;

                if(!CheckLevel.IsOnLevel(gvid))
                {
                    reader.Position += size;
                    continue;
                }

                string clsid = GetClsID(section);

                if(string.IsNullOrEmpty(clsid))
                {
                    LogError(CODES.NOT_CLSID, string.Format(ErrorMessage.GetMessageByCode(CODES.NOT_CLSID), section));
                    return false;
                }

                var obj = CLSID.GetSectionByClsID(reader, clsid);

                if(obj is null)
                {
                    LogError(CODES.NOT_TEMPLATE, string.Format(ErrorMessage.GetMessageByCode(CODES.NOT_TEMPLATE), clsid));
                    return false;
                }

                Sections.Add(obj);
            }

            _ = reader.FindChunk(CHUNK.ALIFE.UNK_EMPTY);

            Log($"* Found sections for target: {Sections.Count}");

            return true;
        }
        private string GetClsID(string section)
        {
            if (ltx == null)
            {
                ltx = new(FilePath.FILE.SYSTEM);
            }

            string clsid = string.IsNullOrEmpty(section) ? section : ltx.ReadString(section, "class");

            if (string.IsNullOrEmpty(clsid))
            {
                var dic = Settings.Sections;

                if (dic.Count > 0 && dic.ContainsKey(section) && dic.TryGetValue(section, out string value))
                {
                    clsid = value;
                }
            }

            return clsid;
        }
    }
}
