using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Simba{
    // Pour comprendre cette classe il faut aller faire un tour ici : Tu selectionne le gameObject "Player/Gfx" et tu va dans widow/animation ici tu peux looker animator et animation pour voir comment c'est construit
    public class GfxUpdater : MonoBehaviour
    {
        Animator animator;
        [SerializeField] GameObject attackEffectPrefab;
        [SerializeField] Vector2 effectRelativePosition;
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void UpdateGfx(bool isGrounded, float xVel, bool attack,bool isCharging, Timer chargeTimer, ref bool isAttacking){

            animator.SetBool("charge", isCharging);

            bool walk = isGrounded && xVel != 0 ; // la vitesse sur l'axe x n'est pas 0 et on est au sol => le joueur marche
            animator.SetBool("walk", walk);
            if(xVel != 0){
                bool direction = xVel > 0 ; // si la vitesse est positive il marche vers la droite : true <=> right, false <=> left
                animator.SetBool("direction", direction);
            }
            if(attack){
                animator.SetTrigger("attack");
                Vector2 position = Vector2.zero;
                bool direction = animator.GetBool("direction");
                if(direction){
                    position = effectRelativePosition + (Vector2)transform.position;
                }
                else{
                    position = new Vector2(-effectRelativePosition.x,effectRelativePosition.y) + (Vector2)transform.position;
                }
                if(chargeTimer.Time >= chargeTimer.MaxTime*0.5f){
                    GameObject go = Instantiate(attackEffectPrefab , (Vector3)position, Quaternion.identity);
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

            // on verifie si le calques n°2(ici c'est pas les calques d'affichage graphique, mais les calques de l'animator, voir dans widow/animation/animator) 
            //celui qui contient les attaques est entrain d'etre jouer,ici on cherche a voir si l'animation de l'attaque est terminé
            var info = animator.GetCurrentAnimatorClipInfo(1); // le 1 c'est pour le 2eme claques
            // si sa longeur est 0 c'est que l'on est dans le "state' de nom "new state" qui sert a faire les transition entre marche / attaques (voir animator du player)
            if(info.Length == 0){
                isAttacking =false;
            }
            else{
                string clipName = info[0].clip.name;
                isAttacking = clipName.Contains("End"); 
                // J'ai découper les animations d'attaque en deux SaberLeft et SaberLeftEnd, pareil pour Right donc ici je vérifie si on est entrain 
                // de charger (ne contient pas End dans le nom) où entrain d'attaquer c'est un peu a la rache cette partie ^^. 
            }

        }
    }


}