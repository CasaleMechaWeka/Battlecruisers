using BattleCruisers.Hotkeys;
using BattleCruisers.Utils;
using System;
using UnityEngine;

namespace BattleCruisers.Data.Models
{
    // FELIX  Adde to GameModel/Serializer test :D
    [Serializable]
    public class HotkeysModel : IHotkeyList
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

        public HotkeysModel()
        {
            PlayerCruiser = KeyCode.LeftArrow;
            Overview = KeyCode.UpArrow;
            EnemyCruiser = KeyCode.RightArrow;
        }

        public override bool Equals(object obj)
        {
            HotkeysModel other = obj as HotkeysModel;

            return
                other != null
                && PlayerCruiser == other.PlayerCruiser
                && Overview == other.Overview
                && EnemyCruiser == other.EnemyCruiser;

        }

        public override int GetHashCode()
        {
            return this.GetHashCode(PlayerCruiser, Overview, EnemyCruiser);
        }
    }
}