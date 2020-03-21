using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yeeter;

public class Player : MonoBehaviour
{
    public Vector3 Velocity = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Velocity.y += Gravity.Strength;
        Velocity.x += Gravity.HorizontalStrength;
        transform.position += Velocity * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider.CompareTag("Platform"))
        {
            InGameDebug.Log("Good job.");
        }
    }
}
