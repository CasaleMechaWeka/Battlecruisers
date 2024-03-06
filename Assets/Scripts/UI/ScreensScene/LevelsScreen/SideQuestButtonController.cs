using BattleCruisers.Scenes;
using BattleCruisers.UI;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.Players;
using UnityEngine;

public class SideQuestButtonController : ElementWithClickSound
{
    private bool enabled;
    private IScreensSceneGod _screensSceneGod;
    private int _sideQuestID;
    protected override ISoundKey ClickSound => SoundKeys.UI.Click;
    private GameObject checkmark;
    private GameObject sideQuestCompleted;
    private GameObject sideQuestIncomplete;
    private GameObject checkbox;
    public int requiredLevel;
    private Transform buttonImages;

    public void Initialise(
        IScreensSceneGod screensSceneGod,
        ISingleSoundPlayer soundPlayer,
        int sideQuestLevelNum,
        int numOfLevelUnlocked,
        bool completed)
    {
        //Most of side quest scripts will need to be modified once side quest manager is done
        _screensSceneGod = screensSceneGod;

        base.Initialise(soundPlayer);
        _sideQuestID = sideQuestLevelNum;
        Enabled = numOfLevelUnlocked >= _sideQuestID;
        enabled = numOfLevelUnlocked >= requiredLevel;
        checkmark = transform.Find("Checked").gameObject;
        checkmark.SetActive(completed && enabled);
        buttonImages = transform.Find("ButtonImages");
        checkbox = transform.Find("Unchecked").gameObject;
        checkbox.SetActive(enabled);

        if (buttonImages != null)
        {
            sideQuestCompleted = buttonImages.transform.Find("CompleteSideQuest").gameObject;
            sideQuestCompleted.SetActive(completed && enabled);
            sideQuestIncomplete = buttonImages.transform.Find("IncompleteSideQuest").gameObject;
            sideQuestIncomplete.SetActive(!completed && enabled);
        }
    }

    protected override void OnClicked()
    {
        base.OnClicked();
        _screensSceneGod.LoadBattleSceneSideQuest(_sideQuestID);
    }
}
