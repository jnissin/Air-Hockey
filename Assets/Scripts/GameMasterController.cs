using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UniRx;

/*
3) Manage players (network: connect/disconnect, game creation, etc.)

*/
public class GameMasterController : MonoBehaviour {
	
	private int _startTime = 0;
	
	public IntReactiveProperty PlayerOneScore { get; protected set; }
	public IntReactiveProperty PlayerTwoScore { get; protected set; }
	public IntReactiveProperty GameTimeSeconds { get; protected set; }
	
	public Text PlayerOneScoreDisplay = null;
	public Text PlayerTwoScoreDisplay = null;
	

	// Use this for initialization
	void Start () {
		PlayerOneScore = new IntReactiveProperty(0);
		PlayerTwoScore = new IntReactiveProperty(0);
		GameTimeSeconds = new IntReactiveProperty(60);
		
		Observable
			.Interval(TimeSpan.FromSeconds(1))
			.Subscribe(_ => GameTimeSeconds.Value += 1);
			
		PlayerOneScore.Subscribe( score => PlayerOneScoreDisplay.text = score.ToString());
		PlayerTwoScore.Subscribe( score => PlayerTwoScoreDisplay.text = score.ToString());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
