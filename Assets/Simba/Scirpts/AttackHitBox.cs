using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    [SerializeField] float rayLength;
    float playerXSize;
    [SerializeField] LayerMask enemiesLayers;
    // Start is called before the first frame update
    void Start()
    {
        Collider2D col = GetComponent<Collider2D>();
        if(col != null){
            playerXSize = col.bounds.extents.x;
        }
    }

    public RaycastHit2D TouchEnemy(bool direction){
        Vector3 playerLeft = transform.position - Vector3.up * playerXSize;
        Vector3 directionToVector = direction? Vector3.right : Vector3.left;
        var hit = Physics2D.Raycast(playerLeft, directionToVector, rayLength, enemiesLayers);
        Debug.DrawLine(playerLeft, playerLeft + directionToVector*rayLength, Color.red);
        return hit;
    }
}
