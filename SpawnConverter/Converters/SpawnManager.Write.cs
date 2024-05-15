using System.IO;
using System.Text;
using System.Numerics;

using SpawnConverter.Types;
using SpawnConverter.FStream;
using SpawnConverter.Converters.Spawns;
using SpawnConverter.Converters.Spawns.Interfaces;
using SpawnConverter.Converters.Spawns.Classes;

using static SpawnConverter.FStream.FilePath;

namespace SpawnConverter.Converters
{
    partial class SpawnManager
    {
        private enum MODE
        {
            WAYS    = 0,
            SECTION = 1,
        }

        private bool Write()
        {
            Log();

            string fdir = Path.Combine(SDK.LEVELS, data.LevelName);

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

            Log();
            Log("Complete.");

            return true;
        }

        private string GetHeaderFile(int count = 0, MODE mode = 0)
        {
            StringBuilder sb = new();

            sb.AppendLine("[guid]");
            sb.AppendLine(StringFormat.Pairs("guid_g0", "6148914691236517205"));
            sb.AppendLine(StringFormat.Pairs("guid_g1", "6148914691236517205"));
            sb.AppendLine();
            sb.AppendLine("[main]");

            if (mode == MODE.SECTION)
            {
                sb.AppendLine(StringFormat.Pairs("flags", "0"));
            }

            sb.AppendLine(StringFormat.Pairs("objects_count", count.ToString()));
            sb.AppendLine(StringFormat.Pairs("version", "0"));
            sb.AppendLine();
            sb.AppendLine("[modif]");
            sb.AppendLine(StringFormat.Pairs("name", ""));
            sb.AppendLine(StringFormat.Pairs("time", "0"));

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

                    FillSection(sec, ref sb, count);

                    count++;
                }
            }
        }
        private void FillGameGraph(ref StringBuilder sb, ref int count)
        {
            int ind = data.Levels.FindIndex(x => x.Name == data.LevelName);
            var lvls = data.Levels;
            var graphs = lvls[ind].GameGraphs;

            if (graphs.Count == 0)
            {
                return;
            }

            foreach (var gg in graphs)
            {
                sb.AppendLine();
                sb.AppendLine($"[object_{count}]");
                sb.AppendLine(StringFormat.Pairs("clsid", "6"));
                sb.AppendLine(StringFormat.Pairs("co_flags", "2"));
                sb.AppendLine(StringFormat.Pairs("name", $"graph_point_{StringFormat.Index(count)}"));
                sb.AppendLine(StringFormat.Pairs("position", StringFormat.Vector(gg.Local_Point)));
                sb.AppendLine(StringFormat.Pairs("rotation", "0.000000, 0.000000, 0.000000"));
                sb.AppendLine(StringFormat.Pairs("scale", "1.000000, 1.000000, 1.000000"));
                sb.AppendLine(StringFormat.Pairs("type", "2"));
                sb.AppendLine(StringFormat.Pairs("version", "23"));

                sb.AppendLine();
                sb.AppendLine($"[object_{count}_spawndata]");
                sb.AppendLine(StringFormat.Pairs("000001", "1"));
                sb.AppendLine(StringFormat.Pairs("000002", "\"graph_point\""));
                sb.AppendLine(StringFormat.Pairs("000003", $"\"graph_point_{StringFormat.Index(count)}\""));
                sb.AppendLine(StringFormat.Pairs("000004", "0"));
                sb.AppendLine(StringFormat.Pairs("000005", "254"));
                sb.AppendLine(StringFormat.Pairs("000006", StringFormat.Vector(gg.Local_Point)));
                sb.AppendLine(StringFormat.Pairs("000007", "0.000000, 0.000000, 0.000000"));
                sb.AppendLine(StringFormat.Pairs("000008", "0"));
                sb.AppendLine(StringFormat.Pairs("000009", "65535"));
                sb.AppendLine(StringFormat.Pairs("000010", "65535"));
                sb.AppendLine(StringFormat.Pairs("000011", "65535"));
                sb.AppendLine(StringFormat.Pairs("000012", "33"));
                sb.AppendLine(StringFormat.Pairs("000013", "128"));
                sb.AppendLine(StringFormat.Pairs("000014", "65535"));
                sb.AppendLine(StringFormat.Pairs("000015", "12"));
                sb.AppendLine(StringFormat.Pairs("000016", "0"));
                sb.AppendLine(StringFormat.Pairs("000017", "0"));
                sb.AppendLine(StringFormat.Pairs("000018", "0"));

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

                            sb.AppendLine(StringFormat.Pairs("000019", $"\"graph_point_{StringFormat.Index(eg.Game_Vertex_ID - lvls[ind].GameGraphs[0].GameVertexID)}\""));
                            sb.AppendLine(StringFormat.Pairs("000020", $"\"{lvls[ind].Name}\""));

                            found_link = true;

                            break;
                        }
                    }
                }

                if (!found_link)
                {
                    sb.AppendLine(StringFormat.Pairs("000019", "\"\""));
                    sb.AppendLine(StringFormat.Pairs("000020", "\"\""));
                }

                sb.AppendLine(StringFormat.Pairs("000021", gg.VertexType.A.ToString()));
                sb.AppendLine(StringFormat.Pairs("000022", gg.VertexType.B.ToString()));
                sb.AppendLine(StringFormat.Pairs("000023", gg.VertexType.C.ToString()));
                sb.AppendLine(StringFormat.Pairs("000024", gg.VertexType.D.ToString()));
                sb.AppendLine(StringFormat.Pairs("fl", "0"));
                sb.AppendLine(StringFormat.Pairs("name", "graph_point"));

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

                basic.AppendLine(StringFormat.Pairs("000027", $"\"{obj.Visual}\""));
                basic.AppendLine(StringFormat.Pairs("000028", obj.Visual_Flags.ToString()));
                basic.AppendLine(StringFormat.Pairs("000029", "0"));
                basic.AppendLine(StringFormat.Pairs("000030", "0"));
                basic.AppendLine(StringFormat.Pairs("000031", "0"));
                basic.AppendLine(StringFormat.Pairs("000032", StringFormat.Single(obj.Health)));
                basic.AppendLine(StringFormat.Pairs("000033", "0"));
                basic.AppendLine(StringFormat.Pairs("000034", "0"));
                basic.AppendLine(StringFormat.Pairs("000035", "65535"));
                basic.AppendLine(StringFormat.Pairs("000036", "0"));
                basic.AppendLine(StringFormat.Pairs("000037", obj.Money.ToString()));
                basic.AppendLine(StringFormat.Pairs("000038", $"\"{obj.Specific_Character}\""));
                basic.AppendLine(StringFormat.Pairs("000039", ((int)obj.Trader_Flags).ToString()));
                basic.AppendLine(StringFormat.Pairs("000040", $"\"{obj.Character_Profile}\""));
                basic.AppendLine(StringFormat.Pairs("000041", "-1"));
                basic.AppendLine(StringFormat.Pairs("000042", "-2147483647"));
                basic.AppendLine(StringFormat.Pairs("000043", "-2147483647"));
                basic.AppendLine(StringFormat.Pairs("000044", $"\"{obj.Character_Name}\""));
                basic.AppendLine(StringFormat.Pairs("000045", obj.Deadbody_Can_Take.ToString()));
                basic.AppendLine(StringFormat.Pairs("000046", obj.Deadbody_Closed.ToString()));
                basic.AppendLine(StringFormat.Pairs("000047", $"\"{obj.Startup_Animation}\""));
                basic.AppendLine(StringFormat.Pairs("000048", obj.Skeleton_Flags.ToString()));
                basic.AppendLine(StringFormat.Pairs("000049", "65535"));
                basic.AppendLine(StringFormat.Pairs("000050", "65535"));
                basic.AppendLine(StringFormat.Pairs("000051", obj.Save_Marker_Start.ToString()));
                basic.AppendLine(StringFormat.Pairs("000052", obj.Start_Position_Filled.ToString()));
            }
            else if (sec.GetType() == typeof(Ammo))
            {
                Ammo obj = sec as Ammo;

                basic.AppendLine(StringFormat.Pairs("000027", $"\"{obj.Visual}\""));
                basic.AppendLine(StringFormat.Pairs("000028", obj.Visual_Flags.ToString()));
                basic.AppendLine(StringFormat.Pairs("000029", StringFormat.Single(obj.Condition)));
                basic.AppendLine(StringFormat.Pairs("000030", "0"));
                basic.AppendLine(StringFormat.Pairs("000031", obj.Elapsed.ToString()));
            }
            else if (sec.GetType() == typeof(Anomalous_Zone))
            {
                Anomalous_Zone obj = sec as Anomalous_Zone;

                shapes = obj.Shapes.Length;
                int ind = FillShapes(obj.Shapes, basic, attach, shapes);

                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", obj.Spase_Restrictor_Type.ToString()));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", StringFormat.Single(obj.Max_Power)));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", ((int)obj.Owner_ID).ToString()));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", "0"));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", "0"));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", "0"));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", StringFormat.Single(obj.Offline_Interactive_Radius)));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", obj.Artefact_Spawn_Count.ToString()));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", "0"));
            }
            else if (sec.GetType() == typeof(Hanging_Lamp))
            {
                Hanging_Lamp obj = sec as Hanging_Lamp;

                basic.AppendLine(StringFormat.Pairs("000027", $"\"{obj.Visual}\""));
                basic.AppendLine(StringFormat.Pairs("000028", obj.Visual_Flags.ToString()));
                basic.AppendLine(StringFormat.Pairs("000029", $"\"{obj.Startup_Animation}\""));
                basic.AppendLine(StringFormat.Pairs("000030", obj.Skeleton_Flags.ToString()));
                basic.AppendLine(StringFormat.Pairs("000031", "65535"));
                basic.AppendLine(StringFormat.Pairs("000032", ((int)obj.Color).ToString()));
                basic.AppendLine(StringFormat.Pairs("000033", StringFormat.Single(obj.Brightness)));
                basic.AppendLine(StringFormat.Pairs("000034", $"\"{obj.Color_Animator}\""));
                basic.AppendLine(StringFormat.Pairs("000035", StringFormat.Single(obj.Range)));
                basic.AppendLine(StringFormat.Pairs("000036", obj.HL_Flags.ToString()));
                basic.AppendLine(StringFormat.Pairs("000037", $"\"{obj.HL_Startup_Animation}\""));
                basic.AppendLine(StringFormat.Pairs("000038", $"\"{obj.Fixed_Bones}\""));
                basic.AppendLine(StringFormat.Pairs("000039", StringFormat.Single(obj.Health)));
                basic.AppendLine(StringFormat.Pairs("000040", StringFormat.Single(obj.Virtual_Size)));
                basic.AppendLine(StringFormat.Pairs("000041", StringFormat.Single(obj.Ambient_Radius)));
                basic.AppendLine(StringFormat.Pairs("000042", StringFormat.Single(obj.Ambient_Power)));
                basic.AppendLine(StringFormat.Pairs("000043", $"\"{obj.Ambient_Texture}\""));
                basic.AppendLine(StringFormat.Pairs("000044", $"\"{obj.Light_Texture}\""));
                basic.AppendLine(StringFormat.Pairs("000045", $"\"{obj.Light_Main_Bone}\""));
                basic.AppendLine(StringFormat.Pairs("000046", StringFormat.Single(obj.Spot_Cone_Angle)));
                basic.AppendLine(StringFormat.Pairs("000047", $"\"{obj.Glow_Texture}\""));
                basic.AppendLine(StringFormat.Pairs("000048", StringFormat.Single(obj.Glow_Radius)));
                basic.AppendLine(StringFormat.Pairs("000049", $"\"{obj.Light_Ambient_Bone}\""));
                basic.AppendLine(StringFormat.Pairs("000050", StringFormat.Single(obj.Volumetric_Quality)));
                basic.AppendLine(StringFormat.Pairs("000051", StringFormat.Single(obj.Volumetric_Intensity)));
                basic.AppendLine(StringFormat.Pairs("000052", StringFormat.Single(obj.Volumetric_Distance)));
            }
            else if (sec.GetType() == typeof(Helicopter))
            {
                Helicopter obj = sec as Helicopter;

                basic.AppendLine(StringFormat.Pairs("000027", $"\"{obj.Visual}\""));
                basic.AppendLine(StringFormat.Pairs("000028", obj.Visual_Flags.ToString()));
                basic.AppendLine(StringFormat.Pairs("000029", $"\"{obj.Motion_Name}\""));
                basic.AppendLine(StringFormat.Pairs("000030", $"\"{obj.Startup_Animation}\""));
                basic.AppendLine(StringFormat.Pairs("000031", obj.Skeleton_Flags.ToString()));
                basic.AppendLine(StringFormat.Pairs("000032", "65535"));
                basic.AppendLine(StringFormat.Pairs("000033", $"\"{obj.HE_Startup_Animation}\""));
                basic.AppendLine(StringFormat.Pairs("000034", $"\"{obj.HE_Engine_Sound}\""));
            }
            else if (sec.GetType() == typeof(Inventory_Box))
            {
                Inventory_Box obj = sec as Inventory_Box;

                basic.AppendLine(StringFormat.Pairs("000027", $"\"{obj.Visual}\""));
                basic.AppendLine(StringFormat.Pairs("000028", obj.Visual_Flags.ToString()));
                basic.AppendLine(StringFormat.Pairs("000029", obj.Can_Take.ToString()));
                basic.AppendLine(StringFormat.Pairs("000030", obj.Closed.ToString()));
                basic.AppendLine(StringFormat.Pairs("000031", $"\"{obj.Tip_Text}\""));
            }
            else if (sec.GetType() == typeof(Item))
            {
                Item obj = sec as Item;

                basic.AppendLine(StringFormat.Pairs("000027", $"\"{obj.Visual}\""));
                basic.AppendLine(StringFormat.Pairs("000028", obj.Visual_Flags.ToString()));
                basic.AppendLine(StringFormat.Pairs("000029", StringFormat.Single(obj.Condition)));
                basic.AppendLine(StringFormat.Pairs("000030", "0"));
            }
            else if (sec.GetType() == typeof(Level_Changer))
            {
                Level_Changer obj = sec as Level_Changer;

                shapes = obj.Shapes.Length;
                int ind = FillShapes(obj.Shapes, basic, attach, shapes);

                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", obj.Spase_Restrictor_Type.ToString()));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", "65535"));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", "-1"));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", "0.000000"));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", "0.000000"));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", "0.000000"));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", "0.000000, 0.000000, 0.000000"));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", $"\"{obj.Level_To_Change}\""));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", $"\"{obj.Level_Point_To_Change}\""));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", obj.Silent_Mode.ToString()));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", obj.Save_Marker.ToString()));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", $"\"{obj.Level_Changer_Invitation}\""));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", obj.Invitation_Enable.ToString()));
            }
            else if (sec.GetType() == typeof(M_Car))
            {
                M_Car obj = sec as M_Car;

                basic.AppendLine(StringFormat.Pairs("000027", $"\"{obj.Visual}\""));
                basic.AppendLine(StringFormat.Pairs("000028", obj.Visual_Flags.ToString()));
                basic.AppendLine(StringFormat.Pairs("000029", $"\"{obj.Startup_Animation}\""));
                basic.AppendLine(StringFormat.Pairs("000030", obj.Skeleton_Flags.ToString()));
                basic.AppendLine(StringFormat.Pairs("000031", "65535"));
                basic.AppendLine(StringFormat.Pairs("000032", StringFormat.Single(obj.Health)));
            }
            else if (sec.GetType() == typeof(M_Trader))
            {
                M_Trader obj = sec as M_Trader;

                basic.AppendLine(StringFormat.Pairs("000027", $"\"{obj.Visual}\""));
                basic.AppendLine(StringFormat.Pairs("000028", obj.Visual_Flags.ToString()));
                basic.AppendLine(StringFormat.Pairs("000029", obj.Money.ToString()));
                basic.AppendLine(StringFormat.Pairs("000030", $"\"{obj.Specific_Character}\""));
                basic.AppendLine(StringFormat.Pairs("000031", ((int)obj.Trader_Flags).ToString()));
                basic.AppendLine(StringFormat.Pairs("000032", $"\"{obj.Character_Profile}\""));
                basic.AppendLine(StringFormat.Pairs("000033", "-1"));
                basic.AppendLine(StringFormat.Pairs("000034", "-2147483647"));
                basic.AppendLine(StringFormat.Pairs("000035", "-2147483647"));
                basic.AppendLine(StringFormat.Pairs("000036", $"\"{obj.Character_Name}\""));
                basic.AppendLine(StringFormat.Pairs("000037", obj.Deadbody_Can_Take.ToString()));
                basic.AppendLine(StringFormat.Pairs("000037", obj.Deadbody_Closed.ToString()));
            }
            else if (sec.GetType() == typeof(Outfit))
            {
                Outfit obj = sec as Outfit;

                basic.AppendLine(StringFormat.Pairs("000027", $"\"{obj.Visual}\""));
                basic.AppendLine(StringFormat.Pairs("000028", obj.Visual_Flags.ToString()));
                basic.AppendLine(StringFormat.Pairs("000029", StringFormat.Single(obj.Condition)));
                basic.AppendLine(StringFormat.Pairs("000030", "0"));
                basic.AppendLine(StringFormat.Pairs("000031", obj.Condition.ToString()));
            }
            else if (sec.GetType() == typeof(Physic_Object))
            {
                Physic_Object obj = sec as Physic_Object;

                basic.AppendLine(StringFormat.Pairs("000027", $"\"{obj.Visual}\""));
                basic.AppendLine(StringFormat.Pairs("000028", obj.Visual_Flags.ToString()));
                basic.AppendLine(StringFormat.Pairs("000029", $"\"{obj.Startup_Animation}\""));
                basic.AppendLine(StringFormat.Pairs("000030", obj.Skeleton_Flags.ToString()));
                basic.AppendLine(StringFormat.Pairs("000031", "65535"));
                basic.AppendLine(StringFormat.Pairs("000032", obj.Type.ToString()));
                basic.AppendLine(StringFormat.Pairs("000033", StringFormat.Single(obj.Mass)));
                basic.AppendLine(StringFormat.Pairs("000034", $"\"{obj.Fixed_Bones}\""));
            }
            else if (sec.GetType() == typeof(Smart_Cover))
            {
                Smart_Cover obj = sec as Smart_Cover;

                shapes = obj.Shapes.Length;
                int ind = FillShapes(obj.Shapes, basic, attach, shapes);

                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", $"\"{obj.Description}\""));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", StringFormat.Single(obj.Hold_Position_Time)));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", StringFormat.Single(obj.Enter_MIN_Enemy_Distance)));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", StringFormat.Single(obj.Exit_MIN_Enemy_Distance)));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", obj.Is_Combat_Cover.ToString()));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", obj.Can_Fire.ToString()));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", $"\"{obj.Last_Description}\""));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", "0"));
            }
            else if (sec.GetType() == typeof(Smart_Terrain))
            {
                Smart_Terrain obj = sec as Smart_Terrain;

                shapes = obj.Shapes.Length;
                int ind = FillShapes(obj.Shapes, basic, attach, shapes);

                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", obj.Spase_Restrictor_Type.ToString()));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", "0"));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", "0"));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", "0"));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", "0"));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", "0"));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", "0"));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", "6"));
            }
            else if (sec.GetType() == typeof(Search_Light))
            {
                Search_Light obj = sec as Search_Light;

                basic.AppendLine(StringFormat.Pairs("000027", $"\"{obj.Visual}\""));
                basic.AppendLine(StringFormat.Pairs("000028", obj.Visual_Flags.ToString()));
            }
            else if (sec.GetType() == typeof(Space_Restrictor))
            {
                Space_Restrictor obj = sec as Space_Restrictor;

                shapes = obj.Shapes.Length;
                int ind = FillShapes(obj.Shapes, basic, attach, shapes);

                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", obj.Spase_Restrictor_Type.ToString()));
            }
            else if (sec.GetType() == typeof(Torrid_Zone))
            {
                Torrid_Zone obj = sec as Torrid_Zone;

                shapes = obj.Shapes.Length;
                int ind = FillShapes(obj.Shapes, basic, attach, shapes);

                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", obj.Spase_Restrictor_Type.ToString()));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", StringFormat.Single(obj.Max_Power)));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", "-1"));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", obj.Enabled_Time.ToString()));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", obj.Disabled_Time.ToString()));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", obj.Start_Time_Shift.ToString()));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", $"\"{obj.Motion_Name}\""));
            }
            else if (sec.GetType() == typeof(Weapon))
            {
                Weapon obj = sec as Weapon;

                basic.AppendLine(StringFormat.Pairs("000027", $"\"{obj.Visual}\""));
                basic.AppendLine(StringFormat.Pairs("000028", obj.Visual_Flags.ToString()));
                basic.AppendLine(StringFormat.Pairs("000029", StringFormat.Single(obj.Condition)));
                basic.AppendLine(StringFormat.Pairs("000030", "0"));
                basic.AppendLine(StringFormat.Pairs("000031", obj.Current.ToString()));
                basic.AppendLine(StringFormat.Pairs("000032", obj.Elapsed.ToString()));
                basic.AppendLine(StringFormat.Pairs("000033", obj.WPN_State.ToString()));
                basic.AppendLine(StringFormat.Pairs("000034", obj.Addon_Flags.ToString()));
                basic.AppendLine(StringFormat.Pairs("000035", obj.Ammo_Type.ToString()));
                basic.AppendLine(StringFormat.Pairs("000036", obj.Elapsed_Grenades.ToString()));
            }
            else if (sec.GetType() == typeof(Weapon_ShotGun))
            {
                Weapon_ShotGun obj = sec as Weapon_ShotGun;

                basic.AppendLine(StringFormat.Pairs("000027", $"\"{obj.Visual}\""));
                basic.AppendLine(StringFormat.Pairs("000028", obj.Visual_Flags.ToString()));
                basic.AppendLine(StringFormat.Pairs("000029", StringFormat.Single(obj.Condition)));
                basic.AppendLine(StringFormat.Pairs("000030", "0"));
                basic.AppendLine(StringFormat.Pairs("000031", obj.Current.ToString()));
                basic.AppendLine(StringFormat.Pairs("000032", obj.Elapsed.ToString()));
                basic.AppendLine(StringFormat.Pairs("000033", obj.WPN_State.ToString()));
                basic.AppendLine(StringFormat.Pairs("000034", obj.Addon_Flags.ToString()));
                basic.AppendLine(StringFormat.Pairs("000035", obj.Ammo_Type.ToString()));
                basic.AppendLine(StringFormat.Pairs("000036", obj.Elapsed_Grenades.ToString()));
            }
            else if (sec.GetType() == typeof(Weapon_WGL))
            {
                Weapon_WGL obj = sec as Weapon_WGL;

                basic.AppendLine(StringFormat.Pairs("000027", $"\"{obj.Visual}\""));
                basic.AppendLine(StringFormat.Pairs("000028", obj.Visual_Flags.ToString()));
                basic.AppendLine(StringFormat.Pairs("000029", StringFormat.Single(obj.Condition)));
                basic.AppendLine(StringFormat.Pairs("000030", "0"));
                basic.AppendLine(StringFormat.Pairs("000031", obj.Current.ToString()));
                basic.AppendLine(StringFormat.Pairs("000032", obj.Elapsed.ToString()));
                basic.AppendLine(StringFormat.Pairs("000033", obj.WPN_State.ToString()));
                basic.AppendLine(StringFormat.Pairs("000034", obj.Addon_Flags.ToString()));
                basic.AppendLine(StringFormat.Pairs("000035", obj.Ammo_Type.ToString()));
                basic.AppendLine(StringFormat.Pairs("000036", obj.Elapsed_Grenades.ToString()));
            }
            else if (sec.GetType() == typeof(Zone_Visual))
            {
                Zone_Visual obj = sec as Zone_Visual;

                shapes = obj.Shapes.Length;
                int ind = FillShapes(obj.Shapes, basic, attach, shapes);

                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", obj.Spase_Restrictor_Type.ToString()));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", StringFormat.Single(obj.Max_Power)));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", "-1"));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", obj.Enabled_Time.ToString()));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", obj.Disabled_Time.ToString()));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", obj.Start_Time_Shift.ToString()));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", StringFormat.Single(obj.Offline_Interactive_Radius)));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", obj.Artefact_Spawn_Count.ToString()));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", "0"));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", $"\"{obj.Visual_Name}\""));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", obj.Visual_Flags.ToString()));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", $"\"{obj.Startup_Animation}\""));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", $"\"{obj.Attack_Animation}\""));
                basic.AppendLine(StringFormat.Pairs($"0000{++ind}", "0"));
            }
            else if (sec.GetType() == typeof(PDA))
            {
                PDA obj = sec as PDA;

                basic.AppendLine(StringFormat.Pairs("000027", $"\"{obj.Visual}\""));
                basic.AppendLine(StringFormat.Pairs("000028", obj.Visual_Flags.ToString()));
                basic.AppendLine(StringFormat.Pairs("000029", StringFormat.Single(obj.Condition)));
                basic.AppendLine(StringFormat.Pairs("000030", "0"));
                basic.AppendLine(StringFormat.Pairs("000031", "65535"));
                basic.AppendLine(StringFormat.Pairs("000030", $"\"{obj.Specific_Character}\""));
                basic.AppendLine(StringFormat.Pairs("000030", $"\"{obj.Info_Portion}\""));
            }

            sb.AppendLine($"[object_{count}]");

            if (shapes > 0)
                sb.AppendLine(StringFormat.Pairs("attached_count", "1"));

            sb.AppendLine(StringFormat.Pairs("clsid", "6"));
            sb.AppendLine(StringFormat.Pairs("co_flags", "2"));
            sb.AppendLine(StringFormat.Pairs("name", sec.Name));
            sb.AppendLine(StringFormat.Pairs("position", StringFormat.Vector(sec.Position)));
            sb.AppendLine(StringFormat.Pairs("rotation", StringFormat.Vector(sec.Direction)));
            sb.AppendLine(StringFormat.Pairs("scale", "1.000000, 1.000000, 1.000000"));
            sb.AppendLine(StringFormat.Pairs("type", "2"));
            sb.AppendLine(StringFormat.Pairs("version", "23"));
            sb.AppendLine();

            if (shapes > 0)
            {
                sb.AppendLine($"[object_{count}_attached_0]");
                sb.AppendLine(StringFormat.Pairs("clsid", "4"));
                sb.AppendLine(StringFormat.Pairs("co_flags", "2"));
                sb.AppendLine(StringFormat.Pairs("name", "shape"));
                sb.AppendLine(StringFormat.Pairs("position", StringFormat.Vector(sec.Position)));
                sb.AppendLine(StringFormat.Pairs("rotation", StringFormat.Vector(sec.Direction)));
                sb.AppendLine(StringFormat.Pairs("scale", "1.000000, 1.000000, 1.000000"));

                sb.Append(attach);

                sb.AppendLine(StringFormat.Pairs("shapes_count", shapes.ToString()));
                sb.AppendLine(StringFormat.Pairs("version", "2"));
                sb.AppendLine();
            }

            sb.AppendLine($"[object_{count}_spawndata]");
            sb.AppendLine(StringFormat.Pairs("000001", $"{sec.Dummy16}"));
            sb.AppendLine(StringFormat.Pairs("000002", $"\"{sec.Section_Name}\""));
            sb.AppendLine(StringFormat.Pairs("000003", $"\"{sec.Name}\""));
            sb.AppendLine(StringFormat.Pairs("000004", sec.Game_ID.ToString()));
            sb.AppendLine(StringFormat.Pairs("000005", sec.RP.ToString()));
            sb.AppendLine(StringFormat.Pairs("000006", StringFormat.Vector(sec.Position)));
            sb.AppendLine(StringFormat.Pairs("000007", StringFormat.Vector(sec.Direction)));
            sb.AppendLine(StringFormat.Pairs("000008", "0"));
            sb.AppendLine(StringFormat.Pairs("000009", "65535"));
            sb.AppendLine(StringFormat.Pairs("000010", "65535"));
            sb.AppendLine(StringFormat.Pairs("000011", "65535"));
            sb.AppendLine(StringFormat.Pairs("000012", sec.Flags.ToString()));
            sb.AppendLine(StringFormat.Pairs("000013", sec.Version.ToString()));
            sb.AppendLine(StringFormat.Pairs("000014", sec.Game_Type.ToString()));
            sb.AppendLine(StringFormat.Pairs("000015", sec.Script_Version.ToString()));
            sb.AppendLine(StringFormat.Pairs("000016", "0"));
            sb.AppendLine(StringFormat.Pairs("000017", "65535"));
            sb.AppendLine(StringFormat.Pairs("000018", "0"));
            sb.AppendLine(StringFormat.Pairs("000019", "65535"));
            sb.AppendLine(StringFormat.Pairs("000020", "0.000000"));
            sb.AppendLine(StringFormat.Pairs("000021", "1"));
            sb.AppendLine(StringFormat.Pairs("000022", "-1"));
            sb.AppendLine(StringFormat.Pairs("000023", ((int)sec.Object_flags).ToString()));
            sb.AppendLine(StringFormat.Pairs("000024", $"\"{sec.Custom_Data}\""));
            sb.AppendLine(StringFormat.Pairs("000025", "-1"));
            sb.AppendLine(StringFormat.Pairs("000026", "-1"));

            // Basic
            sb.Append(basic);

            sb.AppendLine(StringFormat.Pairs("fl", "0"));
            sb.AppendLine(StringFormat.Pairs("name", sec.Section_Name));

            attach.Clear();
            basic.Clear();
        }
        private static int FillShapes(IShape[] shapes, StringBuilder sb, StringBuilder att, int count)
        {
            sb.AppendLine(StringFormat.Pairs("000027", count.ToString()));

            StringBuilder offset = new();
            StringBuilder radius = new();
            StringBuilder type = new();

            type.AppendLine(StringFormat.Pairs("shape_type", "0"));

            int ind = 27;

            for (byte i = 0; i < count; i++)
            {


                sb.AppendLine(StringFormat.Pairs($"0000{++ind}", shapes[i].Type.ToString()));
                type.AppendLine(StringFormat.Pairs($"shape_type_{i}", shapes[i].Type.ToString()));

                switch (shapes[i].Type)
                {
                    case 0:

                        sb.AppendLine(StringFormat.Pairs($"0000{++ind}", StringFormat.Vector(((Shape)shapes[i]).OffSet)));
                        sb.AppendLine(StringFormat.Pairs($"0000{++ind}", StringFormat.Single(((Shape)shapes[i]).Radius)));
                        offset.AppendLine(StringFormat.Pairs($"shape_center_{i}", StringFormat.Vector(((Shape)shapes[i]).OffSet)));
                        radius.AppendLine(StringFormat.Pairs($"shape_radius_{i}", StringFormat.Single(((Shape)shapes[i]).Radius)));

                        break;

                    case 1:

                        sb.AppendLine(StringFormat.Pairs($"0000{++ind}", StringFormat.Vector(((Box)shapes[i]).Axis.X)));
                        sb.AppendLine(StringFormat.Pairs($"0000{++ind}", StringFormat.Vector(((Box)shapes[i]).Axis.Y)));
                        sb.AppendLine(StringFormat.Pairs($"0000{++ind}", StringFormat.Vector(((Box)shapes[i]).Axis.Z)));
                        sb.AppendLine(StringFormat.Pairs($"0000{++ind}", StringFormat.Vector(((Box)shapes[i]).OffSet)));
                        offset.AppendLine(StringFormat.Pairs($"shape_matrix_c_{i}", StringFormat.Vector(((Box)shapes[i]).OffSet)));
                        radius.AppendLine(StringFormat.Pairs($"shape_matrix_i_{i}", StringFormat.Vector(((Box)shapes[i]).Axis.X)));
                        radius.AppendLine(StringFormat.Pairs($"shape_matrix_j_{i}", StringFormat.Vector(((Box)shapes[i]).Axis.Y)));
                        radius.AppendLine(StringFormat.Pairs($"shape_matrix_k_{i}", StringFormat.Vector(((Box)shapes[i]).Axis.Z)));

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
                    sb.AppendLine();
                    sb.AppendLine($"[object_{count}]");
                    sb.AppendLine(StringFormat.Pairs("clsid", "7"));
                    sb.AppendLine(StringFormat.Pairs("co_flags", "2"));

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
                                    sb.AppendLine(StringFormat.Pairs($"link_wp_{k}_{l}", $"{link[l].Target_ID}.000000, {val_str}"));
                                }
                            }
                        }
                    }

                    sb.AppendLine(StringFormat.Pairs("name", w.Name));
                    sb.AppendLine(StringFormat.Pairs("position", "0.000000, 0.000000, 0.000000"));
                    sb.AppendLine(StringFormat.Pairs("rotation", "0.000000, 0.000000, 0.000000"));
                    sb.AppendLine(StringFormat.Pairs("scale", "1.000000, 1.000000, 1.000000"));
                    sb.AppendLine(StringFormat.Pairs("type", "0"));
                    sb.AppendLine(StringFormat.Pairs("version", "19"));

                    if (w.Points.Length > 0)
                    {
                        var points = w.Points;

                        for (uint k = 0; k < points.Length; k++)
                        {
                            sb.AppendLine(StringFormat.Pairs($"wp_{k}_flags", points[k].Flag.ToString()));
                            sb.AppendLine(StringFormat.Pairs($"wp_{k}_name", points[k].Name));
                            sb.AppendLine(StringFormat.Pairs($"wp_{k}_pos", StringFormat.Vector(points[k].Position)));
                            sb.AppendLine(StringFormat.Pairs($"wp_{k}_selected", "off"));
                        }
                    }

                    sb.AppendLine(StringFormat.Pairs("wp_count", w.Points.Length.ToString()));
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

        private sealed class StringFormat
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

            internal static string Index(int val)
            {
                string str_ind = $"{val}";
                int length = str_ind.Length;

                if (str_ind.Length < 4)
                {
                    for (int i = 0; i < 4 - length; i++)
                    {
                        str_ind = "0" + str_ind;
                    }
                }

                return str_ind;
            }
        }
    }
}
