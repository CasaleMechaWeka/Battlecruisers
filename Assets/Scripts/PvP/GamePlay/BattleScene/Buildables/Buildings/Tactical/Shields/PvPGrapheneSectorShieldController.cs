using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Timers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;
using BattleCruisers.Utils.Localisation;
using System.Collections.Generic;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.ScreensScene.ProfileScreen;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Tactical.Shields {

    public class PvPGrapheneSectorShieldController : PvPSectorShieldController
    {
            public UnityEvent onShieldDepleted;
            public UnityEvent onShieldDamaged;


            protected override void OnHealthGone()
            {
                base.OnHealthGone();
                onShieldDepleted.Invoke();
            }

            protected override void OnTakeDamage()
            {
                base.OnTakeDamage();
                onShieldDamaged.Invoke();
            }


            public void SetShieldHealth(float newHealth)
            {
                _healthTracker.SetHealth(newHealth);
            }
    }
}