using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour {

	private GameObject obj;
	private Vector2 size;
	private int amount;

	public Unit(GameObject obj, Vector2 size, int amount) {
		this.obj = obj;
		this.size = size;
		this.amount = amount;
	}

	public void setObject(GameObject obj) {
		this.obj = obj;
	}

	public void setSize(Vector2 size) {
		this.size = size;
	}

	public void setSize(float x, float y) {
		this.size = new Vector2(x,y);
	}

	public void setAmount(int amount) {
		this.amount = amount;
	}

	public Vector2 getSize() {
		return size;
	}

	public GameObject getObj() {
		return obj;
	}

	public int getAmount() {
		return amount;
	}
}
