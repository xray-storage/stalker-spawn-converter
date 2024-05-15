using SpawnConverter.FStream;
using SpawnConverter.Converters.Spawns.Interfaces;
using SpawnConverter.Converters.Spawns.Templates;

namespace SpawnConverter.Converters.Spawns.Classes
{
    public class Inventory_Box : Template_Section, ICSE_ALife_Dynamic_Object_Visual
    {
        // =============== BASIC ===============
        // CSE_ALife_Dynamic_Object_Visual
        public string Visual { get; set; }
        public byte Visual_Flags { get; set; }

        // CSE_ALife_Inventory_Box
        public byte Can_Take { get; set; }
        public byte Closed { get; set; }
        public string Tip_Text { get; set; }

        public Inventory_Box(XrFileReader reader) : base(reader) => TypeName = GetType().Name;

        private protected override void ReadBasic(XrFileReader reader)
        {
            CSE_ALife_Dynamic_Object_Visual.ReadBasic(reader, this);

            Can_Take = reader.ReadByte();
            Closed = reader.ReadByte();
            Tip_Text = reader.ReadStringZ();
        }
    }
}
