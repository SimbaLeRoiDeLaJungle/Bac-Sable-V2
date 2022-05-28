using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Simba{
    public class GameController : MonoBehaviour
    {
        static GameController instance;
        [SerializeField] PlayerController playerController;
        [SerializeField] List<Character> characters;
        public List<Character> Characters { get { return characters; } }
        public PlayerController PlayerController { get { return playerController; } }
        int currentCharacter = 0;
        void Start()
        {
            if(instance == null){
                instance = this;
            }
        }

        void FixedUpdate()
        {
            playerController.CustomUpdate2();
        }


        public void SwitchCharacter(){
            currentCharacter = (currentCharacter == 0)? 1 : 0;
            Character character = characters[currentCharacter];
            playerController.SetCharacter(character);
        }
    }
}