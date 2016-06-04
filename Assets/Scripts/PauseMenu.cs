using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	public GameObject blackScreen;

	private bool isShowing;

	private Vector2 offScreen = new Vector2(-1000f, -1000f);
	private Vector2 middle;

	void Start () {
		middle = new Vector2(GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta.x/2,
			GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta.y/2);
	}

	public void ToggleShowMenu() {
		ShowMenu(!isShowing);
	}

	public void ShowMenu(bool b) {
		isShowing = b;

		if (isShowing) {
			GetComponent<RectTransform>().anchoredPosition = middle;
			blackScreen.SetActive(true);
		} else {
			GetComponent<RectTransform>().anchoredPosition = offScreen;
			blackScreen.SetActive(false);
		}
	}
}
