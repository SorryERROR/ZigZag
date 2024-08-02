using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SZZ
{
	/// <summary>
	/// This script switches between several objects in a sequence. The current shown object number is recorded
    /// in a PlayerPrefs record so it can be kept across several playthroughs
	/// </summary>
	public class SZZIRSwitchObjects : MonoBehaviour
	{
        public Transform[] switchObjects;
        internal int switchIndex = 0;

        public string playerPrefsName = "TimesOfDay";

        internal int index;
	
		void Start()
		{
            switchIndex = PlayerPrefs.GetInt(playerPrefsName, switchIndex);

            for ( index = 0; index < switchObjects.Length; index++)
            {
                if (index == switchIndex) switchObjects[index].gameObject.SetActive(true);
                else switchObjects[index].gameObject.SetActive(false);
            }

            if (switchIndex < switchObjects.Length - 1) switchIndex++;
            else switchIndex = 0;

            PlayerPrefs.SetInt(playerPrefsName, switchIndex);
        }
	}
}