using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.Utils;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.UI.Common;
using BattleCruisers.UI.Commands;
using System;
using System.Linq;
using UnityEngine;
using BattleCruisers.Network.Multiplay.Scenes;
using Arena = BattleCruisers.Network.Multiplay.Matchplay.Shared.Arena;


namespace BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen
{
    public class MultiplayScreenController : ScreenController
    {
        private Arena _curSelectedArena;
        public Arena CurSelectedArena => _curSelectedArena;

        public CanvasGroupButton cancelButton, battleButton;

        public ButtonController nextBattleButton, previousBattleButton;
        private ICommand _nextBattleCommand, _previousBattleCommand;
        public override void OnPresenting(object activationParameter)
        {

        }
        public void Initialise(IMultiplayScreensSceneGod multiplayScreensSceneGod,
            ISingleSoundPlayer soundPlayer,
            IDataProvider dataProvider)
        {
            base.Initialise(multiplayScreensSceneGod);

            Helper.AssertIsNotNull(dataProvider);
            Helper.AssertIsNotNull(cancelButton, battleButton, nextBattleButton, previousBattleButton);

            _nextBattleCommand = new Command(NextBattleCommandExecute, CanNextBattleCommandExecute);
            nextBattleButton.Initialise(soundPlayer, _nextBattleCommand);

            _previousBattleCommand = new Command(PreviousBattleCommandExecute, CanPreviousBattleCommandExecute);
            previousBattleButton.Initialise(soundPlayer, _previousBattleCommand);

            cancelButton.Initialise(soundPlayer, Cancel);
            battleButton.Initialise(soundPlayer, LoadBattle);
        }

        private void NextBattleCommandExecute()
        {
            OnNextArena();
            Debug.Log(_curSelectedArena);
        }
        private bool CanNextBattleCommandExecute()
        {
            return true;
        }

        private void PreviousBattleCommandExecute()
        {
            OnPrevArena();
            Debug.Log(_curSelectedArena);
        }
        private bool CanPreviousBattleCommandExecute()
        {
            return true;
        }

        private void LoadBattle()
        {
            Invoke("StartBattle", 0.3f);
        }
        private void StartBattle()
        {
            _multiplayScreensSceneGod.GotoMatchmakingScreen();
        }




        public void GotoArena()
        {
            Debug.Log("Goto Arena!");
        }

        public void OnNextArena()
        {
            int m_nextArena = ((int)_curSelectedArena + 1);
            if (m_nextArena > (int)Enum.GetValues(typeof(Arena)).Cast<Arena>().Last())
            {
                m_nextArena = 0;
            }
            _curSelectedArena = (Arena)m_nextArena;
        }
        public void OnPrevArena()
        {
            int m_nextArena = ((int)_curSelectedArena - 1);
            if (m_nextArena < 0)
            {
                m_nextArena = (int)Enum.GetValues(typeof(Arena)).Cast<Arena>().Last();
            }
            _curSelectedArena = (Arena)m_nextArena;
        }


        public override void Cancel()
        {
            _multiplayScreensSceneGod.LoadMainMenuScene();
        }
    }

}

