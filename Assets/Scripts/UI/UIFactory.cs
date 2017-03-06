using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IUIFactory
{
	GameObject CreatePanel(bool isActive);
	Button CreateBuildingCategoryButton(HorizontalLayoutGroup buttonParent, BuildingGroup group);
	Button CreateBuildingButton(HorizontalLayoutGroup buttonParent, Building building);
	Button CreateBackButton(HorizontalLayoutGroup buttonParent);
}

public class UIFactory : MonoBehaviour, IUIFactory
{
	private Canvas _canvas;

	public BuildMenuController buildMenu;
	public GameObject panelPrefab;
	public Button buildingCategoryButtonPrefab;
	public Button buildingButtonPrefab;
	public Button backButtonPrefab;

	public void Awake()
	{
		_canvas = GetComponent<Canvas>();
	}

	public GameObject CreatePanel(bool isActive)
	{
		GameObject panel = Instantiate(panelPrefab);
		panel.SetActive(isActive);
		panel.transform.SetParent(_canvas.transform);
		RectTransform rectTransform = panel.GetComponent<RectTransform>();
		rectTransform.anchoredPosition = new Vector2(0, 0);
		return panel;
	}

	public Button CreateBuildingCategoryButton(HorizontalLayoutGroup buttonParent, BuildingGroup group)
	{
		Button button = (Button)Instantiate(buildingCategoryButtonPrefab);
		button.transform.SetParent(buttonParent.transform, worldPositionStays: false);
		button.GetComponent<BuildingCategoryButton>().Initialize(group, buildMenu);
		return button;
	}

	public Button CreateBuildingButton(HorizontalLayoutGroup buttonParent, Building building)
	{
		Button button = (Button)Instantiate(buildingButtonPrefab);
		button.transform.SetParent(buttonParent.transform, worldPositionStays: false);
		button.GetComponent<BuildingButtonController>().Initialize(building, buildMenu);
		return button;
	}

	public Button CreateBackButton(HorizontalLayoutGroup buttonParent)
	{
		Button backButton = (Button)Instantiate(backButtonPrefab);
		backButton.transform.SetParent(buttonParent.transform, worldPositionStays: false);
		backButton.GetComponent<BackButtonController>().Initialize(buildMenu);
		return backButton;
	}
}
