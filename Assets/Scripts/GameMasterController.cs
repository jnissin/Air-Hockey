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
	public GameObject PuckPrefab = null;
	

	// Use this for initialization
	void Start () {
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
				Destroy(ev.gameObject);
			});

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
