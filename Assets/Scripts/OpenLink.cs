using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLink : MonoBehaviour
{
    public string linkString;
    
    public void Open()
    {
        Application.OpenURL(linkString);
    }
}
