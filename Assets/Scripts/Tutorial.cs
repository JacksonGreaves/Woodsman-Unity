using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

	public Canvas canvas;
	public GameData data;
	public CameraController camC;

	// GO.Find() can't find GO's that are initially inactive, so we need references here.
	public GameObject Tut1;
	public GameObject Tut2;
	public GameObject Tut3;
	public GameObject Tut4;
	public GameObject Tut5;
	public GameObject Tut6;
	public GameObject Tut7;
	public GameObject Tut8;
	public GameObject Tut9;

	private int currentStep;

	public void StartTutorial() {
		currentStep = 0;
		camC.canClick = false;
		data.Game_PauseResume(true);
		GameObject.Find("StoreButton").GetComponent<Button>().interactable = false;
		StartCoroutine(TutorialSteps());
	}

	private IEnumerator TutorialSteps() {
		while (true) {
			// Time.timeScale set to 0 creates some problems with coroutine waiting,
			// but for some reason, using a utility coroutine solves this issue.
			yield return WaitForRealSeconds(Time.deltaTime);
			if (Input.GetMouseButtonDown(0)) {
				currentStep += 1;
			}

			switch (currentStep) {
			case 0:
				Tut1.SetActive(true);
				break;
			case 1:
				Tut1.SetActive(false);
				Tut2.SetActive(true);
				break;
			case 2:
				Tut2.SetActive(false);
				Tut3.SetActive(true);
				GameObject.Find("CutSelection").GetComponent<CutSelectionPopout>().SetSelected(true);
				GameObject.Find("Buttons").GetComponent<CutSelectionButtonsPopout>().SetSelection(true);
				break;
			case 3:
				Tut3.SetActive(false);
				Tut4.SetActive(true);
				break;
			case 4:
				Tut4.SetActive(false);
				Tut5.SetActive(true);
				GameObject.Find("CutSelection").GetComponent<CutSelectionPopout>().SetSelected(false);
				GameObject.Find("Buttons").GetComponent<CutSelectionButtonsPopout>().SetSelection(false);
				GameObject.Find("StatsMouseOverHandler").GetComponent<StatsPopout>().EnterPanel();
				break;
			case 5:
				Tut5.SetActive(false);
				Tut6.SetActive(true);
				GameObject.Find("StatsMouseOverHandler").GetComponent<StatsPopout>().ExitPanel();
				break;
			case 6:
				Tut6.SetActive(false);
				Tut7.SetActive(true);
				break;
			case 7:
				Tut7.SetActive(false);
				Tut8.SetActive(true);
				GameObject.Find("StorePanel").GetComponent<StoreMenu>().ShowStore(true);
				break;
			case 8:
				Tut8.SetActive(false);
				Tut9.SetActive(true);
				GameObject.Find("StorePanel").GetComponent<StoreMenu>().ShowStore(false);
				GameObject.Find("StorePanel").GetComponent<StoreMenu>().EnableStoreButtons();
				break;
			case 9:
				Tut9.SetActive(false);
				data.Game_PauseResume(false);
				GameObject.Find("ContinueText").SetActive(false);
				camC.canClick = true;
				GameObject.Find("PausePanel").GetComponent<PauseMenu>().isEnabled = true;
				GameObject.Find("StoreButton").GetComponent<Button>().interactable = true;
				GameObject.Find("StorePanel").GetComponent<StoreMenu>().blackEnabled = true;
				gameObject.SetActive(false);
				break;
			}
		}
	}

	private static IEnumerator WaitForRealSeconds(float time) {
		float start = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < start + time) {
			yield return null;
		}
	}
}
