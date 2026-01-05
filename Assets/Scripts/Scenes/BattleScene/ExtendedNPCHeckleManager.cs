using UnityEngine;
using BattleCruisers.UI.BattleScene.Heckles;

namespace BattleCruisers.Scenes.BattleScene
{
    /// <summary>
    /// Basic heckle manager for NPC interactions.
    /// Simplified version without chain battle functionality.
    /// </summary>
    public class ExtendedNPCHeckleManager : MonoBehaviour
    {
        private HeckleMessage _heckleMessage;

        /// <summary>
        /// Initialize the heckle manager.
        /// </summary>
        /// <param name="message">The UI component to display heckles</param>
        public void Initialize(HeckleMessage message)
        {
            _heckleMessage = message;
        }

        /// <summary>
        /// Show a heckle by index.
        /// </summary>
        /// <param name="heckleIndex">Index of the heckle to show</param>
        public void ShowHeckle(int heckleIndex)
        {
            if (_heckleMessage != null)
            {
                _heckleMessage.Show(heckleIndex);
            }
        }
    }
}
