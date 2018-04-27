using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Helpers;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras;

namespace BattleCruisers.Cruisers
{
    public interface ICruiserFactory
    {
        void InitialiseCruiser(
            Cruiser cruiser, 
            ICruiser enemyCruiser, 
            IUIManager uiManager, 
            ICameraController cameraController,
            Faction faction, 
            Direction facingDirection);
        
		ICruiserHelper CreatePlayerHelper(IUIManager uiManager, ICameraController camera);
		ICruiserHelper CreateAIHelper(IUIManager uiIManager, ICameraController camera);
    }
}