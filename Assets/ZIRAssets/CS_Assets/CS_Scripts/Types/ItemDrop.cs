using UnityEngine;
using System;

namespace SZZ.Types
{
	[Serializable]
	public class ItemDrop 
	{
		//The object that can be dropped
		public Transform droppedObject;
		
		//The drop chance of the object
		public int dropChance = 1;
	}
}