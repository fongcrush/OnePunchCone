using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jangseung : IEnvironment
{
    public override void Stay(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

        }
        if (collision.gameObject.tag == "Enemy")
        {

        }
    }

    public override void Exit(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

        }
        if (collision.gameObject.tag == "Enemy")
        {

        }
    }
}
