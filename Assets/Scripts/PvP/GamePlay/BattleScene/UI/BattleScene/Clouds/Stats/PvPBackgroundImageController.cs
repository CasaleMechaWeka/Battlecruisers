using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Clouds.Stats
{
    public class PvPBackgroundImageController : MonoBehaviour
    {
        private IPvPPrefabContainer<PvPBackgroundImageStats> _statsContainer;

        public SpriteRenderer background;

        public void Initialise(IPvPPrefabContainer<PvPBackgroundImageStats> statsContainer, float cameraAspectRatio, IPvPBackgroundImageCalculator calculator)
        {
            PvPHelper.AssertIsNotNull(statsContainer, calculator);
            Assert.IsNotNull(background);
            Assert.IsTrue(cameraAspectRatio > 0);

            _statsContainer = statsContainer;
            IPvPBackgroundImageStats stats = _statsContainer.Prefab;

            if (stats.Sprite == null)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);

            transform.position = calculator.FindPosition(stats, cameraAspectRatio);
            transform.localScale = new Vector3(stats.Scale.x, stats.Scale.y, 1);
            transform.rotation = Quaternion.Euler(0, 0, stats.ZRotation);

            background.sprite = stats.Sprite;
            background.color = stats.Colour;
            background.flipX = stats.FlipX;
            background.flipY = stats.FlipY;
            background.sortingOrder = stats.OrderInLayer;
        }

        void OnDestroy()
        {
            if (_statsContainer != null)
            {
                Addressables.Release(_statsContainer.Handle);
                _statsContainer = null;
            }
        }
    }
}