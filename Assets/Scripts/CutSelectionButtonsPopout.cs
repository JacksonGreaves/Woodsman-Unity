using UnityEngine;
using System.Collections;

public class CutSelectionButtonsPopout : MonoBehaviour {

	private bool isPoppedOut;
	private Vector2 startingPos;
	private float popoutY = 64f; // Destination Y - change if UI elements get moved around!

	void Start () {
		isPoppedOut = false;
		// StatsPopout did this by specifying a parent and getting its x value,
		// but in hindsight, it's an unnecessary step.
		startingPos = new Vector2(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y);
	}

	public void SetSelection(bool b) {
		isPoppedOut = b;
		if (isPoppedOut)
			StartCoroutine(ShowCoroutine());
		else
			StartCoroutine(HideCoroutine());
	}

	public void ToggleSelection () {
		isPoppedOut = !isPoppedOut;
		if (isPoppedOut)
			StartCoroutine(ShowCoroutine());
		else
			StartCoroutine(HideCoroutine());
	}

	private IEnumerator ShowCoroutine() {
		StopCoroutine(HideCoroutine());
		while (gameObject.transform.localPosition.y > popoutY + 0.05 && isPoppedOut) {
			float y = Mathf.Lerp(gameObject.transform.localPosition.y, popoutY, 0.2f);
			gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x,
				y, gameObject.transform.localPosition.z);
			yield return new WaitForSeconds(Time.deltaTime);
		}
		yield return new WaitForSeconds(0f);
	}

	private IEnumerator HideCoroutine() {
		StopCoroutine(ShowCoroutine());
		while (gameObject.transform.localPosition.y < startingPos.y - 0.05 && !isPoppedOut) {
			float y = Mathf.Lerp(gameObject.transform.localPosition.y, startingPos.y, 0.2f);
			gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x,
				y, gameObject.transform.localPosition.z);
			yield return new WaitForSeconds(Time.deltaTime);
		}
		yield return new WaitForSeconds(0f);
	}
}
