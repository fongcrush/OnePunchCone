using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    private float x;

    private void Start()
    {
        x = transform.position.x;
        Debug.Log(x);
    }
    private void Update()
    {
        if (Mathf.Abs(x - transform.position.x) >= 4.0f)
        {
            Debug.Log(transform.position.x);
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
