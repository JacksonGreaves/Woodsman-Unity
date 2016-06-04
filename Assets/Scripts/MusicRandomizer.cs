using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicRandomizer : MonoBehaviour {

	private AudioSource source;
	private List<AudioClip> musicList;

	void Start () {
		source = GetComponent<AudioSource>();
		musicList = new List<AudioClip>();
		// Populate list of music here
		musicList.Add((AudioClip) Resources.Load("Summer Day"));
		musicList.Add((AudioClip) Resources.Load("Guts and Bourbon"));
		StartCoroutine(Player());
	}

	private IEnumerator Player() {
		//source.Play();
		//yield return new WaitForSeconds(source.clip.length);
		while (true) {
			source.clip = musicList[Random.Range(0, musicList.Count)];
			if (source.clip.name == "Guts and Bourbon") {
				source.volume = 0.15f;
			} else {
				source.volume = 0.2f;
			}
			source.Play();
			yield return new WaitForSeconds(source.clip.length);
		}
	}
}
