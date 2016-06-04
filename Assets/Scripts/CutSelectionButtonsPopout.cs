using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CutSelectionButtonsPopout : MonoBehaviour {

	public GameObject mask;
	public GameObject pauseButton;

	private bool isPoppedOut;
	private float posStart;
	private float posEnd;

	void Start () {
		isPoppedOut = false;
		posStart = GetComponent<RectTransform>().anchoredPosition.y;
		posEnd = posStart - mask.GetComponent<RectTransform>().sizeDelta.y + pauseButton.GetComponent<RectTransform>().sizeDelta.y;
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

		RectTransform rect = GetComponent<RectTransform>();

		while (rect.anchoredPosition.y > posEnd + 0.05f && isPoppedOut) {
			float y = Mathf.Lerp(rect.anchoredPosition.y, posEnd, 0.2f);
			rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, y);
			yield return new WaitForSeconds(Time.deltaTime);
		}
		yield return new WaitForSeconds(0f);
	}

	private IEnumerator HideCoroutine() {
		StopCoroutine(ShowCoroutine());

		RectTransform rect = GetComponent<RectTransform>();

		while (rect.anchoredPosition.y < posStart - 0.05f && !isPoppedOut) {
			float y = Mathf.Lerp(rect.anchoredPosition.y, posStart, 0.2f);
			rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, y);
			yield return new WaitForSeconds(Time.deltaTime);
		}
		yield return new WaitForSeconds(0f);
	}
}
