using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ground : MonoBehaviour
{

    public bool onGround { get; private set;}
    public bool onWall { get; private set; }

    private float friction;

    public Vector2 contactNormal { get; private set; }

    private PhysicsMaterial2D material;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        evaluateCollision(collision);
        //retrieveFriction(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        onGround = false;
        onWall = false;
        friction = 0.0f;
    }

    public void evaluateCollision(Collision2D collision)
    {
        for(int i=0; i < collision.contactCount; i++)
        {
            contactNormal = collision.GetContact(i).normal;
            onGround |= contactNormal.y >= 0.9f;
            onWall |= Mathf.Abs(contactNormal.x) >= 0.9f;
        }
    }

    private void retrieveFriction(Collision2D collision)
    {
        material = collision.rigidbody.sharedMaterial;

        friction = 0;
        if(material != null)
        {
            friction = material.friction;
        }
    }

    public bool getOnGround()
    {
        return onGround;
    }

    public float getFriction()
    {
        return friction;
    }
}
