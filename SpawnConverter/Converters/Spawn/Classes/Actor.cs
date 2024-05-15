using System.Numerics;

using SpawnConverter.Types;
using SpawnConverter.FStream;
using SpawnConverter.Converters.Spawns.Interfaces;
using SpawnConverter.Converters.Spawns.Templates;

namespace SpawnConverter.Converters.Spawns.Classes
{
    public class Actor : Template_Section, ICSE_ALife_Dynamic_Object_Visual, ICSE_Alife_Creature_Abstract, ICSE_Alife_Trader_Abstract, ICSE_PH_Skeleton
    {
        // =============== BASIC ===============
        // CSE_ALife_Dynamic_Object_Visual
        public string Visual { get; set; }
        public byte Visual_Flags { get; set; }

        // CSE_Alife_Creature_Abstract
        public byte Team { get; set; }
        public byte Squad { get; set; }
        public byte Group { get; set; }
        public float Health { get; set; }
        public Vector2s16 Dynamic_OUT_Restrictions { get; set; }
        public Vector2s16 Dynamic_IN_Restrictions { get; set; }
        public ushort Killer_ID { get; set; }
        public ulong Game_Death_Time { get; set; }

        // CSE_Alife_Trader_Abstract
        public uint Money { get; set; }
        public string Specific_Character { get; set; }
        public uint Trader_Flags { get; set; }
        public string Character_Profile { get; set; }
        public int Community_Index { get; set; }
        public int Rank { get; set; }
        public int Reputation { get; set; }
        public string Character_Name { get; set; }
        public byte Deadbody_Can_Take { get; set; }
        public byte Deadbody_Closed { get; set; }

        // CSE_PH_Skeleton
        public string Startup_Animation { get; set; }
        public byte Skeleton_Flags { get; set; }
        public ushort Source_ID { get; set; }

        // CSE_Alife_Creature_Actor
        public ushort Holder_ID { get; set; }

        // se_actor
        public byte? Save_Marker_Start { get; set; }
        public byte? Start_Position_Filled { get; set; }
        public byte? Save_Marker_End { get; set; }


        // =============== UPDATE ==============
        // CSE_Alife_Creature_Abstract
        public float UPD_Health { get; set; }
        public uint UPD_TimeStamp { get; set; }
        public byte UPD_Flags { get; set; }
        public Vector3 UPD_Position { get; set; }
        public float UPD_Model { get; set; }
        public Vector3 UPD_Torso { get; set; }
        public byte UPD_Team { get; set; }
        public byte UPD_Squad { get; set; }
        public byte UPD_Group { get; set; }

        // CSE_Alife_Creature_Actor
        public ushort UPD_MState { get; set; }
        public Vector3s16 UPD_Accel { get; set; }
        public Vector3s16 UPD_Velocity { get; set; }
        public float UPD_Radiation { get; set; }
        public byte UPD_Weapon { get; set; }
        public ushort UPD_Num_Items { get; set; }


        public Actor(XrFileReader reader) : base(reader) => TypeName = GetType().Name;


        private protected override void ReadBasic(XrFileReader reader)
        {
            CSE_ALife_Dynamic_Object_Visual.ReadBasic(reader, this);
            CSE_Alife_Creature_Abstract.ReadBasic(reader, this);
            CSE_Alife_Trader_Abstract.ReadBasic(reader, this);
            CSE_PH_Skeleton.ReadBasic(reader, this);
            
            Holder_ID = reader.ReadUInt16();
            
            if (reader.ChunkReamains() > 0)
            {
                Save_Marker_Start = reader.ReadByte();
                Start_Position_Filled = reader.ReadByte();
                Save_Marker_End = reader.ReadByte();
            }
        }
        private protected override void ReadUpdate(XrFileReader reader)
        {
            if (reader.ChunkReamains() == 0)
            {
                return;
            }

            CSE_ALife_Dynamic_Object_Visual.ReadUpdate(reader, this);
            CSE_Alife_Creature_Abstract.ReadUpdate(reader, this);
            CSE_Alife_Trader_Abstract.ReadUpdate(reader, this);
            CSE_PH_Skeleton.ReadUpdate(reader, this);

            UPD_MState = reader.ReadUInt16();
            UPD_Accel = reader.ReadVector3s16();
            UPD_Velocity = reader.ReadVector3s16();
            UPD_Radiation = reader.ReadFloat();
            UPD_Weapon = reader.ReadByte();
            UPD_Num_Items = reader.ReadUInt16();
        }
    }
}
