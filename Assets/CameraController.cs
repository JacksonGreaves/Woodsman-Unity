using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float posx;
	public float posy;
	public float posz;

	public float rotx;

	private float speed;
	private Rigidbody rb;

	private float camSpdLow = 	15f;
	private float camSpdMed = 	25f;
	private float camSpdHigh = 	20f;

	private int zoomLevel;
	private float highestTerrainY;

	private int lastDirection;

	void Start () {
		StartCoroutine(LateStart());
		speed = 0f;
		zoomLevel = 0;
		rb = GetComponent<Rigidbody>();
	}

	private IEnumerator LateStart() {
		// Camera must start after terrain generation since it samples the terrain height
		// at its starting point to figure out its initial Y position.
		yield return new WaitForSeconds(0.1f);
		highestTerrainY = GetTerrainHighestPoint();
		Debug.Log(highestTerrainY);
		transform.position = new Vector3(posx, highestTerrainY + posy, posz);
		// Gimbal lock won't be an issue, so it's easier to deal with euler Vec3's here.
		// Camera is hard-coded to start at 45 degrees rotated along Y to give
		// a more isometric feel.
		transform.rotation = Quaternion.Euler(new Vector3(rotx, 45f, 0f));
	}

	void Update () {
		// Since camera's Y value is rotated 45 degrees, the controls will have
		// to be tilted as well.
		// Yrot = 0 degrees : forward is (0, 0, 1)
		// Yrot = 45 degrees: forward is (1, 0, 1)
		if (Input.GetKey(KeyCode.W)) {
			handleMovement(new Vector3(1f, 0f, 1f));
		}
		if (Input.GetKey(KeyCode.A)) {
			handleMovement(new Vector3(-1f, 0f, 1f));
		}
		if (Input.GetKey(KeyCode.S)) {
			handleMovement(new Vector3(-1f, 0f, -1f));
		}
		if (Input.GetKey(KeyCode.D)) {
			handleMovement(new Vector3(1f, 0f, -1f));
		}

		if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) &&
			!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D)) {
			rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, 0.2f);
			rb.angularVelocity = Vector3.zero;
		}

		if (Input.GetKeyDown(KeyCode.E)) {
			if (zoomLevel <= 3) {
				zoomLevel += 1;
				if (transform.position.y > highestTerrainY + (20 - zoomLevel*2.5))
					rb.velocity = rb.transform.forward * 50;
				lastDirection = 1;
			}
		}
		if (Input.GetKeyDown(KeyCode.Q)) {
			if (zoomLevel > 0) {
				zoomLevel -= 1;
				if (transform.position.y < highestTerrainY + (20 - zoomLevel*2.5))
					rb.velocity = -rb.transform.forward * 50;
				lastDirection = -1;
			}
		}

		if (lastDirection == 1) {
			if (transform.position.y > highestTerrainY + (20 - zoomLevel*2.5)) {
				rb.velocity = Vector3.Lerp(rb.velocity, rb.transform.forward * 50, 0.1f);
			} else {
				lastDirection = 0;
			}
		} else if (lastDirection == -1) {
			if (transform.position.y < highestTerrainY + (20 - zoomLevel*2.5)) {
				rb.velocity = Vector3.Lerp(rb.velocity, -rb.transform.forward * 50, 0.1f);
			} else {
				lastDirection = 0;
			}
		}

		if (transform.position.y < highestTerrainY) {
			rb.velocity = Vector3.zero;
		}
	}

	private float GetTerrainHighestPoint() {
		float highestPoint = 0;
		var t = Terrain.activeTerrain;
		for (int xx = (int)t.transform.position.x; xx < t.terrainData.size.x; xx += 10) {
			for (int yy = (int)t.transform.position.z; yy < t.terrainData.size.z; yy += 10) {
				float point = t.SampleHeight(new Vector3(xx, 0f, yy));
				if (point > highestPoint)
					highestPoint = point;
			}
			
		}
		return highestPoint;
		
	}
	private void handleMovement(Vector3 direction) {
		if (rb.velocity.magnitude >= 15f) {
			if (rb.velocity.magnitude >= 25f) {
				rb.velocity = Vector3.Lerp(rb.velocity,
					direction * camSpdHigh, 0.1f);
			} else {
				rb.velocity = Vector3.Lerp(rb.velocity,
					direction * camSpdMed, 0.1f);
			}
		} else {
			rb.velocity = Vector3.Lerp(rb.velocity,
				direction * camSpdLow, 0.1f);
		}
	}
}
