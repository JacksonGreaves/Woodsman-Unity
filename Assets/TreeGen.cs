using UnityEngine;
using System.Collections;

public class TreeGen : MonoBehaviour {

	private int MAX_TREE_COUNT = 1000;
	private int TREES_PER_UNIT = 10;
	
	void Start () {
		Unit treeUnit = new Unit((GameObject)Resources.Load("Tree"), new Vector3(10, 10));
		SpawnUnitInRange(treeUnit, new Vector2(0,0), new Vector2(100,100));
	}

	public void SpawnUnitInRange(Unit unit, Vector2 startPoint, Vector2 endPoint) {
		for (float xx = 0; xx < (endPoint.x - startPoint.x); xx += unit.getSize().x) {
			for (float yy = 0; yy < (endPoint.y - startPoint.y); yy += unit.getSize().y) {
				foreach (Vector2 pos in UnitScatter(unit, TREES_PER_UNIT, new Vector2(xx,yy))) {
					Instantiate(unit.getObj(), new Vector3(pos.x, 0f, pos.y), Quaternion.Euler(Vector3.forward));
				}
			}
		}
	}

	private Vector2[] UnitScatter(Unit unit, int amount, Vector2 startPoint) {
		Vector2[] scattered = new Vector2[amount];

		for (int i = 0; i < amount; i++) {
			scattered[i] = new Vector2(Random.Range(startPoint.x, startPoint.x + unit.getSize().x),
									Random.Range(startPoint.y, startPoint.y + unit.getSize().y));
		}

		return scattered;
	}

	void Update () {
	
	}
}
