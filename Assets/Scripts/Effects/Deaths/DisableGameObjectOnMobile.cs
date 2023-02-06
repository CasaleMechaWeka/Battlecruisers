using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableGameObjectOnMobile : MonoBehaviour
{

    void Start()
    {
        if (UnityEngine.Application.platform != UnityEngine.RuntimePlatform.WindowsEditor &&
     UnityEngine.Application.platform != UnityEngine.RuntimePlatform.WindowsPlayer)
        {
                gameObject.SetActive(false);
        }
    }

}
