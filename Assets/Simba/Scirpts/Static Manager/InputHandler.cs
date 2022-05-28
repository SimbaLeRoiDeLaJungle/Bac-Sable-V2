using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Simba{
    // Va stocker les Inputs Si le joeuur veut changer les touches c'est avec cettes classe que l'on procedera
    public class InputHandler : MonoBehaviour
    {
        static Dictionary<KeyCode, bool[]> inputs;
        static ActionPad actionPad;
        
        static Dictionary<Action, KeyCode> personalKey;

        void Start()
        {
            inputs = new Dictionary<KeyCode, bool[]>();
            bool[] b = new bool[2];
            b[0] = false;
            b[1] = false;
            inputs.Add(KeyCode.Space, b);

            //

            personalKey = new Dictionary<Action, KeyCode> (){
                {Action.First, KeyCode.Space},
                {Action.Second, KeyCode.RightControl}
            };
        }

        void FixedUpdate()
        {
            bool[] b = new bool[2];
            b[0] = inputs[KeyCode.Space][1];
            b[1] = Input.GetKey(KeyCode.Space);
            inputs[KeyCode.Space] = b;
        }

        public static bool KeyDown(Action action){
            KeyCode keycode = personalKey[action];
            return inputs[keycode][1];
        }

        public static bool KeyRelease(Action action){
            KeyCode keycode = personalKey[action];
            return inputs[keycode][0] && !inputs[keycode][1];
        }

        public static bool KeyPressed(Action action){
            KeyCode keycode = personalKey[action];
            return !inputs[keycode][0] && inputs[keycode][1];
        } 

        public static Vector2 GetAxesInput(){
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            return new Vector2(x,y);
        }
    }

    public enum Action{
        First,
        Second
    }
}