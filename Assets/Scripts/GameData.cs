using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameData : MonoBehaviour {

	private List<UnitParent> selectedParents;
	private float woodCutCount;
	private int money;

	public float treeGrowbackWaitTime;
	public float treeGrowbackSpeed;

	void Start () {
		woodCutCount = 0;
		money = 0;
		selectedParents = new List<UnitParent>();
		StartCoroutine(DayCounter());
	}

	public void AddParentToSelected(UnitParent parent) {
		selectedParents.Add(parent);
	}

	public void RemoveParentFromSelected(UnitParent parent) {
		selectedParents.Remove(parent);
	}

	public void AddToWoodCount(int count) {
		woodCutCount += count;
		UpdateWoodCount();
	}

	public void AddMoney(int muns) {
		money += muns;
	}

	public void ResetWoodCount() {
		woodCutCount = 0;
		UpdateWoodCount();
	}

	public void ResetMoney() {
		money = 0;
	}

	public UnitParent[] GetSelectedParentsAsArray() {
		UnitParent[] array = new UnitParent[selectedParents.Count];
		for (int i = 0; i < selectedParents.Count; i++) {
			array[i] = selectedParents[i];
		}
		return array;
	}

	public List<UnitParent> GetSelectedParentsAsList() {
		return selectedParents;
	}

	public void killSelectedTrees() {
		int c = selectedParents.Count;
		int wood = 0;
		for (int i = 0; i < c; i++) {
			UnitParent up = selectedParents[0];
			RemoveParentFromSelected(up);
			up.killSelectedTrees(treeGrowbackWaitTime, treeGrowbackSpeed/100);
			wood += 1;
		}
		AddToWoodCount(wood);
	}

	public void HalfTest() {
		woodCutCount += 0.5f;
		UpdateWoodCount();
	}

	private void UpdateWoodCount() {
		var t = GameObject.Find("WoodCount").GetComponent<Text>();
		if (woodCutCount % 10 == 0) {
			// Int (whole wood)
			t.text = ((int)woodCutCount).ToString();
		} else {
			// Float (half wood)
			t.text = woodCutCount.ToString();
		}
	}

	private IEnumerator DayCounter() {
		while (true) {
			yield return new WaitForSeconds(10f);
			var t = GameObject.Find("DayCount").GetComponent<Text>();
			t.text = (int.Parse(t.text) + 1).ToString();
		}
	}
}
