using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UniRx;
using UniRx.Triggers;

/*
3) Manage players (network: connect/disconnect, game creation, etc.)

*/
using System.Collections.Generic;


public class GameMasterController : MonoBehaviour
{
	private static GameMasterController _instance = null;

	public IntReactiveProperty PlayerOneScore { get; protected set; }
	public IntReactiveProperty PlayerTwoScore { get; protected set; }
	public StringReactiveProperty PlayerOneName { get; protected set; }
	public StringReactiveProperty PlayerTwoName { get; protected set; }

	public Text PlayerOneNameDisplay = null;
	public Text PlayerTwoNameDisplay = null;
	public Text PlayerOneScoreDisplay = null;
	public Text PlayerTwoScoreDisplay = null;
	public GameObject PlayerOneGoal = null;
	public GameObject PlayerTwoGoal = null;
	public BoxCollider2D playerOneBounds = null;
	public BoxCollider2D playerTwoBounds = null;
	public Transform playerOneSpawnPosition = null;
	public Transform playerTwoSpawnPosition = null;

	public	GameObject		playerPrefab = null;
	public GameObject PuckPrefab = null;
	
	private int				_numPlayers = 0;

	public static GameMasterController Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<GameMasterController> ();
				DontDestroyOnLoad (_instance.gameObject);
			}
			
			return _instance;
		}
		
		protected set
		{
			_instance = value;
		}
	}
	
	// Use this for initialization
	void Start ()
	{
		PlayerOneScore = new IntReactiveProperty(0);
		PlayerTwoScore = new IntReactiveProperty(0);
		PlayerOneName = new StringReactiveProperty("Player A");
		PlayerTwoName = new StringReactiveProperty("Player B");
			
		PlayerOneScore.Subscribe( score => PlayerOneScoreDisplay.text = score.ToString());
		PlayerTwoScore.Subscribe( score => PlayerTwoScoreDisplay.text = score.ToString());
		PlayerOneName.Subscribe( name => PlayerOneNameDisplay.text = name);
		PlayerTwoName.Subscribe( name => PlayerTwoNameDisplay.text = name);
		
		PlayerOneGoal
			.OnTriggerEnter2DAsObservable ()
			.Where (collision => collision.gameObject.CompareTag("Puck"))
			.Subscribe (_ => PlayerTwoScore.Value += 1);
		
		PlayerTwoGoal
			.OnTriggerEnter2DAsObservable ()
			.Where (collision => collision.gameObject.CompareTag("Puck"))
			.Subscribe (_ => PlayerOneScore.Value += 1);

		PlayerOneGoal
			.OnTriggerExit2DAsObservable ()
			.Merge (PlayerTwoGoal.OnTriggerExit2DAsObservable())
			.Where (ev => ev.gameObject.CompareTag("Puck"))
			.Delay (TimeSpan.FromSeconds (0.5))
			.Subscribe(ev => {
				ev.gameObject
		           .OnDestroyAsObservable ()
		           .Delay (TimeSpan.FromSeconds (1))
		           .Subscribe (_ => Instantiate (PuckPrefab));
			
				ev.gameObject.SetActive (false);
				Destroy (ev.gameObject);
			});

	}

	void Awake ()
	{
		if (_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad (this);
		}
		else
		{
			if (this != _instance)
			{
				Destroy (this.gameObject);
			}
		}
	}

	public void SpawnPlayer ()
	{
		if (_numPlayers < 2)
		{
			GameObject newPlayer = Instantiate (playerPrefab) as GameObject;
			PlayerController newPlayerController = newPlayer.GetComponent<PlayerController> ();
			
			if (_numPlayers == 0)
			{
				newPlayer.transform.position = playerOneSpawnPosition.position;
				newPlayerController.PlayerBounds = playerOneBounds.bounds;
			}
			else
			{
				newPlayer.transform.position = playerTwoSpawnPosition.position;
				newPlayerController.PlayerBounds = playerTwoBounds.bounds;
			}
			
			_numPlayers++;
		}
		else
		{
			Debug.LogError ("Cannot spawn more than two players");
		}
	}
}
