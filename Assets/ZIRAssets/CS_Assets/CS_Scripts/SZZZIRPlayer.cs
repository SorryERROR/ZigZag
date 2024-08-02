using UnityEngine;
using System.Collections;

namespace SZZ
{
	public class SZZZIRPlayer : MonoBehaviour 
	{
		/// <summary>
		/// This script defines the player, its movement and turning speed, as well as the effects and sounds associated with it.
		/// </summary>
		internal Transform thisTransform;
		internal GameObject gameController;
		
		//The player's movement speed, and variables that check if the player is moving now, where it came from, and where it's going to
		public float speed = 0.1f;
		
		//How fast the player rotates towards its target direction
		public float turnSpeed = 100;
		internal float turnDirection = 0;
		
		//A list of all the wheels that can turn
		public Transform[] wheels;
		
		//Death effects that show when the player is killed
		public Transform deathEffect;

		// The animation that plays when we win
		public string animationVictory = "PlayerVictory";

		// Did the player win the game?
		internal bool isVictorious = false;
		
		//Various sounds and their source
		public AudioClip[] soundTurn;
		public AudioSource SZZSounds;

		void  Start ()
		{
			thisTransform = transform;
			
			gameController = GameObject.FindGameObjectWithTag("GameController");
		}

		void  Update ()
		{
			if ( Time.timeScale > 0 && isVictorious == false )
			{
				if ( Mathf.Abs(Mathf.DeltaAngle( thisTransform.eulerAngles.y, turnDirection)) > 5 )    thisTransform.eulerAngles = new Vector3( thisTransform.eulerAngles.x, Mathf.LerpAngle( thisTransform.eulerAngles.y, turnDirection, Time.deltaTime * turnSpeed), thisTransform.eulerAngles.z);
				else    thisTransform.eulerAngles = new Vector3( thisTransform.eulerAngles.x, turnDirection, thisTransform.eulerAngles.z);
				
				thisTransform.Translate( Vector3.forward * speed * Time.deltaTime, Space.Self);
				
				wheels[0].eulerAngles = new Vector3( wheels[0].eulerAngles.x, Mathf.LerpAngle( wheels[0].eulerAngles.y, turnDirection, Time.deltaTime * turnSpeed), wheels[0].eulerAngles.z);
				wheels[1].eulerAngles = new Vector3( wheels[1].eulerAngles.x, Mathf.LerpAngle( wheels[1].eulerAngles.y, turnDirection, Time.deltaTime * turnSpeed), wheels[1].eulerAngles.z);
			}
		}
		
		void  Die (  int deathType   )
		{
			gameController.SendMessage("GameOver", 2);
			
			if ( deathEffect )    Instantiate( deathEffect, thisTransform.position, thisTransform.rotation);

			Destroy(gameObject);
		}

		void Victory()
		{
			isVictorious = true;

			if ( GetComponent<Animation>() && animationVictory != string.Empty )    GetComponent<Animation>().Play(animationVictory);
		}
		
		void  SetTurn (  float direction   )
		{
			turnDirection = direction;
			
			if (soundTurn.Length > 0 ) SZZSounds.PlayOneShot(soundTurn[Mathf.FloorToInt(Random.value * soundTurn.Length)]);
		}
	}
}