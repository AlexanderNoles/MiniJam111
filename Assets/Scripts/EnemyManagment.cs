using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagment : MonoBehaviour
{
    public static EnemyManagment _instance;

    [Header("Gremlin")]
    public List<BoundingBox2D> gremlinBounds = new List<BoundingBox2D>();
    public static List<Vector3> gremlinPositions = null;
    public static List<Vector3> getGremlinPositions()
    {
        if(gremlinPositions == null)
        {
            gremlinPositions = enemyPostions("Gremlin");

            return gremlinPositions;
        }

        return gremlinPositions;
    }

    [Header("Bat")]
    public List<BoundingBox2D> batBounds = new List<BoundingBox2D>();
    public static List<Vector3> batPositions = null;
    public static List<Vector3> getBatPositions()
    {
        if (batPositions == null)
        {
            batPositions = enemyPostions("Bat");

            return batPositions;
        }

        return batPositions;
    }

    [Header("Worm")]
    public List<BoundingBox2D> wormBounds = new List<BoundingBox2D>();


    private int numberOfEnemiesKilled = 0;

    public static void EnemyKilled()
    {
        _instance.numberOfEnemiesKilled += 1;
    }

    public static int GetNumberOfEnemyKilled()
    {
        return _instance.numberOfEnemiesKilled;
    }

    private void Start()
    {
        _instance = this;
    }

    public static Vector3 playerPosition = Vector3.zero;


#if UNITY_EDITOR
    private void Update()
    {
        foreach (BoundingBox2D box in gremlinBounds)
        {
            box.Draw();
        }

        foreach (BoundingBox2D box1 in batBounds)
        {
            box1.Draw();
        }

        foreach (BoundingBox2D box2 in wormBounds)
        {
            box2.Draw();
        }
    }
#endif

    private void LateUpdate()
    {
        if (PlayerManagment.levelLost) return;
        playerPosition = PlayerMovement._instance.transform.position;
        gremlinPositions = null;
    }

    private static List<Vector3> enemyPostions(string name)
    {
        List<Vector3> toReturn = new List<Vector3>();

        foreach(Transform child in _instance.transform)
        {
            if (child.name.Contains(name))
            {
                toReturn.Add(child.position);
            }
        }

        return toReturn;
    }
}
