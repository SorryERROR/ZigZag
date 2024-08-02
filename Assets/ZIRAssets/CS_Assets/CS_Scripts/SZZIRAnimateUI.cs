using UnityEngine;

namespace SZZ
{

	public class SZZIRAnimateUI : MonoBehaviour
	{
		internal float currentTime;
		
		internal float previousTime;
		
		internal float deltaTime;
		
		[Tooltip("The intro animation for this UI element")]
		public AnimationClip introAnimation;

		internal Animation animationObject;
		
		internal float animationTime = 0;
		
		internal bool isAnimating = false;
		
		[Tooltip("Should the animation be played immediately when the UI element is enabled?")]
		public bool playOnEnabled = true;
		
		void Awake()
		{
			previousTime = currentTime = Time.realtimeSinceStartup;
			
			animationObject = GetComponent<Animation>();
		}
		
		void Update()
		{
			if ( introAnimation && isAnimating == true )
			{

				currentTime = Time.realtimeSinceStartup;
				
				deltaTime = currentTime - previousTime;
				
				previousTime = currentTime;
				
				animationObject[introAnimation.name].time = animationTime;
				
				animationObject.Sample();
				
				animationTime += deltaTime;
				
				if ( animationTime >= animationObject.clip.length )
				{
					animationObject[introAnimation.name].time = animationObject.clip.length;
					
					animationObject.Sample();
					
					isAnimating = false;
				}
			}
		}

		void OnEnable()
		{
			if ( playOnEnabled == true )
			{
				PlayAnimation();
			}
		}

		public void PlayAnimation()
		{
			if ( introAnimation ) 
			{
				animationTime = 0;
			
				previousTime = currentTime = Time.realtimeSinceStartup;
			
				isAnimating = true;

				animationObject.Play();
			}
		}
	}
}

