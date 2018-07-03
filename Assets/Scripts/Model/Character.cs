using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character
{
	public int age;
	public int birthday;
	public int birthplace;

	public Class clas;
	public int level;
	public int exp;
	public int hp;
	public int maxHP;

	public int str;
	public int vit;
	public int intl;
	public int men;
	public int agi;
	public int dex;
	public int leadership;

	public int headItem;
	public int bodyItem;
	public int rightHandItem;
	public int leftHandItem;
	public int accessoryItem;
//inventory
}

public enum Class{ Fighter, Mage, Healer}
