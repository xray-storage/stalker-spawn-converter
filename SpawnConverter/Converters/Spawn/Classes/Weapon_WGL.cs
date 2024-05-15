using System.Numerics;

using SpawnConverter.Types;
using SpawnConverter.FStream;
using SpawnConverter.Converters.Spawns.Interfaces;
using SpawnConverter.Converters.Spawns.Templates;

namespace SpawnConverter.Converters.Spawns.Classes
{
    public class Weapon_WGL : Template_Section, ICSE_ALife_Dynamic_Object_Visual, ICSE_ALife_Inventory_Item, ICSE_ALife_Item_Weapon, ICSE_ALife_Item_Weapon_Magazined
    {
        // =============== BASIC ===============
        // CSE_ALife_Dynamic_Object_Visual
        public string Visual { get; set; }
        public byte Visual_Flags { get; set; }

        // CSE_ALife_Inventory_Item
        public float Condition { get; set; }
        public Vector3u32str Upgrades { get; set; }

        // CSE_ALife_Item_Weapon
        public ushort Current { get; set; }
        public ushort Elapsed { get; set; }
        public byte WPN_State { get; set; }
        public byte Addon_Flags { get; set; }
        public byte Ammo_Type { get; set; }
        public byte Elapsed_Grenades { get; set; }

        // =============== UPDATE ==============
        // CSE_ALife_Item_Weapon_Magazined_WGL
        public byte UPD_Grenade_Mode { get; set; }

        // CSE_ALife_Inventory_Item
        public byte UPD_Num_Item { get; set; }
        public Vector3 UPD_State_Force { get; set; }
        public Vector3 UPD_State_Torque { get; set; }
        public Vector3 UPD_State_Position { get; set; }
        public Quaternion UPD_State_Quaternion { get; set; }
        public Vector3 UPD_State_Angular_Velocity { get; set; }
        public Vector3 UPD_State_Linear_Velocity { get; set; }
        public byte UPD_Freeze_Time { get; set; }

        // CSE_ALife_Item_Weapon
        public byte UPD_Condition { get; set; }
        public byte UPD_WPN_Flags { get; set; }
        public ushort UPD_Elapsed { get; set; }
        public byte UPD_Flags { get; set; }
        public byte UPD_Ammo_Type { get; set; }
        public byte UPD_WPN_State { get; set; }
        public byte UPD_Zoom { get; set; }

        // CSE_ALife_Item_Weapon_Magazined
        public byte UPD_Current_Fire_Mode { get; set; }

        public Weapon_WGL(XrFileReader reader) : base(reader) => TypeName = GetType().Name;

        private protected override void ReadBasic(XrFileReader reader)
        {
            CSE_ALife_Dynamic_Object_Visual.ReadBasic(reader, this);
            CSE_ALife_Inventory_Item.ReadBasic(reader, this);
            CSE_ALife_Item_Weapon.ReadBasic(reader, this);
            CSE_ALife_Item_Weapon_Magazined.ReadBasic(reader, this);
        }
        private protected override void ReadUpdate(XrFileReader reader)
        {
            UPD_Grenade_Mode = reader.ReadByte();

            if (reader.ChunkReamains() > 0)
            {
                CSE_ALife_Dynamic_Object_Visual.ReadUpdate(reader, this);
                CSE_ALife_Inventory_Item.ReadUpdate(reader, this);
                CSE_ALife_Item_Weapon.ReadUpdate(reader, this);
                CSE_ALife_Item_Weapon_Magazined.ReadUpdate(reader, this);
            }
        }
    }
}
