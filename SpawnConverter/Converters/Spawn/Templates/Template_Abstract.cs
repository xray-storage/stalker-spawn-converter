using SpawnConverter.FStream;
using SpawnConverter.Converters.Spawns.Interfaces;

namespace SpawnConverter.Converters.Spawns.Templates
{
    internal static class CSE_ALife_Dynamic_Object_Visual
    {
        internal static void ReadBasic<T>(XrFileReader reader, T section) where T : ICSE_ALife_Dynamic_Object_Visual
        {
            section.Visual = reader.ReadStringZ();
            section.Visual_Flags = reader.ReadByte();
        }
        internal static void ReadUpdate<T>(XrFileReader reader, T section) where T : ICSE_ALife_Dynamic_Object_Visual
        {

        }
    }

    internal static class CSE_Alife_Creature_Abstract
    {
        internal static void ReadBasic<T>(XrFileReader reader, T section) where T : ICSE_Alife_Creature_Abstract
        {

            section.Team = reader.ReadByte();
            section.Squad = reader.ReadByte();
            section.Group = reader.ReadByte();
            section.Health = reader.ReadFloat();
            section.Dynamic_OUT_Restrictions = reader.ReadVector2s16();
            section.Dynamic_IN_Restrictions = reader.ReadVector2s16();
            section.Killer_ID = reader.ReadUInt16();
            section.Game_Death_Time = reader.ReadUInt64();
        }
        internal static void ReadUpdate<T>(XrFileReader reader, T section) where T : ICSE_Alife_Creature_Abstract
        {
            section.UPD_Health = reader.ReadFloat();
            section.UPD_TimeStamp = reader.ReadUInt32();
            section.UPD_Flags = reader.ReadByte();
            section.UPD_Position = reader.ReadVector();
            section.UPD_Model = reader.ReadFloat();
            section.UPD_Torso = reader.ReadVector();
            section.UPD_Team = reader.ReadByte();
            section.UPD_Squad = reader.ReadByte();
            section.UPD_Group = reader.ReadByte();
        }
    }

    internal static class CSE_Alife_Trader_Abstract
    {
        internal static void ReadBasic<T>(XrFileReader reader, T section) where T : ICSE_Alife_Trader_Abstract
        {
            section.Money = reader.ReadUInt32();
            section.Specific_Character = reader.ReadStringZ();
            section.Trader_Flags = reader.ReadUInt32();
            section.Character_Profile = reader.ReadStringZ();
            section.Community_Index = reader.ReadInt32();
            section.Rank = reader.ReadInt32();
            section.Reputation = reader.ReadInt32();
            section.Character_Name = reader.ReadStringZ();
            section.Deadbody_Can_Take = reader.ReadByte();
            section.Deadbody_Closed = reader.ReadByte();
        }
        internal static void ReadUpdate<T>(XrFileReader reader, T section) where T : ICSE_Alife_Trader_Abstract
        {

        }
    }

    internal static class CSE_ALife_Anomalous_Zone
    {
        internal static void ReadBasic<T>(XrFileReader reader, T section) where T : ICSE_ALife_Anomalous_Zone
        {
            section.Offline_Interactive_Radius = reader.ReadFloat();
            section.Artefact_Spawn_Count = reader.ReadUInt16();
            section.Artefact_Position_Offset = reader.ReadUInt32();
        }
        internal static void ReadUpdate<T>(XrFileReader reader, T section) where T : ICSE_ALife_Anomalous_Zone
        {

        }
    }

    internal static class CSE_PH_Skeleton
    {
        internal static void ReadBasic<T>(XrFileReader reader, T section) where T : ICSE_PH_Skeleton
        {
            section.Startup_Animation = reader.ReadStringZ();
            section.Skeleton_Flags = reader.ReadByte();
            section.Source_ID = reader.ReadUInt16();
        }
        internal static void ReadUpdate<T>(XrFileReader reader, T section) where T : ICSE_PH_Skeleton
        {

        }
    }

    internal static class CSE_ALife_Inventory_Item
    {
        private const byte NoAngular = 2;
        private const byte NoLinear = 4;

        internal static void ReadBasic<T>(XrFileReader reader, T section) where T : ICSE_ALife_Inventory_Item
        {
            section.Condition = reader.ReadFloat();
            section.Upgrades = reader.ReadVectorStr();
        }
        internal static void ReadUpdate<T>(XrFileReader reader, T section) where T : ICSE_ALife_Inventory_Item
        {
            section.UPD_Num_Item = reader.ReadByte();

            if (section.UPD_Num_Item > 0)
            {
                section.UPD_State_Force = reader.ReadVector();
                section.UPD_State_Torque = reader.ReadVector();
                section.UPD_State_Position = reader.ReadVector();
                section.UPD_State_Quaternion = reader.ReadQuaternion();

                if (((section.UPD_Num_Item >> 5) & NoAngular) == 0)
                {
                    section.UPD_State_Angular_Velocity = reader.ReadVector();
                }

                if (((section.UPD_Num_Item >> 5) & NoLinear) == 0)
                {
                    section.UPD_State_Linear_Velocity = reader.ReadVector();
                }

                section.UPD_Freeze_Time = reader.ReadByte();
            }
        }
    }

    internal static class CSE_Shape
    {
        internal static void ReadBasic<T>(XrFileReader reader, T section) where T : ICSE_Shape
        {
            section.Shapes_Count = reader.ReadByte();
            section.Shapes = reader.ReadShapes(section.Shapes_Count);
        }
        internal static void ReadUpdate<T>(XrFileReader reader, T section) where T : ICSE_Shape
        {

        }
    }

    internal static class CSE_ALife_Space_Restrictor
    {
        internal static void ReadBasic<T>(XrFileReader reader, T section) where T : ICSE_ALife_Space_Restrictor
        {
            section.Spase_Restrictor_Type = reader.ReadByte();
        }
        internal static void ReadUpdate<T>(XrFileReader reader, T section) where T : ICSE_ALife_Space_Restrictor
        {

        }
    }

    internal static class CSE_ALife_Custom_Zone
    {
        internal static void ReadBasic<T>(XrFileReader reader, T section) where T : ICSE_ALife_Custom_Zone
        {
            section.Max_Power = reader.ReadFloat();
            section.Owner_ID = reader.ReadUInt32();
            section.Enabled_Time = reader.ReadUInt32();
            section.Disabled_Time = reader.ReadUInt32();
            section.Start_Time_Shift = reader.ReadUInt32();
        }
        internal static void ReadUpdate<T>(XrFileReader reader, T section) where T : ICSE_ALife_Custom_Zone
        {

        }
    }

    internal static class CSE_Motion
    {
        internal static void ReadBasic<T>(XrFileReader reader, T section) where T : ICSE_Motion
        {
            section.Motion_Name = reader.ReadStringZ();
        }
        internal static void ReadUpdate<T>(XrFileReader reader, T section) where T : ICSE_Motion
        {

        }
    }

    internal static class CSE_ALife_Item_Weapon
    {
        internal static void ReadBasic<T>(XrFileReader reader, T section) where T : ICSE_ALife_Item_Weapon
        {
            section.Current = reader.ReadUInt16();
            section.Elapsed = reader.ReadUInt16();
            section.WPN_State = reader.ReadByte();
            section.Addon_Flags = reader.ReadByte();
            section.Ammo_Type = reader.ReadByte();
            section.Elapsed_Grenades = reader.ReadByte();
        }
        internal static void ReadUpdate<T>(XrFileReader reader, T section) where T : ICSE_ALife_Item_Weapon
        {
            section.UPD_Condition = reader.ReadByte();
            section.UPD_WPN_Flags = reader.ReadByte();
            section.UPD_Elapsed = reader.ReadUInt16();
            section.UPD_Flags = reader.ReadByte();
            section.UPD_Ammo_Type = reader.ReadByte();
            section.UPD_WPN_State = reader.ReadByte();
            section.UPD_Zoom = reader.ReadByte();
        }
    }

    internal static class CSE_ALife_Item_Weapon_Magazined
    {
        internal static void ReadBasic<T>(XrFileReader reader, T section) where T : ICSE_ALife_Item_Weapon_Magazined
        {
            
        }
        internal static void ReadUpdate<T>(XrFileReader reader, T section) where T : ICSE_ALife_Item_Weapon_Magazined
        {
            section.UPD_Current_Fire_Mode = reader.ReadByte();
        }
    }
}
