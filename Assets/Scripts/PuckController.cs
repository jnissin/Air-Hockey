using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class PuckController : MonoBehaviour
{
	[SerializeField]
	private	GameObject	_trail			= null;

	private AudioSource _audioSource 	= null;
	private Rigidbody2D _rigidbody 		= null;

	public AudioSource AudioSource
	{
		get				{ return _audioSource; 	}
		protected set	{ _audioSource = value;	}
	}

	public Rigidbody2D Rigidbody
	{
		get				{ return _rigidbody; 	}
		protected set	{ _rigidbody = value;	}
	}

	public GameObject Trail
	{
		get				{ return _trail; 	}
		protected set	{ _trail = value;	}
	}

	// Use this for initialization
	void Start ()
	{
		AudioSource = GetComponent<AudioSource> ();
		Rigidbody = GetComponent<Rigidbody2D> ();

		this.UpdateAsObservable ()
			.Select (_ => GetPositionVector ())
			.DistinctUntilChanged ()
			.Subscribe (position => Trail.transform.localPosition = position);			
	}

	Vector3 GetPositionVector ()
	{
		if (Rigidbody.velocity.magnitude > 0.0f)
		{
			Vector2 posXY = Rigidbody.velocity.normalized * -0.2f;
			return new Vector3 (posXY.x, posXY.y, 0.0f);
		}
		else
		{
			return Vector3.zero;
		}
	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		AudioSource.Play ();
	}
}
