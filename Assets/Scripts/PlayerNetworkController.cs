using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using TouchScript.Gestures.Simple;

public class PlayerNetworkController : NetworkBehaviour
{
	[SyncVar]			private Vector3			syncPosition 		= Vector3.zero;
	[SerializeField]	private	Rigidbody2D		playerRigidbody;
	[SerializeField] 	private Transform 		playerTransform;
	[SerializeField]	private float			lerpRate		 	= 15.0f;
	[SerializeField]	private	float			transmitThreshold	= 1.0f;
						private	Vector3			lastPosition		= Vector3.zero;

	void Start ()
	{
		if (isLocalPlayer)
		{
			GetComponent<SimplePanGesture> ().enabled = true;
			GetComponent<PlayerController> ().enabled = true;
		}
	}

	void FixedUpdate ()
	{
		TransmitPosition ();
	}

	void Update ()
	{
		LerpPosition ();
	}

	void LerpPosition ()
	{
		if (!isLocalPlayer)
		{
			playerRigidbody.MovePosition (Vector3.Lerp (playerTransform.position, syncPosition, Time.deltaTime * lerpRate));
		}
	}

	[Command]
	void CmdProvidePositionToServer (Vector3 position)
	{
		syncPosition = position;
	}

	[ClientCallback]
	void TransmitPosition ()
	{
		if (isLocalPlayer && Vector3.Distance (playerTransform.position, lastPosition) > transmitThreshold)
		{
			CmdProvidePositionToServer (playerTransform.position);
			lastPosition = playerTransform.position;
		}
	}
}
