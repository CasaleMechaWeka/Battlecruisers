using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattleCruisers.UI.Common.BuildableDetails
{
    public class UnitDetailController : MonoBehaviour
    {
        public CanvasGroupButton leftNav, rightNav;
        private IUnit _selectedUnit;
        public IUnit SelectedUnit
        {
            get { return _selectedUnit; }
            set
            {
                _selectedUnit = value;
            }
        }
        private IDataProvider _dataProvider;
        private IPrefabFactory _prefabFactory;
        private ISingleSoundPlayer _soundPlayer;
        private ILocTable _commonStrings;
        private Dictionary<IBuilding, List<int>> _unlockedVariants;
        private int _index;

        public void Initialize(IDataProvider dataProvider, IPrefabFactory prefabFactory, ISingleSoundPlayer soundPlayer, ILocTable commonString)
        {
            Helper.AssertIsNotNull(dataProvider, prefabFactory, soundPlayer);
            Helper.AssertIsNotNull(leftNav, rightNav);
            _dataProvider = dataProvider;
            _prefabFactory = prefabFactory;
            _soundPlayer = soundPlayer;
            _commonStrings = commonString;

            leftNav.Initialise(_soundPlayer, LeftNavButton_OnClicked);
            rightNav.Initialise(_soundPlayer, RightNavButton_OnClicked);
        }
        private void LeftNavButton_OnClicked()
        {

        }
        private void RightNavButton_OnClicked()
        {

        }

    }
}
