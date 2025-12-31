using UnityEngine;
using UnityEngine.UI;
using BattleCruisers.Data;

public class BonusCard : MonoBehaviour
{
    [SerializeField] private Text bonusNameText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Button selectButton;

    private ChainBattleBonus bonus;
    private System.Action<ChainBattleBonus> onSelected;

    public void SetupBonus(ChainBattleBonus bonus, System.Action<ChainBattleBonus> callback)
    {
        this.bonus = bonus;
        this.onSelected = callback;

        bonusNameText.text = bonus.bonusName;
        descriptionText.text = bonus.description;

        selectButton.onClick.AddListener(() => onSelected(bonus));
    }
}
