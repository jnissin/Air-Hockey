using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using TouchScript.Gestures.Simple;
using System;

public class PlayerController : MonoBehaviour
{
	private Bounds			_playerBounds;
	private Vector3 		_localPositionToGo 		= Vector3.zero;
	private Vector3 		_worldDeltaPosition 	= Vector3.zero;

	public Bounds PlayerBounds
	{
		get { return _playerBounds; }
		set { _playerBounds = value; }
	}

	private Rigidbody2D Rigidbody
	{
		get;
		set;
	}

	void Awake()
	{
		Rigidbody = GetComponent<Rigidbody2D> ();
		_localPositionToGo = transform.localPosition;
	}
	
	void FixedUpdate ()
	{
		Rigidbody.MovePosition (new Vector2 (_localPositionToGo.x, _localPositionToGo.y));
	}

	void OnEnable()
	{
		if (GetComponent<SimplePanGesture>() != null)
		{
			GetComponent<SimplePanGesture>().Panned += PanHandler;
		}
	}

	void OnDisable ()
	{
		if (GetComponent<SimplePanGesture>() != null)
		{
			GetComponent<SimplePanGesture> ().Panned -= PanHandler;
		}
	}

	void PanHandler(object sender, EventArgs e)
	{
		var gesture = (SimplePanGesture)sender;

		//Vector3 newWorldPos = transform.TransformPoint (_localPositionToGo + gesture.LocalDeltaPosition);

		//if (_playerBounds.Contains (gesture.WorldTransformCenter))
		//{
			_localPositionToGo += gesture.LocalDeltaPosition;
		//}
	}
}
