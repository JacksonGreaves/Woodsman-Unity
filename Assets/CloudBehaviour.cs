using UnityEngine;
using System.Collections;

public class CloudBehaviour : MonoBehaviour {


	private float startAlpha;
	private float alpha;
	private Color color;

	public float alphaFloor;
	public float generalSpeed;
	public float speedVariance;
	public float generalHeight;
	public float heightVariance;
	private float speed;

	void Start () {
		color = GetComponent<Renderer>().material.color;
		startAlpha = color.a;
		alpha = startAlpha;
		ResetObjectPlacement();
		transform.position = new Vector3(transform.position.x, transform.position.y, Random.Range(-100f, 100f));
	}

	void FixedUpdate () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100f)) {
			if (hit.transform.tag == "Cloud") {
				if (alpha != alphaFloor) {
					alpha = Mathf.Lerp(alpha, alphaFloor, 0.1f);
				}
			} else {
				if (alpha != startAlpha) {
					alpha = Mathf.Lerp(alpha, startAlpha, 0.1f);
				}
			}
		}

		if (alpha != startAlpha && alpha != alphaFloor) {
			GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, alpha);
		}

		GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().transform.up * -speed;

		if (transform.position.z > 150) {
			ResetObjectPlacement();
		}
	}

	private void ResetObjectPlacement() {
		transform.position = new Vector3(
			Random.Range(20f, 80f),
			Random.Range(generalHeight-heightVariance, generalHeight+heightVariance),
			Random.Range(-200f, -50f));
		transform.rotation = Quaternion.Euler(270f, Random.Range(-5f, 5f), 0f);
		transform.localScale = new Vector3(Random.Range(2f, 3f), Random.Range(2f, 3f), Random.Range(2f, 3f));
		speed = Random.Range(generalSpeed-speedVariance, generalSpeed+speedVariance);
	}
}
