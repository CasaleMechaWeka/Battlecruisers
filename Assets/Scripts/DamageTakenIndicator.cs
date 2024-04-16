using UnityEngine;
using UnityEngine.UI;

//This class is used by the HealthDial when damage is taken to show a visual indication on
public class DamageTakenIndicator : MonoBehaviour
{
    public Image image;
    public GameObject imageObject;
    private bool hidden = true;

    public void UpdateDamageTakenIndicator()
    {
        if (hidden == true)
        {
            ShowDamageTaken();
            Invoke("hideDamageTaken", 0.5f);
        }
    }

    public void ShowDamageTaken()
    {
        imageObject.SetActive(true);
        hidden = false;
    }

    public void HideDamageTaken()
    {

        //Debug.Log("Color should be clear");
        imageObject.SetActive(false);
        //Debug.Log("COLOR SHOULD BE CLEAR");
        hidden = true;
    }

}
