using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.Properties;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public class BodykitDetailController : MonoBehaviour
    {
        IBroadcastingProperty<HullKey> _selectedHull;
        private HullType _selectedHullType => GetHullType(DataProvider.GameModel.PlayerLoadout.Hull);
        private HullType _hullType;
        public HullType hullType
        {
            get { return _hullType; }
            set
            {
                _hullType = value;
                if (_unlockedBodykits.ContainsKey(_hullType))
                {
                    if (_unlockedBodykits[_hullType].Count > 0)
                    {
                        if (_unlockedBodykits[_hullType].IndexOf(DataProvider.GameModel.PlayerLoadout.SelectedBodykit) == 0)
                        {
                            if (_unlockedBodykits[_hullType].Count == 1)
                            {
                                leftNavButton.gameObject.SetActive(true);
                                rightNavButton.gameObject.SetActive(false);
                            }
                            else
                            {
                                leftNavButton.gameObject.SetActive(true);
                                rightNavButton.gameObject.SetActive(true);
                            }
                            _index = 0;
                            ShowBodyKitDetail(DataProvider.GameModel.PlayerLoadout.SelectedBodykit);
                            return;
                        }
                        else if (_unlockedBodykits[_hullType].IndexOf(DataProvider.GameModel.PlayerLoadout.SelectedBodykit) == _unlockedBodykits[_hullType].Count - 1)
                        {
                            leftNavButton.gameObject.SetActive(true);
                            rightNavButton.gameObject.SetActive(false);
                            _index = _unlockedBodykits[_hullType].Count - 1;
                            ShowBodyKitDetail(DataProvider.GameModel.PlayerLoadout.SelectedBodykit);
                            return;
                        }
                        else if (_unlockedBodykits[_hullType].IndexOf(DataProvider.GameModel.PlayerLoadout.SelectedBodykit) < 0)
                        {
                            leftNavButton.gameObject.SetActive(false);
                            rightNavButton.gameObject.SetActive(true);
                            _index = -1;
                            ShowOriginCruiser();
                            return;
                        }
                        else
                        {
                            leftNavButton.gameObject.SetActive(true);
                            rightNavButton.gameObject.SetActive(true);
                            _index = _unlockedBodykits[_hullType].IndexOf(DataProvider.GameModel.PlayerLoadout.SelectedBodykit);
                            ShowBodyKitDetail(DataProvider.GameModel.PlayerLoadout.SelectedBodykit);
                            return;
                        }
                    }
                    else
                    {
                        leftNavButton.gameObject.SetActive(false);
                        rightNavButton.gameObject.SetActive(false);
                        _index = -1;
                        ShowOriginCruiser();
                    }
                }
                else
                {
                    leftNavButton.gameObject.SetActive(false);
                    rightNavButton.gameObject.SetActive(false);
                    _index = -1;
                    ShowOriginCruiser();
                }
            }
        }

        private PrefabFactory _prefabFactory;
        private ISingleSoundPlayer _soundPlayer;
        private Dictionary<HullType, List<int>> _unlockedBodykits = new Dictionary<HullType, List<int>>();
        private int _index;
        public CanvasGroupButton leftNavButton, rightNavButton;

        public void Initialise(PrefabFactory prefabFactory, ISingleSoundPlayer soundPlayer)
        {
            Helper.AssertIsNotNull(prefabFactory, soundPlayer);
            Helper.AssertIsNotNull(leftNavButton, rightNavButton);
            _prefabFactory = prefabFactory;
            _soundPlayer = soundPlayer;

            leftNavButton.Initialise(_soundPlayer, LeftNavButton_OnClicked);
            rightNavButton.Initialise(_soundPlayer, RightNavButton_OnClicked);
            CollectUnlockedBodykits();
        }

        public void RegisterSelectedHull(IBroadcastingProperty<HullKey> selectedHull)
        {
            _selectedHull = selectedHull;
            _selectedHull.ValueChanged += OnSelectedNewHullType;
        }

        private void OnSelectedNewHullType(object sender, EventArgs e)
        {
            if (_index == -1)
            {
                DataProvider.GameModel.PlayerLoadout.SelectedBodykit = -1;
            }
            else
            {
                DataProvider.GameModel.PlayerLoadout.SelectedBodykit = _unlockedBodykits[_hullType][_index];
            }
            DataProvider.SaveGame();
        }
        private void LeftNavButton_OnClicked()
        {
            --_index;
            if (_index <= -1)
            {
                _index = -1;
                leftNavButton.gameObject.SetActive(false);
                rightNavButton.gameObject.SetActive(true);
                if (_hullType == _selectedHullType)
                {
                    DataProvider.GameModel.PlayerLoadout.SelectedBodykit = -1;
                    DataProvider.SaveGame();
                }
                ShowOriginCruiser();
                return;
            }
            else
            {
                leftNavButton.gameObject.SetActive(true);
                rightNavButton.gameObject.SetActive(true);
            }
            if (_hullType == _selectedHullType)
            {
                DataProvider.GameModel.PlayerLoadout.SelectedBodykit = _unlockedBodykits[_hullType][_index];
                DataProvider.SaveGame();
            }
            ShowBodyKitDetail(_unlockedBodykits[_hullType][_index]);
        }

        private void RightNavButton_OnClicked()
        {
            ++_index;
            if (_index >= _unlockedBodykits[_hullType].Count - 1)
            {
                _index = _unlockedBodykits[_hullType].Count - 1;
                leftNavButton.gameObject.SetActive(true);
                rightNavButton.gameObject.SetActive(false);
            }
            else
            {
                leftNavButton.gameObject.SetActive(true);
                rightNavButton.gameObject.SetActive(true);
            }
            if (_hullType == _selectedHullType)
            {
                DataProvider.GameModel.PlayerLoadout.SelectedBodykit = _unlockedBodykits[_hullType][_index];
                DataProvider.SaveGame();
            }
            ShowBodyKitDetail(_unlockedBodykits[_hullType][_index]);
        }

        private void ShowBodyKitDetail(int index)
        {
            if (index < 0)
                return;
            Bodykit bodykit = _prefabFactory.GetBodykit(StaticPrefabKeys.BodyKits.GetBodykitKey(index));
            GetComponent<ComparableCruiserDetailsController>().itemName.text = LocTableCache.CommonTable.GetString(StaticData.Bodykits[index].NameStringKeyBase);
            GetComponent<ComparableCruiserDetailsController>().itemDescription.text = LocTableCache.CommonTable.GetString(StaticData.Bodykits[index].DescriptionKeyBase);
            GetComponent<ComparableCruiserDetailsController>().itemImage.sprite = bodykit.BodykitImage;
        }

        private void ShowOriginCruiser()
        {
            GetComponent<ComparableCruiserDetailsController>().ShowItemDetails();
        }

        private HullType GetHullType(HullKey hullKey)
        {
            switch (hullKey.PrefabName)
            {
                case "Trident":
                    return HullType.Trident;
                case "BlackRig":
                    return HullType.BlackRig;
                case "Bullshark":
                    return HullType.Bullshark;
                case "Eagle":
                    return HullType.Eagle;
                case "Flea":
                    return HullType.Flea;
                case "Goatherd":
                    return HullType.Goatherd;
                case "Hammerhead":
                    return HullType.Hammerhead;
                case "Longbow":
                    return HullType.Longbow;
                case "Megalodon":
                    return HullType.Megalodon;
                case "Megalith":
                    return HullType.Megalith;
                case "Microlodon":
                    return HullType.Microlodon;
                case "Raptor":
                    return HullType.Raptor;
                case "Rickshaw":
                    return HullType.Rickshaw;
                case "Rockjaw":
                    return HullType.Rockjaw;
                case "Pistol":
                    return HullType.Pistol;
                case "Shepherd":
                    return HullType.Shepherd;
                case "TasDevil":
                    return HullType.TasDevil;
                default:
                    return HullType.Yeti;
            }
        }

        public void CollectUnlockedBodykits()
        {
            _unlockedBodykits = new Dictionary<HullType, List<int>>();
            for (int i = 0; i < StaticData.Bodykits.Count; i++)
            {
                if (DataProvider.GameModel.PurchasedBodykits.Contains(i))
                {
                    Bodykit bodykit = _prefabFactory.GetBodykit(StaticPrefabKeys.BodyKits.GetBodykitKey(i));
                    if (_unlockedBodykits.ContainsKey(bodykit.cruiserType))
                    {
                        _unlockedBodykits[bodykit.cruiserType].Add(i);
                    }
                    else
                    {
                        _unlockedBodykits[bodykit.cruiserType] = new List<int> { i };
                    }
                }
            }
        }
    }
}

