using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.Panels;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using System.Collections.Generic;
using BattleCruisers.Utils.Properties;
using UnityEngine;
using BattleCruisers.UI.ScreensScene;
using BattleCruisers.Data.Static;
using System.Linq;
using BattleCruisers.UI.ScreensScene.ProfileScreen;

public class ShopItemPanel : Panel
{
    public int numberOfItems = 12;
    public void Initialise(
        ISingleSoundPlayer soundPlayer,
        IPrefabFactory prefabFactory,
        IGameModel gameModel)
    {
        ShopCaptainButton[] itemButton = GetComponentsInChildren<ShopCaptainButton>(includeInactive: true);
        List<int> captainList = RandomIndex();
        foreach (ShopCaptainButton captain in itemButton)
        {
            foreach (CaptainExoKey key in StaticPrefabKeys.CaptainExos.AllKeys)
            {
                CaptainExoData temp = prefabFactory.GetCaptainExo(key);
                if (captainList.Contains(temp.captainIndex))
                {
                    Debug.Log(temp);
                    captainList.Remove(temp.captainIndex);
                    captain.Initialise(soundPlayer, prefabFactory, temp);
                    break;
                }
            }
        }
        

    }

    private List<int> RandomIndex()
    {
        List<int> randomList = new();
        int limit = 0;
        while(limit <= numberOfItems)
        {
            int rand = UnityEngine.Random.Range(1,StaticPrefabKeys.CaptainExos.AllKeys.Count);
            if(!randomList.Contains(rand))
            {
                randomList.Add(rand);
                limit++;
            }
        }
        randomList.Sort();
        return randomList;
    }
}
