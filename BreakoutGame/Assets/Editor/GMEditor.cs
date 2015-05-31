using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(GM))]
public class GMEditor : Editor {

	// Přístup k třídě GM
	private GM gm;

	// GUI variables
	public bool showPlayerSet = true;
	public bool showLevelSet = false;

	// Main ToolBar
	private int mainToolbarInt = 0;
	private string[] mainToolbar = new string[] {"Overwiew", "Player", "Level", "Packagues"};

	// Packagues ToolBar
	private int packToolbarInt = 0;
	private string[] packToolbar = new string[] {"Money Packs", "Mandatory Packs", "RandomPacks"};

	// Coins variables
	public List<GameObject> coinsList = new List<GameObject>();
	private int[] numberCoins;

	public override void OnInspectorGUI()
	{
		gm = (GM)target;

		EditorGUILayout.Space();
		mainToolbarInt = GUILayout.Toolbar(mainToolbarInt, mainToolbar, EditorStyles.toolbarButton);

		switch(mainToolbarInt)
		{
		case 0:

			break;
		case 1:
			PlayerSetting();
			break;
		case 2:
			LevelSetting();
			break;
		case 3:
			PackaguesSettings();
			break;
		}
	}


	//************************
	// PLAYER SETTINGS
	//************************
	void PlayerSetting()
	{
		// Nastavení pro Hráče
		showPlayerSet = EditorGUILayout.Foldout(showPlayerSet, "Player Settings");
		if (showPlayerSet)
		{
			gm.lives = EditorGUILayout.IntField("Lives:", gm.lives);
		}
	}

	//************************
	// LEVEL SETTINGS
	//************************
	void LevelSetting(){
		// Nastavení levelu
		showLevelSet = EditorGUILayout.Foldout(showLevelSet, "Level Settings");
		if (showLevelSet)
		{
			gm.brickPrefab = (GameObject) EditorGUILayout.ObjectField("Brick Prefab:", gm.brickPrefab, typeof(GameObject), true);
			gm.paddle = (GameObject) EditorGUILayout.ObjectField("Paddle Prefab:", gm.paddle, typeof(GameObject), true);
			gm.maxMoneyLevel = EditorGUILayout.IntField("Value of money in level:", gm.maxMoneyLevel);
			
			// Dočasné objekty. V dalších verzích bude nejspíše vše nahrazeno.
			gm.deathParticles = (GameObject) EditorGUILayout.ObjectField("Death Particles Prefab:", gm.deathParticles, typeof(GameObject), true);
			gm.youWon = (GameObject) EditorGUILayout.ObjectField("You won::", gm.youWon, typeof(GameObject), true);
			gm.gameOver = (GameObject) EditorGUILayout.ObjectField("Game Over:", gm.gameOver, typeof(GameObject), true);
			
			
		}
	}

	//************************
	// PACKAGUES SETTINGS
	//************************

	void PackaguesSettings()
	{
		EditorGUILayout.Space();
		packToolbarInt = GUILayout.Toolbar(packToolbarInt, packToolbar, EditorStyles.toolbarButton);

		switch(packToolbarInt)
		{
		case 0:
			MoneyPackagues();
			break;
		case 1:

			break;
		case 2:

			break;
		}
	}


	// MONEY PACKAGUES
	//********************
	void MoneyPackagues()
	{
		bool editCoins = false;

		//maxMoneyLevel nesmí být menší než 0.
		if(gm.maxMoneyLevel < 0)
			gm.maxMoneyLevel = 0;

		// Generování pole na rozdělení peněz
		if (gm.maxMoneyLevel == 0)
		{
			EditorGUILayout.HelpBox("The \"maxMoneyLevel\" field has not value. The game will not have any money packagues.", MessageType.Warning);
		}else{
			GUILayout.Space(20);

			if (coinsList.Count == 0)
			{
				EditorGUILayout.HelpBox("The list of coins is empty. You can not set the Many Packagues.", MessageType.Warning);
				GUILayout.Space(5);
				editCoins = true;
				SetCoins();
				DropAreaGUI();
			}


			if(!editCoins)
			{
				GUI.backgroundColor = new Color(0.9f,1f,0.75f,1f);
				if(GUILayout.Button("Edit Money Packagues"))
					editCoins = true;
				GUI.backgroundColor = new Color(0.9f,0.9f,0.9f,1f);
			//	SetCoins();
			}
		}



			if(GUILayout.Button("Edit the List of Coins"))
			   SetCoins();
				
		

	}

	void SetCoins()
	{
		int currentMoney = 60;


		GUILayout.Label("List of Coins:", EditorStyles.boldLabel);

		if (coinsList.Count == 0)
		{
			EditorGUILayout.HelpBox("The list of coins is empty. You can not set the Many Packagues.", MessageType.Warning);
		}else{
//			if(toolbarInt == 0)
//			{
				for(int i = 0; i < coinsList.Count; i++)
				{
					GUILayout.BeginHorizontal();

					coinsList[i] = (GameObject) EditorGUILayout.ObjectField(coinsList[i], typeof(GameObject), true);
					if(coinsList[i] != null)
						EditorGUILayout.LabelField("Value: " + coinsList[i].GetComponent<Coin>().SetValue.ToString());
					if(GUILayout.Button("-", GUILayout.Width(20f))) coinsList.RemoveAt(i);

					GUILayout.EndHorizontal();

				}
			if(GUILayout.Button("Close editing"))
			   OverwiewCoins();
		}

   


// Tlačítko na přidávání prázdných položek do seznamu. Není potřeba.
//		if(GUILayout.Button("Add Coin"))
//		{
//			coinsList.Add(null);
//		}


		if(numberCoins.Length == 0)
		{
			CreateCoinsArrey();
		}else{

		GUILayout.Label("Mony in the Level: " + gm.maxMoneyLevel, EditorStyles.boldLabel);
		GUILayout.Label("Current money: " + CallculateCurrMoney().ToString());
		if(currentMoney > gm.maxMoneyLevel)
			currentMoney = gm.maxMoneyLevel;
		Rect r = EditorGUILayout.BeginVertical();
		EditorGUI.ProgressBar(r, (float)CallculateCurrMoney() / (float)gm.maxMoneyLevel, "Current money");
		GUILayout.Space(16);
		EditorGUILayout.EndVertical();
		
	//	Debug.Log(numberCoins.Length);

//		Debug.Log(numberCoins.Length);

		for (int i = 0; i < coinsList.Count; i++)
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label(coinsList[i].name + " ("
			                + coinsList[i].GetComponent<Coin>().SetValue.ToString()
			                + "): ");
			numberCoins[i] = EditorGUILayout.IntField(numberCoins[i]);
			EditorGUILayout.EndHorizontal();
		}
		}

		if(GUILayout.Button("Resset coins"))
		{
			for(int i = 0; i < numberCoins.Length; i++)
			{
				numberCoins[i] = 0;
			}
		}

		if(GUILayout.Button("Send coins to the List of packagues"))
		{
			Debug.Log("posílám");
		}

	}

	void OverwiewCoins()
	{
		
	}

	void ListOfPackagues()
	{
		if(gm.packagues == null)
		{
			EditorGUILayout.HelpBox("The PackageList is empty.",MessageType.Info);
		}else{
			EditorGUILayout.HelpBox("The PackageList is not empty.",MessageType.Info);
		}
	}

	int[] CreateCoinsArrey()
	{
		numberCoins = new int[coinsList.Count];
		for (int i = 0; i < coinsList.Count; i++)
		{
			numberCoins[i] = 0;
		}
		return numberCoins;
	}

	int CallculateCurrMoney()
	{
		int currMon = 0;

		for(int i = 0; i < coinsList.Count; i++)
		{
			currMon += numberCoins[i] * coinsList[i].GetComponent<Coin>().SetValue;
		}
		return currMon;
	}

	public void DropAreaGUI ()
	{
		Event evt = Event.current;
		Rect drop_area = GUILayoutUtility.GetRect (0.0f, 50.0f, GUILayout.ExpandWidth (true));
		GUI.Box (drop_area, "Add Coin prefab");
		
		switch (evt.type) {
		case EventType.DragUpdated:
		case EventType.DragPerform:
			if (!drop_area.Contains (evt.mousePosition))
				return;
			
			DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
			
			if (evt.type == EventType.DragPerform) {
				DragAndDrop.AcceptDrag ();
				
				foreach (GameObject dragged_object in DragAndDrop.objectReferences) {
					// Do On Drag Stuff here

					coinsList.Add(dragged_object);
				}
			}
			break;
		}
	}


}
