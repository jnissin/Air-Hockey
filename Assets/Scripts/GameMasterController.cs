using UnityEngine;
using System.Collections;
using UniRx;

/*
1) Hold score
2) Hold gametime
3) Manage players (network: connect/disconnect, game creation, etc.)

*/
public class GameMasterController : MonoBehaviour {
	
	public ReactiveProperty<int> PlayerOneScore { get; protected set; }
	public ReactiveProperty<int> PlayerTwoScore { get; protected set; }

	// Use this for initialization
	void Start () {
		PlayerOneScore = new ReactiveProperty<int>(0);
		PlayerTwoScore = new ReactiveProperty<int>(0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
