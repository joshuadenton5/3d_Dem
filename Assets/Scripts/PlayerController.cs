using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float translation = Input.GetAxisRaw("Vertical");
        float straffe = Input.GetAxisRaw("Horizontal");
        var moveDirection = (straffe * transform.right + translation * transform.forward).normalized;

        Vector3 y = new Vector3(0, rb.velocity.y, 0); //accounting for the velocity in the y axis
        rb.velocity = moveDirection * speed * Time.deltaTime;
        rb.velocity += y;
    }
}
