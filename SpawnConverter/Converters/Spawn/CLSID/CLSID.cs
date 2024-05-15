using SpawnConverter.FStream;
using SpawnConverter.Converters.Spawns.Classes;
using SpawnConverter.Converters.Spawns.Interfaces;

namespace SpawnConverter.Converters.Spawns.Clsid
{
    internal static class CLSID
    {
        internal static ISection GetSectionByClsID(XrFileReader reader, string clsid) 
        {
            return clsid switch
            {
                "S_ACTOR"   => new Actor(reader),               // actor
                "SPC_RS_S"  => new Space_Restrictor(reader),    // space_restrictor 
                "O_BRKBL"   => new Breakable_Object(reader),    // breakable_object
                "O_CLMBL"   => new Climable_Object(reader),     // climable_object
                "Z_CFIRE"   => new Anomalous_Zone(reader),      // campfire
                "SCRIPTZN"  => new Space_Restrictor(reader),    // script_zone
                "S_INVBOX"  => new Inventory_Box(reader),       // Inventory Box
                "LVL_CHNG"  => new Level_Changer(reader),       // level_changer
                "SO_HLAMP"  => new Hanging_Lamp(reader),        // lights_hanging_lamp
                "AI_TRD_S"  => new M_Trader(reader),            // m_trader
                "II_FOOD"   => new Item(reader),                // booster
                "S_FOOD"    => new Item(reader),                // booster
                "O_PHYSIC"  => new Physic_Object(reader),       // physic_object
                "P_DSTRBL"  => new Physic_Object(reader),       // physic_destroyable_object
                "O_DSTR_S"  => new Physic_Object(reader),       // physic_destroyable_object
                "O_PHYS_S"  => new Physic_Object(reader),       // awr_physic_object
                "SMRT_C_S"  => new Smart_Cover(reader),         // smart_cover
                "SMRTTRRN"  => new Smart_Terrain(reader),       // smart_terrain
                "O_SEARCH"  => new Search_Light(reader),        // search_light
                "Z_MBALD"   => new Anomalous_Zone(reader),      // anomalies_zones
                "Z_TORRID"  => new Torrid_Zone(reader),         // anomalies_zones
                "Z_RADIO"   => new Anomalous_Zone(reader),      // anomalies_zones
                "ZS_NGRAV"  => new Anomalous_Zone(reader),      // anomalies_zones
                "ZS_BFUZZ"  => new Zone_Visual(reader),         // anomalies_zones
                "ZS_MBALD"  => new Anomalous_Zone(reader),      // anomalies_zones
                "ZS_RADIO"  => new Anomalous_Zone(reader),      // anomalies_zones
                "ZS_GALAN"  => new Anomalous_Zone(reader),      // anomalies_zones
                "ZS_MINCE"  => new Anomalous_Zone(reader),      // anomalies_zones
                "ZS_TORRD"  => new Torrid_Zone(reader),         // anomalies_zones
                "II_ATTCH"  => new Item(reader),                // attachable_item
                "SCRPTCAR"  => new M_Car(reader),               // m_car
                "AMMO_S"    => new Ammo(reader),                // ammo
                "S_OG7B"    => new Ammo(reader),                // ammo
                "S_VOG25"   => new Ammo(reader),                // ammo
                "S_M209"    => new Ammo(reader),                // ammo
                "WP_RG6"    => new Weapon_ShotGun(reader),      // weapons
                "WP_PM"     => new Weapon(reader),              // weapons
                "WP_AK74"   => new Weapon_WGL(reader),          // weapons
                "WP_LR300"  => new Weapon(reader),              // weapons
                "WP_RPG7"   => new Weapon(reader),              // weapons
                "WP_ASHTG"  => new Weapon_ShotGun(reader),      // weapons
                "WP_BM16"   => new Weapon_ShotGun(reader),      // weapons
                "WP_SVD"    => new Weapon(reader),              // weapons
                "WP_SVU"    => new Weapon(reader),              // weapons
                "WP_HPSA"   => new Weapon(reader),              // weapons
                "WP_VAL"    => new Weapon(reader),              // weapons
                "WP_BINOC"  => new Weapon(reader),              // binoculars
                "WP_SCOPE"  => new Item(reader),                // weapon_addons
                "WP_SILEN"  => new Item(reader),                // weapon_addons
                "WP_GLAUN"  => new Item(reader),                // weapon_addons
                "G_F1"      => new Item(reader),                // greanade
                "G_F1_S"    => new Item(reader),                // greanade
                "G_RGD5_S"  => new Item(reader),                // greanade
                "C_HLCP_S"  => new Helicopter(reader),          // helicopter
                "EQU_STLK"  => new Outfit(reader),              // outfit
                "E_STLK"    => new Outfit(reader),              // outfit
                "E_HLMET"   => new Outfit(reader),              // helmet
                "DET_SIMP"  => new Item(reader),                // detector
                "DET_ELIT"  => new Item(reader),                // detector
                "S_PDA"     => new PDA(reader),                 // pda
                "D_PDA"     => new PDA(reader),                 // pda
                "SCRPTART"  => new Item(reader),                // artefacts
                "ARTEFACT"  => new Item(reader),                // artefacts
                "S_EXPLO"   => new Item(reader),                // explosive
                "II_EXPLO"  => new Item(reader),                // explosive
                _           => null,                            // unknown
            };
        }
    }
}
