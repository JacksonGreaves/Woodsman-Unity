using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameData : MonoBehaviour {

	private List<UnitParent> selectedParents;
	private float woodCutCount;
	private int money;
	private int days;
	private int daysLeft;
	private int currentQuota;
	private List<int> quotas = new List<int> {	250, 300, 350, 400, 500,
												600, 700, 800, 900, 1000,
												1100, 1200, 1300, 1350, 1450,
												1550, 1600, 1700, 1750, 1800,
												1900, 2000, 2200, 2300, 2400,
												2450, 2500	};

	public float treeGrowbackWaitTime;
	public float treeGrowbackSpeed;

	void Start () {
		currentQuota = quotas[0];
		woodCutCount = 0;
		money = 0;
		days = 0;
		daysLeft = 30;
		selectedParents = new List<UnitParent>();
		StartCoroutine(DayCounter());
	}

	public void AddParentToSelected(UnitParent parent) {
		selectedParents.Add(parent);
	}

	public void RemoveParentFromSelected(UnitParent parent) {
		selectedParents.Remove(parent);
	}

	public void AddToWoodCount(float count) {
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
		float wood = 0f;
		for (int i = 0; i < c; i++) {
			UnitParent up = selectedParents[0];
			RemoveParentFromSelected(up);
			up.killSelectedTrees(treeGrowbackWaitTime, treeGrowbackSpeed/100);
			wood += 10f;
		}
		AddToWoodCount(wood);
	}

	public void HalfTest() {
		woodCutCount += 5f;
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

	private void triggerQuota() {
		woodCutCount -= currentQuota;
		var i = quotas.IndexOf(currentQuota)+1;
		if (i >= quotas.Count)
			i -= 1;
		currentQuota = quotas[i];
		UpdateWoodCount();
		GameObject.Find("WoodQuotaCount").GetComponent<Text>().text = currentQuota.ToString();
	}

	private IEnumerator DayCounter() {
		while (true) {
			GameObject.Find("DayCount").GetComponent<Text>().text = days.ToString();
			GameObject.Find("DayLeftCount").GetComponent<Text>().text = daysLeft.ToString();
			GameObject.Find("WoodQuotaCount").GetComponent<Text>().text = currentQuota.ToString();
			yield return new WaitForSeconds(10f);
			days += 1;
			daysLeft = 30 - (days%30);
			if (daysLeft == 30) {
				triggerQuota();
			}
		}
	}
}
