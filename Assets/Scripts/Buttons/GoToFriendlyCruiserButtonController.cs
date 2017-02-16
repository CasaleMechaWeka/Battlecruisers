using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GoToFriendlyCruiserButtonController : MonoBehaviour 
{
	public Button button;
	public CameraController cameraController;

	void Start () 
	{
		Button buttonInner = button.GetComponent<Button>();
		buttonInner.onClick.AddListener(OnClick);
	}

	void OnClick()
	{
		cameraController.FocusOnFriendlyCruiser();
	}
}
