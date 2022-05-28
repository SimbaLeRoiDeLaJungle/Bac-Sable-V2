using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CharacterSystem;
using StaticManager;

namespace UI {
    public class CharacterSelectorButton : MonoBehaviour
    {
        Button _button;
        [SerializeField] Image characterImage;
        Character _character;
        void Awake(){
            _button = GetComponent<Button>();

        }

        public void SetCharacter(Character character){
            characterImage.sprite = character.sprite;
            _character = character;
            _button.onClick.AddListener(delegate {
                GameController.SwitchCharacter(_character);
            });
        }
    }
}