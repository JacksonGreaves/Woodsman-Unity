using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitParent : MonoBehaviour {

	private List<GameObject> children = new List<GameObject>();
	private bool isSelected;

	void Start () {
		isSelected = false;
	}

	public void setSelected(bool b) {
		isSelected = b;

		if (isSelected) {
			foreach (GameObject go in this.children) {
				foreach (Transform child in go.transform) {
					if (child.name == "Branches") {
						foreach (Transform branchChild in child) {
							branchChild.gameObject.GetComponent<Renderer>().sharedMaterial = (Material)Resources.Load("Tree-Main");
						}
					} else {
						child.gameObject.GetComponent<Renderer>().sharedMaterial = (Material)Resources.Load("Tree-Main");
					}
				}
			}
		} else {
			foreach (GameObject go in this.children) {
				foreach (Transform child in go.transform) {
					if (child.name == "Branches") {
						foreach (Transform branchChild in child) {
							branchChild.gameObject.GetComponent<Renderer>().sharedMaterial = (Material)Resources.Load("Tree-Selected");
						}
					} else {
						child.gameObject.GetComponent<Renderer>().sharedMaterial = (Material)Resources.Load("Tree-Selected");
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

	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			setSelected(!isSelected);
		}
	}
}
