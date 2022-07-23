using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MainMenu,
        InLevel
    }
    public static GameState currentGameState = GameState.MainMenu;

    //Main Menu

    //In Level
    public Transform enemyHolder = null;
    public enum EnemyTypes
    {
        Gremlin,
        Bat,
        Worm
    }
    public float[] enemySpawnChances = { 0.5f, 0.5f, 0.1f };
    private int numberOfEnemiesToSpawn = 2;
    private float trueNOETS = 2.0f;
    private int thresholdForSpawning = 0; //The number of enemies left before we spawn the next wave
    private int maxNumberOfEnemies = 10; //The max number of enemies ever

    private int CalculateNumberOfEnemies()
    {
        return enemyHolder.childCount;
    }

    private float timeBuffer = 3.0f;

    private void Update()
    {
        if(currentGameState == GameState.MainMenu)
        {
            if (Input.GetMouseButtonDown(0))
            {
                currentGameState = GameState.InLevel;
                SceneManager.LoadScene(1);
            }
        }
        else if(currentGameState == GameState.InLevel)
        {
            if(timeBuffer > 0)
            {
                timeBuffer -= Time.deltaTime;
                return;
            }

            if(enemyHolder == null)
            {
                FindEnemyHolder();
            }

            if(CalculateNumberOfEnemies() <= thresholdForSpawning)
            {
                int i = 0;
                while(i < numberOfEnemiesToSpawn)
                {
                    // SPAWN NEW ENEMY
                    if (SpawnEnemy())
                    {
                        i++;
                        if (CalculateNumberOfEnemies() >= maxNumberOfEnemies)
                        {
                            break;
                        }
                    }
                    // SPAWN NEW ENEMY
                }

                //Update threshold for spawning and number of enemies to spawn
                if(numberOfEnemiesToSpawn <= maxNumberOfEnemies)
                {
                    trueNOETS += 0.5f;
                    numberOfEnemiesToSpawn = Mathf.FloorToInt(trueNOETS);
                    thresholdForSpawning = Mathf.FloorToInt(numberOfEnemiesToSpawn / 2.0f);
                }
            }
        }
    }

    private bool SpawnEnemy()
    {
        //Pick new enemy
        EnemyTypes enemyType = EnemyTypes.Gremlin;
        float calculatedChance = Random.Range(0.0f, 1.0f);
        float builtUpChance = 0.0f;
        for(int j = 0; j < enemySpawnChances.Length; j++)
        {
            float currentEnemySpawnChance = enemySpawnChances[j];
            if(currentEnemySpawnChance + builtUpChance >= calculatedChance)
            {
                enemyType = (EnemyTypes)j;
                break;
            }

            builtUpChance += currentEnemySpawnChance;
        }

        //Actually spawn in enemy
        //Find appropritate location
        List<BoundingBox2D> boundingBoxes = new List<BoundingBox2D>();
        if (enemyType == EnemyTypes.Gremlin)
        {
            boundingBoxes = EnemyManagment._instance.gremlinBounds;
        }
        else if(enemyType == EnemyTypes.Bat)
        {
            boundingBoxes = EnemyManagment._instance.batBounds;
        }
        else if(enemyType == EnemyTypes.Worm)
        {
            boundingBoxes = EnemyManagment._instance.wormBounds;
        }
        int infinteLoopCatch = 0;
        while(infinteLoopCatch < 100)
        {
            //Pick a random bounding box
            BoundingBox2D currentBoundingBox = boundingBoxes[Random.Range(0, boundingBoxes.Count)];
            //Find a random position in that bounding box
            Vector3 chosenPosition = new Vector3
                (
                Random.Range(currentBoundingBox.center.x - currentBoundingBox.extents.x, currentBoundingBox.center.x + currentBoundingBox.extents.x),
                Random.Range(currentBoundingBox.center.y - currentBoundingBox.extents.y, currentBoundingBox.center.y + currentBoundingBox.extents.y)
                );
            //If position is valid then spawn enemy and return true
            if((chosenPosition - PlayerMovement._instance.transform.position).sqrMagnitude > 2.0f)
            {
                ((GameObject)Instantiate(Resources.Load(enemyType.ToString()), chosenPosition, Quaternion.identity)).transform.parent = enemyHolder;
                return true;
            }
            infinteLoopCatch++;
        }

        return false;
    }

    private void FindEnemyHolder()
    {
        enemyHolder = GameObject.Find("EnemyHolder").transform;
    }
}
