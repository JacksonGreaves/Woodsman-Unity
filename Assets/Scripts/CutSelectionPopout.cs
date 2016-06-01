using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CutSelectionPopout : MonoBehaviour {


	private bool isPoppedOut;
	private Vector2 startingPos;
	private float popoutX = 0f; // Destination X - change if UI elements get moved around!

	void Start () {
		isPoppedOut = false;
		// StatsPopout did this by specifying a parent and getting its x value,
		// but in hindsight, it's an unnecessary step.
		startingPos = new Vector2(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y);
	}

	public void SetSelected (bool b) {
		if (b != isPoppedOut) {
			isPoppedOut = b;
			if (isPoppedOut) {
				foreach (Transform child in gameObject.transform) {
					if (child.name == "CutSelectionButton") {
						child.GetComponent<Button>().enabled = true;
					}
				}
				StartCoroutine(ShowCoroutine());
			} else {
				foreach (Transform child in gameObject.transform) {
					if (child.name == "CutSelectionButton") {
						child.GetComponent<Button>().enabled = false;
					}
				}
				StartCoroutine(HideCoroutine());
			}
		}
	}

	private IEnumerator ShowCoroutine() {
		StopCoroutine(HideCoroutine());
		while (gameObject.transform.localPosition.x > popoutX + 0.05 && isPoppedOut) {
			float x = Mathf.Lerp(gameObject.transform.localPosition.x, popoutX, 0.2f);
			gameObject.transform.localPosition = new Vector3(x,
				gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
			yield return new WaitForSeconds(Time.deltaTime);
		}
		yield return new WaitForSeconds(0f);
	}

	private IEnumerator HideCoroutine() {
		StopCoroutine(ShowCoroutine());
		while (gameObject.transform.localPosition.x < startingPos.x - 0.05 && !isPoppedOut) {
			float x = Mathf.Lerp(gameObject.transform.localPosition.x, startingPos.x, 0.2f);
			gameObject.transform.localPosition = new Vector3(x,
				gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
			yield return new WaitForSeconds(Time.deltaTime);
		}
		yield return new WaitForSeconds(0f);
	}
	
}
