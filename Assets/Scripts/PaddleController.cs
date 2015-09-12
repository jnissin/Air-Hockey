using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using TouchScript.Gestures.Simple;
using System;

public class PaddleController : MonoBehaviour
{
	private Vector3 _localPositionToGo 	= Vector3.zero;
	private Vector3	_lastLocalPosition 	= Vector3.zero;
	private Vector3 _worldDeltaPosition = Vector3.zero;

	private Rigidbody2D Rigidbody
	{
		get;
		set;
	}

	void Awake()
	{
		_localPositionToGo = _lastLocalPosition = transform.localPosition;
		Rigidbody = GetComponent<Rigidbody2D> ();
	}
	
	void FixedUpdate ()
	{
		_lastLocalPosition = _localPositionToGo;
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

	private void PanHandler(object sender, EventArgs e)
	{
		var gesture = (SimplePanGesture)sender;
		
		_localPositionToGo += gesture.LocalDeltaPosition;
	}
}
