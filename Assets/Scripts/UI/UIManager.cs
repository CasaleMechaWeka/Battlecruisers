using BattleCruisers.UI;
using BattleCruisers.UI.BuildMenus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.UI
{
	public class UIManager : MonoBehaviour 
	{
		public CameraController cameraController;
		public BuildMenuController buildMenuController;
		public HealthBarController friendlyCruiserHealthBar;
		public HealthBarController enemyCruiserHealthBar;
		
		// Use this for initialization
		void Start () 
		{
			friendlyCruiserHealthBar.gameObject.SetActive(true);
			enemyCruiserHealthBar.gameObject.SetActive(false);

			cameraController.CameraTransitionStarted += OnCameraTransitionStarted;
			cameraController.CameraTransitionCompleted += OnCameraTransitionCompleted;
		}

		private void OnCameraTransitionStarted(object sender, CameraTransitionArgs e)
		{
			switch (e.Origin)
			{
				case CameraState.FriendlyCruiser:
					buildMenuController.HideBuildMenu();
					friendlyCruiserHealthBar.gameObject.SetActive(false);
					break;

				case CameraState.EnemyCruiser:
					enemyCruiserHealthBar.gameObject.SetActive(false);
					break;
			}
		}

		private void OnCameraTransitionCompleted(object sender, CameraTransitionArgs e)
		{
			switch (e.Destination)
			{
				case CameraState.FriendlyCruiser:
					buildMenuController.ShowBuildMenu();
					friendlyCruiserHealthBar.gameObject.SetActive(true);
					break;

				case CameraState.EnemyCruiser:
					enemyCruiserHealthBar.gameObject.SetActive(true);
					break;
			}
		}
	}
}