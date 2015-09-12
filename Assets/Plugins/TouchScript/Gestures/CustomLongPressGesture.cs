/*
 * @author Valentin Simonov / http://va.lent.in/
 */

using System;
using System.Collections;
using System.Collections.Generic;
using TouchScript.Utils;
using TouchScript.Utils.Attributes;
using UnityEngine;
using TouchScript.Layers;

namespace TouchScript.Gestures
{
	/// <summary>
	/// Gesture which recognizes a point cluster which didn't move for specified time since it appeared.
	/// </summary>
	[AddComponentMenu("TouchScript/Gestures/Custom Long Press Gesture")]
	public class CustomLongPressGesture : Gesture
	{
		#region Public properties
		
		/// <summary>
		/// Total time in seconds required to hold touches still.
		/// </summary>
		public float TimeToPress
		{
			get { return timeToPress; }
			set { timeToPress = value; }
		}

		public List<int> StartedTouchIDs;
		public List<int> FailedTouchIDs;
		public List<int> SuccessfulTouchIDs;

		public Dictionary<int, Vector2> TouchPositions;

		#endregion
		
		#region Private variables
		
		[SerializeField]
		private float timeToPress = 1;

//		private Vector3 projectionNormal = Vector3.forward;
//		private Vector3 center = Vector3.zero;
//		private Plane worldTransformPlane;

		private Dictionary<ITouch, Coroutine> coroutineDict;
		#endregion
		
		#region Unity methods
		
		/// <inheritdoc />
		protected override void OnEnable()
		{
			base.OnEnable();
			coroutineDict = new Dictionary<ITouch, Coroutine> ();
			TouchPositions = new Dictionary<int, Vector2> ();
//			worldTransformPlane = new Plane(projectionNormal, center);
		}
		
		#endregion
		
		#region Gesture callbacks
		
		/// <inheritdoc />
		protected override void touchesBegan(IList<ITouch> touches)
		{
			base.touchesBegan(touches);

			if (touches.Count == activeTouches.Count) this.SetState (GestureState.Began);

			foreach (ITouch touch in touches) {
				TouchPositions.Add (touch.Id, Camera.main.ScreenToWorldPoint(touch.Position) );
				Coroutine coroutine = StartCoroutine ("wait", touch);
				coroutineDict.Add(touch, coroutine);
				StartedTouchIDs.Add (touch.Id);
				this.SetState(GestureState.Changed);
			}
		}
		
		/// <inheritdoc />
		protected override void touchesEnded(IList<ITouch> touches)
		{
			base.touchesEnded(touches);
			
			if (activeTouches.Count == 0) 
			{
				setState (GestureState.Ended);
			}
			else {
				foreach(ITouch touch in touches)
				{
					StopCoroutine(coroutineDict[touch]);
					FailedTouchIDs.Add (touch.Id);
					this.SetState(GestureState.Changed);
				}
			}
		}
		
		/// <inheritdoc />
		protected override void reset()
		{
			base.reset();
//			Debug.Log ("Reseting");
			StopAllCoroutines();
			FailedTouchIDs = new List<int>();
			SuccessfulTouchIDs = new List<int>();
			StartedTouchIDs = new List<int>();
			coroutineDict = new Dictionary<ITouch, Coroutine> ();
			TouchPositions = new Dictionary<int, Vector2> ();
		}
		
		#endregion
		
		#region Private functions
		
		private IEnumerator wait(ITouch touch)
		{
			// WaitForSeconds is affected by time scale!
			var targetTime = Time.unscaledTime + TimeToPress;
			while (targetTime > Time.unscaledTime) yield return null;

			SuccessfulTouchIDs.Add(touch.Id);
			setState(GestureState.Changed);
		}
		
		#endregion
	}
}
