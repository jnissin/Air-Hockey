using UnityEngine;
using System.Collections;

public class PuckController : MonoBehaviour
{

	private AudioSource m_audioSource = null;

	public AudioSource AudioSource
	{
		get				{ return m_audioSource; 	}
		protected set	{ m_audioSource = value;	}
	}

	// Use this for initialization
	void Start ()
	{
		AudioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		AudioSource.Play ();
	}
}
