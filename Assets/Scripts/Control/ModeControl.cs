using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeControl : MonoBehaviour
{
	public List<GameObject> modeGOs = new List<GameObject>();
	public int modeIndex;
	public Faction playerFaction;

	private void Start()
	{
		//Load Map
		//Load factions
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Tab))
		{
			CycleModes();
		}
	}

	private void CycleModes()
	{
		modeIndex++;
		Debug.Log(modeGOs.Count);
		if (modeIndex >= modeGOs.Count)
			modeIndex = 0;
		foreach(GameObject go in modeGOs)
		{
			go.SetActive(false);
		}
		modeGOs[modeIndex].SetActive(true);
	}
}
