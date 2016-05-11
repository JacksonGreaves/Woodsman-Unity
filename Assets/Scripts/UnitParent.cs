using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitParent : MonoBehaviour {

	private List<GameObject> children = new List<GameObject>();
	private bool isSelected;
	public Unit unit;

	void Start () {
		isSelected = false;
	}

	public void setSelected(bool b) {
		isSelected = b;
		Material main = (Material)Resources.Load("Tree-Main");
		Material selected = (Material)Resources.Load("Tree-Selected");

		foreach (GameObject go in this.children) {
			foreach (Transform child in go.transform) {
				if (child.name == "Branches") {
					foreach (Transform branchChild in child) {
						// Branches
						if (isSelected) {
							branchChild.gameObject.GetComponent<Renderer>().sharedMaterial = selected;
						} else {
							branchChild.gameObject.GetComponent<Renderer>().sharedMaterial = main;
						}
					}
				} else {
					// Trunk
					if (isSelected) {
						child.gameObject.GetComponent<Renderer>().sharedMaterial = selected;
					} else {
						child.gameObject.GetComponent<Renderer>().sharedMaterial = main;
					}
				}
			}
		}
	}

	public void AddChild(GameObject go) {
		children.Add(go);
	}

	public List<GameObject> GetChildren() {
		return children;
	}

	void Update() {
		if (Input.GetMouseButton(0)) {
			var camx = Camera.main.GetComponent<CameraController>().point.x;
			var camy = Camera.main.GetComponent<CameraController>().point.z;
			var px = transform.position.x;
			var py = transform.position.z;
			if (camx >= px && camx < (px + unit.getSize().x) &&
				camy >= py && camy < (py + unit.getSize().y)) {
				if (camx >= 0f && camx < 100f && camy >= 0f && camy < 100f) {
					setSelected(true);
				}
			} else {
				if (!Input.GetKey(KeyCode.LeftShift)) {
					setSelected(false);
				}
			}
		}
	}
}
