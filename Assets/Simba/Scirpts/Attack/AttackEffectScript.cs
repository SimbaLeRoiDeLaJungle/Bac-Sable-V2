using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffectScript : MonoBehaviour
{
    [SerializeField] ParticleSystem ps;
    SpriteRenderer sr;
    [SerializeField] float duration;
    void Start()
    {
       sr = GetComponent<SpriteRenderer>();
    }

    public IEnumerator Play(){
        yield return new WaitForSeconds(duration);
        sr.color = Color.clear;
        ps.Play();
        while (ps.IsAlive())
        {
            yield return null;
        }
        Destroy(this.gameObject, 0.1f);
        yield return new WaitForSeconds(0.1f);
    } 


}
