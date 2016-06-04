using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class StatsPopout : MonoBehaviour {

	public GameObject parent;
	public GameObject handle;
	public List<GameObject> components;

	private bool isMouseOver;
	private float posStart;
	private float posEnd;
	private float handleAlpha;

	void Start () {
		handleAlpha = 1f; // No opacity
		isMouseOver = false;
		posStart = parent.GetComponent<RectTransform>().anchoredPosition.x;
		posEnd = GetComponent<RectTransform>().sizeDelta.x - handle.GetComponent<RectTransform>().sizeDelta.x;
	}
	
	public void EnterPanel() {
		isMouseOver = true;
		StartCoroutine(EnterCoroutine());
	}

	public void ExitPanel() {
		isMouseOver = false;
		StartCoroutine(ExitCoroutine());
	}

	private void HandleColor(float x) {
		// Get the opacity ratio
		handleAlpha = Mathf.Abs((posStart+posEnd-x)/posEnd);
		// Change handle opacity
		handle.GetComponent<Image>().color = new Color(
			handle.GetComponent<Image>().color.r,
			handle.GetComponent<Image>().color.g,
			handle.GetComponent<Image>().color.b,
			handleAlpha);
		foreach (Transform child in handle.transform) {
			// Change opacity of its children (those that have Color, anyway)
			if (child.name == "Image") {
				child.GetComponent<Image>().color = new Color(
					child.GetComponent<Image>().color.r,
					child.GetComponent<Image>().color.g,
					child.GetComponent<Image>().color.b,
					handleAlpha);
			} else if (child.name == "Text") {
				child.GetComponent<Text>().color = new Color(
					child.GetComponent<Text>().color.r,
					child.GetComponent<Text>().color.g,
					child.GetComponent<Text>().color.b,
					handleAlpha);
			}
		}
	}

	private IEnumerator EnterCoroutine() {
		// Ensure the opposite routine isn't running
		StopCoroutine(ExitCoroutine());

		RectTransform p = parent.GetComponent<RectTransform>();

		while (p.anchoredPosition.x < posStart + posEnd - 0.05f && isMouseOver) {
			// Loop stops when end position is close to 0 or if mouse leaves
			// Position is smoothed (interpolated) for effect
			float x = Mathf.Lerp(p.anchoredPosition.x, posStart + posEnd, 0.2f);
			p.anchoredPosition = new Vector2(x, p.anchoredPosition.y);
			// Handle fades out as the menu comes in, relative to menu position.
			HandleColor(x);
			// Delay keeps the visuals working
			yield return new WaitForSeconds(Time.deltaTime);
		}
		yield return new WaitForSeconds(0f);
	}

	private IEnumerator ExitCoroutine() {
		// Same as EnterRoutine, except position is different and loop ends on mouse enter.
		StopCoroutine(EnterCoroutine());

		RectTransform p = parent.GetComponent<RectTransform>();

		while (p.anchoredPosition.x > posStart + 0.05f && !isMouseOver) {
			float x = Mathf.Lerp(p.anchoredPosition.x, posStart, 0.2f);
			p.anchoredPosition = new Vector2(x, p.anchoredPosition.y);
			//parent.transform.localPosition = new Vector3(x,
			//	parent.transform.localPosition.y, parent.transform.localPosition.z);
			HandleColor(x);
			yield return new WaitForSeconds(Time.deltaTime);
		}
		yield return new WaitForSeconds(0f);
	}
}
