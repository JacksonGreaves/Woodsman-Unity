using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitParent : MonoBehaviour {

	private List<GameObject> children = new List<GameObject>();
	private bool isSelected;
	public Unit unit;


	private bool canBeSelected;
	private bool areTreesSick;

	private Material matMain;
	private Material matSelected;
	private Material matTMain;
	private Material matTSelected;
	private Material matGrowing;

	private GameData data;

	private float growbackFactor;

	void Start () {
		data = GameObject.Find("GameHandler").GetComponent<GameData>();
		isSelected = false;
		canBeSelected = true;
		growbackFactor = 1f;
		areTreesSick = false;

		matMain = (Material)Resources.Load("Tree-Main");
		matSelected = (Material)Resources.Load("Tree-Selected");
		matTMain = (Material)Resources.Load("Tree-TrunkMain");
		matTSelected = (Material)Resources.Load("Tree-TrunkSelected");
		matGrowing = (Material)Resources.Load("Tree-Growing");
	}

	public void setSelected(bool b) {
		isSelected = b;

		if (isSelected) {
			changeMaterials(matSelected, matTSelected);
		} else {
			changeMaterials(matMain, matTMain);
		}
	}

	public bool getSelected() {
		return isSelected;
	}

	private void changeMaterials(Material branchMaterial, Material trunkMaterial) {
		foreach (GameObject go in this.children) {
			foreach (Transform child in go.transform) {
				if (child.name == "Branches") {
					foreach (Transform branchChild in child) {
						branchChild.gameObject.GetComponent<Renderer>().sharedMaterial = branchMaterial;
					}
				} else {
					child.gameObject.GetComponent<Renderer>().sharedMaterial = trunkMaterial;
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
			changeMaterials(matGrowing, matGrowing);
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
		setSelected(false);
	}
}
