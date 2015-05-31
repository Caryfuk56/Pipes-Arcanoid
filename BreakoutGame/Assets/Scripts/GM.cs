using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GM : MonoBehaviour {

	public int lives = 3;
	public int bricks = 24;
	public float resetDelay = 1f;
	public Text livesText;
	public GameObject gameOver;
	public GameObject youWon;
	public GameObject brickPrefab;
	public GameObject paddle;
	public GameObject deathParticles;

	public int maxMoneyLevel;

	public List<PackageList> allPackages = new List<PackageList>();
	public List<PackaguesToGame>packagues = new List<PackaguesToGame>();

	public static GM instance = null;

	private GameObject clonePaddle;
	private int[] mandatoryOrder;


	// Use this for initialization
	void Awake ()
	{
		if(instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		Setup();
	}


	public void Setup()
	{
		clonePaddle = Instantiate(paddle, transform.position, Quaternion.identity) as GameObject;
		Instantiate(brickPrefab, transform.position, Quaternion.identity);
	}

	public void LoseLife()
	{
		lives --;
		livesText.text = "Lives: " + lives;
		Instantiate(deathParticles, clonePaddle.transform.position, Quaternion.identity);
		Destroy(clonePaddle);
		Invoke("SetupPaddle", resetDelay);
		CheckGameOver();
	}

	public void DestroyBrick()
	{
		bricks --;
		CheckGameOver();
	}

	void CheckGameOver()
	{
		if(bricks < 1)
		{
			youWon.SetActive(true);
			Time.timeScale = .25f;
			Invoke("Reset", resetDelay);
		}

		if (lives < 1)
		{
			gameOver.SetActive(true);
			Time.timeScale = .25f;
			Invoke("Reset", resetDelay);
		}
	}

	void Reset()
	{
		Time.timeScale = 1;
		Application.LoadLevel(Application.loadedLevel);
	}

	void SetupPaddle()
	{
		clonePaddle = Instantiate(paddle, transform.position, Quaternion.identity) as GameObject;
	}


	// Set and instantiate packages.

//	void OrderMandatoryPackages()
//	{
//		mandatoryOrder = new int[]
//	}

}
