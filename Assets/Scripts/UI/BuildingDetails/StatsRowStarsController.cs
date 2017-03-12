using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class StatsRowStarsController : MonoBehaviour 
{
	private Text _rowLabel;
	private Image[] _stars;

	private const int MIN_RATING = 0;
	private const int MAX_RATING = 5;

	void Start() 
	{
		_rowLabel = GetComponentInChildren<Text>();
		_stars = GetComponentsInChildren<Image>();
		Assert.IsTrue(_stars.Length == MAX_RATING);
	}

	public void Initialise(string statName, int statRating)
	{
		Debug.Log($"StatsRowStarsController.Initialise() statName: {statName}  statRating: {statRating}");

		_rowLabel.text = statName;

		for (int i = 0; i < _stars.Length; ++i)
		{
			Image star = _stars[i];
			star.gameObject.SetActive(i < statRating);
		}
	}
}
