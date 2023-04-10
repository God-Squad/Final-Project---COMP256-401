using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;

    public float jumpHeight = 5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Debug.Log("Game Over");
            Destroy(gameObject);
        }
    }
}
