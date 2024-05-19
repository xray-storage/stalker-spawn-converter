using SpawnConverter.FStream;
using SpawnConverter.Converters.Spawns.Interfaces;
using SpawnConverter.Converters.Spawns.Templates;

namespace SpawnConverter.Converters.Spawns.Classes
{
    public class M_Car : Template_Section, ICSE_ALife_Dynamic_Object_Visual, ICSE_PH_Skeleton
    {
        // =============== BASIC ===============
        // CSE_Alife_Dynamic_Object_Visual
        public string Visual { get; set; }
        public byte Visual_Flags { get; set; }

        // CSE_PH_Skeleton
        public string Startup_Animation { get; set; }
        public byte Skeleton_Flags { get; set; }
        public ushort Source_ID { get; set; }

        // CSE_ALife_Car
        public float Health { get; set; }

        public M_Car(XrFileReader reader) : base(reader) { }

        private protected override void ReadBasic(XrFileReader reader)
        {
            CSE_ALife_Dynamic_Object_Visual.ReadBasic(reader, this);
            CSE_PH_Skeleton.ReadBasic(reader, this);

            Health = reader.ReadFloat();
        }
    }
}
