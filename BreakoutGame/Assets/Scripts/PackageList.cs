using UnityEngine;
using System.Collections;

public enum TypeOfElement
{
	coin,
	mandatoryItem,
	randomItem
}

[System.Serializable]

public class PackageList {

	public GameObject prefab;
	public int possibility;
	public int maxRepeat;
	public TypeOfElement type;

	public PackageList(GameObject pref, int poss, int max, TypeOfElement t)
	{
		prefab = pref;
		possibility = poss;
		maxRepeat = max;
		type = t;
	}
}
