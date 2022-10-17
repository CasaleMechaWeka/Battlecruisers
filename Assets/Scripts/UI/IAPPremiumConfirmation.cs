using BattleCruisers.Data;
using BattleCruisers.UI;
using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class IAPPremiumConfirmation : MonoBehaviour
{
    [SerializeField]
    public AudioSource _uiAudioSource;
    private ISingleSoundPlayer _soundPlayer;
    public CanvasGroupButton Button_No;
    public CanvasGroupButton Button_Upgrade;
    public AdvertisingBannerScrollingText AdvertistingBanner;
    public IAPManager IAPController;
    public Text TextPrice;
    
    // Start is called before the first frame update
    void Start()
    {       
        _soundPlayer
        = new SingleSoundPlayer(
            new SoundFetcher(),
            new EffectVolumeAudioSource(
                new AudioSourceBC(_uiAudioSource),
                ApplicationModelProvider.ApplicationModel.DataProvider.SettingsManager, 1));
        Button_No.Initialise(_soundPlayer, Close);
        Button_Upgrade.Initialise(_soundPlayer, UpgradeToPremium);
    }

    void OnEnable() {
        Product premiumVersionProduct = IAPManager.instance.storeController.products.WithID("premium_version");
        if (premiumVersionProduct != null)
        {
            TextPrice.text = premiumVersionProduct.metadata.localizedPriceString;
        }

        AdvertistingBanner.stopAdvert();
    }

    private void Close() {
        Invoke("HideSelf", 0.25f);
    }

    private void HideSelf() {
        gameObject.SetActive(false);
        AdvertistingBanner.startAdvert();
    }

    public void UpgradeToPremium() {
        Debug.Log("upgradddding");
        Close();
        AdvertistingBanner.stopAdvert();
        IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
        applicationModel.DataProvider.GameModel.PremiumEdition = true;
        applicationModel.DataProvider.SaveGame();
    }

    public void UpgradeToPremiumFailed()
    {
        Debug.Log("upgradddding has failed please try again");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
