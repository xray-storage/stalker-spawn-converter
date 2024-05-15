using System.Numerics;

using SpawnConverter.Types;
using SpawnConverter.FStream;
using SpawnConverter.Converters.Spawns.Interfaces;
using SpawnConverter.Converters.Spawns.Templates;

namespace SpawnConverter.Converters.Spawns.Classes
{
    public class Ammo : Template_Section, ICSE_ALife_Dynamic_Object_Visual, ICSE_ALife_Inventory_Item
    {
        // =============== BASIC ===============
        // CSE_ALife_Dynamic_Object_Visual
        public string Visual { get; set; }
        public byte Visual_Flags { get; set; }

        // CSE_ALife_Inventory_Item
        public float Condition { get; set; }
        public Vector3u32str Upgrades { get; set; }

        // CSE_Alife_Item_Ammo
        public ushort Elapsed { get; set; }


        // =============== UPDATE ==============
        // CSE_ALife_Inventory_Item
        public byte UPD_Num_Item { get; set; }
        public Vector3 UPD_State_Force { get; set; }
        public Vector3 UPD_State_Torque { get; set; }
        public Vector3 UPD_State_Position { get; set; }
        public Quaternion UPD_State_Quaternion { get; set; }
        public Vector3 UPD_State_Angular_Velocity { get; set; }
        public Vector3 UPD_State_Linear_Velocity { get; set; }
        public byte UPD_Freeze_Time { get; set; }
        
        // CSE_Alife_Item_Ammo
        public ushort UPD_Elapsed { get; set; }


        public Ammo(XrFileReader reader) : base(reader) => TypeName = GetType().Name;


        private protected override void ReadBasic(XrFileReader reader)
        {
            CSE_ALife_Dynamic_Object_Visual.ReadBasic(reader, this);
            CSE_ALife_Inventory_Item.ReadBasic(reader, this);
            Elapsed = reader.ReadUInt16();
        }
        private protected override void ReadUpdate(XrFileReader reader)
        {
            if (reader.ChunkReamains() > 0)
            {
                CSE_ALife_Dynamic_Object_Visual.ReadUpdate(reader, this);
                CSE_ALife_Inventory_Item.ReadUpdate(reader, this);
                UPD_Elapsed = reader.ReadUInt16();
            }
        }
    }
}
