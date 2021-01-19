using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public class BackgroundImageController : MonoBehaviour
    {
        private IPrefabContainer<BackgroundImageStats> _statsContainer;

        public SpriteRenderer background;

        public void Initialise(IPrefabContainer<BackgroundImageStats> statsContainer, float cameraAspectRatio, IBackgroundImageCalculator calculator)
        {
            Helper.AssertIsNotNull(statsContainer, calculator);
            Assert.IsNotNull(background);
            Assert.IsTrue(cameraAspectRatio > 0);

            _statsContainer = statsContainer;
            IBackgroundImageStats stats = _statsContainer.Prefab;

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