using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TooltipHandler : MonoBehaviour {

	private RectTransform rect;

	private bool isVisible;
	private bool stateChanged;

	private Vector2 offScreen = new Vector2(-1000f, -1000f);
	private Vector2 side;
	/*
	 * (0,0) = Top Left
	 * (0,1) = Top Right
	 * (1,0) = Bot Left
	 * (1,1) = Bot Right
	*/

	private Text text;

	void Start () {
		rect = gameObject.GetComponent<RectTransform>();
		text = gameObject.GetComponentInChildren<Text>();
		isVisible = false;
		side = new Vector2(0,0);
	}
	
	void LateUpdate () {
		if (isVisible) {
			if (Input.mousePosition.x + rect.sizeDelta.x > GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta.x) {
				if (Input.mousePosition.y < rect.sizeDelta.y) {
					side = new Vector2(1,1);
				} else {
					side = new Vector2(0,1);
				}
			} else {
				if (Input.mousePosition.y < rect.sizeDelta.y) {
					side = new Vector2(1,0);
				} else {
					side = new Vector2(0,0);
				}
			}
			HandlePosition();
		} else {
			rect.anchoredPosition = offScreen;
		}
	}

	public void SetSelected(bool b) {
		isVisible = b;
	}

	public void SetTooltip(string tip) {
		if (tip == "Worker") {
			text.text = "Worker units work slowly, but take greater care of the landscape.";
		} else if (tip == "Machine") {
			text.text = "Katt units work at a standard rate, and normalize the speed bonus of the landscape.";
		} else if (tip == "Team") {
			text.text = "Team units work very quickly, but their actions devestate the trees' growback rate!";
		} else {
			text.text = "";
		}
	}

	private void HandlePosition() {
		if (side.x == 1) {
			if (side.y == 1) {
				// Bottom right
				rect.anchoredPosition = new Vector2(Input.mousePosition.x - rect.sizeDelta.x, Input.mousePosition.y + rect.sizeDelta.y);
			} else {
				// Bottom left
				rect.anchoredPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y + rect.sizeDelta.y);
			}
		} else {
			if (side.y == 1) {
				// Top right
				rect.anchoredPosition = new Vector2(Input.mousePosition.x - rect.sizeDelta.x, Input.mousePosition.y);
			} else {
				// Top left (default)
				rect.anchoredPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			}
		}
	}
}
