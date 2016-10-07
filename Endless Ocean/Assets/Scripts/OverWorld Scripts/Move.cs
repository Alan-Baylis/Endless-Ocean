using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Move : MonoBehaviour
{
    public Vector3 teleportPoint;
    public Rigidbody rb;
    float speed = 20f;
    float bSpeed = 10f;
    float boostSpeed = 40f;
    // float maxSpeed = 20f;
    // float accel = .2f;
    float rotationspeed = 50;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }



     void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
        }
        // Backward movement
        if (Input.GetKey(KeyCode.S))
        {
            //rb.MovePosition(transform.position / speed * Time.deltaTime);
            //rb.AddRelativeForce(transform.forward * -6);
            rb.MovePosition(transform.localPosition -= transform.forward * bSpeed * Time.deltaTime);
        }
            // Left movement
            if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.down * rotationspeed * Time.deltaTime);
        }
        // Right movement
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * rotationspeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            rb.MovePosition(transform.position + transform.forward * boostSpeed * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.B))
        {
            Application.LoadLevel("Ship Scene");
        }
    }
}
