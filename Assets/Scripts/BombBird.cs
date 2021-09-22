using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBird : Bird
{
    public float radiusBomb;
    public float force;
    public GameObject ExplodeEffect;

    public LayerMask LayerToHit;

    public override void OnTap()
    {
        StartCoroutine(Explode(0f));
        //Explode();
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Obstacle")
            StartCoroutine(Explode(0.5f));
        //Explode();
    }

    private IEnumerator Explode(float second)
    {
        yield return new WaitForSeconds(second);
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, radiusBomb, LayerToHit);

        foreach (Collider2D obj in objects)
        {
            Vector2 direction = obj.transform.position - transform.position;

            obj.GetComponent<Rigidbody2D>().AddForce(direction * force);
        }

        GameObject ExplodeEffectIns = Instantiate(ExplodeEffect, transform.position, Quaternion.identity);
        Destroy(ExplodeEffectIns, 1f);
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusBomb);
    }
}
