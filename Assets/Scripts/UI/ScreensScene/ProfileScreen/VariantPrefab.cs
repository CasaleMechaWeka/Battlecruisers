using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Utils.Fetchers;
using System;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.ProfileScreen
{
    public class VariantPrefab : Prefab, IVariantPrefab
    {
        public int _index;
        public int variantIndex => _index;
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
            if (prefabFactory == null)
            {
                Debug.LogError("PrefabFactory is null");
                return null;
            }

            IPrefabKey buildingKey = GetPrefabKey();
            if (buildingKey == null)
            {
                Debug.LogError($"Building key is null for parent: {_parent}");
                return null;
            }

            var buildingWrapperPrefab = prefabFactory.GetBuildingWrapperPrefab(buildingKey);
            if (buildingWrapperPrefab == null)
            {
                Debug.LogError($"Building wrapper prefab is null for key: {buildingKey}");
                return null;
            }

            return buildingWrapperPrefab.Buildable;
        }


        public IPvPBuilding GetBuilding(IPvPPrefabFactory prefabFactory)
        {
            if (prefabFactory == null)
            {
                Debug.LogError("PrefabFactory is null");
                return null;
            }

            IPvPPrefabKey buildingKey = GetPvPPrefabKey();
            if (buildingKey == null)
            {
                Debug.LogError("PvP Building key is null");
                return null;
            }

            var buildingWrapperPrefab = prefabFactory.GetBuildingWrapperPrefab(buildingKey);
            if (buildingWrapperPrefab == null)
            {
                Debug.LogError($"PvP Building wrapper prefab is null for key: {buildingKey}");
                return null;
            }

            return buildingWrapperPrefab.Buildable;
        }


        public IUnit GetUnit(IPrefabFactory prefabFactory)
        {
            IUnit unit = null;
            if (prefabFactory != null)
                unit = prefabFactory.GetUnitWrapperPrefab(GetPrefabKey()).Buildable;

            return unit;
        }

        public IPvPUnit GetUnit(IPvPPrefabFactory prefabFactory)
        {
            IPvPUnit unit = null;
            if (prefabFactory != null)
                unit = prefabFactory.GetUnitWrapperPrefab(GetPvPPrefabKey()).Buildable;

            return unit;
        }

        public IPrefabKey GetPrefabKey()
        {
            IPrefabKey prefabKey = null;
            switch (_parent)
            {
                // Buildings
                case ParentBuildable.AirFactory:
                    prefabKey = new BuildingKey(BuildingCategory.Factory, "AirFactory");
                    break;
                case ParentBuildable.NavalFactory:
                    prefabKey = new BuildingKey(BuildingCategory.Factory, "NavalFactory");
                    break;
                case ParentBuildable.EngineeringBay:
                    prefabKey = new BuildingKey(BuildingCategory.Factory, "EngineeringBay");
                    break;
                case ParentBuildable.EngineeringBay4:
                    prefabKey = new BuildingKey(BuildingCategory.Factory, "EngineeringBay4");
                    break;
                case ParentBuildable.EngineeringBay8:
                    prefabKey = new BuildingKey(BuildingCategory.Factory, "EngineeringBay8");
                    break;
                case ParentBuildable.ShieldGenerator:
                    prefabKey = new BuildingKey(BuildingCategory.Tactical, "ShieldGenerator");
                    break;
                case ParentBuildable.StealthGenerator:
                    prefabKey = new BuildingKey(BuildingCategory.Tactical, "StealthGenerator");
                    break;
                case ParentBuildable.SpySatelliteLauncher:
                    prefabKey = new BuildingKey(BuildingCategory.Tactical, "SpySatelliteLauncher");
                    break;
                case ParentBuildable.LocalBooster:
                    prefabKey = new BuildingKey(BuildingCategory.Tactical, "LocalBooster");
                    break;
                case ParentBuildable.ControlTower:
                    prefabKey = new BuildingKey(BuildingCategory.Tactical, "ControlTower");
                    break;
                case ParentBuildable.GrapheneBarrier:
                    prefabKey = new BuildingKey(BuildingCategory.Tactical, "GrapheneBarrier");
                    break;
                case ParentBuildable.AntiShipTurret:
                    prefabKey = new BuildingKey(BuildingCategory.Defence, "AntiShipTurret");
                    break;
                case ParentBuildable.AntiAirTurret:
                    prefabKey = new BuildingKey(BuildingCategory.Defence, "AntiAirTurret");
                    break;
                case ParentBuildable.Mortar:
                    prefabKey = new BuildingKey(BuildingCategory.Defence, "Mortar");
                    break;
                case ParentBuildable.SamSite:
                    prefabKey = new BuildingKey(BuildingCategory.Defence, "SamSite");
                    break;
                case ParentBuildable.TeslaCoil:
                    prefabKey = new BuildingKey(BuildingCategory.Defence, "TeslaCoil");
                    break;
                case ParentBuildable.Coastguard:
                    prefabKey = new BuildingKey(BuildingCategory.Defence, "Coastguard");
                    break;
                case ParentBuildable.FlakTurret:
                    prefabKey = new BuildingKey(BuildingCategory.Defence, "FlakTurret");
                    break;
                case ParentBuildable.CIWS:
                    prefabKey = new BuildingKey(BuildingCategory.Defence, "CIWS");
                    break;
                case ParentBuildable.Artillery:
                    prefabKey = new BuildingKey(BuildingCategory.Offence, "Artillery");
                    break;
                case ParentBuildable.RocketLauncher:
                    prefabKey = new BuildingKey(BuildingCategory.Offence, "RocketLauncher");
                    break;
                case ParentBuildable.Railgun:
                    prefabKey = new BuildingKey(BuildingCategory.Offence, "Railgun");
                    break;
                case ParentBuildable.MLRS:
                    prefabKey = new BuildingKey(BuildingCategory.Offence, "MLRS");
                    break;
                case ParentBuildable.GatlingMortar:
                    prefabKey = new BuildingKey(BuildingCategory.Offence, "GatlingMortar");
                    break;
                case ParentBuildable.IonCannon:
                    prefabKey = new BuildingKey(BuildingCategory.Offence, "IonCannon");
                    break;
                case ParentBuildable.MissilePod:
                    prefabKey = new BuildingKey(BuildingCategory.Offence, "MissilePod");
                    break;
                case ParentBuildable.Cannon:
                    prefabKey = new BuildingKey(BuildingCategory.Offence, "Cannon");
                    break;
                case ParentBuildable.BlastVLS:
                    prefabKey = new BuildingKey(BuildingCategory.Offence, "BlastVLS");
                    break;
                case ParentBuildable.FirecrackerVLS:
                    prefabKey = new BuildingKey(BuildingCategory.Offence, "FirecrackerVLS");
                    break;

                // Units
                case ParentBuildable.Bomber:
                    prefabKey = new UnitKey(UnitCategory.Aircraft, "Bomber");
                    break;
                case ParentBuildable.Fighter:
                    prefabKey = new UnitKey(UnitCategory.Aircraft, "Fighter");
                    break;
                case ParentBuildable.Gunship:
                    prefabKey = new UnitKey(UnitCategory.Aircraft, "Gunship");
                    break;
                case ParentBuildable.SteamCopter:
                    prefabKey = new UnitKey(UnitCategory.Aircraft, "SteamCopter");
                    break;
                case ParentBuildable.Broadsword:
                    prefabKey = new UnitKey(UnitCategory.Aircraft, "Broadsword");
                    break;
                case ParentBuildable.StratBomber:
                    prefabKey = new UnitKey(UnitCategory.Aircraft, "StratBomber");
                    break;
                case ParentBuildable.SpyPlane:
                    prefabKey = new UnitKey(UnitCategory.Aircraft, "SpyPlane");
                    break;
                case ParentBuildable.MissileFighter:
                    prefabKey = new UnitKey(UnitCategory.Aircraft, "MissileFighter");
                    break;
                case ParentBuildable.AttackBoat:
                    prefabKey = new UnitKey(UnitCategory.Naval, "AttackBoat");
                    break;
                case ParentBuildable.AttackRIB:
                    prefabKey = new UnitKey(UnitCategory.Naval, "AttackRIB");
                    break;
                case ParentBuildable.Frigate:
                    prefabKey = new UnitKey(UnitCategory.Naval, "Frigate");
                    break;
                case ParentBuildable.Destroyer:
                    prefabKey = new UnitKey(UnitCategory.Naval, "Destroyer");
                    break;
                case ParentBuildable.SiegeDestroyer:
                    prefabKey = new UnitKey(UnitCategory.Naval, "SiegeDestroyer");
                    break;
                case ParentBuildable.ArchonBattleship:
                    prefabKey = new UnitKey(UnitCategory.Naval, "ArchonBattleship");
                    break;
                case ParentBuildable.GlassCannoneer:
                    prefabKey = new UnitKey(UnitCategory.Naval, "GlassCannoneer");
                    break;
                case ParentBuildable.GunBoat:
                    prefabKey = new UnitKey(UnitCategory.Naval, "GunBoat");
                    break;
                case ParentBuildable.RocketTurtle:
                    prefabKey = new UnitKey(UnitCategory.Naval, "RocketTurtle");
                    break;
                case ParentBuildable.FlakTurtle:
                    prefabKey = new UnitKey(UnitCategory.Naval, "FlakTurtle");
                    break;
            }

            if (prefabKey == null)
            {
                Debug.LogError($"Prefab key is null for parent: {_parent}");
            }

            return prefabKey;
        }



        public IPvPPrefabKey GetPvPPrefabKey()
        {
            switch (_parent)
            {
                case ParentBuildable.AirFactory:
                    return new PvPBuildingKey(BuildingCategory.Factory, "PvPAirFactory");
                case ParentBuildable.NavalFactory:
                    return new PvPBuildingKey(BuildingCategory.Factory, "PvPNavalFactory");
                case ParentBuildable.EngineeringBay:
                    return new PvPBuildingKey(BuildingCategory.Factory, "PvPEngineeringBay");
                case ParentBuildable.EngineeringBay4:
                    return new PvPBuildingKey(BuildingCategory.Factory, "PvPEngineeringBay4");
                case ParentBuildable.EngineeringBay8:
                    return new PvPBuildingKey(BuildingCategory.Factory, "PvPEngineeringBay8");
                case ParentBuildable.ShieldGenerator:
                    return new PvPBuildingKey(BuildingCategory.Tactical, "PvPShieldGenerator");
                case ParentBuildable.StealthGenerator:
                    return new PvPBuildingKey(BuildingCategory.Tactical, "PvPStealthGenerator");
                case ParentBuildable.SpySatelliteLauncher:
                    return new PvPBuildingKey(BuildingCategory.Tactical, "PvPSpySatelliteLauncher");
                case ParentBuildable.LocalBooster:
                    return new PvPBuildingKey(BuildingCategory.Tactical, "PvPLocalBooster");
                case ParentBuildable.ControlTower:
                    return new PvPBuildingKey(BuildingCategory.Tactical, "PvPControlTower");
                case ParentBuildable.GrapheneBarrier:
                    return new PvPBuildingKey(BuildingCategory.Tactical, "PvPGrapheneBarrier");
                case ParentBuildable.AntiShipTurret:
                    return new PvPBuildingKey(BuildingCategory.Defence, "PvPAntiShipTurret");
                case ParentBuildable.AntiAirTurret:
                    return new PvPBuildingKey(BuildingCategory.Defence, "PvPAntiAirTurret");
                case ParentBuildable.Mortar:
                    return new PvPBuildingKey(BuildingCategory.Defence, "PvPMortar");
                case ParentBuildable.SamSite:
                    return new PvPBuildingKey(BuildingCategory.Defence, "PvPSamSite");
                case ParentBuildable.TeslaCoil:
                    return new PvPBuildingKey(BuildingCategory.Defence, "PvPTeslaCoil");
                case ParentBuildable.Coastguard:
                    return new PvPBuildingKey(BuildingCategory.Defence, "PvPCoastguard");
                case ParentBuildable.FlakTurret:
                    return new PvPBuildingKey(BuildingCategory.Defence, "PvPFlakTurret");
                case ParentBuildable.CIWS:
                    return new PvPBuildingKey(BuildingCategory.Defence, "PvPCIWS");
                case ParentBuildable.Artillery:
                    return new PvPBuildingKey(BuildingCategory.Offence, "PvPArtillery");
                case ParentBuildable.RocketLauncher:
                    return new PvPBuildingKey(BuildingCategory.Offence, "PvPRocketLauncher");
                case ParentBuildable.Railgun:
                    return new PvPBuildingKey(BuildingCategory.Offence, "PvPRailgun");
                case ParentBuildable.MLRS:
                    return new PvPBuildingKey(BuildingCategory.Offence, "PvPMLRS");
                case ParentBuildable.GatlingMortar:
                    return new PvPBuildingKey(BuildingCategory.Offence, "PvPGatlingMortar");
                case ParentBuildable.IonCannon:
                    return new PvPBuildingKey(BuildingCategory.Offence, "PvPIonCannon");
                case ParentBuildable.MissilePod:
                    return new PvPBuildingKey(BuildingCategory.Offence, "PvPMissilePod");
                case ParentBuildable.Cannon:
                    return new PvPBuildingKey(BuildingCategory.Offence, "PvPCannon");

                case ParentBuildable.Bomber:
                    return new PvPUnitKey(PvPUnitCategory.Aircraft, "PvPBomber");
                case ParentBuildable.Fighter:
                    return new PvPUnitKey(PvPUnitCategory.Aircraft, "PvPFighter");
                case ParentBuildable.MissileFighter:
                    return new PvPUnitKey(PvPUnitCategory.Aircraft, "PvPMissileFighter");
                case ParentBuildable.Gunship:
                    return new PvPUnitKey(PvPUnitCategory.Aircraft, "PvPGunship");
                case ParentBuildable.SteamCopter:
                    return new PvPUnitKey(PvPUnitCategory.Aircraft, "PvPSteamCopter");
                case ParentBuildable.Broadsword:
                    return new PvPUnitKey(PvPUnitCategory.Aircraft, "PvPBroadsword");
                case ParentBuildable.StratBomber:
                    return new PvPUnitKey(PvPUnitCategory.Aircraft, "PvPStratBomber");
                case ParentBuildable.SpyPlane:
                    return new PvPUnitKey(PvPUnitCategory.Aircraft, "PvPSpyPlane");
                case ParentBuildable.AttackBoat:
                    return new PvPUnitKey(PvPUnitCategory.Naval, "PvPAttackBoat");
                case ParentBuildable.AttackRIB:
                    return new PvPUnitKey(PvPUnitCategory.Naval, "PvPAttackRIB");
                case ParentBuildable.Frigate:
                    return new PvPUnitKey(PvPUnitCategory.Naval, "PvPFrigate");
                case ParentBuildable.Destroyer:
                    return new PvPUnitKey(PvPUnitCategory.Naval, "PvPDestroyer");
                case ParentBuildable.SiegeDestroyer:
                    return new PvPUnitKey(PvPUnitCategory.Naval, "PvPSiegeDestroyer");
                case ParentBuildable.ArchonBattleship:
                    return new PvPUnitKey(PvPUnitCategory.Naval, "PvPArchonBattleship");
                case ParentBuildable.GlassCannoneer:
                    return new PvPUnitKey(PvPUnitCategory.Naval, "PvPGlassCannoneer");
                case ParentBuildable.GunBoat:
                    return new PvPUnitKey(PvPUnitCategory.Naval, "PvPGunBoat");
                case ParentBuildable.RocketTurtle:
                    return new PvPUnitKey(PvPUnitCategory.Naval, "PvPRocketTurtle");
                case ParentBuildable.FlakTurtle:
                    return new PvPUnitKey(PvPUnitCategory.Naval, "PvPFlakTurtle");
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
                case ParentBuildable.GrapheneBarrier:
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
                case ParentBuildable.FlakTurret:
                    ret = false;
                    break;
                case ParentBuildable.CIWS:
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
                case ParentBuildable.Cannon:
                    ret = false;
                    break;
                case ParentBuildable.BlastVLS:
                    ret = false;
                    break;
                case ParentBuildable.FirecrackerVLS:
                    ret = false;
                    break;
                case ParentBuildable.Bomber:
                    ret = true;
                    break;
                case ParentBuildable.Fighter:
                    ret = true;
                    break;
                case ParentBuildable.MissileFighter:
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
                case ParentBuildable.StratBomber:
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
                case ParentBuildable.SiegeDestroyer:
                    ret = true;
                    break;
                case ParentBuildable.ArchonBattleship:
                    ret = true;
                    break;
                case ParentBuildable.GlassCannoneer:
                    ret = true;
                    break;
                case ParentBuildable.GunBoat:
                    ret = true;
                    break;
                case ParentBuildable.RocketTurtle:
                    ret = true;
                    break;
                case ParentBuildable.FlakTurtle:
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
