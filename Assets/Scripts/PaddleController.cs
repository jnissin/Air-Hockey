using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using TouchScript.Gestures.Simple;

public class PaddleController : MonoBehaviour
{
	public float m_velocityMultiplier = 1.0f;

	public float VelocityMultiplier
	{
		get				{ return m_velocityMultiplier; 	}
		protected set	{ m_velocityMultiplier = value; }
	}

	private Rigidbody2D Rigidbody
	{
		get;
		set;
	}

	// Use this for initialization
	void Start ()
	{
		Rigidbody = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnEnable ()
	{
		GetComponent<SimplePanGesture> ().Panned += PanHandler;
	}

	void OnDisable ()
	{
		GetComponent<SimplePanGesture> ().Panned -= PanHandler;
	}

	void PanHandler (object sender, System.EventArgs e)
	{
		PanGesture gesture = sender as PanGesture;
		
		if (float.IsNaN (gesture.ScreenPosition.x) || float.IsNaN (gesture.ScreenPosition.y))
			return;
		
		Vector2 newVelocity = new Vector2 (gesture.WorldDeltaPosition.x, gesture.WorldDeltaPosition.y) * VelocityMultiplier;
		GetComponent<Rigidbody2D> ().velocity = newVelocity;
	}
}
