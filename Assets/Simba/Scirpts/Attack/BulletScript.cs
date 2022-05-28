using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] LayerMask contactLayers;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();    
    }

    public void Launch(bool direction, float speed){
        int x = direction ? 1 : -1;
        transform.localScale = new Vector3(x*transform.localScale.x, transform.localScale.y, transform.localScale.z);
        rb.velocity = new Vector2(x*speed,0);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        int layer = other.gameObject.layer;
        if(((contactLayers.value & (1 << layer)) > 0)){
            Destroy(gameObject,0.1f);
        }
    }
}
