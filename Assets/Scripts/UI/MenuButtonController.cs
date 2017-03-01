using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonController : MonoBehaviour 
{
	public void Initialize(string name, Action onClick)
	{
		Button button = GetComponent<Button>();
		button.GetComponentInChildren<Text>().text = name;
		button.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick(){
		Debug.Log ("You have clicked the button!");
	}
}
