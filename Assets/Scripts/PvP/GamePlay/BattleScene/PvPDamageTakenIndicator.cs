using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

//This class is used by the HealthDial when damage is taken to show a visual indication on
public class PvPDamageTakenIndicator : MonoBehaviour
{
    public Image image;
    public GameObject imageObject;
    private bool hidden = true;
    public PvPDamageTakenIndicator()
    {

    }

    public void initialise()
    {

    }

    public void updateDamageTakenIndicator()
    {
        if (hidden == true)
        {
            showDamageTaken();
            Invoke("hideDamageTaken", 0.5f);
        }
    }

    public void showDamageTaken()
    {
        imageObject.SetActive(true);
        hidden = false;
    }

    public void hideDamageTaken()
    {

        //Debug.Log("Color should be clear");
        imageObject.SetActive(false);
        //Debug.Log("COLOR SHOULD BE CLEAR");
        hidden = true;
    }

}
