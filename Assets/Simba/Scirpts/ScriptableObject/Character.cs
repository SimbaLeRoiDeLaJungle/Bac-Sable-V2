using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "character_name", menuName = "ScriptableObjects/Character", order = 1)]
public class Character : ScriptableObject
{
    [SerializeField] RuntimeAnimatorController  animatorController;
    [SerializeField] GameObject prefabAttackEffect;

    public RuntimeAnimatorController GetAnimatorController(){
        return animatorController;
    }

    public GameObject GetPrefabAttackEffect(){
        return prefabAttackEffect;
    }
}
