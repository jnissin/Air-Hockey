using UnityEngine;
using System;
using System.Collections;
using UniRx;

/*
3) Manage players (network: connect/disconnect, game creation, etc.)

*/
public class GameMasterController : MonoBehaviour {
	
	public IntReactiveProperty PlayerOneScore { get; protected set; }
	public IntReactiveProperty PlayerTwoScore { get; protected set; }
	public IntReactiveProperty GameTimeSeconds { get; protected set; }

	// Use this for initialization
	void Start () {
		PlayerOneScore = new IntReactiveProperty(0);
		PlayerTwoScore = new IntReactiveProperty(0);
		GameTimeSeconds = new IntReactiveProperty(60);
		Observable
			.Interval(TimeSpan.FromSeconds(1))
			.Subscribe(_ => GameTimeSeconds.Value = GameTimeSeconds.Value > 1 ? GameTimeSeconds.Value - 1 : 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
