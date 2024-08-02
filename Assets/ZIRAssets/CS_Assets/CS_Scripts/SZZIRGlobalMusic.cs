using UnityEngine;
using System.Collections;

namespace SZZ
{
	public class SZZIRGlobalMusic : MonoBehaviour 
	{
		//The tag of the music source
		public string musicTag = "Music";
		
		//The time this instance of the music source has been in the game
		internal float instanceTime = 0;

		void  Awake()
		{
            //Find all the music objects in the scene
            GameObject[] musicObjects = GameObject.FindGameObjectsWithTag(musicTag);
			
			//Keep only the music object which has been in the game for more than 0 seconds
			if ( musicObjects.Length > 1 )
			{
				foreach( var musicObject in musicObjects )
				{
					if ( musicObject.GetComponent<SZZIRGlobalMusic>().instanceTime <= 0 )    Destroy(gameObject);
				}
			}
		}

		void  Start()
		{
			DontDestroyOnLoad(transform.gameObject);
		}
		
	}
}
