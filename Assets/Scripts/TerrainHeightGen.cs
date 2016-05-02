using UnityEngine;
using System.Collections;
using LibNoise.Unity;
using LibNoise.Unity.Generator;
using TerEdge;

public class TerrainHeightGen : MonoBehaviour {

	private float HEIGHT = 0.15f;
	private float ALPHA = 0.17f;
	private ModuleBase[] mb = new ModuleBase[1];

	private int SEED = 1;
	private float FREQ = 1.7f;
	private int LACUNARITY = 2;
	private float PERSISTENCE = 0.5f;
	private int OCTAVES = 2;

	void Start () {
		TerrainData tdata = new TerrainData();
		tdata.size = new Vector3(300, 600, 300);

		GameObject terrain = Terrain.CreateTerrainGameObject(tdata);
		terrain.transform.position = new Vector3(-100, 0, -100);

		ModuleBase test = new Perlin(FREQ, LACUNARITY, PERSISTENCE, OCTAVES, SEED, QualityMode.High);
		mb[0] = new Perlin(FREQ, LACUNARITY, PERSISTENCE, OCTAVES, SEED, QualityMode.High);
		TerEdge.teFunc.generateHeightmap(terrain, test, HEIGHT, ALPHA);
	}
}
