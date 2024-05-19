using System.IO;
using System.Text;
using System.Numerics;

using SpawnConverter.Types;
using SpawnConverter.FStream;
using SpawnConverter.Converters.Spawns;
using SpawnConverter.Converters.Spawns.Interfaces;
using SpawnConverter.Converters.Spawns.Classes;

using static SpawnConverter.FStream.FilePath;
using static SpawnConverter.Converters.StringFormat;

namespace SpawnConverter.Converters
{
    partial class SpawnManager
    {
        private enum MODE
        {
            WAYS    = 0,
            SECTION = 1,
        }

        private bool WriteAll(bool all = false)
        {
            bool result = true;

            if(!all)
            {
                CheckLevel.LevelName = data.LevelName;

                int ind = data.Levels.FindIndex(x => x.Name.CompareTo(data.LevelName) == 0);

                CheckLevel.GameGraphStart = data.Levels[ind].GameGraphs[0].GameVertexID;
                CheckLevel.GameGraphCount = (ushort)data.Levels[ind].GameGraphs.Count;

                return Write();
            }

            foreach(var lvl in data.Levels)
            {
                CheckLevel.LevelName = lvl.Name;
                CheckLevel.GameGraphStart = lvl.GameGraphs[0].GameVertexID;
                CheckLevel.GameGraphCount = (ushort)lvl.GameGraphs.Count;

                Log();
                Log($"Target: {lvl.Name}");

                if(!Write())
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        private bool Write()
        {
            string fdir = Path.Combine(SDK.LEVELS, CheckLevel.LevelName);

            if(!Directory.Exists(fdir))
            {
                Directory.CreateDirectory(fdir);
            }

            string fpath = CreateFielPath(fdir, FILE.SDK_SPAWN);

            StringBuilder sb = new();
            XrFileWriter writer = new(fpath);

            int count = 0;

            Log($"Create file: {fpath}");
            FillSectionsData(ref sb, ref count);
            
            writer.Write(GetHeaderFile(count, MODE.SECTION), true);
            writer.Write(sb.ToString(), true);
            writer.Close();
            sb.Clear();

            count = 0;
            fpath = CreateFielPath(fdir, FILE.SDK_WAY);

            writer = new XrFileWriter(fpath);

            Log($"Create file: {fpath}");
            FillWaysData(ref sb, ref count);
            
            writer.Write(GetHeaderFile(count, MODE.WAYS), true);
            writer.Write(sb.ToString(), true);
            writer.Close();
            sb.Clear();

            return true;
        }

        private string GetHeaderFile(int count = 0, MODE mode = 0)
        {
            StringBuilder sb = new();

            sb.AppendLine("[guid]");
            sb.AppendLine(Pairs("guid_g0", "6148914691236517205"));
            sb.AppendLine(Pairs("guid_g1", "6148914691236517205"));
            sb.AppendLine();
            sb.AppendLine("[main]");

            if (mode == MODE.SECTION)
            {
                sb.AppendLine(Pairs("flags", "0"));
            }

            sb.AppendLine(Pairs("objects_count", count.ToString()));
            sb.AppendLine(Pairs("version", "0"));
            sb.AppendLine();
            sb.AppendLine("[modif]");
            sb.AppendLine(Pairs("name", ""));
            sb.AppendLine(Pairs("time", "0"));

            return sb.ToString();
        }
        private void FillSectionsData(ref StringBuilder sb, ref int count)
        {
            FillGameGraph(ref sb, ref count);

            var sections = data.Sections.Sections;

            if (sections.Count > 0)
            {
                foreach (var sec in sections)
                {
                    if (sec.Section_Name.CompareTo("breakable_object") == 0 ||
                        sec.Section_Name.CompareTo("climable_object") == 0)
                    {
                        continue;
                    }

                    if(!CheckLevel.IsOnLevel(sec.Game_Vertex_ID))
                    {
                        continue;
                    }

                    FillSection(sec, ref sb, count);

                    count++;
                }
            }
        }
        private void FillGameGraph(ref StringBuilder sb, ref int count)
        {
            int ind = data.Levels.FindIndex(x => x.Name == CheckLevel.LevelName);
            var lvls = data.Levels;
            var graphs = lvls[ind].GameGraphs;

            if (graphs.Count == 0)
            {
                return;
            }

            foreach (var gg in graphs)
            {
                if(!CheckLevel.IsOnLevel(gg.GameVertexID))
                {
                    continue;
                }

                sb.AppendLine();
                sb.AppendLine($"[object_{count}]");
                sb.AppendLine(Pairs("clsid", "6"));
                sb.AppendLine(Pairs("co_flags", "2"));
                sb.AppendLine(Pairs("name", $"graph_point_{count:D4}"));
                sb.AppendLine(Pairs("position", Vector(gg.Local_Point)));
                sb.AppendLine(Pairs("rotation", "0.000000, 0.000000, 0.000000"));
                sb.AppendLine(Pairs("scale", "1.000000, 1.000000, 1.000000"));
                sb.AppendLine(Pairs("type", "2"));
                sb.AppendLine(Pairs("version", "23"));

                sb.AppendLine();
                sb.AppendLine($"[object_{count}_spawndata]");
                sb.AppendLine(Pairs("000001", "1"));
                sb.AppendLine(Pairs("000002", "\"graph_point\""));
                sb.AppendLine(Pairs("000003", $"\"graph_point_{count:D4}\""));
                sb.AppendLine(Pairs("000004", "0"));
                sb.AppendLine(Pairs("000005", "254"));
                sb.AppendLine(Pairs("000006", Vector(gg.Local_Point)));
                sb.AppendLine(Pairs("000007", "0.000000, 0.000000, 0.000000"));
                sb.AppendLine(Pairs("000008", "0"));
                sb.AppendLine(Pairs("000009", "65535"));
                sb.AppendLine(Pairs("000010", "65535"));
                sb.AppendLine(Pairs("000011", "65535"));
                sb.AppendLine(Pairs("000012", "33"));
                sb.AppendLine(Pairs("000013", "128"));
                sb.AppendLine(Pairs("000014", "65535"));
                sb.AppendLine(Pairs("000015", "12"));
                sb.AppendLine(Pairs("000016", "0"));
                sb.AppendLine(Pairs("000017", "0"));
                sb.AppendLine(Pairs("000018", "0"));

                // graph data
                bool found_link = false;

                if (gg.Edges.Length > 0)
                {
                    foreach (var eg in gg.Edges)
                    {
                        if (!CheckLevel.IsOnLevel(eg.Game_Vertex_ID))
                        {
                            ind = lvls.FindIndex(x =>
                                x.GameGraphs[0].GameVertexID <= eg.Game_Vertex_ID &&
                                (ushort)x.GameGraphs.Count + x.GameGraphs[0].GameVertexID > eg.Game_Vertex_ID);

                            sb.AppendLine(Pairs("000019", $"\"graph_point_{eg.Game_Vertex_ID - lvls[ind].GameGraphs[0].GameVertexID:D4}\""));
                            sb.AppendLine(Pairs("000020", $"\"{lvls[ind].Name}\""));

                            found_link = true;

                            break;
                        }
                    }
                }

                if (!found_link)
                {
                    sb.AppendLine(Pairs("000019", "\"\""));
                    sb.AppendLine(Pairs("000020", "\"\""));
                }

                sb.AppendLine(Pairs("000021", gg.VertexType.A.ToString()));
                sb.AppendLine(Pairs("000022", gg.VertexType.B.ToString()));
                sb.AppendLine(Pairs("000023", gg.VertexType.C.ToString()));
                sb.AppendLine(Pairs("000024", gg.VertexType.D.ToString()));
                sb.AppendLine(Pairs("fl", "0"));
                sb.AppendLine(Pairs("name", "graph_point"));

                count++;
            }
        }
        private void FillSection(ISection sec, ref StringBuilder sb, int count)
        {
            sb.AppendLine();

            StringBuilder attach = new();
            StringBuilder basic = new();

            int shapes = 0;

            if (sec.GetType() == typeof(Actor))
            {
                Actor obj = sec as Actor;

                basic.AppendLine(Pairs("000027", $"\"{obj.Visual}\""));
                basic.AppendLine(Pairs("000028", obj.Visual_Flags.ToString()));
                basic.AppendLine(Pairs("000029", "0"));
                basic.AppendLine(Pairs("000030", "0"));
                basic.AppendLine(Pairs("000031", "0"));
                basic.AppendLine(Pairs("000032", Single(obj.Health)));
                basic.AppendLine(Pairs("000033", "0"));
                basic.AppendLine(Pairs("000034", "0"));
                basic.AppendLine(Pairs("000035", "65535"));
                basic.AppendLine(Pairs("000036", "0"));
                basic.AppendLine(Pairs("000037", obj.Money.ToString()));
                basic.AppendLine(Pairs("000038", $"\"{obj.Specific_Character}\""));
                basic.AppendLine(Pairs("000039", ((int)obj.Trader_Flags).ToString()));
                basic.AppendLine(Pairs("000040", $"\"{obj.Character_Profile}\""));
                basic.AppendLine(Pairs("000041", "-1"));
                basic.AppendLine(Pairs("000042", "-2147483647"));
                basic.AppendLine(Pairs("000043", "-2147483647"));
                basic.AppendLine(Pairs("000044", $"\"{obj.Character_Name}\""));
                basic.AppendLine(Pairs("000045", obj.Deadbody_Can_Take.ToString()));
                basic.AppendLine(Pairs("000046", obj.Deadbody_Closed.ToString()));
                basic.AppendLine(Pairs("000047", $"\"{obj.Startup_Animation}\""));
                basic.AppendLine(Pairs("000048", obj.Skeleton_Flags.ToString()));
                basic.AppendLine(Pairs("000049", "65535"));
                basic.AppendLine(Pairs("000050", "65535"));
                basic.AppendLine(Pairs("000051", obj.Save_Marker_Start.ToString()));
                basic.AppendLine(Pairs("000052", obj.Start_Position_Filled.ToString()));
            }
            else if (sec.GetType() == typeof(Ammo))
            {
                Ammo obj = sec as Ammo;

                basic.AppendLine(Pairs("000027", $"\"{obj.Visual}\""));
                basic.AppendLine(Pairs("000028", obj.Visual_Flags.ToString()));
                basic.AppendLine(Pairs("000029", Single(obj.Condition)));
                basic.AppendLine(Pairs("000030", "0"));
                basic.AppendLine(Pairs("000031", obj.Elapsed.ToString()));
            }
            else if (sec.GetType() == typeof(Anomalous_Zone))
            {
                Anomalous_Zone obj = sec as Anomalous_Zone;

                shapes = obj.Shapes.Length;
                int ind = FillShapes(obj.Shapes, basic, attach, shapes);

                basic.AppendLine(Pairs($"{++ind:D6}", obj.Spase_Restrictor_Type.ToString()));
                basic.AppendLine(Pairs($"{++ind:D6}", Single(obj.Max_Power)));
                basic.AppendLine(Pairs($"{++ind:D6}", ((int)obj.Owner_ID).ToString()));
                basic.AppendLine(Pairs($"{++ind:D6}", "0"));
                basic.AppendLine(Pairs($"{++ind:D6}", "0"));
                basic.AppendLine(Pairs($"{++ind:D6}", "0"));
                basic.AppendLine(Pairs($"{++ind:D6}", Single(obj.Offline_Interactive_Radius)));
                basic.AppendLine(Pairs($"{++ind:D6}", obj.Artefact_Spawn_Count.ToString()));
                basic.AppendLine(Pairs($"{++ind:D6}", "0"));
            }
            else if (sec.GetType() == typeof(Hanging_Lamp))
            {
                Hanging_Lamp obj = sec as Hanging_Lamp;

                basic.AppendLine(Pairs("000027", $"\"{obj.Visual}\""));
                basic.AppendLine(Pairs("000028", obj.Visual_Flags.ToString()));
                basic.AppendLine(Pairs("000029", $"\"{obj.Startup_Animation}\""));
                basic.AppendLine(Pairs("000030", obj.Skeleton_Flags.ToString()));
                basic.AppendLine(Pairs("000031", "65535"));
                basic.AppendLine(Pairs("000032", ((int)obj.Color).ToString()));
                basic.AppendLine(Pairs("000033", Single(obj.Brightness)));
                basic.AppendLine(Pairs("000034", $"\"{obj.Color_Animator}\""));
                basic.AppendLine(Pairs("000035", Single(obj.Range)));
                basic.AppendLine(Pairs("000036", obj.HL_Flags.ToString()));
                basic.AppendLine(Pairs("000037", $"\"{obj.HL_Startup_Animation}\""));
                basic.AppendLine(Pairs("000038", $"\"{obj.Fixed_Bones}\""));
                basic.AppendLine(Pairs("000039", Single(obj.Health)));
                basic.AppendLine(Pairs("000040", Single(obj.Virtual_Size)));
                basic.AppendLine(Pairs("000041", Single(obj.Ambient_Radius)));
                basic.AppendLine(Pairs("000042", Single(obj.Ambient_Power)));
                basic.AppendLine(Pairs("000043", $"\"{obj.Ambient_Texture}\""));
                basic.AppendLine(Pairs("000044", $"\"{obj.Light_Texture}\""));
                basic.AppendLine(Pairs("000045", $"\"{obj.Light_Main_Bone}\""));
                basic.AppendLine(Pairs("000046", Single(obj.Spot_Cone_Angle)));
                basic.AppendLine(Pairs("000047", $"\"{obj.Glow_Texture}\""));
                basic.AppendLine(Pairs("000048", Single(obj.Glow_Radius)));
                basic.AppendLine(Pairs("000049", $"\"{obj.Light_Ambient_Bone}\""));
                basic.AppendLine(Pairs("000050", Single(obj.Volumetric_Quality)));
                basic.AppendLine(Pairs("000051", Single(obj.Volumetric_Intensity)));
                basic.AppendLine(Pairs("000052", Single(obj.Volumetric_Distance)));
            }
            else if (sec.GetType() == typeof(Helicopter))
            {
                Helicopter obj = sec as Helicopter;

                basic.AppendLine(Pairs("000027", $"\"{obj.Visual}\""));
                basic.AppendLine(Pairs("000028", obj.Visual_Flags.ToString()));
                basic.AppendLine(Pairs("000029", $"\"{obj.Motion_Name}\""));
                basic.AppendLine(Pairs("000030", $"\"{obj.Startup_Animation}\""));
                basic.AppendLine(Pairs("000031", obj.Skeleton_Flags.ToString()));
                basic.AppendLine(Pairs("000032", "65535"));
                basic.AppendLine(Pairs("000033", $"\"{obj.HE_Startup_Animation}\""));
                basic.AppendLine(Pairs("000034", $"\"{obj.HE_Engine_Sound}\""));
            }
            else if (sec.GetType() == typeof(Inventory_Box))
            {
                Inventory_Box obj = sec as Inventory_Box;

                basic.AppendLine(Pairs("000027", $"\"{obj.Visual}\""));
                basic.AppendLine(Pairs("000028", obj.Visual_Flags.ToString()));
                basic.AppendLine(Pairs("000029", obj.Can_Take.ToString()));
                basic.AppendLine(Pairs("000030", obj.Closed.ToString()));
                basic.AppendLine(Pairs("000031", $"\"{obj.Tip_Text}\""));
            }
            else if (sec.GetType() == typeof(Item))
            {
                Item obj = sec as Item;

                basic.AppendLine(Pairs("000027", $"\"{obj.Visual}\""));
                basic.AppendLine(Pairs("000028", obj.Visual_Flags.ToString()));
                basic.AppendLine(Pairs("000029", Single(obj.Condition)));
                basic.AppendLine(Pairs("000030", "0"));
            }
            else if (sec.GetType() == typeof(Level_Changer))
            {
                Level_Changer obj = sec as Level_Changer;

                shapes = obj.Shapes.Length;
                int ind = FillShapes(obj.Shapes, basic, attach, shapes);

                basic.AppendLine(Pairs($"{++ind:D6}", obj.Spase_Restrictor_Type.ToString()));
                basic.AppendLine(Pairs($"{++ind:D6}", "65535"));
                basic.AppendLine(Pairs($"{++ind:D6}", "-1"));
                basic.AppendLine(Pairs($"{++ind:D6}", "0.000000"));
                basic.AppendLine(Pairs($"{++ind:D6}", "0.000000"));
                basic.AppendLine(Pairs($"{++ind:D6}", "0.000000"));
                basic.AppendLine(Pairs($"{++ind:D6}", "0.000000, 0.000000, 0.000000"));
                basic.AppendLine(Pairs($"{++ind:D6}", $"\"{obj.Level_To_Change}\""));
                basic.AppendLine(Pairs($"{++ind:D6}", $"\"{obj.Level_Point_To_Change}\""));
                basic.AppendLine(Pairs($"{++ind:D6}", obj.Silent_Mode.ToString()));
                basic.AppendLine(Pairs($"{++ind:D6}", obj.Save_Marker.ToString()));
                basic.AppendLine(Pairs($"{++ind:D6}", $"\"{obj.Level_Changer_Invitation}\""));
                basic.AppendLine(Pairs($"{++ind:D6}", obj.Invitation_Enable.ToString()));
            }
            else if (sec.GetType() == typeof(M_Car))
            {
                M_Car obj = sec as M_Car;

                basic.AppendLine(Pairs("000027", $"\"{obj.Visual}\""));
                basic.AppendLine(Pairs("000028", obj.Visual_Flags.ToString()));
                basic.AppendLine(Pairs("000029", $"\"{obj.Startup_Animation}\""));
                basic.AppendLine(Pairs("000030", obj.Skeleton_Flags.ToString()));
                basic.AppendLine(Pairs("000031", "65535"));
                basic.AppendLine(Pairs("000032", Single(obj.Health)));
            }
            else if (sec.GetType() == typeof(M_Trader))
            {
                M_Trader obj = sec as M_Trader;

                basic.AppendLine(Pairs("000027", $"\"{obj.Visual}\""));
                basic.AppendLine(Pairs("000028", obj.Visual_Flags.ToString()));
                basic.AppendLine(Pairs("000029", obj.Money.ToString()));
                basic.AppendLine(Pairs("000030", $"\"{obj.Specific_Character}\""));
                basic.AppendLine(Pairs("000031", ((int)obj.Trader_Flags).ToString()));
                basic.AppendLine(Pairs("000032", $"\"{obj.Character_Profile}\""));
                basic.AppendLine(Pairs("000033", "-1"));
                basic.AppendLine(Pairs("000034", "-2147483647"));
                basic.AppendLine(Pairs("000035", "-2147483647"));
                basic.AppendLine(Pairs("000036", $"\"{obj.Character_Name}\""));
                basic.AppendLine(Pairs("000037", obj.Deadbody_Can_Take.ToString()));
                basic.AppendLine(Pairs("000038", obj.Deadbody_Closed.ToString()));
            }
            else if (sec.GetType() == typeof(Outfit))
            {
                Outfit obj = sec as Outfit;

                basic.AppendLine(Pairs("000027", $"\"{obj.Visual}\""));
                basic.AppendLine(Pairs("000028", obj.Visual_Flags.ToString()));
                basic.AppendLine(Pairs("000029", Single(obj.Condition)));
                basic.AppendLine(Pairs("000030", "0"));
                basic.AppendLine(Pairs("000031", obj.Condition.ToString()));
            }
            else if (sec.GetType() == typeof(Physic_Object))
            {
                Physic_Object obj = sec as Physic_Object;

                basic.AppendLine(Pairs("000027", $"\"{obj.Visual}\""));
                basic.AppendLine(Pairs("000028", obj.Visual_Flags.ToString()));
                basic.AppendLine(Pairs("000029", $"\"{obj.Startup_Animation}\""));
                basic.AppendLine(Pairs("000030", obj.Skeleton_Flags.ToString()));
                basic.AppendLine(Pairs("000031", "65535"));
                basic.AppendLine(Pairs("000032", obj.Type.ToString()));
                basic.AppendLine(Pairs("000033", Single(obj.Mass)));
                basic.AppendLine(Pairs("000034", $"\"{obj.Fixed_Bones}\""));
            }
            else if (sec.GetType() == typeof(Smart_Cover))
            {
                Smart_Cover obj = sec as Smart_Cover;

                shapes = obj.Shapes.Length;
                int ind = FillShapes(obj.Shapes, basic, attach, shapes);

                basic.AppendLine(Pairs($"{++ind:D6}", $"\"{obj.Description}\""));
                basic.AppendLine(Pairs($"{++ind:D6}", Single(obj.Hold_Position_Time)));
                basic.AppendLine(Pairs($"{++ind:D6}", Single(obj.Enter_MIN_Enemy_Distance)));
                basic.AppendLine(Pairs($"{++ind:D6}", Single(obj.Exit_MIN_Enemy_Distance)));
                basic.AppendLine(Pairs($"{++ind:D6}", obj.Is_Combat_Cover.ToString()));
                basic.AppendLine(Pairs($"{++ind:D6}", obj.Can_Fire.ToString()));
                basic.AppendLine(Pairs($"{++ind:D6}", $"\"{obj.Last_Description}\""));
                basic.AppendLine(Pairs($"{++ind:D6}", "0"));
            }
            else if (sec.GetType() == typeof(Smart_Terrain))
            {
                Smart_Terrain obj = sec as Smart_Terrain;

                shapes = obj.Shapes.Length;
                int ind = FillShapes(obj.Shapes, basic, attach, shapes);

                basic.AppendLine(Pairs($"{++ind:D6}", obj.Spase_Restrictor_Type.ToString()));
                basic.AppendLine(Pairs($"{++ind:D6}", "0"));
                basic.AppendLine(Pairs($"{++ind:D6}", "0"));
                basic.AppendLine(Pairs($"{++ind:D6}", "0"));
                basic.AppendLine(Pairs($"{++ind:D6}", "0"));
                basic.AppendLine(Pairs($"{++ind:D6}", "0"));
                basic.AppendLine(Pairs($"{++ind:D6}", "0"));
                basic.AppendLine(Pairs($"{++ind:D6}", "6"));
            }
            else if (sec.GetType() == typeof(Search_Light))
            {
                Search_Light obj = sec as Search_Light;

                basic.AppendLine(Pairs("000027", $"\"{obj.Visual}\""));
                basic.AppendLine(Pairs("000028", obj.Visual_Flags.ToString()));
            }
            else if(sec.GetType() == typeof(Script_Object))
            {
                Script_Object obj = sec as Script_Object;

                basic.AppendLine(Pairs("000027", $"\"{obj.Visual}\""));
                basic.AppendLine(Pairs("000028", obj.Visual_Flags.ToString()));
            }
            else if (sec.GetType() == typeof(Space_Restrictor))
            {
                Space_Restrictor obj = sec as Space_Restrictor;

                shapes = obj.Shapes.Length;
                int ind = FillShapes(obj.Shapes, basic, attach, shapes);

                basic.AppendLine(Pairs($"{++ind:D6}", obj.Spase_Restrictor_Type.ToString()));
            }
            else if (sec.GetType() == typeof(Torrid_Zone))
            {
                Torrid_Zone obj = sec as Torrid_Zone;

                shapes = obj.Shapes.Length;
                int ind = FillShapes(obj.Shapes, basic, attach, shapes);

                basic.AppendLine(Pairs($"{++ind:D6}", obj.Spase_Restrictor_Type.ToString()));
                basic.AppendLine(Pairs($"{++ind:D6}", Single(obj.Max_Power)));
                basic.AppendLine(Pairs($"{++ind:D6}", "-1"));
                basic.AppendLine(Pairs($"{++ind:D6}", obj.Enabled_Time.ToString()));
                basic.AppendLine(Pairs($"{++ind:D6}", obj.Disabled_Time.ToString()));
                basic.AppendLine(Pairs($"{++ind:D6}", obj.Start_Time_Shift.ToString()));
                basic.AppendLine(Pairs($"{++ind:D6}", $"\"{obj.Motion_Name}\""));
            }
            else if (sec.GetType() == typeof(Weapon))
            {
                Weapon obj = sec as Weapon;

                basic.AppendLine(Pairs("000027", $"\"{obj.Visual}\""));
                basic.AppendLine(Pairs("000028", obj.Visual_Flags.ToString()));
                basic.AppendLine(Pairs("000029", Single(obj.Condition)));
                basic.AppendLine(Pairs("000030", "0"));
                basic.AppendLine(Pairs("000031", obj.Current.ToString()));
                basic.AppendLine(Pairs("000032", obj.Elapsed.ToString()));
                basic.AppendLine(Pairs("000033", obj.WPN_State.ToString()));
                basic.AppendLine(Pairs("000034", obj.Addon_Flags.ToString()));
                basic.AppendLine(Pairs("000035", obj.Ammo_Type.ToString()));
                basic.AppendLine(Pairs("000036", obj.Elapsed_Grenades.ToString()));
            }
            else if (sec.GetType() == typeof(Weapon_ShotGun))
            {
                Weapon_ShotGun obj = sec as Weapon_ShotGun;

                basic.AppendLine(Pairs("000027", $"\"{obj.Visual}\""));
                basic.AppendLine(Pairs("000028", obj.Visual_Flags.ToString()));
                basic.AppendLine(Pairs("000029", Single(obj.Condition)));
                basic.AppendLine(Pairs("000030", "0"));
                basic.AppendLine(Pairs("000031", obj.Current.ToString()));
                basic.AppendLine(Pairs("000032", obj.Elapsed.ToString()));
                basic.AppendLine(Pairs("000033", obj.WPN_State.ToString()));
                basic.AppendLine(Pairs("000034", obj.Addon_Flags.ToString()));
                basic.AppendLine(Pairs("000035", obj.Ammo_Type.ToString()));
                basic.AppendLine(Pairs("000036", obj.Elapsed_Grenades.ToString()));
            }
            else if (sec.GetType() == typeof(Weapon_WGL))
            {
                Weapon_WGL obj = sec as Weapon_WGL;

                basic.AppendLine(Pairs("000027", $"\"{obj.Visual}\""));
                basic.AppendLine(Pairs("000028", obj.Visual_Flags.ToString()));
                basic.AppendLine(Pairs("000029", Single(obj.Condition)));
                basic.AppendLine(Pairs("000030", "0"));
                basic.AppendLine(Pairs("000031", obj.Current.ToString()));
                basic.AppendLine(Pairs("000032", obj.Elapsed.ToString()));
                basic.AppendLine(Pairs("000033", obj.WPN_State.ToString()));
                basic.AppendLine(Pairs("000034", obj.Addon_Flags.ToString()));
                basic.AppendLine(Pairs("000035", obj.Ammo_Type.ToString()));
                basic.AppendLine(Pairs("000036", obj.Elapsed_Grenades.ToString()));
            }
            else if (sec.GetType() == typeof(Zone_Visual))
            {
                Zone_Visual obj = sec as Zone_Visual;

                shapes = obj.Shapes.Length;
                int ind = FillShapes(obj.Shapes, basic, attach, shapes);

                basic.AppendLine(Pairs($"{++ind:D6}", obj.Spase_Restrictor_Type.ToString()));
                basic.AppendLine(Pairs($"{++ind:D6}", Single(obj.Max_Power)));
                basic.AppendLine(Pairs($"{++ind:D6}", "-1"));
                basic.AppendLine(Pairs($"{++ind:D6}", obj.Enabled_Time.ToString()));
                basic.AppendLine(Pairs($"{++ind:D6}", obj.Disabled_Time.ToString()));
                basic.AppendLine(Pairs($"{++ind:D6}", obj.Start_Time_Shift.ToString()));
                basic.AppendLine(Pairs($"{++ind:D6}", Single(obj.Offline_Interactive_Radius)));
                basic.AppendLine(Pairs($"{++ind:D6}", obj.Artefact_Spawn_Count.ToString()));
                basic.AppendLine(Pairs($"{++ind:D6}", "0"));
                basic.AppendLine(Pairs($"{++ind:D6}", $"\"{obj.Visual_Name}\""));
                basic.AppendLine(Pairs($"{++ind:D6}", obj.Visual_Flags.ToString()));
                basic.AppendLine(Pairs($"{++ind:D6}", $"\"{obj.Startup_Animation}\""));
                basic.AppendLine(Pairs($"{++ind:D6}", $"\"{obj.Attack_Animation}\""));
                basic.AppendLine(Pairs($"{++ind:D6}", "0"));
            }
            else if (sec.GetType() == typeof(PDA))
            {
                PDA obj = sec as PDA;

                basic.AppendLine(Pairs("000027", $"\"{obj.Visual}\""));
                basic.AppendLine(Pairs("000028", obj.Visual_Flags.ToString()));
                basic.AppendLine(Pairs("000029", Single(obj.Condition)));
                basic.AppendLine(Pairs("000030", "0"));
                basic.AppendLine(Pairs("000031", "65535"));
                basic.AppendLine(Pairs("000032", $"\"{obj.Specific_Character}\""));
                basic.AppendLine(Pairs("000033", $"\"{obj.Info_Portion}\""));
            }

            sb.AppendLine($"[object_{count}]");

            if (shapes > 0)
                sb.AppendLine(Pairs("attached_count", "1"));

            sb.AppendLine(Pairs("clsid", "6"));
            sb.AppendLine(Pairs("co_flags", "2"));
            sb.AppendLine(Pairs("name", sec.Name));
            sb.AppendLine(Pairs("position", Vector(sec.Position)));
            sb.AppendLine(Pairs("rotation", Vector(sec.Direction)));
            sb.AppendLine(Pairs("scale", "1.000000, 1.000000, 1.000000"));
            sb.AppendLine(Pairs("type", "2"));
            sb.AppendLine(Pairs("version", "23"));
            sb.AppendLine();

            if (shapes > 0)
            {
                sb.AppendLine($"[object_{count}_attached_0]");
                sb.AppendLine(Pairs("clsid", "4"));
                sb.AppendLine(Pairs("co_flags", "2"));
                sb.AppendLine(Pairs("name", "shape"));
                sb.AppendLine(Pairs("position", Vector(sec.Position)));
                sb.AppendLine(Pairs("rotation", Vector(sec.Direction)));
                sb.AppendLine(Pairs("scale", "1.000000, 1.000000, 1.000000"));

                sb.Append(attach);

                sb.AppendLine(Pairs("shapes_count", shapes.ToString()));
                sb.AppendLine(Pairs("version", "2"));
                sb.AppendLine();
            }

            sb.AppendLine($"[object_{count}_spawndata]");
            sb.AppendLine(Pairs("000001", $"{sec.Dummy16}"));
            sb.AppendLine(Pairs("000002", $"\"{sec.Section_Name}\""));
            sb.AppendLine(Pairs("000003", $"\"{sec.Name}\""));
            sb.AppendLine(Pairs("000004", sec.Game_ID.ToString()));
            sb.AppendLine(Pairs("000005", sec.RP.ToString()));
            sb.AppendLine(Pairs("000006", Vector(sec.Position)));
            sb.AppendLine(Pairs("000007", Vector(sec.Direction)));
            sb.AppendLine(Pairs("000008", "0"));
            sb.AppendLine(Pairs("000009", "65535"));
            sb.AppendLine(Pairs("000010", "65535"));
            sb.AppendLine(Pairs("000011", "65535"));
            sb.AppendLine(Pairs("000012", sec.Flags.ToString()));
            sb.AppendLine(Pairs("000013", sec.Version.ToString()));
            sb.AppendLine(Pairs("000014", sec.Game_Type.ToString()));
            sb.AppendLine(Pairs("000015", sec.Script_Version.ToString()));
            sb.AppendLine(Pairs("000016", "0"));
            sb.AppendLine(Pairs("000017", "65535"));
            sb.AppendLine(Pairs("000018", "0"));
            sb.AppendLine(Pairs("000019", "65535"));
            sb.AppendLine(Pairs("000020", "0.000000"));
            sb.AppendLine(Pairs("000021", "1"));
            sb.AppendLine(Pairs("000022", "-1"));
            sb.AppendLine(Pairs("000023", ((int)sec.Object_flags).ToString()));
            sb.AppendLine(Pairs("000024", $"\"{sec.Custom_Data}\""));
            sb.AppendLine(Pairs("000025", "-1"));
            sb.AppendLine(Pairs("000026", "-1"));

            // Basic
            sb.Append(basic);

            sb.AppendLine(Pairs("fl", "0"));
            sb.AppendLine(Pairs("name", sec.Section_Name));

            attach.Clear();
            basic.Clear();
        }
        private static int FillShapes(IShape[] shapes, StringBuilder sb, StringBuilder att, int count)
        {
            sb.AppendLine(Pairs("000027", count.ToString()));

            StringBuilder offset = new();
            StringBuilder radius = new();
            StringBuilder type = new();

            type.AppendLine(Pairs("shape_type", "0"));

            int ind = 27;

            for (byte i = 0; i < count; i++)
            {
                sb.AppendLine(Pairs($"{++ind:D6}", shapes[i].Type.ToString()));
                type.AppendLine(Pairs($"shape_type_{i}", shapes[i].Type.ToString()));

                switch (shapes[i].Type)
                {
                    case 0:

                        sb.AppendLine(Pairs($"{++ind:D6}", Vector(((Shape)shapes[i]).OffSet)));
                        sb.AppendLine(Pairs($"{++ind:D6}", Single(((Shape)shapes[i]).Radius)));
                        offset.AppendLine(Pairs($"shape_center_{i}", Vector(((Shape)shapes[i]).OffSet)));
                        radius.AppendLine(Pairs($"shape_radius_{i}", Single(((Shape)shapes[i]).Radius)));

                        break;

                    case 1:

                        sb.AppendLine(Pairs($"{++ind:D6}", Vector(((Box)shapes[i]).Axis.X)));
                        sb.AppendLine(Pairs($"{++ind:D6}", Vector(((Box)shapes[i]).Axis.Y)));
                        sb.AppendLine(Pairs($"{++ind:D6}", Vector(((Box)shapes[i]).Axis.Z)));
                        sb.AppendLine(Pairs($"{++ind:D6}", Vector(((Box)shapes[i]).OffSet)));
                        offset.AppendLine(Pairs($"shape_matrix_c_{i}", Vector(((Box)shapes[i]).OffSet)));
                        radius.AppendLine(Pairs($"shape_matrix_i_{i}", Vector(((Box)shapes[i]).Axis.X)));
                        radius.AppendLine(Pairs($"shape_matrix_j_{i}", Vector(((Box)shapes[i]).Axis.Y)));
                        radius.AppendLine(Pairs($"shape_matrix_k_{i}", Vector(((Box)shapes[i]).Axis.Z)));

                        break;

                    default:
                        break;
                }
            }

            att.Append(offset);
            att.Append(radius);
            att.Append(type);

            offset.Clear();
            radius.Clear();
            type.Clear();

            return ind;
        }
        private void FillWaysData(ref StringBuilder sb, ref int count)
        {
            var way = data.Ways.Ways;

            if (way.Count > 0)
            {
                foreach (var w in way)
                {
                    if(!CheckLevel.IsOnLevel(w.Points[0].GameVertexID))
                    {
                        continue;
                    }

                    sb.AppendLine();
                    sb.AppendLine($"[object_{count}]");
                    sb.AppendLine(Pairs("clsid", "7"));
                    sb.AppendLine(Pairs("co_flags", "2"));

                    if (w.Points.Length > 0)
                    {
                        var points = w.Points;

                        for (uint k = 0; k < points.Length; k++)
                        {
                            if (points[k].Links != null && points[k].Links.Length > 0)
                            {
                                var link = points[k].Links;

                                for (uint l = 0; l < link.Length; l++)
                                {
                                    string val_str = $"{link[l].Weight:0.000000}";
                                    val_str = val_str.Replace(",", ".");
                                    sb.AppendLine(Pairs($"link_wp_{k}_{l}", $"{link[l].Target_ID}.000000, {val_str}"));
                                }
                            }
                        }
                    }

                    sb.AppendLine(Pairs("name", w.Name));
                    sb.AppendLine(Pairs("position", "0.000000, 0.000000, 0.000000"));
                    sb.AppendLine(Pairs("rotation", "0.000000, 0.000000, 0.000000"));
                    sb.AppendLine(Pairs("scale", "1.000000, 1.000000, 1.000000"));
                    sb.AppendLine(Pairs("type", "0"));
                    sb.AppendLine(Pairs("version", "19"));

                    if (w.Points.Length > 0)
                    {
                        var points = w.Points;

                        for (uint k = 0; k < points.Length; k++)
                        {
                            sb.AppendLine(Pairs($"wp_{k}_flags", points[k].Flag.ToString()));
                            sb.AppendLine(Pairs($"wp_{k}_name", points[k].Name));
                            sb.AppendLine(Pairs($"wp_{k}_pos", Vector(points[k].Position)));
                            sb.AppendLine(Pairs($"wp_{k}_selected", "off"));
                        }
                    }

                    sb.AppendLine(Pairs("wp_count", w.Points.Length.ToString()));
                    count++;
                }
            }
        }

        private string CreateFielPath(string dir, string file_name)
        {
            string file_path = Path.Combine(dir, file_name);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            if (File.Exists(file_path))
            {
                string temp_name = file_name.Replace(".", ".~");

                if (File.Exists(Path.Combine(dir, temp_name)))
                {
                    File.Delete(Path.Combine(dir, temp_name));
                }

                File.Move(file_path, Path.Combine(dir, temp_name));
            }

            return file_path;
        }
    }

    public sealed class StringFormat
    {
        internal static string Pairs(string key, string val) => $"{" ",-8}{key,-33}= {val}";

        internal static string Vector(Vector3 vec)
        {
            string x = vec.X.ToString("0.000000");
            string y = vec.Y.ToString("0.000000");
            string z = vec.Z.ToString("0.000000");

            return $"{x.Replace(",", ".")}, {y.Replace(",", ".")}, {z.Replace(",", ".")}";
        }

        internal static string Single(float val) => val.ToString("0.000000").Replace(",", ".");
    }
}
