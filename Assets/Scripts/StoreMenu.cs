using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StoreMenu : MonoBehaviour {

	public GameObject blackScreen;
	public bool isEnabled;
	public bool blackEnabled;

	private Vector2 offScreen = new Vector2(-1000f, -1000f);
	private Vector2 middle;
	private bool buttonsEnabled;

	void Start () {
		isEnabled = false;
		blackEnabled = false;
		buttonsEnabled = false;
		middle = new Vector2(GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta.x/2,
			GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta.y/2);
	}

	public void ToggleStore() {
		ShowStore(!isEnabled);
	}

	public void ShowStore(bool b) {
		isEnabled = b;

		if (isEnabled) {
			GetComponent<RectTransform>().anchoredPosition = middle;
			blackScreen.SetActive(blackEnabled);
		} else {
			GetComponent<RectTransform>().anchoredPosition = offScreen;
			blackScreen.SetActive(false);
		}
	}

	public void ToggleStoreButtons() {
		buttonsEnabled = !buttonsEnabled;

		GameObject.Find("BuyWorkerButton").GetComponent<Button>().interactable = buttonsEnabled;
		GameObject.Find("BuyMachineButton").GetComponent<Button>().interactable = buttonsEnabled;
		GameObject.Find("BuyTeamButton").GetComponent<Button>().interactable = buttonsEnabled;
		GameObject.Find("StoreReturnButton").GetComponent<Button>().interactable = buttonsEnabled;
	}

	public void EnableStoreButtons() {
		GameObject.Find("BuyWorkerButton").GetComponent<Button>().interactable = true;
		GameObject.Find("BuyMachineButton").GetComponent<Button>().interactable = true;
		GameObject.Find("BuyTeamButton").GetComponent<Button>().interactable = true;
		GameObject.Find("StoreReturnButton").GetComponent<Button>().interactable = true;
	}

	public void DisableStoreButtons() {
		GameObject.Find("BuyWorkerButton").GetComponent<Button>().interactable = false;
		GameObject.Find("BuyMachineButton").GetComponent<Button>().interactable = false;
		GameObject.Find("BuyTeamButton").GetComponent<Button>().interactable = false;
		GameObject.Find("StoreReturnButton").GetComponent<Button>().interactable = false;
	}
}
