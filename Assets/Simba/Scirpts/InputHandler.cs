using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Simba{
    // Va stocker les Inputs Si le joeuur veut changer les touches c'est avec cettes classe que l'on procedera
    public class InputHandler : MonoBehaviour
    {
        static Dictionary<KeyCode, bool[]> inputs;
        void Start()
        {
            inputs = new Dictionary<KeyCode, bool[]>();
            bool[] b = new bool[2];
            b[0] = false;
            b[1] = false;
            inputs.Add(KeyCode.Space, b);
        }

        void FixedUpdate()
        {
            bool[] b = new bool[2];
            b[0] = inputs[KeyCode.Space][1];
            b[1] = Input.GetKey(KeyCode.Space);
            inputs[KeyCode.Space] = b;
        }

        public static bool KeyDown(KeyCode keyCode){
            return inputs[keyCode][1];
        }

        public static bool KeyRelease(KeyCode keyCode){
            return inputs[keyCode][0] && !inputs[keyCode][1];
        }

        public static bool KeyPressed(KeyCode keyCode){
            return !inputs[keyCode][0] && inputs[keyCode][1];
        } 

        public static Vector2 GetAxesInput(){
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            return new Vector2(x,y);
        }
    }
}