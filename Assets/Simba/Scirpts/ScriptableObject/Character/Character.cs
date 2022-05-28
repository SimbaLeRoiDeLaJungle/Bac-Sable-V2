using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace Simba{
    [CreateAssetMenu(fileName = "character_name", menuName = "ScriptableObjects/Character", order = 1)]
    public class Character : ScriptableObject
    {
        [Space]
        [Header("GFX")]
        [SerializeField] RuntimeAnimatorController  animatorController;
        [Space]
        [Header("Actions")]
        [SerializeField] ActionPad actionPad;
        [Space]
        [Header("Statistics")]
        [SerializeField] float _speed;
        public float speed { get { return _speed; } }
        [SerializeField] float _jumpSpeed;
        public float jumpSpeed { get { return _jumpSpeed; } }    

        public RuntimeAnimatorController GetAnimatorController(){
            return animatorController;
        }

        public Attack GetAction(Action action){
            if(action == Action.First){
                return actionPad.first;
            }
            else{
                return actionPad.second;
            }
        }
    }
}