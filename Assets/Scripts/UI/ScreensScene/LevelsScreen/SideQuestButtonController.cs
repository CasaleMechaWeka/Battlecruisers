using BattleCruisers.Scenes;
using BattleCruisers.UI;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.Players;
using UnityEngine;
using BattleCruisers.Data;
using BattleCruisers.UI.ScreensScene.LevelsScreen;

public class SideQuestButtonController : ElementWithClickSound
{
    private bool enabled;
    private IScreensSceneGod _screensSceneGod;
    private IStaticData _staticData;
    public int sideQuestID;
    protected override ISoundKey ClickSound => SoundKeys.UI.Click;
    private GameObject checkmark;
    private GameObject sideQuestCompleted;
    private GameObject sideQuestIncomplete;
    private GameObject checkbox;
    private int requiredLevel;
    private int requiredSideQuestID;
    private Transform buttonImages;

    private LevelsSetController levelsSetController;

    public void Initialise(
        IScreensSceneGod screensSceneGod,
        ISingleSoundPlayer soundPlayer,
        IDataProvider dataProvider,
        int numOfLevelsUnlocked,
        bool completed)
    {
        //Most of side quest scripts will need to be modified once side quest manager is done
        _screensSceneGod = screensSceneGod;
        _staticData = dataProvider.StaticData;
        levelsSetController = transform.parent.GetComponent<LevelsSetController>();

        if (levelsSetController == null)
            Debug.LogError("LevelsSetController component was not found");

        base.Initialise(soundPlayer);
        requiredLevel = _staticData.SideQuests[sideQuestID].UnlockRequirementLevel;
        requiredSideQuestID = _staticData.SideQuests[sideQuestID].RequiredSideQuestID;

        completed = dataProvider.GameModel.IsSideQuestCompleted(sideQuestID);

        if (requiredSideQuestID != -1)
            enabled = (numOfLevelsUnlocked >= requiredLevel) && dataProvider.GameModel.IsSideQuestCompleted(requiredSideQuestID);
        else
            enabled = numOfLevelsUnlocked >= requiredLevel;

        Enabled = enabled;
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
        Debug.Log($"SideQuestButton clicked for sideQuestID: {sideQuestID}");

        int firstLevelOfStage = -1;
        if (levelsSetController != null)
            firstLevelOfStage = levelsSetController.firstLevelIndex;
        _screensSceneGod.GoToSideQuestTrashScreen(sideQuestID, firstLevelOfStage);
    }
}
