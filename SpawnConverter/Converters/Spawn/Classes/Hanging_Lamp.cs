using SpawnConverter.FStream;
using SpawnConverter.Converters.Spawns.Interfaces;
using SpawnConverter.Converters.Spawns.Templates;

namespace SpawnConverter.Converters.Spawns.Classes
{
    public class Hanging_Lamp : Template_Section, ICSE_ALife_Dynamic_Object_Visual, ICSE_PH_Skeleton
    {
        // =============== BASIC ===============
        // CSE_ALife_Dynamic_Object_Visual
        public string Visual { get; set; }
        public byte Visual_Flags { get; set; }

        // CSE_PH_Skeleton
        public string Startup_Animation { get; set; }
        public byte Skeleton_Flags { get; set; }
        public ushort Source_ID { get; set; }

        // CSE_ALife_Object_Hanging_Lamp
        public uint Color { get; set; }
        public float Brightness { get; set; }
        public string Color_Animator { get; set; }
        public float Range { get; set; }
        public ushort HL_Flags { get; set; }
        public string HL_Startup_Animation { get; set; }
        public string Fixed_Bones { get; set; }
        public float Health { get; set; }
        public float Virtual_Size { get; set; }
        public float Ambient_Radius { get; set; }
        public float Ambient_Power { get; set; }
        public string Ambient_Texture { get; set; }
        public string Light_Texture { get; set; }
        public string Light_Main_Bone { get; set; }
        public float Spot_Cone_Angle { get; set; }
        public string Glow_Texture { get; set; }
        public float Glow_Radius { get; set; }
        public string Light_Ambient_Bone { get; set; }
        public float Volumetric_Quality { get; set; }
        public float Volumetric_Intensity { get; set; }
        public float Volumetric_Distance { get; set; }

        public Hanging_Lamp(XrFileReader reader) : base(reader) { }

        private protected override void ReadBasic(XrFileReader reader)
        {
            CSE_ALife_Dynamic_Object_Visual.ReadBasic(reader, this);
            CSE_PH_Skeleton.ReadBasic(reader, this);

            // CSE_ALife_Object_Hanging_Lamp
            Color = reader.ReadUInt32();
            Brightness = reader.ReadFloat();
            Color_Animator = reader.ReadStringZ();
            Range = reader.ReadFloat();
            HL_Flags = reader.ReadUInt16();
            HL_Startup_Animation = reader.ReadStringZ();
            Fixed_Bones = reader.ReadStringZ();
            Health = reader.ReadFloat();
            Virtual_Size = reader.ReadFloat();
            Ambient_Radius = reader.ReadFloat();
            Ambient_Power = reader.ReadFloat();
            Ambient_Texture = reader.ReadStringZ();
            Light_Texture = reader.ReadStringZ();
            Light_Main_Bone = reader.ReadStringZ();
            Spot_Cone_Angle = reader.ReadFloat();
            Glow_Texture = reader.ReadStringZ();
            Glow_Radius = reader.ReadFloat();
            Light_Ambient_Bone = reader.ReadStringZ();
            Volumetric_Quality = reader.ReadFloat();
            Volumetric_Intensity = reader.ReadFloat();
            Volumetric_Distance = reader.ReadFloat();
        }
    }
}
