using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaulJob : Job
{
	public Vector3 targetLocation;
	public List<Item> items  = new List<Item>();
}
