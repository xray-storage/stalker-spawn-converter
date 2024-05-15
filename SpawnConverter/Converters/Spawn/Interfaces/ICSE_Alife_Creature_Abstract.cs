using System.Numerics;
using SpawnConverter.Types;

namespace SpawnConverter.Converters.Spawns.Interfaces
{
    // CSE_Alife_Creature_Abstract
    public interface ICSE_Alife_Creature_Abstract
    {
        public byte Team { get; set; }
        public byte Squad { get; set; }
        public byte Group { get; set; }
        public float Health { get; set; }
        public Vector2s16 Dynamic_OUT_Restrictions { get; set; }
        public Vector2s16 Dynamic_IN_Restrictions { get; set; }
        public ushort Killer_ID { get; set; }
        public ulong Game_Death_Time { get; set; }

        public float UPD_Health { get; set; }
        public uint UPD_TimeStamp { get; set; }
        public byte UPD_Flags { get; set; }
        public Vector3 UPD_Position { get; set; }
        public float UPD_Model { get; set; }
        public Vector3 UPD_Torso { get; set; }
        public byte UPD_Team { get; set; }
        public byte UPD_Squad { get; set; }
        public byte UPD_Group { get; set; }
    }
}
