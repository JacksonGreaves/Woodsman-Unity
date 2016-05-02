using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

	private GameObject obj;
	private Vector2 size;

	public Unit (GameObject obj, Vector2 size) {
		this.obj = obj;
		this.size = size;
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

	public Vector2 getSize() {
		return size;
	}

	public GameObject getObj() {
		return obj;
	}
}
