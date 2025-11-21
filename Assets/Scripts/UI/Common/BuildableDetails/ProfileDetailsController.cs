using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.PostBattleScreen;
using BattleCruisers.UI;
using BattleCruisers.UI.ScreensScene;
using BattleCruisers.UI.ScreensScene.BattleHubScreen;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Fetchers.Sprites;
using BattleCruisers.Utils.Localisation;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class ProfileDetailsController : MonoBehaviour
{
    [Header("Heckles")]
    public GameObject HeckleSelector;
    public CanvasGroupButton EditHeckleButton;
    public CanvasGroupButton SelectHeckleButton;
    public Text HeckleText;

    public Button[] HeckleButtons;
    public Text[] HecklePreviews;
    public GameObject[] HecklePreviewHighlights;
    public GameObject HeckleContainer;
    public GameObject HeckleButonPrefab;

    [Header("Exos")]

    public GameObject ExoSelectorPanel;

    public CanvasGroupButton EditExoButton;

    public CanvasGroupButton SelectExoButton;

    public CanvasGroupButton LeftCancelZone;
    public CanvasGroupButton RightCancelZone;

    GameObject currentCaptainRender;

    public GameObject CaptainCamera;
    public Transform CaptainRenderContainer;
    public GameObject ExoContainer;
    public GameObject ExoButonPrefab;
    public Image RibbonIcon;
    public Image RibbonIconHighlight;

    [Header("Elo")]
    public Text EloText;
    public GameObject EloContainer;
    [Header("Bounty")]
    public Text BountyText;
    public GameObject BountyContainer;

    [Header("Player Name")]
    public Text PlayerNameText;
    public Button EditNameButton;
    public InputNamePopupPanelController NameInputOverlay;

    [Header("Player Rank")]
    public Image RankImage;
    public Text RankNameText;

    [Header("Lifetime Destruction")]
    public Text LifetimeDestructionValue;
    public Slider RankProgress;
    public Text RankMinValue;
    public Text RankMaxValue;

    SingleSoundPlayer SoundPlayer;

    List<CaptainSelectionItemController> instantiatedExoItems = new List<CaptainSelectionItemController>();
    List<HeckleItemContainerV3> intstantiatedHeckleItems = new List<HeckleItemContainerV3>();

    Sprite[] exoSprites;

    int currentExoIndex = -1;
    int currentHeckleIndex = -1;

    int currentHeckleSlotIndex = 0;
    int[] exoIndexToItemIndex;
    int[] heckleIndexToItemIndex;

    string million, billion, trillion, quadrillion;

    public async Task Initialize(SingleSoundPlayer soundPlayer)
    {
        List<Task<Sprite>> loadTasks = new List<Task<Sprite>>();

        for (int i = 0; i < StaticData.Captains.Count; i++)
            loadTasks.Add(SpriteFetcher.GetSpriteAsync($"{SpritePaths.ExoImagesPath}NPC-{i.ToString("00")}.png"));

        million = LocTableCache.CommonTable.GetString("Million");
        billion = LocTableCache.CommonTable.GetString("Billion");
        trillion = LocTableCache.CommonTable.GetString("Trillion");
        quadrillion = LocTableCache.CommonTable.GetString("Quadrillion");

        exoSprites = await Task.WhenAll(loadTasks);

        SoundPlayer = soundPlayer;

        Helper.AssertIsNotNull(HeckleSelector, ExoSelectorPanel, EditHeckleButton, EditExoButton,
                               SelectExoButton, SelectHeckleButton, LeftCancelZone, RightCancelZone,
                               EloContainer, HeckleText, HeckleButtons, HecklePreviews,
                               HecklePreviewHighlights, CaptainCamera, CaptainRenderContainer, ExoContainer,
                               HeckleContainer, ExoButonPrefab, HeckleButonPrefab, EloText,
                               PlayerNameText, EditNameButton, NameInputOverlay, RankImage,
                               RankNameText, BountyText, BountyContainer);

        Assert.IsTrue(HeckleButtons.Length == 3);
        Assert.IsTrue(HecklePreviews.Length == 3);
        Assert.IsTrue(HecklePreviewHighlights.Length == 3);

        currentHeckleIndex = DataProvider.GameModel.PlayerLoadout.CurrentHeckles[0];

        HecklePreviewHighlights[0].SetActive(false);
        HecklePreviewHighlights[1].SetActive(false);
        HecklePreviewHighlights[2].SetActive(false);

        EditHeckleButton.Initialise(soundPlayer, ShowHeckleSelector);
        EditExoButton.Initialise(soundPlayer, ShowExoSelector);

        SelectExoButton.Initialise(soundPlayer, SelectExo);
        SelectHeckleButton.Initialise(soundPlayer, SelectHeckle);

        LeftCancelZone.Initialise(soundPlayer, CancelSelectors);
        RightCancelZone.Initialise(soundPlayer, CancelSelectors);

        HeckleButtons[0].onClick.AddListener(() => ChangeHeckleSlot(0));
        HeckleButtons[1].onClick.AddListener(() => ChangeHeckleSlot(1));
        HeckleButtons[2].onClick.AddListener(() => ChangeHeckleSlot(2));

        EditNameButton.onClick.AddListener(ShowNameInputOverlay);
        NameInputOverlay.Initialise(soundPlayer);
        InputNamePopupPanelController.NameChangedCallback.AddListener(UpdatePlayerName);

        long score = DataProvider.GameModel.LifetimeDestructionScore;
        int rank = DestructionRanker.CalculateRank(score);

        RankImage.sprite = await SpriteFetcher.GetSpriteAsync($"{SpritePaths.RankImagesPath}Rank{rank}.png");
        RankNameText.text = LocTableCache.CommonTable.GetString($"Rank{rank}");
        long minRankValue = DestructionRanker.CalculateLevelXP(rank);
        long nextRankValue = DestructionRanker.CalculateLevelXP(1 + rank);

        RankMinValue.text = FormatNumber(minRankValue);
        if (rank != 33)
            RankMaxValue.text = FormatNumber(nextRankValue);
        else
            RankMaxValue.text = "---";

        RankProgress.value = Mathf.Min(1000, (score - minRankValue) / (nextRankValue - minRankValue) * 1000);

        int exoIndex = int.Parse(DataProvider.GameModel.PlayerLoadout.CurrentCaptain.PrefabName[10..]);

        RibbonIcon.sprite = exoSprites[exoIndex];
        RibbonIconHighlight.sprite = exoSprites[exoIndex];
    }

    public void UpdatePlayerName()
    {
        PlayerNameText.text = DataProvider.GameModel.PlayerName;
    }

    public void ShowNameInputOverlay()
    {
        NameInputOverlay.gameObject.SetActive(true);
    }

    public void ShowProfile()
    {
        ChangeHeckleSlot(0);
        CancelSelectors();

        gameObject.SetActive(true);
        CaptainCamera.SetActive(true);

        ShowCurrentCaptain();
        ShowHecklePreviews();

        EloText.text = DataProvider.GameModel.BattleWinScore.ToString("F0");
#if !ENABLE_BOUNTIES
        BountyContainer.SetActive(false);
#endif
#if ENABLE_BOUNTIES
        BountyText.text = DataProvider.GameModel.Bounty.ToString("F0");
#endif
        UpdatePlayerName();
    }

    void ShowHeckleSelector()
    {
        EloContainer.SetActive(false);
        BountyContainer.SetActive(false);
        ExoSelectorPanel.SetActive(false);
        HeckleSelector.SetActive(true);

        LeftCancelZone.gameObject.SetActive(true);
        RightCancelZone.gameObject.SetActive(true);

        DisplayHeckleList();
    }

    void ShowExoSelector()
    {
        EloContainer.SetActive(false);
        BountyContainer.SetActive(false);
        HeckleSelector.SetActive(false);
        ExoSelectorPanel.SetActive(true);

        LeftCancelZone.gameObject.SetActive(true);
        RightCancelZone.gameObject.SetActive(true);

        DisplayExoList();
    }

    void CancelSelectors()
    {
        LeftCancelZone.gameObject.SetActive(false);
        RightCancelZone.gameObject.SetActive(false);
        EloContainer.SetActive(true);
#if ENABLE_BOUNTIES
        BountyContainer.SetActive(true);
#endif
        HeckleSelector.SetActive(false);
        ExoSelectorPanel.SetActive(false);

        SelectHeckleButton.gameObject.SetActive(false);

        if (exoIndexToItemIndex != null)
            instantiatedExoItems[exoIndexToItemIndex[currentExoIndex]].ClickedFeedback.SetActive(false);

        currentExoIndex = int.Parse(DataProvider.GameModel.PlayerLoadout.CurrentCaptain.PrefabName[10..]);
        currentHeckleIndex = DataProvider.GameModel.PlayerLoadout.CurrentHeckles[currentHeckleSlotIndex];

        ShowHeckleText(currentHeckleIndex);
        ShowCurrentCaptain();
    }

    void ShowHecklePreviews()
    {
        int hecklesLimit = Mathf.Min(3, DataProvider.GameModel.PlayerLoadout.CurrentHeckles.Count);

        for (int i = 0; i < hecklesLimit; i++)
        {
            int heckleIndex = DataProvider.GameModel.PlayerLoadout.CurrentHeckles[i];
            HeckleData heckleData = StaticData.Heckles[heckleIndex];
            HecklePreviews[i].text = LocTableCache.HecklesTable.GetString(heckleData.StringKeyBase);
            if (i == 0)
            {
                ShowHeckleText(heckleIndex);
                currentHeckleIndex = heckleIndex;
            }
        }
    }

    void DisplayExoList()
    {
        List<int> exos = DataProvider.GameModel.PurchasedExos;
        exos.Sort();
        foreach (CaptainSelectionItemController item in instantiatedExoItems)
            DestroyImmediate(item.gameObject);

        instantiatedExoItems.Clear();

        exoIndexToItemIndex = new int[exos[^1] + 1];

        for (int i = 0; i < exos.Count; i++)
        {
            int exoIndex = exos[i];
            exoIndexToItemIndex[exoIndex] = i;

            CaptainSelectionItemController itemController = Instantiate(ExoButonPrefab, ExoContainer.transform).GetComponent<CaptainSelectionItemController>();
            instantiatedExoItems.Add(itemController);

            itemController.StaticInitialise(SoundPlayer,
                                            exoSprites[exoIndex],
                                            this,
                                            exoIndex);
            if (currentExoIndex == -1
             && StaticData.Captains[exoIndex].NameStringKeyBase == DataProvider.GameModel.PlayerLoadout.CurrentCaptain.PrefabName)
                currentExoIndex = exoIndex;
        }

        instantiatedExoItems[exoIndexToItemIndex[currentExoIndex]].ClickedFeedback.SetActive(true);
    }

    public void DisplayHeckleList()
    {
        List<int> heckles = DataProvider.GameModel.PurchasedHeckles.GetRange(0, DataProvider.GameModel.PurchasedHeckles.Count);
        heckles.Sort();

        foreach (HeckleItemContainerV3 item in intstantiatedHeckleItems)
            DestroyImmediate(item.gameObject);
        intstantiatedHeckleItems.Clear();

        List<int> heckleSlots = new List<int> { 0, 1, 2 };
        heckleSlots.Remove(currentHeckleSlotIndex);

        if (DataProvider.GameModel.PlayerLoadout.CurrentHeckles.Count > heckleSlots[0])
            heckles.Remove(DataProvider.GameModel.PlayerLoadout.CurrentHeckles[heckleSlots[0]]);
        if (DataProvider.GameModel.PlayerLoadout.CurrentHeckles.Count > heckleSlots[1])
            heckles.Remove(DataProvider.GameModel.PlayerLoadout.CurrentHeckles[heckleSlots[1]]);

        heckleIndexToItemIndex = new int[heckles[^1] + 1];

        for (int i = 0; i < heckles.Count; i++)
        {
            int heckleIndex = heckles[i];
            heckleIndexToItemIndex[heckleIndex] = i;

            HeckleItemContainerV3 itemController = Instantiate(HeckleButonPrefab, HeckleContainer.transform).GetComponent<HeckleItemContainerV3>();
            intstantiatedHeckleItems.Add(itemController);

            itemController.StaticInitialise(SoundPlayer, this, heckleIndex);
        }

        intstantiatedHeckleItems[heckleIndexToItemIndex[currentHeckleIndex]].ClickedFeedback.SetActive(true);
    }

    void ChangeHeckleSlot(int newSlotIndex)
    {
        newSlotIndex = Mathf.Clamp(newSlotIndex, 0, 2);
        HecklePreviewHighlights[currentHeckleSlotIndex].SetActive(false);
        HecklePreviewHighlights[newSlotIndex].SetActive(true);
        currentHeckleSlotIndex = newSlotIndex;
        currentHeckleIndex = DataProvider.GameModel.PlayerLoadout.CurrentHeckles[newSlotIndex];

        ShowHeckleText(currentHeckleIndex);

        DisplayHeckleList();
    }

    public void ShowCaptain(int newExoIndex)
    {
        if (currentCaptainRender != null)
            Destroy(currentCaptainRender);
        CaptainExoKey exoKey = StaticPrefabKeys.CaptainExos.GetCaptainExoKey(newExoIndex);
        CaptainExo playerExo = Instantiate(PrefabFactory.GetCaptainExo(exoKey), CaptainRenderContainer);
        playerExo.gameObject.transform.localScale = Vector3.one * 0.5f;
        currentCaptainRender = playerExo.gameObject;

        instantiatedExoItems[exoIndexToItemIndex[currentExoIndex]].ClickedFeedback.SetActive(false);
        instantiatedExoItems[exoIndexToItemIndex[newExoIndex]].ClickedFeedback.SetActive(true);
        currentExoIndex = newExoIndex;
    }

    void ShowCurrentCaptain()
    {
        if (currentCaptainRender != null)
            Destroy(currentCaptainRender);
        CaptainExo playerExo = Instantiate(PrefabFactory.GetCaptainExo(DataProvider.GameModel.PlayerLoadout.CurrentCaptain), CaptainRenderContainer);
        playerExo.gameObject.transform.localScale = Vector3.one * 0.5f;
        currentCaptainRender = playerExo.gameObject;
    }

    public void ShowHeckle(int newHeckleIndex)
    {
        ShowHeckleText(newHeckleIndex);

        intstantiatedHeckleItems[heckleIndexToItemIndex[currentHeckleIndex]].ClickedFeedback.SetActive(false);
        intstantiatedHeckleItems[heckleIndexToItemIndex[newHeckleIndex]].ClickedFeedback.SetActive(true);
        currentHeckleIndex = newHeckleIndex;

        if (currentHeckleIndex != DataProvider.GameModel.PlayerLoadout.CurrentHeckles[currentHeckleSlotIndex])
            SelectHeckleButton.gameObject.SetActive(true);
        else
            SelectHeckleButton.gameObject.SetActive(false);
    }

    public void ShowHeckleText(int heckleIndex)
    {
        HeckleText.text = LocTableCache.HecklesTable.GetString($"Heckle{heckleIndex.ToString("000")}");
    }

    void SelectExo()
    {
        DataProvider.GameModel.PlayerLoadout.CurrentCaptain = new CaptainExoKey(StaticData.Captains[currentExoIndex].NameStringKeyBase);
        RibbonIcon.sprite = exoSprites[currentExoIndex];
        RibbonIconHighlight.sprite = exoSprites[currentExoIndex];

        DataProvider.SaveGame();
        _ = DataProvider.CloudSave();

        CancelSelectors();
    }

    void SelectHeckle()
    {
        DataProvider.GameModel.PlayerLoadout.CurrentHeckles[currentHeckleSlotIndex] = currentHeckleIndex;
        HecklePreviews[currentHeckleSlotIndex].text = LocTableCache.HecklesTable.GetString($"Heckle{currentHeckleIndex.ToString("000")}");
        DataProvider.SaveGame();
        _ = DataProvider.CloudSave();

        CancelSelectors();
    }

    public void HideDetails()
    {
        HecklePreviewHighlights[0].SetActive(false);
        HecklePreviewHighlights[1].SetActive(false);
        HecklePreviewHighlights[2].SetActive(false);

        CancelSelectors();
        Destroy(currentCaptainRender);
        gameObject.SetActive(false);
    }

    //taken from https://stackoverflow.com/questions/30180672/string-format-numbers-to-millions-thousands-with-rounding
    string FormatNumber(long num)
    {
        num = num * 1000;
        long i = (long)Math.Pow(10, (int)Math.Max(0, Math.Log10(num) - 2));
        num = num / i * i;
        if (num >= 1000000000000)
            return "$" + (num / 1000000000000D).ToString("0.##") + " " + quadrillion;
        if (num >= 1000000000)
            return "$" + (num / 1000000000D).ToString("0.##") + " " + trillion;
        if (num >= 1000000)
            return "$" + (num / 1000000D).ToString("0.##") + " " + billion;
        if (num >= 1000)
            return "$" + (num / 1000D).ToString("0.##") + " " + million;

        return "$" + num.ToString("#,0");
    }
}
