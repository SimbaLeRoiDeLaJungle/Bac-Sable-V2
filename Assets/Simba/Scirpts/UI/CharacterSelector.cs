using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CharacterSystem;
using StaticManager;
namespace UI{
    public class CharacterSelector : MonoBehaviour
    {
        [SerializeField] GameObject buttonPrefab;

        
        void Start(){
            Init();
        }
        
        public void Init(){
            List<Character> characters = GameController.GetCharacters();
            foreach (var character in characters)
            {
                GameObject go = Instantiate(buttonPrefab, transform);        
                var script = go.GetComponent<CharacterSelectorButton>();
                script.SetCharacter(character);
            }
        }
    }
}

