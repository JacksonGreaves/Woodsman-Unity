using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CutSelectionPopout : MonoBehaviour {

	public GameObject mask;

	private bool isPoppedOut;
	private float posStart;
	private float posEnd;

	void Start () {
		isPoppedOut = false;
		posStart = GetComponent<RectTransform>().anchoredPosition.x;
		posEnd = posStart - mask.GetComponent<RectTransform>().sizeDelta.x;
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

		RectTransform rect = GetComponent<RectTransform>();

		while (rect.anchoredPosition.x > posEnd + 0.05 && isPoppedOut) {
			float x = Mathf.Lerp(rect.anchoredPosition.x, posEnd, 0.2f);
			rect.anchoredPosition = new Vector2(x, rect.anchoredPosition.y);
			yield return new WaitForSeconds(Time.deltaTime);
		}
		yield return new WaitForSeconds(0f);
	}

	private IEnumerator HideCoroutine() {
		StopCoroutine(ShowCoroutine());

		RectTransform rect = GetComponent<RectTransform>();

		while (rect.anchoredPosition.x < posStart - 0.05 && !isPoppedOut) {
			float x = Mathf.Lerp(rect.anchoredPosition.x, posStart, 0.2f);
			rect.anchoredPosition = new Vector2(x, rect.anchoredPosition.y);
			yield return new WaitForSeconds(Time.deltaTime);
		}
		yield return new WaitForSeconds(0f);
	}
	
}
