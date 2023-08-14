using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattleCruisers.UI.ScreensScene.ShopScreen
{
    [Serializable]
    public class IAPData : IIAPData
    {
        public int _IAPType;
        public int IAPType => _IAPType;

        public string _IAPNameKeyBase;
        public string IAPNameKeyBase => _IAPNameKeyBase;

        public string _IAPDescriptionKeyBase;
        public string IAPDescriptionKeyBase => _IAPDescriptionKeyBase;

        public string _IAPIconName;
        public string IAPIconName => _IAPIconName;

        public float _IAPCost;
        public float IAPCost => _IAPCost;

        public int _IAPCoins;
        public int IAPCoins => _IAPCoins;

        public IAPData(int iapType = 0, string iapNameKeyBase = "Coins100Pack", string iapDescriptionKeybase = "Coins100Pack", string iapIconName = "Coins100Pack", float iapCost = 0.99f, int iapCoins = 100)
        {
            _IAPType = iapType;
            _IAPNameKeyBase = iapNameKeyBase;
            _IAPDescriptionKeyBase = iapDescriptionKeybase;
            _IAPIconName = iapIconName;
            _IAPCost = iapCost;
            _IAPCoins = iapCoins;
        }
    }
}
