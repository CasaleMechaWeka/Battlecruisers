using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class XPBar : MonoBehaviour
{
    public Slider levelBar;

    private void OnEnable()
    {
        if(levelBar == null)
        {
            levelBar = GetComponentInChildren<Slider>();
        }
    }

    public void setValues(int currentXP, int nextLevelXP)
    {
        levelBar.maxValue = nextLevelXP;
        levelBar.value = currentXP;
    }
}
