using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideDiscordLink : MonoBehaviour
{
    public GameObject discordLink;
    // Start is called before the first frame update
    void Start()
    {
#if NO_SOCIAL_MEDIA
        DisableDiscordLink();
#endif
    }

    private void DisableDiscordLink()
    {
        discordLink.gameObject.SetActive(false);
    }
}
