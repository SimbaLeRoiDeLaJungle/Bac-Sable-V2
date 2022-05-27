using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float reaction;
    Rigidbody2D rb;
    bool isGroogy;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeHit(float power, bool direction){
        Vector3 dir = direction ? Vector3.right : Vector3.left ;
        Vector3 dF = power*reaction*(dir+Vector3.up);
        rb.AddForce(dF, ForceMode2D.Impulse);
    }
}
