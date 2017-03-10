using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButtonController : MonoBehaviour 
{
	public Image buildingImage;
	public Image slotImage;

	public void Initialize(Building building, IBuildMenuController buildMenuController, Sprite slotSprite)
	{
		Button button = GetComponent<Button>();
		button.GetComponentInChildren<Text>().text = building.buildingName;
		button.onClick.AddListener(() => buildMenuController.SelectBuilding(building));

		SpriteRenderer spriteRenderer = building.GetComponent<SpriteRenderer>();
		buildingImage.sprite = spriteRenderer.sprite;
		slotImage.sprite = slotSprite;
	}
}
