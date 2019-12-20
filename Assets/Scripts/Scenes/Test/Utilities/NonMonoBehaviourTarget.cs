using System;
using System.Collections.ObjectModel;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityCommon.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Utilities
{
    public class NonMonoBehaviourTarget : ITarget
    {
        public Faction Faction { get; set; }
        public TargetType TargetType { get; set; }
        public Vector2 Velocity { get; set; }
        public ReadOnlyCollection<TargetType> AttackCapabilities { get; set; }
        public TargetValue TargetValue { get; set; }
        public Color Color { get; set; }
        public bool IsInScene { get; set; }
        public Vector2 Size { get; set; }
        public ITransform Transform { get; set; }
        public Vector2 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public float HealthGainPerDroneS { get; set; }
        public IRepairCommand RepairCommand { get; set; }
        public ITextMesh NumOfRepairDronesText { get; set; }
        public bool IsDestroyed { get; set; }
        public float Health { get; set; }
        public float MaxHealth { get; set; }
        public GameObject GameObject { get; set; }
        public ITarget LastDamagedSource { get; set; }

        public event EventHandler<DamagedEventArgs> Damaged;
        public event EventHandler HealthChanged;
        public event EventHandler<DestroyedEventArgs> Destroyed;

        public HighlightArgs CreateHighlightArgs(IHighlightArgsFactory highlightArgsFactory)
        {
            throw new NotImplementedException();
        }

        public void Destroy()
        {
            throw new NotImplementedException();
        }

        public void TakeDamage(float damageAmount, ITarget damageSource)
        {
            throw new NotImplementedException();
        }
    }
}