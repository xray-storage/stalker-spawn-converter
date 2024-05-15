using System.Numerics;

using SpawnConverter.FStream;
using SpawnConverter.Converters.Spawns.Interfaces;
using SpawnConverter.Converters.Spawns.Templates;

namespace SpawnConverter.Converters.Spawns.Classes
{
    public class Physic_Object : Template_Section, ICSE_ALife_Dynamic_Object_Visual, ICSE_PH_Skeleton
    {
        // =============== BASIC ===============
        // CSE_ALife_Dynamic_Object_Visual
        public string Visual { get; set; }
        public byte Visual_Flags { get; set; }

        // CSE_PH_Skeleton
        public string Startup_Animation { get; set; }
        public byte Skeleton_Flags { get; set; }
        public ushort Source_ID { get; set; }

        // CSE_ALife_Object_Physic
        public uint Type { get; set; }
        public float Mass { get; set; }
        public string Fixed_Bones { get; set; }

        // =============== UPDATE ==============
        // CSE_ALife_Object_Physic
        public byte UPD_Num_Item { get; set; }
        public Vector3 UPD_State_Force { get; set; }
        public Vector3 UPD_State_Torque { get; set; }
        public Vector3 UPD_State_Position { get; set; }
        public Quaternion UPD_State_Quaternion { get; set; }
        public Vector3 UPD_State_Angular_Velocity { get; set; }
        public Vector3 UPD_State_Linear_Velocity { get; set; }
        public byte UPD_Freeze_Time { get; set; }

        public Physic_Object(XrFileReader reader) : base(reader) => TypeName = GetType().Name;

        private protected override void ReadBasic(XrFileReader reader)
        {
            CSE_ALife_Dynamic_Object_Visual.ReadBasic(reader, this);
            CSE_PH_Skeleton.ReadBasic(reader, this);

            Type = reader.ReadUInt32();
            Mass = reader.ReadFloat();
            Fixed_Bones = reader.ReadStringZ();
        }
        private protected override void ReadUpdate(XrFileReader reader)
        {
            if (reader.ChunkReamains() > 0)
            {
                CSE_ALife_Dynamic_Object_Visual.ReadUpdate(reader, this);
                CSE_PH_Skeleton.ReadUpdate(reader, this);

                UPD_Num_Item = reader.ReadByte();

                if (UPD_Num_Item > 0)
                {
                    UPD_State_Force = reader.ReadVector();
                    UPD_State_Torque = reader.ReadVector();
                    UPD_State_Position = reader.ReadVector();
                    UPD_State_Quaternion = reader.ReadQuaternion();

                    if (((UPD_Num_Item >> 5) & (byte)Vel.NoAngular) == 0)
                        UPD_State_Angular_Velocity = reader.ReadVector();

                    if (((UPD_Num_Item >> 5) & (byte)Vel.NoLinear) == 0)
                        UPD_State_Linear_Velocity = reader.ReadVector();

                    UPD_Freeze_Time = reader.ReadByte();
                }
            }
        }
    }
}
