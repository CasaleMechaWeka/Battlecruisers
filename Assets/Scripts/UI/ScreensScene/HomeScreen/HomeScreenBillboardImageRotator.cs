using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.HomeScreen
{
    public class HomeScreenBillboardImageRotator : MonoBehaviour
    {
        private Transform[] childSprites;
        private int currentIndex = -1;

        void Start()
        {
            // Get all child transforms (sprites)
            childSprites = GetComponentsInChildren<Transform>();
            // Deactivate all children initially
            foreach (var child in childSprites)
            {
                if (child != transform) // Exclude the parent object itself
                {
                    child.gameObject.SetActive(false);
                }
            }
            // Activate the first child
            ActivateRandomChild();
            // Start the coroutine to change the child every 10 seconds
            InvokeRepeating(nameof(ActivateRandomChild), 10f, 10f);
        }

        private void ActivateRandomChild()
        {
            // Deactivate the current child
            if (currentIndex != -1)
            {
                childSprites[currentIndex].gameObject.SetActive(false);
            }

            // Choose a new random index
            currentIndex = Random.Range(0, childSprites.Length - 1);
            // Activate the new child
            childSprites[currentIndex].gameObject.SetActive(true);
        }
    }
}
