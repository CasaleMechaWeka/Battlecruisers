using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingsMenuController : MonoBehaviour 
{
	// FELIX  Don't really need these fields?
	private BuildMenuController _buildMenu;
	private Button _buttonPrefab;
	private IList<IBuilding> _buildings;

	public void Initialize(
		BuildMenuController buildMenu,
		Button buttonPrefab,
		Button backButtonPrefab,
		IList<IBuilding> buildings)
	{
		_buildMenu = buildMenu;
		_buttonPrefab = buttonPrefab;
		_buildings = buildings;

		// Create building buttons
		HorizontalLayoutGroup buttonGroup = GetComponent<HorizontalLayoutGroup>();

		for (int i = 0; i < _buildings.Count; ++i)
		{
			Button button = (Button)Instantiate(buttonPrefab);
			button.transform.SetParent(buttonGroup.transform, worldPositionStays: false);
			button.GetComponent<BuildingButtonController>().Initialize(_buildings[i], buildMenu);
		}

		// Create a back butotn
		Button backButton = (Button)Instantiate(backButtonPrefab);
		backButton.transform.SetParent(buttonGroup.transform, worldPositionStays: false);
		backButton.GetComponent<BackButtonController>().Initialize(buildMenu);
	}

	// Use this for initialization
	void Start () 
	{
		Debug.Log("BuildMenuPanelController.Start()");
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
