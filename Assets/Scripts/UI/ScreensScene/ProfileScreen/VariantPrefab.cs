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
        public ParentBuildable parent => _parent;

        public VariantType _variantType;
        public VariantType variantType => _variantType;

        public StatVariant statVariant;
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
        public int laser_duration;

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
    }
}
