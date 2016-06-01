using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameData : MonoBehaviour {

	private float SECONDS_PER_DAY = 10f;

	private List<UnitParent> selectedParents;
	private CutSelectionPopout cutPopout;
	private int woodCutCount;
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

	public float defaultGrowthRate;
	public static float workerCutTime;
	public static float machineCutTime;
	public static float teamCutTime;

	void Start () {
		defaultGrowthRate = 5f * SECONDS_PER_DAY;
		workerCutTime = 3f * SECONDS_PER_DAY;
		machineCutTime = 1f * SECONDS_PER_DAY;
		teamCutTime = 0.5f * SECONDS_PER_DAY;
		currentQuota = quotas[0];
		woodCutCount = 0;
		money = 3000;
		days = 0;
		daysLeft = 30;
		selectedParents = new List<UnitParent>();
		cutPopout = GameObject.Find("CutSelection").GetComponent<CutSelectionPopout>();
		StartCoroutine(DayCounter());
		UpdateWoodCount();
		UpdateMoneyCount();
	}

	public void AddParentToSelected(UnitParent parent) {
		selectedParents.Add(parent);
		UpdateCutButton();
	}

	public void RemoveParentFromSelected(UnitParent parent) {
		selectedParents.Remove(parent);
	}

	public void ResetSelectedParents() {
		int c = selectedParents.Count;
		for (int i = 0; i < c; i++) {
			UnitParent up = selectedParents[0];
			RemoveParentFromSelected(up);
			up.setSelected(false);
		}
		UpdateCutButton();
	}

	public void AddToWoodCount(int count) {
		woodCutCount += count;
		UpdateWoodCount();
	}

	public void AddMoney(int muns) {
		money += muns;
		UpdateMoneyCount();
	}

	public void ResetWoodCount() {
		woodCutCount = 0;
		UpdateWoodCount();
	}

	public void ResetMoney() {
		money = 0;
		UpdateMoneyCount();
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

	public void CutTrees_Worker() {
		int c = selectedParents.Count;
		int wood = 0;
		int muns = 0;
		for (int i = 0; i < c; i++) {
			UnitParent up = selectedParents[0];
			RemoveParentFromSelected(up);
			up.CutDownUnit(workerCutTime);
			wood += 10;
			muns += 50;
		}
		AddToWoodCount(wood);
		AddMoney(muns);
	}

	public void CutTrees_Machine() {
		int c = selectedParents.Count;
		int wood = 0;
		int muns = 0;
		for (int i = 0; i < c; i++) {
			UnitParent up = selectedParents[0];
			RemoveParentFromSelected(up);
			up.CutDownUnit(machineCutTime);
			wood += 10;
			muns += 50;
		}
		AddToWoodCount(wood);
		AddMoney(muns);
	}

	public void CutTrees_Team() {
		int c = selectedParents.Count;
		int wood = 0;
		int muns = 0;
		for (int i = 0; i < c; i++) {
			UnitParent up = selectedParents[0];
			RemoveParentFromSelected(up);
			up.CutDownUnit(teamCutTime);
			wood += 10;
			muns += 50;
		}
		AddToWoodCount(wood);
		AddMoney(muns);
	}

	private void UpdateCutButton() {
		if (selectedParents.Count > 0) {
			cutPopout.SetSelected(true);
		} else {
			cutPopout.SetSelected(false);
		}
	}

	private void UpdateWoodCount() {
		var t = GameObject.Find("WoodCount").GetComponent<Text>();
		t.text = woodCutCount.ToString();
	}

	private void UpdateMoneyCount() {
		Text t = GameObject.Find("MoneyCount").GetComponent<Text>();
		t.text = money.ToString();
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
			yield return new WaitForSeconds(SECONDS_PER_DAY);
			days += 1;
			daysLeft = 30 - (days%30);
			if (daysLeft == 30) {
				triggerQuota();
			}
		}
	}
}
