using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//mes imports(interne) 
using AttackSystem;
using Utilitary;
using StaticManager;
namespace CharacterSystem{
    /// <summary>
    /// Permet de controller le joueur.
    /// </summary>
    /// Sa loop est a l'interieur de GameController.
    public class PlayerController : MonoBehaviour
    {
        //ScriptableObjects
        [SerializeField] Character character;
        
        // Components
        [SerializeField] GfxUpdater gfx;
        Rigidbody2D rb;
        GroundChecker groundChecker;
        AttackHitBox attackHitBox;

        // variables
        CharacterState state; 
        Timer actionTimer; /*!< Permet de regarder la charge des attaques */

        //proprety
        public bool direction{get; private set; }/*!< vrai <=> le joeur regarde a droite et faux <=> le joueur regarde à gauche */

        
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        /// <summary>
        /// MonoBeahviours Abstract Method
        /// </summary>
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            if(rb == null){
                Debug.Log("error : RigidBody2D missing.");
            }
            groundChecker = GetComponent<GroundChecker>();
            if(groundChecker == null){
                Debug.Log("error : GroundChecker missing.");
            }
            attackHitBox = GetComponent<AttackHitBox>();
            actionTimer = new Timer(2,TimerMode.LOCK);
            gfx.SetCharacter(character);
        }

        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        /// <summary>
        /// Update pricipale, jouer à l'interieur de GameController
        /// </summary>
        public void CustomUpdate(){
        
            //récupère les inputs
            Vector2 axesInput = InputHandler.GetAxesInput();
            bool spacePressed = InputHandler.KeyPressed(GameAction.First); // charge l'attaque
            bool spaceRelease = InputHandler.KeyRelease(GameAction.First); // lance l'attaque
            bool spaceDown = InputHandler.KeyDown(GameAction.First);
            
            // récupère des information

            bool isGrounded = groundChecker.CheckGroundContact();

            
            if(spacePressed || spaceRelease || spaceDown){
                bool attackLaunch = false;
                Attack attack = character.GetAction(GameAction.First);
                bool attackCanCharge = attack.Settings.canCharge;
                if(!attackCanCharge){
                    if(!state.isAttacking && spacePressed){
                        state.isAttacking = true;
                        attackLaunch = true;
                        rb.velocity = rb.velocity.y * Vector3.up;
                    }
                }
                else{
                    if(spacePressed){
                        // On commence a charger l'attaque
                        state.isCharging = true;
                        actionTimer = new Timer(attack.Settings.maxChargeTime, TimerMode.LOCK);
                        rb.velocity = rb.velocity.y * Vector3.up;
                    }
                    else if(spaceDown){
                        // L'attaque est entrain de charger
                        bool maxCharge = actionTimer.Update(Time.fixedDeltaTime);
                        rb.velocity = rb.velocity.y * Vector3.up;
                    }
                    else{
                        // On lance l'attaque (space release)
                        state.isAttacking = true;
                        state.isCharging = false;
                        attackLaunch = true;
                        rb.velocity = rb.velocity.y * Vector3.up;
                    }
                }
                if(attackLaunch){
                    StartCoroutine(LaunchAttack(GameAction.First, actionTimer.time));
                }
            }
            else{
                if(state.isAttacking || state.isCharging){
                    // On est dans une frame ou le joueur est entrain de faire l'attaque ou de charger (On peut peut etre enlever charger ici)
                    rb.velocity = rb.velocity.y * Vector3.up;
                }
                else{
                    MooveControl(axesInput, isGrounded);
                }
            }
            gfx.UpdateGfx(isGrounded, rb.velocity.x, state, actionTimer);
        }

        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        /// <summary>
        /// Gére les mouvements + saut en fonction des inputs.
        /// </summary>
        public void MooveControl(Vector2 axesInput, bool isGrounded){
            float dt = Time.fixedDeltaTime; // temps entre 2 fixedUpdate
            //mvt latéraux type : vitesse constante 
            float latVelocity = axesInput.x * character.speed;
            rb.velocity = latVelocity * Vector3.right + rb.velocity.y * Vector3.up;
            //set la variable direction
            if(axesInput.x > 0){
                direction = true;
            }
            else if(axesInput.x < 0){
               direction = false;
            }
            //saut type : Réaliste avec Impulsion
            if(axesInput.y > 0 && isGrounded){
                Vector3 dF = Vector3.zero;
                dF = Vector3.up * character.jumpSpeed * dt;
                rb.AddForce(dF, ForceMode2D.Impulse);
            }
        }

        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        /// <summary>
        /// Permet de changer le Character (change aussi le GfxUpdater)
        /// </summary>
        public void SetCharacter(Character character){
            this.character = character;
            gfx.SetCharacter(character);
        }

        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        /// <summary>
        /// Pour lancer une attaque en syncro avec le GfxUpdater
        /// </summary>
        IEnumerator LaunchAttack(GameAction action, float chargeTime = 1f){
            state.isAttacking = true;
            Attack attack = character.GetAction(action);
            gfx.LaunchAction(action);
            yield return gfx.Ready();
            while (gfx.IsInAttackAnimation())
            {
                yield return null;
            } 
            if(attack.Settings.attackType == AttackType.Distance && chargeTime >= attack.Settings.minChargeTime){
                Vector3 dir = direction? Vector3.right : Vector3.left;
                Vector3 relPos = attack.Settings.bulletRelativePosition;
                Vector3 position = transform.position + dir*relPos.x+Vector3.up*relPos.y;
                var go = Instantiate(attack.prefabBullet, position, Quaternion.identity);
                var bullet = go.GetComponent<BulletScript>();
                float power = (1 + chargeTime/attack.Settings.maxChargeTime) * 0.5f * attack.Settings.power;
                bullet.Launch(direction, 3f, power);
            }
            else if(attack.Settings.attackType == AttackType.Contact){
                var hit = attackHitBox.TouchEnemy(direction, attack); 
                // On regarde si l'épée touche quelque chose
                if(hit.collider != null){
                    EnemyController ec = hit.collider.gameObject.GetComponent<EnemyController>();// On récupère un script de l'enemi pour qu'il prennent les dégats
                    float power = (1 + chargeTime/attack.Settings.maxChargeTime) * 0.5f * attack.Settings.power; // Calcul a la louche pour que plus on charge plus l'enemi part loin
                    ec.TakeHit(power, direction);
                }
            }
            yield return new WaitForSeconds(0.4f);
            state.isAttacking = false;
            yield return null;
        }

        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
    } 

}


