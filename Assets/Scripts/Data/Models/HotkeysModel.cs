using BattleCruisers.Hotkeys;
using BattleCruisers.Utils;
using System;
using UnityEngine;

namespace BattleCruisers.Data.Models
{
    [Serializable]
    public class HotkeysModel : IHotkeysModel, IHotkeyList
    {
        [SerializeField]
        private KeyCode _playerCruiser;
        public KeyCode PlayerCruiser
        {
            get => _playerCruiser;
            set => _playerCruiser = value;
        }

        [SerializeField]
        private KeyCode _overview;
        public KeyCode Overview
        {
            get => _overview;
            set => _overview = value;
        }

        [SerializeField]
        private KeyCode _enemyCruiser;
        public KeyCode EnemyCruiser
        {
            get => _enemyCruiser;
            set => _enemyCruiser = value;
        }

        [SerializeField]
        private KeyCode _attackBoat;
        public KeyCode AttackBoat
        {
            get => _attackBoat;
            set => _attackBoat = value;
        }

        [SerializeField]
        private KeyCode _frigate;
        public KeyCode Frigate
        {
            get => _frigate;
            set => _frigate = value;
        }

        [SerializeField]
        private KeyCode _destroyer;
        public KeyCode Destroyer
        {
            get => _destroyer;
            set => _destroyer = value;
        }

        [SerializeField]
        private KeyCode _archon;
        public KeyCode Archon
        {
            get => _archon;
            set => _archon = value;
        }

        public HotkeysModel()
        {
            // Navigation
            PlayerCruiser = KeyCode.LeftArrow;
            Overview = KeyCode.UpArrow;
            EnemyCruiser = KeyCode.RightArrow;

            // Boats
            AttackBoat = KeyCode.Q;
            Frigate = KeyCode.W;
            Destroyer = KeyCode.E;
            Archon = KeyCode.R;
        }

        public override bool Equals(object obj)
        {
            HotkeysModel other = obj as HotkeysModel;

            return
                other != null
                // Navigation
                && PlayerCruiser == other.PlayerCruiser
                && Overview == other.Overview
                && EnemyCruiser == other.EnemyCruiser
                // Boats
                && AttackBoat == other.AttackBoat
                && Frigate == other.Frigate
                && Destroyer == other.Destroyer
                && Archon == other.Archon;

        }

        public override int GetHashCode()
        {
            return 
                this.GetHashCode(
                    PlayerCruiser, Overview, EnemyCruiser,
                    AttackBoat, Frigate, Destroyer, Archon);
        }
    }
}