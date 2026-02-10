using UnityEngine;

public class ObstacleScore : MonoBehaviour
{
    private Transform bird;
    private bool scored = false;

    void Start()
    {
        // Find the bird by tag
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
            bird = player.transform;
        else
            Debug.LogError("No object with tag 'Player' found for ObstacleScore!");
    }

    void Update()
    {
        if (bird == null || scored)
            return;

        // When bird passes the obstacle
        if (transform.position.z < bird.position.z)
        {
            scored = true;

            // Find the HUD in the scene
            HUDController hud = FindFirstObjectByType<HUDController>();
            if (hud != null)
                hud.AddScore();
        }
    }
}
