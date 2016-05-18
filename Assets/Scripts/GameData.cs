using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameData : MonoBehaviour {

	private List<UnitParent> selectedParents;

	public float treeGrowbackWaitTime;
	public float treeGrowbackSpeed;

	void Start () {
		selectedParents = new List<UnitParent>();
	}

	public void AddParentToSelected(UnitParent parent) {
		selectedParents.Add(parent);
	}

	public void RemoveParentFromSelected(UnitParent parent) {
		selectedParents.Remove(parent);
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
		for (int i = 0; i < c; i++) {
			UnitParent up = selectedParents[0];
			RemoveParentFromSelected(up);
			up.killSelectedTrees(treeGrowbackWaitTime, treeGrowbackSpeed/100);
		}
	}
}
