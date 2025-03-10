using UnityEngine;

public class OpenLink : MonoBehaviour
{
    public string linkString;

    public void Open()
    {
        Application.OpenURL(linkString);
    }
}
