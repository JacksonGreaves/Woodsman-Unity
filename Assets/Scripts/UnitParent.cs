using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitParent : MonoBehaviour {

	private List<GameObject> children = new List<GameObject>();
	private bool isSelected;
	public Unit unit;

	private GameData data;

	private bool canBeSelected;

	void Start () {
		isSelected = false;
		canBeSelected = true;
		data = GameObject.Find("GameHandler").GetComponent<GameData>();
	}

	public void setSelected(bool b) {
		isSelected = b;
		Material main = (Material)Resources.Load("Tree-Main");
		Material selected = (Material)Resources.Load("Tree-Selected");
		Material tMain = (Material)Resources.Load("Tree-TrunkMain");
		Material tSelected = (Material)Resources.Load("Tree-TrunkSelected");

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
						child.gameObject.GetComponent<Renderer>().sharedMaterial = tSelected;
					} else {
						child.gameObject.GetComponent<Renderer>().sharedMaterial = tMain;
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

	public void killSelectedTrees(float wait, float speed) {
		setSelected(false);
		StartCoroutine(RespawnTrees(wait, speed));
	}

	void Update() {
		if (canBeSelected && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
			if (Input.GetMouseButton(0)) {
				var camx = Camera.main.GetComponent<CameraController>().point.x;
				var camy = Camera.main.GetComponent<CameraController>().point.z;
				var px = transform.position.x;
				var py = transform.position.z;
				if (camx >= px && camx < (px + unit.getSize().x) &&
					camy >= py && camy < (py + unit.getSize().y)) {
					if (camx >= 0f && camx < 100f && camy >= 0f && camy < 100f && !isSelected) {
						setSelected(true);
						data.AddParentToSelected(GetComponent<UnitParent>());
					}
				} else {
					if (!Input.GetKey(KeyCode.LeftShift) && isSelected) {
						setSelected(false);
						data.RemoveParentFromSelected(GetComponent<UnitParent>());
					}
				}
			}
		}
	}

	private IEnumerator RespawnTrees(float wait, float speed) {
		canBeSelected = false;
		foreach (GameObject go in children) {
			go.isStatic = false;
			go.transform.localScale = Vector3.zero;
		}
		yield return new WaitForSeconds(wait);
		float scale = 0f;
		while (scale < 1f) {
			foreach (GameObject go in children) {
				go.transform.localScale = Vector3.one * scale;
			}
			scale += speed;
			yield return new WaitForSeconds(Time.deltaTime);
		}
		foreach (GameObject go in children) {
			go.isStatic = true;
		}
		canBeSelected = true;
	}
}
