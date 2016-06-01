using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TerEdge;
using LibNoise.Unity;
using LibNoise.Unity.Generator;

public class GameGeneration : MonoBehaviour {

	// Might be unncessary, since this constant is only used once.
	private int TREES_PER_UNIT = 10;

	private Terrain terrain;

	// Perlin noise data for terrain generation
	private float HEIGHT = 0.15f;
	private float SPLAT_HEIGHT = 0.5f;
	private float ALPHA = 0.17f;
	private float SPLAT_ALPHA = 1.0f;
	private int SEED = 0;
	private float FREQ = 1.7f;
	private float SPLAT_FREQ = 1f;
	private int LACUNARITY = 3;
	private int SPLAT_LACUNARITY = 2;
	private float PERSISTENCE = 0.5f;
	private float SPLAT_PERSISTENCE = 0.8f;
	private int OCTAVES = 3;
	private int SPLAT_OCTAVES = 9;

	void Start() {
		// Units are simply data structures that hold some info pertinent to
		// tree generation, and come in handy for the grid-based nature of
		// the game.
		Unit treeUnit = new Unit((GameObject)Resources.Load("Tree"), new Vector2(10, 10), TREES_PER_UNIT);
		Unit outsideTreeUnit = new Unit((GameObject)Resources.Load("Tree"), new Vector2(10, 10), 3);

		// Game objects to be the parent of the soon-to-be-generated tree objects.
		GameObject treeParent = new GameObject();
		treeParent.name = "Tree Parent";
		treeParent.tag = "TreeParent";
		GameObject bgTrees = new GameObject();
		bgTrees.name = "Background Tree Parent";
		bgTrees.tag = "BGTrees";

		// Terrain must be generated before the trees so the trees can properly
		// Y-align with the terrain height.
		GenerateTerrain();

		// Selected trees exist under "Tree Parent" and spawn in groups of 10 from
		// (0,0) to (100,100) on the X/Z axes.
		SpawnUnitInRange(treeUnit, new Vector2(0,0), new Vector2(100,100), "TreeParent");
		// Background trees spawn in groups of 3 around the game area, 100 pixels wide
		// (think 100-unit thick layer around the center)
		SpawnUnitInRange(outsideTreeUnit, new Vector2(-100,-100), new Vector2(0,200), "BGTrees");
		SpawnUnitInRange(outsideTreeUnit, new Vector2(0,-100), new Vector2(100,0), "BGTrees");
		SpawnUnitInRange(outsideTreeUnit, new Vector2(0, 100), new Vector2(100, 200), "BGTrees");
		SpawnUnitInRange(outsideTreeUnit, new Vector2(100,-100), new Vector2(200,200), "BGTrees");

		// When instantiated, the Cloud prefab sets itself up; we only need to spawn them.
		Instantiate(Resources.Load("Cloud"));
		Instantiate(Resources.Load("Cloud"));
		Instantiate(Resources.Load("Cloud"));

		// It is not necessary to de-activate this object, but it seems better to
		// de-activate objects that aren't used anymore, rather than leave them idle.
		gameObject.SetActive(false);
	}

	public void SpawnUnitInRange(Unit unit, Vector2 startPoint, Vector2 endPoint, string tag = "Untagged") {
		// "Units" of trees are spawned with a Unit object, an X/Z axes range, and a Unity tag.
		// (Here, X/Z will be referred to as X/Y since the generation is a 2D process.)
		for (float xx = startPoint.x; xx < endPoint.x; xx += unit.getSize().x) {
			for (float yy = startPoint.y; yy < endPoint.y; yy += unit.getSize().y) {
				// A parent prefab for units of trees is used as well.
				GameObject treeUnitParent = (GameObject) Instantiate(Resources.Load("UnitParent"));

				// Unit parent name is given a name relating to its X/Y position.
				treeUnitParent.name = "Unit_" + ((int)xx).ToString("000") + "_" + ((int)yy).ToString("000");
				treeUnitParent.transform.position = new Vector3(xx, 0f, yy);
				treeUnitParent.GetComponent<UnitParent>().unit = unit;

				// UnitScatter returns an array of random 2D positions. Instantiated
				// trees use these positions along with the terrain height to
				// set their positions.
				foreach (Vector2 pos in UnitScatter(unit, new Vector2(xx,yy))) {
					GameObject tree = (GameObject) Instantiate(unit.getObj(),
						new Vector3(pos.x,
							terrain.SampleHeight(new Vector3(pos.x, 0f, pos.y)) - 0.1f,
							pos.y),
						Quaternion.Euler(new Vector3(0f, Random.Range(0f, 360f), 0f))
					);

					// Tree is parented to a Unit parent, and the Unit parent's
					// list of children is populated.
					tree.transform.parent = treeUnitParent.transform;
					treeUnitParent.GetComponent<UnitParent>().AddChild(tree);
				}
				// If a tag was not specified, then unit parents are not parented to an object.
				// Else, unit parent now has a parent. ("Untagged" is just an unused placeholder name;
				// parenting to "Untagged" will raise a NullReferenceException.)
				if (tag != "Untagged") {
					treeUnitParent.transform.parent = GameObject.FindGameObjectWithTag(tag).transform;
				}
			}
		}
	}

	private void GenerateTerrain() {
		// TerrainData is an object that contains data for terrain (go figure). Terrain
		// GameObjects are special in that they hold a great deal of information,
		// and because of this, they require a separate data object.
		TerrainData tdata = new TerrainData();
		tdata.size = new Vector3(400, 600, 400);

		// SplatPrototypes are Splatmap data types. In English, they're the objects
		// that hold texture data. TerrainData requires a SplatPrototype array
		// in order to hold information about its available textures.
		SplatPrototype[] textures = new SplatPrototype[3];
		textures[0] = new SplatPrototype();
		textures[0].texture = (Texture2D) Resources.Load("grass");
		textures[1] = new SplatPrototype();
		textures[1].texture = (Texture2D) Resources.Load("grassForest");
		textures[1].tileSize = new Vector2(10f,10f); // Tile size shrunk from default (15f,15f)
		textures[2] = new SplatPrototype();
		textures[2].texture = (Texture2D) Resources.Load("grassHill");

		tdata.splatPrototypes = textures;

		// TerrainData is applied to a newly-created Terrain object.
		GameObject terrainObj = Terrain.CreateTerrainGameObject(tdata);
		terrainObj.transform.position = new Vector3(-100, 0, -100);
		terrain = terrainObj.GetComponent<Terrain>();

		// Generating random terrain and texturing requires a two-step process.
		// First, we generate a heightmap for the terrain, and then we use
		// Unity's splatmapping process to apply texturing based on that heightmap.
		// Next, we generate a new heightmap for the terrain, to give off the impression
		// that the texturing was done randomly.

		// Not sure what ModuleBase is, but the Perlin data type has something to do with it.
		ModuleBase mb;

		// Terrain heightmap randomization uses Perlin noise (given some variables).
		SEED = Random.Range(0, 65535);
		mb = new Perlin(SPLAT_FREQ, SPLAT_LACUNARITY, SPLAT_PERSISTENCE, SPLAT_OCTAVES, SEED, QualityMode.High);
		teFunc.generateHeightmap(terrainObj, mb, SPLAT_HEIGHT, SPLAT_ALPHA);

		// The following code does the texturing based off of several rules for each texture.
		// Some modified help was needed for this part.

		// START CODE FROM OUTSIDE SOURCE: https://alastaira.wordpress.com/2013/11/14/procedural-terrain-splatmapping/
		float[,,] splatData = new float[tdata.alphamapWidth, tdata.alphamapHeight, tdata.alphamapLayers];

		for (int y = 0; y < tdata.alphamapHeight; y++) {
			for (int x = 0; x < tdata.alphamapWidth; x++) {
				//Normalise x/y coordinates to range 0-1
				float y_01 = (float)y/(float)tdata.alphamapHeight;
				float x_01 = (float)x/(float)tdata.alphamapWidth;
				// Sample the height at this location (GetHeight expects ints corresponding to
				// locations in the heightmap array
				float height = tdata.GetHeight(
					Mathf.RoundToInt(y_01 * tdata.heightmapHeight),
					Mathf.RoundToInt(x_01 * tdata.heightmapWidth)
				);
				// Calc the terrain normal (this is in normalised coordinated relative
				// to the overall dimensions) (this can be used for texture application)
				Vector3 normal = tdata.GetInterpolatedNormal(y_01, x_01);

				// Calc terrain steepness
				float steepness = tdata.GetSteepness(y_01, x_01);

				// Setup array to recod the texture mixes at this point
				float[] weights = new float[tdata.alphamapLayers];

				// RULE 1: Texture 0 has constant influence
				weights[0] = 0.5f;

				// RULE 2: Texture 1 is applied on normal surfaces that have a positive Z axis.
				weights[1] = (height/100f) * Mathf.Clamp01(normal.z);

				// RULE 3: Texture 2 is stronger on flatter terrain
				// Steepness is unbounded; we normalize it by dividing the extend
				// of heightmap height and scale factor. We subtract result from
				// 2.0 to give greater flatness weighting and multiply by pos y normal
				// to give more influence.
				weights[2] = (2f - Mathf.Clamp01(steepness*steepness/(tdata.heightmapHeight/4.0f))) * (2f * Mathf.Clamp01(normal.y));

				float z = 0f;
				for (int i = 0; i < tdata.alphamapLayers; i++)
					z += weights[i];

				for (int i = 0; i < tdata.alphamapLayers; i++) {
					// Normalize so that sum of all texture weights = 1
					weights[i] /= z;
					// ASsign to splatmap array
					splatData[x,y,i] = weights[i];
				}
			}
		}
		// Assign new splatmap to terrain data
		tdata.SetAlphamaps(0,0,splatData);

		// END CODE FROM OUTSIDE SOURCE

		float[,] flat = new float[33,33];

		terrainObj.GetComponent<Terrain>().terrainData.SetHeights(0, 0, flat);

		SEED = Random.Range(0, 65535);
		mb = new Perlin(FREQ, LACUNARITY, PERSISTENCE, OCTAVES, SEED, QualityMode.High);
		teFunc.generateHeightmap(terrainObj, mb, HEIGHT, ALPHA);
	}

	private Vector2[] UnitScatter(Unit unit, Vector2 startPoint) {
		Vector2[] scattered = new Vector2[unit.getAmount()];

		for (int i = 0; i < unit.getAmount(); i++) {
			bool nextXYIsValid = false;
			float xx = 0f;
			float yy = 0f;

			while (!nextXYIsValid) {
				xx = Random.Range(startPoint.x, startPoint.x + unit.getSize().x);
				yy = Random.Range(startPoint.y, startPoint.y + unit.getSize().y);
				nextXYIsValid = true;
				foreach (Vector2 coord in scattered) {
					if (Mathf.Abs(coord.x - xx) <= 0.6f && Mathf.Abs(coord.y - yy) <= 0.6f) {
						nextXYIsValid = false;
					}
				}
			}

			scattered[i] = new Vector2(xx, yy);
		}

		return scattered;
	}
}
