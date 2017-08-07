using System.Collections.Generic;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.Providers
{
    public abstract class AntiUnitBuildOrderProvider
    {
        private readonly IStaticData _staticData;
        private readonly IPrefabKey _basicDefenceKey, _advancedDefenceKey;

        private const int MIN_NUM_OF_SLOTS = 1;

        public AntiUnitBuildOrderProvider(IStaticData staticData, IPrefabKey basicDefenceKey, IPrefabKey advancedDefenceKey)
        {
            Helper.AssertIsNotNull(staticData, basicDefenceKey, advancedDefenceKey);

            _staticData = staticData;
            _basicDefenceKey = basicDefenceKey;
            _advancedDefenceKey = advancedDefenceKey;
        }

        public IList<IPrefabKey> CreateBuildOrder(int numOfDeckSlots, int levelNum)
        {
			IList<IPrefabKey> buildOrder = new List<IPrefabKey>();

            int numOfSlotsToUse = FindNumOfSlotsToUse(numOfDeckSlots);
            Assert.IsTrue(numOfDeckSlots > MIN_NUM_OF_SLOTS);

            buildOrder.Add(_basicDefenceKey);

            IPrefabKey defenceKey = _staticData.IsBuildableAvailable(_advancedDefenceKey, levelNum) ? _advancedDefenceKey : _basicDefenceKey;               

            while (buildOrder.Count < numOfSlotsToUse)
            {
                buildOrder.Add(defenceKey);
            }

            return buildOrder;
        }

        protected abstract int FindNumOfSlotsToUse(int numOfDeckSlots);
    }
}
