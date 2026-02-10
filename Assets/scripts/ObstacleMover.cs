using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstacleMover : MonoBehaviour
{
    public float speed = 5f;
    public BirdController bird;      // reference naar de speler
    public float destroyDistance = 10f;

    void Update()
    {
        if (bird != null && bird.gameStarted)
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);

            // vernietig als obstakel voorbij de speler is
            if (transform.position.z < bird.transform.position.z - destroyDistance)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Game over!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
