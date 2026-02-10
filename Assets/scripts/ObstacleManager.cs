using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [Header("Obstacle Settings")]
    public GameObject[] obstacleVariants;

    public float spawnZ = 50f;
    public float obstacleSpacing = 40f;   // vaste afstand tussen obstakels

    [Header("Colors")]
    public Color color1 = Color.green;
    public Color color2 = Color.yellow;
    public Color color3 = Color.red;
    public Color color4 = Color.blue;
    private int colorIndex = 0;

    [Header("Movement")]
    public float obstacleSpeed = 5f;

    [Header("Player")]
    public BirdController bird;

    private GameObject lastSpawned;

    void Start()
    {
        // Pre-spawn een aantal obstakels
        int preSpawnCount = 6;

        for (int i = 0; i < preSpawnCount; i++)
        {
            lastSpawned = SpawnObstacle(i * obstacleSpacing);
        }
    }

    void Update()
    {
        if (!bird.gameStarted)
            return;

        // Check of we een nieuw obstakel moeten spawnen
        if (lastSpawned != null)
        {
            float distanceMoved = spawnZ - lastSpawned.transform.position.z;

            if (distanceMoved >= obstacleSpacing)
            {
                lastSpawned = SpawnObstacle(0);
            }
        }
    }

    GameObject SpawnObstacle(float zOffset)
    {
        int index = Random.Range(0, obstacleVariants.Length);

        float yPos = obstacleVariants[index].transform.position.y;

        Vector3 spawnPos = new Vector3(0, yPos, spawnZ + zOffset);

        GameObject obstacle = Instantiate(
            obstacleVariants[index],
            spawnPos,
            Quaternion.identity,
            transform
        );

        ObstacleMover mover = obstacle.AddComponent<ObstacleMover>();
        mover.speed = obstacleSpeed;
        mover.bird = bird;
        mover.destroyDistance = 5f;

        // Kleuren
        Color chosenColor;
        switch (colorIndex)
        {
            case 0: chosenColor = color1; break;
            case 1: chosenColor = color2; break;
            case 2: chosenColor = color3; break;
            default: chosenColor = color4; break;
        }

        foreach (Renderer r in obstacle.GetComponentsInChildren<Renderer>())
        {
            r.material.color = chosenColor;
        }

        colorIndex = (colorIndex + 1) % 4;

        return obstacle;
    }
}
