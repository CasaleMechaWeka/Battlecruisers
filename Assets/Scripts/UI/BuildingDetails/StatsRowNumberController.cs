using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsRowNumberController : MonoBehaviour 
{
	public Text rowLabel;
	public Text rowValue;

	public void Initialise(string label, string value)
	{
		Debug.Log($"StatsRowNumberController.Initialise() label: {label}  value: {value}");

		rowLabel.text = label;
		rowValue.text = value;
	}
}
