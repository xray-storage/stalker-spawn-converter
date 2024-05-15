using System.Numerics;

using SpawnConverter.Types;
using SpawnConverter.FStream;
using SpawnConverter.Converters.Spawns.Interfaces;
using SpawnConverter.Converters.Spawns.Templates;

namespace SpawnConverter.Converters.Spawns.Classes
{
    public class Item : Template_Section, ICSE_ALife_Dynamic_Object_Visual, ICSE_ALife_Inventory_Item
    {
        // =============== BASIC ===============
        // CSE_ALife_Dynamic_Object_Visual
        public string Visual { get; set; }
        public byte Visual_Flags { get; set; }

        // CSE_ALife_Inventory_Item
        public float Condition { get; set; }
        public Vector3u32str Upgrades { get; set; }

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

        public Item(XrFileReader reader) : base(reader) => TypeName = GetType().Name;

        private protected override void ReadBasic(XrFileReader reader)
        {
            CSE_ALife_Dynamic_Object_Visual.ReadBasic(reader, this);
            CSE_ALife_Inventory_Item.ReadBasic(reader, this);

            // Фикс документов из лаб. Которые могут быть аттач, а не PDA. 4 лишних байта.
            reader.Position += reader.ChunkReamains();
        }

        private protected override void ReadUpdate(XrFileReader reader)
        {
            if (reader.ChunkReamains() > 0)
            {
                CSE_ALife_Dynamic_Object_Visual.ReadUpdate(reader, this);
                CSE_ALife_Inventory_Item.ReadUpdate(reader, this);
            }
        }
    }
}
