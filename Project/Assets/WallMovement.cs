using UnityEngine;

public class WallMovement : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);

        if (transform.position.z < -5.5f)
        {
            Destroy(gameObject);
        }
    }
}
