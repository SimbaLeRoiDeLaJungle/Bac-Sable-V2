using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace StaticManager{
    /// <summary>
    /// Stock les inputs.
    /// </summary>
    /// Utilités : <br/>
    /// (i) Pour savoir quelle touche est pressé, relaché.
    /// (ii) Pour faire l'interface entre les commandes dans settings et les inputs du joueur
    public class InputHandler : MonoBehaviour
    {
        static Dictionary<KeyCode, bool[]> inputs; /*!< stock les inputs des deux dernière fixedUpdate. La première(indice 0) est la plus ancienne */
        
        static Dictionary<GameAction, KeyCode> personalKey; /*!< Stock les controles choisi par l'utilisateur */
        
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        /// <summary>
        /// MonoBeahviours Abstract Method
        /// </summary>
        void Start()
        {
            inputs = new Dictionary<KeyCode, bool[]>();
            bool[] b = new bool[2];
            b[0] = false;
            b[1] = false;
            inputs.Add(KeyCode.Space, b);
            inputs.Add(KeyCode.RightControl, b);
            //

            personalKey = new Dictionary<GameAction, KeyCode> (){
                {GameAction.First, KeyCode.Space},
                {GameAction.Second, KeyCode.RightControl}
            };
        }

        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        /// <summary>
        /// MonoBeahviours Abstract Method
        /// </summary>
        void FixedUpdate()
        {
            bool[] b = new bool[2];
            KeyCode key1 = personalKey[GameAction.First];
            b[0] = inputs[key1][1];
            b[1] = Input.GetKey(key1);
            inputs[key1] = b;

            KeyCode key2 = personalKey[GameAction.Second];
            b = new bool[2];
            b[0] = inputs[key2][1];
            b[1] = Input.GetKey(key2);
            inputs[key2] = b;
        }

        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        /// <summary>
        /// Retourne vrai si la key qui correspond a action est actuellement presser.
        /// </summary>
        public static bool KeyDown(GameAction action){
            KeyCode keycode = personalKey[action];
            return inputs[keycode][1];
        }

        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        /// <summary>
        /// Retourne vrai si la key qui correspond a action vien d'etre relacher.
        /// </summary>
        public static bool KeyRelease(GameAction action){
            KeyCode keycode = personalKey[action];
            return inputs[keycode][0] && !inputs[keycode][1];
        }

        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        /// <summary>
        /// Retourne vrai si la key qui correspond a action vien juste d'etre presser.
        /// </summary>
        public static bool KeyPressed(GameAction action){
            KeyCode keycode = personalKey[action];
            return !inputs[keycode][0] && inputs[keycode][1];
        } 

        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        /// <summary>
        /// Retourne un Vecteur2 = (x,y) avec -1<= x, y <= 1  qui correspond aux inputs horizontaux et latéraux.
        /// </summary>
        public static Vector2 GetAxesInput(){
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            return new Vector2(x,y);
        }
        
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        /// <summary>
        /// 
        /// </summary>
        public static InputData GetPrioritizeInput(){
            // Action 1
            GameAction action = GameAction.First;
            bool isPressed = KeyPressed(action);
            if(isPressed){
                return new InputData(InputType.Press, action); 
            }
            bool isDown = KeyDown(action);
            if(isDown){
                return new InputData(InputType.Down, action); 
            }
            bool isRelease = KeyRelease(action);
            if(isRelease){
                return new InputData(InputType.Release, action);
            }

            // Action 2
            action = GameAction.Second;
            isPressed = KeyPressed(action);
            if(isPressed){
                return new InputData(InputType.Press, action); 
            }
            isDown = KeyDown(action);
            if(isDown){
                return new InputData(InputType.Down, action); 
            }
            isRelease = KeyRelease(action);
            if(isRelease){
                return new InputData(InputType.Release, action);
            }

            return new InputData(InputType.None, action);
        }

    }

    public struct InputData{
        public InputType inputType{get;set;}
        public GameAction action{get;set;}

        public InputData(InputType p_type, GameAction p_action){
            inputType = p_type;
            action = p_action;
        }
    }

    public enum InputType{
        Release,
        Press,
        Down,
        None
    }
}