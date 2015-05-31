using UnityEngine;
using System.Collections;

[System.Serializable]
public class PackaguesToGame {

	public GameObject prefab;
	public TypeOfElement type;

	public PackaguesToGame (GameObject go, TypeOfElement t)
	{
		prefab = go;
		type = t;
	}

}
