using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using TouchScript.Gestures.Simple;
using System;

public class PaddleController : MonoBehaviour
{
	[SerializeField]
	private BoxCollider2D 	_playerBoundsCollider 	= null;

	private Bounds			_playerBounds;
	private Vector3 		_localPositionToGo 		= Vector3.zero;
	private Vector3 		_worldDeltaPosition 	= Vector3.zero;

	private Rigidbody2D Rigidbody
	{
		get;
		set;
	}

	void Awake()
	{
		Rigidbody = GetComponent<Rigidbody2D> ();

		_localPositionToGo = transform.localPosition;
		_playerBounds = _playerBoundsCollider.bounds;
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

		Vector3 newWorldPos = transform.TransformPoint (_localPositionToGo + gesture.LocalDeltaPosition);

		if (_playerBounds.Contains (gesture.WorldTransformCenter))
		{
			_localPositionToGo += gesture.LocalDeltaPosition;
		}
	}
}
