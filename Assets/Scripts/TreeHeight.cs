using UnityEngine;
using System.Collections;

public class TreeHeight : MonoBehaviour {

	void Start () {
		transform.Find("Branches").transform.localPosition = new Vector3(0f, Random.Range(0.7f, 1.5f), 0f);
		gameObject.isStatic = true;
		foreach (Transform child in transform) {
			child.gameObject.isStatic = true;
			if (child.name == "Branches") {
				foreach (Transform branchChild in child) {
					branchChild.gameObject.isStatic = true;
				}
			}
		}			
	}
}
