using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    [SerializeField] float rayLength;
    float playerXSize;
    float playerYSize;
    [SerializeField] LayerMask enemiesLayers;
    // Start is called before the first frame update
    void Start()
    {
        Collider2D col = GetComponent<Collider2D>();
        if(col != null){
            playerXSize = col.bounds.extents.x;
            playerYSize = col.bounds.extents.y;
        }
    }

    public RaycastHit2D TouchEnemy(bool direction){
        Vector3 playerLeft = transform.position - Vector3.up * playerXSize;
        Vector3 directionToVector = direction? Vector3.right : Vector3.left;
        var hit = Physics2D.Raycast(playerLeft, directionToVector, rayLength, enemiesLayers);
        Vector3 playerSemiTopLeft = playerLeft + new Vector3(0,playerYSize,0);
        var hit2 = Physics2D.Raycast(playerSemiTopLeft, directionToVector, rayLength, enemiesLayers);
        Debug.DrawLine(playerLeft, playerLeft + directionToVector*rayLength, Color.red);
        Debug.DrawLine(playerSemiTopLeft, playerSemiTopLeft + directionToVector*rayLength, Color.red);
        if(hit2.collider == null){
            return hit;
        }
        return hit2;
    }
}
