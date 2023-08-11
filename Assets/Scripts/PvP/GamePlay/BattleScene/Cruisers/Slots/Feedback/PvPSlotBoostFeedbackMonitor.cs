using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Utils;
using Unity.Netcode;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots.Feedback
{
    public class PvPSlotBoostFeedbackMonitor
    {
        private readonly PvPSlot _parentSlot;
        private readonly IPvPBoostStateFinder _boostStateFinder;
        private readonly IPvPBoostFeedback _boostFeedback;

        public PvPSlotBoostFeedbackMonitor(PvPSlot parentSlot, IPvPBoostStateFinder boostStateFinder, IPvPBoostFeedback boostFeedback, bool isClient)
        {
            Helper.AssertIsNotNull(parentSlot, boostStateFinder, boostFeedback);

            _parentSlot = parentSlot;
            _boostStateFinder = boostStateFinder;
            _boostFeedback = boostFeedback;

            _parentSlot.Building.ValueChanged += (sender, e) => UpdateSlotBoostFeedback();
            _parentSlot.BoostProviders.CollectionChanged += (sender, e) => UpdateSlotBoostFeedback();

            // pvp
            if (isClient)
            {
                _parentSlot.pvp_Building_NetworkObjectID.OnValueChanged += UpdateSlotBoostFeedback_PvP_NetworkObjectID;
                _parentSlot.pvp_BoostProviders_Num.OnValueChanged += UpdateSlotBoostFeedback_PvP_BoostProvidersNum;
            }

        }

        private void UpdateSlotBoostFeedback()
        {
            _boostFeedback.State = _boostStateFinder.FindState(_parentSlot.BoostProviders.Count, _parentSlot.Building.Value);
        }

        private void UpdateSlotBoostFeedback_PvP_NetworkObjectID(ulong oldVal, ulong newVal)
        {
            if (newVal == 0)
                return;
            NetworkObject obj = PvPBattleSceneGodClient.Instance.GetNetworkObject(newVal);
            if (obj != null)
            {
                IPvPBuilding building = obj.gameObject.GetComponent<PvPBuildableWrapper<IPvPBuilding>>()?.Buildable?.Parse<IPvPBuilding>();
                if (building != null)
                {
                    _boostFeedback.State = _boostStateFinder.FindState(_parentSlot.pvp_BoostProviders_Num.Value, building);
                }
            }
        }

        private void UpdateSlotBoostFeedback_PvP_BoostProvidersNum(int oldVal, int newVal)
        {
            if (_parentSlot.pvp_Building_NetworkObjectID.Value == 0)
                return;
            NetworkObject obj = PvPBattleSceneGodClient.Instance.GetNetworkObject(_parentSlot.pvp_Building_NetworkObjectID.Value);
            if (obj != null)
            {
                IPvPBuilding building = obj.gameObject.GetComponent<PvPBuildableWrapper<IPvPBuilding>>()?.Buildable?.Parse<IPvPBuilding>();
                if (building != null)
                {
                    _boostFeedback.State = _boostStateFinder.FindState(newVal, building);
                }
            }
        }
    }
}