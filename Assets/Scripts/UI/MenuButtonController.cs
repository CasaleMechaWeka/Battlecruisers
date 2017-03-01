using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MenuButtonController : MonoBehaviour 
{
	public void Initialize(string name, UnityAction onClick)
	{
		Button button = GetComponent<Button>();
		button.GetComponentInChildren<Text>().text = name;
		button.onClick.AddListener(onClick);
	}
}
