using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character
{
	public string charName;
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

	public void GenStartingStats()
	{
		charName = "blah";
		age = Random.Range(15, 24);
		birthday = Random.Range(0, 365);
		birthplace = Random.Range(0, 10);

		clas = Class.Civilian;
		level = 0;
		exp = 0;
		hp = 10;
		maxHP = 10;

		str = Random.Range(0, 50);
		vit = Random.Range(0, 50);
		intl = Random.Range(0, 50);
		men = Random.Range(0, 50);
		agi = Random.Range(0, 50);
		dex = Random.Range(0, 50);
		leadership = Random.Range(0, 50);
	}
}

public enum Class{ Civilian, Fighter, Mage, Healer}
