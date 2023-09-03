using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideDiscordLink : MonoBehaviour
{
    public GameObject discordLink;
    // Start is called before the first frame update
    void Start()
    {
#if THIRD_PARTY_PUBLISHER
        DisableDiscordLink();
#endif
    }

    private void DisableDiscordLink()
    {
        discordLink.gameObject.SetActive(false);
    }
}
