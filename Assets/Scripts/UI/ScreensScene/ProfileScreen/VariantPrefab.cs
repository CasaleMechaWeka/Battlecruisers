using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Scenes;
using BattleCruisers.Utils.Fetchers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.ProfileScreen
{
    public class VariantPrefab : Prefab, IVariantPrefab
    {
        public Sprite _variantSprite;
        public Sprite variantSprite => _variantSprite;

        public ParentBuildable _parent;
        public ParentBuildable parent
        {
            get { return _parent; }
            set
            {
                _parent = value;
            }
        }

        public VariantType _variantType;
        public VariantType variantType => _variantType;
        public StatVariant statVariant;

        public string GetParentName(IPrefabFactory prefabFactory)
        {
            return IsUnit() ? GetUnit(prefabFactory).Name : GetBuilding(prefabFactory).Name;
        }

        public IBuilding GetBuilding(IPrefabFactory prefabFactory)
        {
            IBuilding building = null;
            if (prefabFactory != null)
            {
                building = prefabFactory.GetBuildingWrapperPrefab(GetPrefabKey()).Buildable;
            }
            return building;
        }

        public IPvPBuilding GetBuilding(IPvPPrefabFactory prefabFactory)
        {
            IPvPBuilding building = null;
            if (prefabFactory != null)
            {
                building = prefabFactory.GetBuildingWrapperPrefab(GetPvPPrefabKey()).Buildable;
            }
            return building;
        }

        public IUnit GetUnit(IPrefabFactory prefabFactory)
        {
            IUnit unit = null;
            if (prefabFactory != null)
            {
                unit = prefabFactory.GetUnitWrapperPrefab(GetPrefabKey()).Buildable;
            }
            return unit;
        }

        public IPvPUnit GetUnit(IPvPPrefabFactory prefabFactory)
        {
            IPvPUnit unit = null;
            if (prefabFactory != null)
            {
                unit = prefabFactory.GetUnitWrapperPrefab(GetPvPPrefabKey()).Buildable;
            }
            return unit;
        }

        public IPrefabKey GetPrefabKey()
        {
            switch (_parent)
            {
                case ParentBuildable.AirFactory:
                    return new BuildingKey(BuildingCategory.Factory, "AirFactory");
                case ParentBuildable.NavalFactory:
                    return new BuildingKey(BuildingCategory.Factory, "NavalFactory");
                case ParentBuildable.EngineeringBay:
                    return new BuildingKey(BuildingCategory.Factory, "EngineeringBay");
                case ParentBuildable.EngineeringBay4:
                    return new BuildingKey(BuildingCategory.Factory, "EngineeringBay4");
                case ParentBuildable.EngineeringBay8:
                    return new BuildingKey(BuildingCategory.Factory, "EngineeringBay8");
                case ParentBuildable.ShieldGenerator:
                    return new BuildingKey(BuildingCategory.Tactical, "ShieldGenerator");
                case ParentBuildable.StealthGenerator:
                    return new BuildingKey(BuildingCategory.Tactical, "StealthGenerator");
                case ParentBuildable.SpySatelliteLauncher:
                    return new BuildingKey(BuildingCategory.Tactical, "SpySatelliteLauncher");
                case ParentBuildable.LocalBooster:
                    return new BuildingKey(BuildingCategory.Tactical, "LocalBooster");
                case ParentBuildable.ControlTower:
                    return new BuildingKey(BuildingCategory.Tactical, "ControlTower");
                case ParentBuildable.AntiShipTurret:
                    return new BuildingKey(BuildingCategory.Defence, "AntiShipTurret");
                case ParentBuildable.AntiAirTurret:
                    return new BuildingKey(BuildingCategory.Defence, "AntiAirTurret");
                case ParentBuildable.Mortar:
                    return new BuildingKey(BuildingCategory.Defence, "Mortar");
                case ParentBuildable.SamSite:
                    return new BuildingKey(BuildingCategory.Defence, "SamSite");
                case ParentBuildable.TeslaCoil:
                    return new BuildingKey(BuildingCategory.Defence, "TeslaCoil");
                case ParentBuildable.Coastguard:
                    return new BuildingKey(BuildingCategory.Defence, "Coastguard");
                case ParentBuildable.Artillery:
                    return new BuildingKey(BuildingCategory.Offence, "Artillery");
                case ParentBuildable.RocketLauncher:
                    return new BuildingKey(BuildingCategory.Offence, "RocketLauncher");
                case ParentBuildable.Railgun:
                    return new BuildingKey(BuildingCategory.Offence, "Railgun");
                case ParentBuildable.MLRS:
                    return new BuildingKey(BuildingCategory.Offence, "MLRS");
                case ParentBuildable.GatlingMortar:
                    return new BuildingKey(BuildingCategory.Offence, "GatlingMortar");
                case ParentBuildable.IonCannon:
                    return new BuildingKey(BuildingCategory.Offence, "IonCannon");
                case ParentBuildable.MissilePod:
                    return new BuildingKey(BuildingCategory.Offence, "MissilePod");
                case ParentBuildable.Bomber:
                    return new UnitKey(UnitCategory.Aircraft, "Bomber");
                case ParentBuildable.Fighter:
                    return new UnitKey(UnitCategory.Aircraft, "Fighter");
                case ParentBuildable.Gunship:
                    return new UnitKey(UnitCategory.Aircraft, "Gunship");
                case ParentBuildable.SteamCopter:
                    return new UnitKey(UnitCategory.Aircraft, "SteamCopter");
                case ParentBuildable.Broadsword:
                    return new UnitKey(UnitCategory.Aircraft, "Broadsword");
                case ParentBuildable.AttackBoat:
                    return new UnitKey(UnitCategory.Naval, "AttackBoat");
                case ParentBuildable.AttackRIB:
                    return new UnitKey(UnitCategory.Naval, "AttackRIB");
                case ParentBuildable.Frigate:
                    return new UnitKey(UnitCategory.Naval, "Frigate");
                case ParentBuildable.Destroyer:
                    return new UnitKey(UnitCategory.Naval, "Destroyer");
                case ParentBuildable.ArchonBattleship:
                    return new UnitKey(UnitCategory.Naval, "ArchonBattleship");
            }
            return null;
        }


        public IPvPPrefabKey GetPvPPrefabKey()
        {
            switch (_parent)
            {
                case ParentBuildable.AirFactory:
                    return new PvPBuildingKey(PvPBuildingCategory.Factory, "PvPAirFactory");
                case ParentBuildable.NavalFactory:
                    return new PvPBuildingKey(PvPBuildingCategory.Factory, "PvPNavalFactory");
                case ParentBuildable.EngineeringBay:
                    return new PvPBuildingKey(PvPBuildingCategory.Factory, "PvPEngineeringBay");
                case ParentBuildable.EngineeringBay4:
                    return new PvPBuildingKey(PvPBuildingCategory.Factory, "PvPEngineeringBay4");
                case ParentBuildable.EngineeringBay8:
                    return new PvPBuildingKey(PvPBuildingCategory.Factory, "PvPEngineeringBay8");
                case ParentBuildable.ShieldGenerator:
                    return new PvPBuildingKey(PvPBuildingCategory.Tactical, "PvPShieldGenerator");
                case ParentBuildable.StealthGenerator:
                    return new PvPBuildingKey(PvPBuildingCategory.Tactical, "PvPStealthGenerator");
                case ParentBuildable.SpySatelliteLauncher:
                    return new PvPBuildingKey(PvPBuildingCategory.Tactical, "PvPSpySatelliteLauncher");
                case ParentBuildable.LocalBooster:
                    return new PvPBuildingKey(PvPBuildingCategory.Tactical, "PvPLocalBooster");
                case ParentBuildable.ControlTower:
                    return new PvPBuildingKey(PvPBuildingCategory.Tactical, "PvPControlTower");
                case ParentBuildable.AntiShipTurret:
                    return new PvPBuildingKey(PvPBuildingCategory.Defence, "PvPAntiShipTurret");
                case ParentBuildable.AntiAirTurret:
                    return new PvPBuildingKey(PvPBuildingCategory.Defence, "PvPAntiAirTurret");
                case ParentBuildable.Mortar:
                    return new PvPBuildingKey(PvPBuildingCategory.Defence, "PvPMortar");
                case ParentBuildable.SamSite:
                    return new PvPBuildingKey(PvPBuildingCategory.Defence, "PvPSamSite");
                case ParentBuildable.TeslaCoil:
                    return new PvPBuildingKey(PvPBuildingCategory.Defence, "PvPTeslaCoil");
                case ParentBuildable.Coastguard:
                    return new PvPBuildingKey(PvPBuildingCategory.Defence, "PvPCoastguard");
                case ParentBuildable.Artillery:
                    return new PvPBuildingKey(PvPBuildingCategory.Offence, "PvPArtillery");
                case ParentBuildable.RocketLauncher:
                    return new PvPBuildingKey(PvPBuildingCategory.Offence, "PvPRocketLauncher");
                case ParentBuildable.Railgun:
                    return new PvPBuildingKey(PvPBuildingCategory.Offence, "PvPRailgun");
                case ParentBuildable.MLRS:
                    return new PvPBuildingKey(PvPBuildingCategory.Offence, "PvPMLRS");
                case ParentBuildable.GatlingMortar:
                    return new PvPBuildingKey(PvPBuildingCategory.Offence, "PvPGatlingMortar");
                case ParentBuildable.IonCannon:
                    return new PvPBuildingKey(PvPBuildingCategory.Offence, "PvPIonCannon");
                case ParentBuildable.MissilePod:
                    return new PvPBuildingKey(PvPBuildingCategory.Offence, "PvPMissilePod");
                case ParentBuildable.Bomber:
                    return new PvPUnitKey(PvPUnitCategory.Aircraft, "PvPBomber");
                case ParentBuildable.Fighter:
                    return new PvPUnitKey(PvPUnitCategory.Aircraft, "PvPFighter");
                case ParentBuildable.Gunship:
                    return new PvPUnitKey(PvPUnitCategory.Aircraft, "PvPGunship");
                case ParentBuildable.SteamCopter:
                    return new PvPUnitKey(PvPUnitCategory.Aircraft, "PvPSteamCopter");
                case ParentBuildable.Broadsword:
                    return new PvPUnitKey(PvPUnitCategory.Aircraft, "PvPBroadsword");
                case ParentBuildable.AttackBoat:
                    return new PvPUnitKey(PvPUnitCategory.Naval, "PvPAttackBoat");
                case ParentBuildable.AttackRIB:
                    return new PvPUnitKey(PvPUnitCategory.Naval, "PvPAttackRIB");
                case ParentBuildable.Frigate:
                    return new PvPUnitKey(PvPUnitCategory.Naval, "PvPFrigate");
                case ParentBuildable.Destroyer:
                    return new PvPUnitKey(PvPUnitCategory.Naval, "PvPDestroyer");
                case ParentBuildable.ArchonBattleship:
                    return new PvPUnitKey(PvPUnitCategory.Naval, "PvPArchonBattleship");
            }
            return null;
        }
        public bool IsUnit()
        {
            bool ret = false;
            switch (_parent)
            {
                case ParentBuildable.AirFactory:
                    ret = false;
                    break;
                case ParentBuildable.NavalFactory:
                    ret = false;
                    break;
                case ParentBuildable.EngineeringBay:
                    ret = false;
                    break;
                case ParentBuildable.EngineeringBay4:
                    ret = false;
                    break;
                case ParentBuildable.EngineeringBay8:
                    ret = false;
                    break;
                case ParentBuildable.ShieldGenerator:
                    ret = false;
                    break;
                case ParentBuildable.StealthGenerator:
                    ret = false;
                    break;
                case ParentBuildable.SpySatelliteLauncher:
                    ret = false;
                    break;
                case ParentBuildable.LocalBooster:
                    ret = false;
                    break;
                case ParentBuildable.ControlTower:
                    ret = false;
                    break;
                case ParentBuildable.AntiShipTurret:
                    ret = false;
                    break;
                case ParentBuildable.AntiAirTurret:
                    ret = false;
                    break;
                case ParentBuildable.Mortar:
                    ret = false;
                    break;
                case ParentBuildable.SamSite:
                    ret = false;
                    break;
                case ParentBuildable.TeslaCoil:
                    ret = false;
                    break;
                case ParentBuildable.Coastguard:
                    ret = false;
                    break;
                case ParentBuildable.Artillery:
                    ret = false;
                    break;
                case ParentBuildable.RocketLauncher:
                    ret = false;
                    break;
                case ParentBuildable.Railgun:
                    ret = false;
                    break;
                case ParentBuildable.MLRS:
                    ret = false;
                    break;
                case ParentBuildable.GatlingMortar:
                    ret = false;
                    break;
                case ParentBuildable.IonCannon:
                    ret = false;
                    break;
                case ParentBuildable.MissilePod:
                    ret = false;
                    break;
                case ParentBuildable.Bomber:
                    ret = true;
                    break;
                case ParentBuildable.Fighter:
                    ret = true;
                    break;
                case ParentBuildable.Gunship:
                    ret = true;
                    break;
                case ParentBuildable.SteamCopter:
                    ret = true;
                    break;
                case ParentBuildable.Broadsword:
                    ret = true;
                    break;
                case ParentBuildable.AttackBoat:
                    ret = true;
                    break;
                case ParentBuildable.AttackRIB:
                    ret = true;
                    break;
                case ParentBuildable.Frigate:
                    ret = true;
                    break;
                case ParentBuildable.Destroyer:
                    ret = true;
                    break;
                case ParentBuildable.ArchonBattleship:
                    ret = true;
                    break;
            }
            return ret;
        }
    }

    [Serializable]
    public class StatVariant
    {
        [Header("Building Stats")]
        public int drone_num;
        public float build_time;
        public float max_health;

        [Header("Turret Stats")]
        public float fire_rate;
        public float range;
        public float min_range;
        public float accuracy;
        public float rotate_speed;
        public int burst_fire_rate;
        public int burst_size;
        public float laser_duration;
        public float damagePerS; // cannon

        [Header("Projectile Stats")]
        public float initial_velocity_multiplier;
        public float damage;
        public float max_velocity;
        public float gravity_scale;
        public float damage_radius;
        public float detection_range;
        public float cruising_altitude;

        [Header("Booster/ControlTower Stats")]
        public float boost_multiplier;

        [Header("Shield Stats")]
        public float shield_recharge_delay;
        public float shield_recharge_rate;
        public float shield_health;
    }
}
