using UnityEngine;

namespace SZZ
{
	public class SZZIRPlaySound : MonoBehaviour
	{
		public AudioClip[] audioList;
	
		public AudioSource SZZSound;
		public bool playOnStart = true;
	
		void Start()
		{
			if (SZZSound == null)
				SZZSound = GameObject.FindGameObjectWithTag("Sound").GetComponent<AudioSource>();
			
			if( playOnStart == true )
				PlaySound();
		}
	
		void PlaySound()
		{
			if( audioList.Length > 0 )
				SZZSound.PlayOneShot(audioList[Mathf.FloorToInt(Random.value * audioList.Length)]);
		}
	
		void PlaySound(int soundIndex)
		{
			if(audioList.Length > 0 ) 
				SZZSound.PlayOneShot(audioList[soundIndex]);
		}

        /// <summary>
		/// Plays the sound
		/// </summary>
		public void PlaySound(AudioClip sound)
        {
            if (sound)
            {
                SZZSound.PlayOneShot(sound);
            }
        }
    }
}