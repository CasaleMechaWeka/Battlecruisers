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

        [SerializeField]
        private KeyCode _factories;
        public KeyCode Factories
        {
            get => _factories;
            set => _factories = value;
        }

        [SerializeField]
        private KeyCode _defensives;
        public KeyCode Defensives
        {
            get => _defensives;
            set => _defensives = value;
        }

        [SerializeField]
        private KeyCode _offensives;
        public KeyCode Offensives
        {
            get => _offensives;
            set => _offensives = value;
        }

        [SerializeField]
        private KeyCode _tacticals;
        public KeyCode Tacticals
        {
            get => _tacticals;
            set => _tacticals = value;
        }

        [SerializeField]
        private KeyCode _ultras;
        public KeyCode Ultras
        {
            get => _ultras;
            set => _ultras = value;
        }

        public HotkeysModel()
        {
            // Navigation
            PlayerCruiser = KeyCode.LeftArrow;
            Overview = KeyCode.UpArrow;
            EnemyCruiser = KeyCode.RightArrow;

            // Building categories
            Factories = KeyCode.A;
            Defensives = KeyCode.S;
            Offensives = KeyCode.D;
            Tacticals = KeyCode.F;
            Ultras = KeyCode.G;

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
                // Building categories
                && Factories == other.Factories
                && Defensives == other.Defensives
                && Offensives == other.Offensives
                && Tacticals == other.Tacticals
                && Ultras == other.Ultras
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
                    Factories, Defensives, Offensives, Tacticals, Ultras,
                    AttackBoat, Frigate, Destroyer, Archon);
        }
    }
}