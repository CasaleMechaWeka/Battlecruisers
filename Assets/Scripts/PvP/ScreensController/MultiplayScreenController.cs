using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.Utils;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.UI.Common;
using BattleCruisers.UI.Commands;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using BattleCruisers.Network.Multiplay.Scenes;
using BattleCruisers.Utils.Fetchers.Sprites;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using Map = BattleCruisers.Network.Multiplay.Matchplay.Shared.Map;



namespace BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen
{
    public class MultiplayScreenController : ScreenController
    {
        private Map _curSelectedMap;
        public Map CurSelectedMap => _curSelectedMap;
        public CanvasGroupButton homeButton, arenaButton, loadoutButton, shopButton, leaderboardButton;
        public CanvasGroupButton nextArenaButton, previousArenaButton;
        public CanvasGroupButton battleButton;
        private ICommand _nextBattleCommand, _previousBattleCommand;
        Dictionary<Map, Sprite> _maps = new Dictionary<Map, Sprite>();
        IPvPArenaBackgroundSpriteProvider arenaSpritesProvider;

        [SerializeField]
        Image c_arenaBackground;
        public override void OnPresenting(object activationParameter)
        {

        }
        public void Initialise(IMultiplayScreensSceneGod multiplayScreensSceneGod,
            ISingleSoundPlayer soundPlayer,
            IDataProvider dataProvider)
        {
            base.Initialise(multiplayScreensSceneGod);

            Helper.AssertIsNotNull(dataProvider);
            Helper.AssertIsNotNull(homeButton, battleButton, nextArenaButton, previousArenaButton);



            nextArenaButton.Initialise(soundPlayer, OnNextArena);
            previousArenaButton.Initialise(soundPlayer, OnPrevArena);

            homeButton.Initialise(soundPlayer, Cancel);
            loadoutButton.Initialise(soundPlayer, OpenLoadout);
            shopButton.Initialise(soundPlayer, OpenShop);
            leaderboardButton.Initialise(soundPlayer, OpenLeaderboard);
            arenaButton.Initialise(soundPlayer, OpenArenaSelection);

            battleButton.Initialise(soundPlayer, LoadBattle);

            SpriteFetcher spriteFetcher = new SpriteFetcher();
            arenaSpritesProvider = new PvPArenaBackgroundSpriteProvider(spriteFetcher);

#pragma warning disable 4014
            LoadPvPBackground();
#pragma warning restore 4014

        }

        async Task LoadPvPBackground()
        {
            for (int i = 0; i < (int)Enum.GetValues(typeof(Map)).Cast<Map>().Last() + 1; i++)
            {
                Map map = (Map)i;
                ISpriteWrapper pvpArenaSprite = await arenaSpritesProvider.GetSpriteAsync(map);
                _maps.Add(map, pvpArenaSprite.Sprite);
            }
        }

        // private void NextBattleCommandExecute()
        // {
        //     OnNextArena();
        // }
        // private bool CanNextBattleCommandExecute()
        // {
        //     return true;
        // }

        // private void PreviousBattleCommandExecute()
        // {
        //     OnPrevArena();
        // // }

        // private bool CanPreviousBattleCommandExecute()
        // {
        //     return true;
        // }

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
            int m_nextArena = ((int)_curSelectedMap + 1);
            if (m_nextArena > (int)Enum.GetValues(typeof(Map)).Cast<Map>().Last())
            {
                m_nextArena = 0;
            }
            _curSelectedMap = (Map)m_nextArena;
            c_arenaBackground.sprite = _maps[_curSelectedMap];
        }
        public void OnPrevArena()
        {
            int m_nextArena = ((int)_curSelectedMap - 1);
            if (m_nextArena < 0)
            {
                m_nextArena = (int)Enum.GetValues(typeof(Map)).Cast<Map>().Last();
            }
            _curSelectedMap = (Map)m_nextArena;
            c_arenaBackground.sprite = _maps[_curSelectedMap];
        }


        private void OpenLoadout()
        {
            Debug.Log("you are opening loadout");
        }

        private void OpenArenaSelection()
        {
            Debug.Log("you are opening arena selection");
        }
        private void OpenShop()
        {
            Debug.Log("you are opening shop");
        }

        private void OpenLeaderboard()
        {
            Debug.Log("you are opening leaderboard");
        }

        public override void Cancel()
        {
            _multiplayScreensSceneGod.LoadMainMenuScene();
        }
    }

}

