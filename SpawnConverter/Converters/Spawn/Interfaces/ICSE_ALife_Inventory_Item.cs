using System.Numerics;
using SpawnConverter.Types;

namespace SpawnConverter.Converters.Spawns.Interfaces
{
    // CSE_ALife_Inventory_Item
    public interface ICSE_ALife_Inventory_Item
    {
        public float Condition { get; set; }
        public Vector3u32str Upgrades { get; set; }

        public byte UPD_Num_Item { get; set; }
        public Vector3 UPD_State_Force { get; set; }
        public Vector3 UPD_State_Torque { get; set; }
        public Vector3 UPD_State_Position { get; set; }
        public Quaternion UPD_State_Quaternion { get; set; }
        public Vector3 UPD_State_Angular_Velocity { get; set; }
        public Vector3 UPD_State_Linear_Velocity { get; set; }
        public byte UPD_Freeze_Time { get; set; }
    }
}
