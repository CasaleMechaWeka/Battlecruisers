using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using BattleCruisers.Data;

public class BonusSelectionPanel : MonoBehaviour
{
    [SerializeField] private BonusCard[] bonusCards;
    [SerializeField] private CanvasGroup canvasGroup;

    private System.Action<ChainBattleBonus> onBonusSelected;

    public void ShowBonusSelection(ChainBattleBonus[] bonuses, System.Action<ChainBattleBonus> callback)
    {
        onBonusSelected = callback;

        for (int i = 0; i < bonusCards.Length && i < bonuses.Length; i++)
        {
            bonusCards[i].SetupBonus(bonuses[i], OnCardSelected);
        }

        // Animate in
        StartCoroutine(FadeIn());
    }

    private void OnCardSelected(ChainBattleBonus selectedBonus)
    {
        onBonusSelected?.Invoke(selectedBonus);
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        float elapsed = 0f;
        while (elapsed < 0.3f)
        {
            canvasGroup.alpha = elapsed / 0.3f;
            elapsed += Time.unscaledDeltaTime; // Use unscaled during pause
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    private IEnumerator FadeOut()
    {
        float elapsed = 0f;
        while (elapsed < 0.3f)
        {
            canvasGroup.alpha = 1f - (elapsed / 0.3f);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }
}
