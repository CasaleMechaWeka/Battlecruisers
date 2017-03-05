using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IUIFactory
{
	GameObject CreatePanel(bool isActive);
}

public class UIFactory : MonoBehaviour, IUIFactory
{
	private Canvas _canvas;
	private GameObject _panelPrefab;

	public void Initialize(Canvas canvas, GameObject panelPrefab)
	{
		_canvas = canvas;
		_panelPrefab = panelPrefab;
	}

	public GameObject CreatePanel(bool isActive)
	{
		GameObject panel = Instantiate(_panelPrefab);
		panel.SetActive(isActive);
		panel.transform.SetParent(_canvas.transform);
		RectTransform rectTransform = panel.GetComponent<RectTransform>();
		rectTransform.anchoredPosition = new Vector2(0, 0);
		return panel;
	}
}
