using UnityEngine.SceneManagement;
using System.Collections;
using SZZ.Types;
using UnityEngine;
using UnityEngine.UI;

namespace SZZ
{
	public class SZZIRGameController : MonoBehaviour 
	{
		public Transform cameraObject;
		
		public Transform[] playerObjects;
		public int currentPlayer = 0;
		
		internal Vector3 playerPos;
		internal Vector3 playerPrevPos;
		
		public string turnButton = "Submit";
		
		public Transform groundObject;
		
		public Transform[] groundDetails;
		
		public float detailsOffset = 4;
		
		public Transform nextRoadSection;
		
		internal float turnDirection = 0;
        internal float[] turnDirections = { 0, 90 };
        internal int turnDirectionIndex = 0;
		
		internal float roadDirection = 0;
		
		internal bool  previousWasATurn = false;
		
		public int defaultRoadSize = 2;
		
		public Transform roadStraight;
		
		public Transform roadTurn;
		
		public Vector2 roadSectionLength = new Vector2(0,5);
		
		public int precreateRoads = 20;
		
		public ItemDrop[] itemDrops;
		internal Transform[] itemDropList;
		
		public float itemDropOffset = 0.5f;
		
		public int score = 0;
		public Transform scoreText;
		public string scoreTextSuffix = "";
		internal int highScore = 0;

        public int scoreToWin = 0;
		
		public string moneyPlayerPrefs = "Money";
		
		public float gameSpeed = 1;
		
		public int levelUpEveryScore = 500;
		internal int increaseCount = 0;
		
		public float levelUpSpeedIncrease = 0.1f;
		
		public string[] levelUpMessages;
		
		public Transform messageObject;
		
		public Transform gameCanvas;
		public Transform pauseCanvas;
		public Transform gameOverCanvas;
        public Transform victoryCanvas;

        internal bool  isGameOver = false;
		
		public string mainMenuLevelName = "MainMenu";
		
		public AudioClip soundLevelUp;
		public AudioClip soundGameOver;
        public AudioClip soundVictory;

        public AudioSource SZZSounds;
		
		public string confirmButton = "Submit";
		
		public string pauseButton = "Cancel";
		internal bool  isPaused = false;
		
		internal int index = 0;

        void Awake()
        {
            if (pauseCanvas) pauseCanvas.gameObject.SetActive(true);
        }

 
        void  Start ()
		{
			ChangeScore(0);
			
			if ( gameOverCanvas )    gameOverCanvas.gameObject.SetActive(false);
            if ( victoryCanvas)    victoryCanvas.gameObject.SetActive(false);
            if (pauseCanvas) pauseCanvas.gameObject.SetActive(false);

            highScore = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "_HighScore", 0);

            int totalDrops = 0;
			int totalDropsIndex = 0;
			
			for ( index = 0 ; index < itemDrops.Length ; index++ )
			{
				totalDrops += itemDrops[index].dropChance;
			}
			
			itemDropList = new Transform[totalDrops];
			
			for ( index = 0 ; index < itemDrops.Length ; index++ )
			{
				int dropChanceCount = 0;
				
				while ( dropChanceCount < itemDrops[index].dropChance )
				{
					itemDropList[totalDropsIndex] = itemDrops[index].droppedObject;
					
					dropChanceCount++;
					
					totalDropsIndex++;
				}
			}
			
			currentPlayer = PlayerPrefs.GetInt("CurrentPlayer", currentPlayer);
			
			SetPlayer(currentPlayer);
			
			playerPos = playerPrevPos = playerObjects[currentPlayer].position;
			
			if ( cameraObject == null )    cameraObject = GameObject.FindGameObjectWithTag("MainCamera").transform;
			
			createSection(precreateRoads);


            LevelUpEffect();
        }

		void  Update()
		{	
			if ( isGameOver == true )
			{
				if ( Input.GetButtonDown(confirmButton) )
				{
					Restart();
				}
				
				if ( Input.GetButtonDown(pauseButton) )
				{
					MainMenu();
				}
			}
			else
			{
				if ( isPaused == true )
				{
					if ( Input.GetButtonDown(pauseButton) )    Unpause();
					if ( Input.GetButtonDown(confirmButton) )    Unpause();
				}
				else
				{
					if ( Input.GetButtonDown(turnButton) )    TurnPlayer();
					
					if ( Input.GetButtonDown(pauseButton) )    Pause();
				}
			}
			
			if ( cameraObject && groundObject )
			{
				groundObject.position = new Vector3( cameraObject.position.x, groundObject.position.y, cameraObject.position.z);
				
				playerPos = cameraObject.position;
				
				groundObject.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(-0.4f * playerPos.x, -0.4f * playerPos.z);
				
				playerPrevPos = playerPos;
			}
		}


		void LateUpdate()
		{
			if ( playerObjects[currentPlayer] )    
			{
				if ( Vector3.Distance( playerObjects[currentPlayer].position, nextRoadSection.position ) < 20 )
				{ 
					createSection(1);
				}
				
				if ( cameraObject )
				{
					cameraObject.position = new Vector3( playerObjects[currentPlayer].position.x, cameraObject.position.y, playerObjects[currentPlayer].position.z);
				}
				
				if ( messageObject )
				{
					messageObject.position = playerObjects[currentPlayer].position;
				}
			}
		}
		
		void  createSection (  int sectionCount   )
		{
			while ( sectionCount > 0 )
			{
				sectionCount--;
				
				if ( previousWasATurn == false && roadTurn )
				{
					Transform newTurnSection = Instantiate(roadTurn) as Transform;
					
					if ( roadDirection == 90 )
					{
						roadDirection = 0;
						
						newTurnSection.position = nextRoadSection.position + new Vector3(defaultRoadSize,0,0);

						newTurnSection.eulerAngles = new Vector3( newTurnSection.eulerAngles.x, 180, newTurnSection.eulerAngles.z);
					}
					else
					{
						roadDirection = 90;
						
						newTurnSection.position = nextRoadSection.position + new Vector3(0,0,defaultRoadSize);
					}
					
					nextRoadSection = newTurnSection;
					
					previousWasATurn = true;
				}
				else if ( roadStraight )
				{
					int roadSectionLengthTemp = Mathf.RoundToInt(Random.Range(roadSectionLength.x, roadSectionLength.y));
					
					float itemOffset = Random.Range(-itemDropOffset, itemDropOffset);
					
					for ( index = 0; index < roadSectionLengthTemp; index++)
					{
						Transform newStraightSection= Instantiate(roadStraight) as Transform;
						
						if ( roadDirection == 0 )
						{
							newStraightSection.position = nextRoadSection.position + new Vector3(0,0,defaultRoadSize);
						}
						else
						{
							newStraightSection.position = nextRoadSection.position + new Vector3(defaultRoadSize,0,0);
							
							newStraightSection.eulerAngles = new Vector3( newStraightSection.eulerAngles.x, 90, newStraightSection.eulerAngles.z);
						}
						
						nextRoadSection = newStraightSection; 
						
						Transform newItem = Instantiate( itemDropList[Mathf.FloorToInt(Random.Range(0, itemDropList.Length))] ) as Transform;
						
						newItem.position = newStraightSection.position;
						
						newItem.Translate( newStraightSection.right * itemOffset, Space.Self);
						
                        if (index > 0 && index < roadSectionLengthTemp - 1)
                        {
                            Transform newGroundObject = Instantiate(groundDetails[Mathf.FloorToInt(Random.Range(0, groundDetails.Length))]) as Transform;

                            newGroundObject.position = new Vector3(newStraightSection.position.x, newStraightSection.position.y + groundObject.position.y, newStraightSection.position.z);

                            float minimumOffset = defaultRoadSize + newGroundObject.GetComponentInChildren<MeshRenderer>().bounds.size.x;

                            if ( roadDirection == 90 )
                            {
                                newGroundObject.Translate( newStraightSection.right * Random.Range( minimumOffset, minimumOffset + detailsOffset ), Space.World);

                            }
                            else if ( roadDirection == 0 )
                            {
                                newGroundObject.Translate( newStraightSection.right * Random.Range( -minimumOffset, -minimumOffset - detailsOffset ), Space.World);

                            }

                            newGroundObject.Rotate(Vector3.up * Random.Range(0, 360), Space.World);

                        }
                    }
					
					previousWasATurn = false;
				}
				
			}
			
		}
		
		void  ChangeScore (  int changeValue   )
		{
			score += changeValue;
			
			if ( scoreText )    scoreText.GetComponent<Text>().text = score.ToString() + scoreTextSuffix;

            if (scoreText.GetComponent<Animation>()) scoreText.GetComponent<Animation>().Play();

            increaseCount += changeValue;

            if (scoreToWin > 0 && score >= scoreToWin)
            {
                Victory();

                return;
            }

            if ( increaseCount >= levelUpEveryScore )
			{
				increaseCount -= levelUpEveryScore;
				
				LevelUp();
			}
           
            
		}
		
		void  LevelUp ()
		{
			gameSpeed += levelUpSpeedIncrease;
			
			Time.timeScale = gameSpeed;
			
			LevelUpEffect();
		}
		
		void  LevelUpEffect ()
		{
			if ( messageObject )
			{
				if ( messageObject.GetComponent<Animation>() )    messageObject.GetComponent<Animation>().Play();
				
				if ( levelUpMessages.Length > 0 )    messageObject.Find("Base/Text").GetComponent<Text>().text = levelUpMessages[Mathf.FloorToInt(Random.Range(0, levelUpMessages.Length))];
			}
			
			if ( soundLevelUp )    SZZSounds.PlayOneShot(soundLevelUp);
		}
		
		public void  Pause ()
		{
			isPaused = true;
			
			Time.timeScale = 0;
			
			if ( pauseCanvas )    pauseCanvas.gameObject.SetActive(true);
			if ( gameCanvas )    gameCanvas.gameObject.SetActive(false);
		}

        /// <summary>
        /// Unpauses the game
        /// </summary>
        public void Unpause ()
		{
			isPaused = false;
			
			//Set timescale back to the current game speed
			Time.timeScale = gameSpeed;
			
			//Hide the pause screen and show the game screen
			if ( pauseCanvas )    pauseCanvas.gameObject.SetActive(false);
			if ( gameCanvas )    gameCanvas.gameObject.SetActive(true);
		}
		
		IEnumerator GameOver(float delay)
		{
			yield return new WaitForSeconds(delay);
			
			isGameOver = true;
			
			//Remove the pause and game screens
			if ( pauseCanvas )    Destroy(pauseCanvas.gameObject);
			if ( gameCanvas )    Destroy(gameCanvas.gameObject);
			
			//Get the number of money we have
			int totalMoney = PlayerPrefs.GetInt( moneyPlayerPrefs, 0);
			
			//Add to the number of money we collected in this game
			totalMoney += score;
			
			//Record the number of money we have
			PlayerPrefs.SetInt( moneyPlayerPrefs, totalMoney);
			
			if ( gameOverCanvas )    
			{
				gameOverCanvas.gameObject.SetActive(true);
				
				gameOverCanvas.Find("Base/TextScore").GetComponent<Text>().text = "SCORE " + score.ToString();
				
				if ( score > highScore )    
				{
					highScore = score;

                    PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_HighScore", score);
                }

                gameOverCanvas.Find("Base/TextHighScore").GetComponent<Text>().text = "HIGH SCORE " + highScore.ToString();
			}

            if (soundGameOver ) SZZSounds.PlayOneShot(soundGameOver);
        }

		void Victory()
        {
            isGameOver = true;

			if ( playerObjects[currentPlayer] )    playerObjects[currentPlayer].SendMessage("Victory");

            if (pauseCanvas) Destroy(pauseCanvas.gameObject);
            if (gameCanvas) Destroy(gameCanvas.gameObject);

            int totalMoney = PlayerPrefs.GetInt(moneyPlayerPrefs, 0);

            totalMoney += score;

            PlayerPrefs.SetInt(moneyPlayerPrefs, totalMoney);

            if (victoryCanvas)
            {
                victoryCanvas.gameObject.SetActive(true);

                victoryCanvas.Find("Base/TextScore").GetComponent<Text>().text = "SCORE " + score.ToString();

                if (score > highScore)
                {
                    highScore = score;

                    PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_HighScore", score);
                }

                victoryCanvas.Find("Base/TextHighScore").GetComponent<Text>().text = "HIGH SCORE " + highScore.ToString();
            }

            if (soundVictory) SZZSounds.PlayOneShot(soundVictory);
        }

		void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        void MainMenu()
        {
            SceneManager.LoadScene(mainMenuLevelName);
        }

        void  SetPlayer (  int playerNumber   )
		{
			//Go through all the players, and hide each one except the current player
			for(index = 0; index < playerObjects.Length; index++)
			{
				if ( index != playerNumber )    playerObjects[index].gameObject.SetActive(false);
				else    playerObjects[index].gameObject.SetActive(true);
			}
		}
        
		public void TurnPlayer ()
		{
			if ( isPaused == false && playerObjects[currentPlayer] )    
			{
                // Use the turn directions from the list
                if (turnDirections.Length > 0)
                {
                    // Go to the next available turn direction, or start over from the first direction
                    if (turnDirectionIndex < turnDirections.Length - 1) turnDirectionIndex++;
                    else turnDirectionIndex = 0;

                    // Set the direction
                    turnDirection = turnDirections[turnDirectionIndex];
                }
                else // Or use the default 0 to 90 to 0 turns
                {
                    if ( turnDirection == 0 )    turnDirection = 90;
                    else turnDirection = 0;
                }

                // Make the player turn
                playerObjects[currentPlayer].SendMessage("SetTurn", turnDirection);
			}
		}
	}
}