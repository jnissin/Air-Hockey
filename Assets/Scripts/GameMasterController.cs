using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UniRx;
using UniRx.Triggers;

/*
3) Manage players (network: connect/disconnect, game creation, etc.)

*/
public class GameMasterController : MonoBehaviour {
	
	private int _startTime = 0;

	public IntReactiveProperty PlayerOneScore { get; protected set; }
	public IntReactiveProperty PlayerTwoScore { get; protected set; }
	public IntReactiveProperty GameTimeSeconds { get; protected set; }

	public Text PlayerOneNameDisplay = null;
	public Text PlayerTwoNameDisplay = null;
	public Text PlayerOneScoreDisplay = null;
	public Text PlayerTwoScoreDisplay = null;
	public GameObject PlayerOneGoal = null;
	public GameObject PlayerTwoGoal = null;
	public GameObject PuckPrefab = null;
	

	// Use this for initialization
	void Start () {
		PlayerTwoScore = new IntReactiveProperty(0);
		PlayerOneScore = new IntReactiveProperty(0);
		GameTimeSeconds = new IntReactiveProperty(60);
		
		Observable
			.Interval(TimeSpan.FromSeconds(1))
			.Subscribe(_ => GameTimeSeconds.Value += 1);
			
		PlayerTwoScore.Subscribe( score => PlayerTwoScoreDisplay.text = score.ToString());
		PlayerOneScore.Subscribe( score => PlayerOneScoreDisplay.text = score.ToString());
		
		PlayerTwoGoal
			.OnTriggerEnter2DAsObservable ()
			.Where (collision => collision.gameObject.CompareTag("Puck"))
			.Subscribe (_ => PlayerOneScore.Value += 1);
		
		PlayerOneGoal
			.OnTriggerEnter2DAsObservable ()
			.Where (collision => collision.gameObject.CompareTag("Puck"))
			.Subscribe (_ => PlayerTwoScore.Value += 1);

		PlayerTwoGoal
			.OnTriggerExit2DAsObservable ()
			.Merge (PlayerOneGoal.OnTriggerExit2DAsObservable())
			.Where (trigger => trigger.gameObject.CompareTag("Puck"))
			.Delay (TimeSpan.FromSeconds (0.5))
			.Subscribe(ev => {
				ev.gameObject
		           .OnDestroyAsObservable ()
		           .Delay (TimeSpan.FromSeconds (1))
		           .Subscribe (_ => Instantiate (PuckPrefab));
			
//				ev.gameObject.SetActive (false);
				Destroy (ev.gameObject);
			});

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
