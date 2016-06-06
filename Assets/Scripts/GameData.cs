using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameData : MonoBehaviour {

	private bool gamePaused;

	private float SECONDS_PER_DAY = 10f;

	private List<UnitParent> selectedParents;
	private CutSelectionPopout cutPopout;
	private int woodCutCount;
	private int money;
	private int days;
	private int daysLeft;
	private int currentQuota;
	
	// Heads-up: 1 hour of gameplay = 12 quotas
	private List<int> quotas = new List<int> {	250, 300, 350, 400, 500,
												600, 700, 800, 900, 1000,
												1100, 1200, 1300, 1350, 1450,
												1550, 1600, 1700, 1750, 1800,
												1900, 2000, 2200, 2300, 2400,
												2450, 2500	};

	public static float workerCutTime;
	public static float machineCutTime;
	public static float teamCutTime;

	private int workerCount;
	private int machineCount;
	private int teamCount;

	void Start () {
		workerCutTime = 3f * SECONDS_PER_DAY;
		machineCutTime = 1f * SECONDS_PER_DAY;
		teamCutTime = 0.5f * SECONDS_PER_DAY;
		currentQuota = quotas[0];
		woodCutCount = 0;
		money = 3000;
		days = 0;
		daysLeft = 30;
		workerCount = 2;
		machineCount = 0;
		teamCount = 0;
		selectedParents = new List<UnitParent>();
		cutPopout = GameObject.Find("CutSelection").GetComponent<CutSelectionPopout>();
		StartCoroutine(DayCounter());
		UpdateWoodCount();
		UpdateMoneyCount();
		UpdateUnitCount();
	}

	public void AddParentToSelected(UnitParent parent) {
		selectedParents.Add(parent);
		UpdateCutButton();
		UpdateUnitCount();
	}

	public void RemoveParentFromSelected(UnitParent parent) {
		selectedParents.Remove(parent);
		UpdateUnitCount();
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
		Cut(10, 50, workerCutTime, "sawing");
	}

	public void CutTrees_Machine() {
		Cut(10, 50, machineCutTime, "machinery");
	}

	public void CutTrees_Team() {
		Cut(10, 50, teamCutTime, "team");
	}

	public void Game_TogglePause() {
		Game_PauseResume(!gamePaused);
	}

	public void Game_PauseResume(bool pause) {
		gamePaused = pause;

		if (gamePaused) {
			Time.timeScale = 0f;
		} else {
			Time.timeScale = 1f;
		}
	}

	public void Game_Exit() {
		Application.Quit();
	}

	public void SetWorkerCount(int c) {
		workerCount = c;
		UpdateUnitCount();
	}

	public int GetWorkerCount() {
		return workerCount;
	}

	public void SetMachineCount(int c) {
		machineCount = c;
		UpdateUnitCount();
	}

	public int GetMachineCount() {
		return machineCount;
	}

	public void SetTeamCount(int c) {
		teamCount = c;
		UpdateUnitCount();
	}

	public int GetTeamCount() {
		return teamCount;
	}

	public void PurchaseWorker() {
		if (money >= 1000) {
			money -= 1000;
			workerCount += 1;
		}
		UpdateMoneyCount();
	}

	public void PurchaseMachine() {
		if (money >= 4000) {
			money -= 4000;
			machineCount += 1;
		}
		UpdateMoneyCount();
	}

	public void PurchaseTeam() {
		if (money >= 10000) {
			money -= 10000;
			teamCount += 1;
		}
		UpdateMoneyCount();
	}

	private void Cut(int woodPrice, int munsPrice, float time, string sound) {
		int c = selectedParents.Count;

		if (sound == "sawing") {
			workerCount -= c;
		} else if (sound == "machinery") {
			machineCount -= c;
		} else if (sound == "team") {
			teamCount -= c;
		}

		UpdateUnitCount();

		int wood = 0;
		int muns = 0;

		float audioXMin = 100f;
		float audioZMin = 100f;
		float audioXMax = 0f;
		float audioZMax = 0f;

		for (int i = 0; i < c; i++) {
			UnitParent up = selectedParents[0];
			if (audioXMin > up.transform.position.x) {
				audioXMin = up.transform.position.x;
			}
			if (audioXMax < up.transform.position.x + up.unit.getSize().x) {
				audioXMax = up.transform.position.x + up.unit.getSize().x;
			}
			if (audioZMin > up.transform.position.z) {
				audioZMin = up.transform.position.z;
			}
			if (audioZMax < up.transform.position.z + up.unit.getSize().y) {
				audioZMax = up.transform.position.z + up.unit.getSize().y;
			}
			RemoveParentFromSelected(up);
			up.CutDownUnit(time);
			wood += woodPrice;
			muns += munsPrice;
		}

		AddToWoodCount(wood);
		AddMoney(muns);

		float audioX = (audioXMin + audioXMax) / 2;
		float audioZ = (audioZMin + audioZMax) / 2;

		GameObject go = (GameObject) Instantiate(Resources.Load("CuttingAudio"),
			new Vector3(audioX, Terrain.activeTerrain.SampleHeight(new Vector3(audioX, 100f, audioZ)), audioZ),
			Quaternion.Euler(Vector3.forward));

		AudioSource a = go.GetComponent<AudioSource>();
		a.clip = (AudioClip) Resources.Load(sound);
		if (sound == "machinery")
			a.volume -= 0.4f;
		a.Play();
		if (sound == "team") {
			// Let the Team cutting sound go on for its length
			GameObject.Destroy(go, a.clip.length);
		} else {
			GameObject.Destroy(go, time);
		}
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

	private void UpdateUnitCount() {
		Text worker = GameObject.Find("WorkerButton").GetComponentInChildren<Text>();
		Text machine = GameObject.Find("MachineButton").GetComponentInChildren<Text>();
		Text team = GameObject.Find("TeamButton").GetComponentInChildren<Text>();

		worker.text = string.Format("Worker Unit ({0})", workerCount);
		machine.text = string.Format("The Katt Unit ({0})", machineCount);
		team.text = string.Format("The Team Unit ({0})", teamCount);

		GameObject.Find("WorkerButton").GetComponent<Button>().interactable = (selectedParents.Count <= workerCount);
		GameObject.Find("MachineButton").GetComponent<Button>().interactable = (selectedParents.Count <= machineCount);
		GameObject.Find("TeamButton").GetComponent<Button>().interactable = (selectedParents.Count <= teamCount);
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
