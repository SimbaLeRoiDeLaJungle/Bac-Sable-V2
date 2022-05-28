using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AttackSystem;
using Utilitary;
namespace CharacterSystem{
    /// <summary>
    /// Gère les graphismes des characters
    /// </summary>
    /// Pour comprendre cette classe il faut aller faire un tour ici : Tu selectionne le gameObject "Player/Gfx",
    /// et tu va dans widow/animation ici tu peux looker animator et animation pour voir comment c'est construit
    public class GfxUpdater : MonoBehaviour
    {
        Animator animator;
        Character character;
        bool _launchAttack; /*!< Set dans LaunchAction, un-set dans UpdateGfx quand l'attaque est lancé */
        GameAction _currentAction; /*!< Si une action est demender */
        bool _ready = true; /*!<utiliser pour checker quand l'animation de l'attaque est finie */


        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        /// <summary>
        /// MonoBeahviours Abstract Method
        /// </summary>
        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        /// <summary>
        /// Met a jour les Graphisme jouer dans PlayerController.CustomUpdate()
        /// </summary>
        public void UpdateGfx(bool isGrounded, float xVel, CharacterState state, Timer chargeTimer){

            animator.SetBool("charge", state.isCharging);

            bool walk = isGrounded && xVel != 0 ; // la vitesse sur l'axe x n'est pas 0 et on est au sol => le joueur marche
            animator.SetBool("walk", walk);
            if(xVel != 0){
                bool direction = xVel > 0 ; // si la vitesse est positive il marche vers la droite : true <=> right, false <=> left
                animator.SetBool("direction", direction);
            }
            if(_launchAttack){
                _launchAttack = false;
                _ready = true;
                Attack attack = character.GetAction(_currentAction);
                animator.SetTrigger("attack");
                Vector2 position = Vector2.zero;
                bool direction = animator.GetBool("direction");
                if(direction){
                    position = attack.effectRelativePosition + (Vector2)transform.position;
                }
                else{
                    position = new Vector2(-attack.effectRelativePosition.x,attack.effectRelativePosition.y) + (Vector2)transform.position;
                }
                bool cond1 = attack.Settings.canCharge && chargeTimer.Time >= attack.Settings.minChargeTime;
                bool cond2 = !attack.Settings.canCharge;
                bool cond3 = attack.prefabEffect != null;
                if( (cond1 || cond2) && cond3){
                    GameObject go = Instantiate(attack.prefabEffect, (Vector3)position, Quaternion.identity);
                    var script = go.GetComponent<AttackEffectScript>();
                    if(direction){
                        Vector3 scale = go.transform.localScale;
                        go.transform.localScale = new Vector3(-scale.x, scale.y, 1);
                    }
                    if(script != null){
                        StartCoroutine(script.Play());
                    }
                }
            }
        }
        
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        /// <summary>
        /// Met a jour le character et l'animator
        /// </summary>
        public void SetCharacter(Character character){
            this.character = character;
            animator.runtimeAnimatorController = character.GetAnimatorController();
        }

        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        /// <summary>
        /// Pour passer GfxUpdater en mode lancement d'action
        /// </summary>
        public void LaunchAction(GameAction action){
            _currentAction = action;
            _launchAttack = true;
            _ready = false;
        }

        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        /// <summary>
        /// Revoi vrai tant que le character est entrain d'effectuer l'attaque.
        /// ATTACKTYPE : <br>
        /// (Contact) Les dégats sont appliqué quand cette fonction renvoi vrai.<br>
        /// (Distance) Le projectil est envoyer quand cette fonction renvoi vrai.  
        /// </summary>
        public bool IsInAttackAnimation(){
            var info = animator.GetCurrentAnimatorClipInfo(1); // le 1 c'est pour le 2eme claques
            // si sa longeur est 0 c'est que l'on est dans le "state' de nom "new state" qui sert a faire les transition entre marche / attaques (voir animator du player)
            if(info.Length == 0){
                return false;
            }
            else{
                string clipName = info[0].clip.name;
                Attack attack = character.GetAction(_currentAction);
                if(attack.Settings.canCharge){
                    return clipName.Contains("End") && animator.GetCurrentAnimatorStateInfo(1).normalizedTime < attack.Settings.timeOfHit;
                }
                return !clipName.Contains("End");

                // J'ai découper les animations d'attaque en deux SaberLeft et SaberLeftEnd, pareil pour Right donc ici je vérifie si on est entrain 
                // de charger (ne contient pas End dans le nom) où entrain d'attaquer c'est un peu a la rache cette partie ^^. 
            }
        }

        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        /// <summary>
        /// Pour sychronysé avec le PlayerController.
        /// </summary>
        public IEnumerator Ready(){
            int MAX_LOOP = 100;
            int cmp = 0;
            while (!_ready)
            {
                cmp++;
                if(cmp>MAX_LOOP){
                    Debug.Log("ouch!");
                    break;
                }
                yield return null;
            }
            yield return null;
        }
        
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-

    }
}