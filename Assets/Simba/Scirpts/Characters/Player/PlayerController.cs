using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Simba{
    
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] Character character;
        Rigidbody2D rb;
        GroundChecker groundChecker;
        CharacterState state;
        [SerializeField] GfxUpdater gfx;
        Timer actionTimer;
        bool direction;
        AttackHitBox attackHitBox;
        public bool Direction{ get { return direction; } }

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

/*         public void CustomUpdate() {
            float dt = Time.fixedDeltaTime; // temps entre 2 fixedUpdate

            //récupère les inputs
            Vector2 axesInput = InputHandler.GetAxesInput();
            bool spacePressed = InputHandler.KeyPressed(Action.First); // charge l'attaque
            bool spaceRelease = InputHandler.KeyRelease(Action.First); // lance l'attaque
            bool spaceDown = InputHandler.KeyDown(Action.First);
            
            // récupère des information
            bool attackCanCharge = character.GetAction(Action.First).Settings.canCharge;
            bool isGrounded = groundChecker.CheckGroundContact();

            bool attackLaunch = false;
            if(spacePressed){
                // On commence a charger l'attaque
                isCharging = true;
                chargeTimer.Reset();
                rb.velocity = rb.velocity.y * Vector3.up;
            }
            else if(spaceDown){
                // L'attaque est entrain de charger
                bool maxCharge = chargeTimer.Update(Time.fixedDeltaTime);
                rb.velocity = rb.velocity.y * Vector3.up;
            }
            else if(spaceRelease){
                // On lance l'attaque
                isAttacking = true;
                isCharging = false;
                attackLaunch = true;
                rb.velocity = rb.velocity.y * Vector3.up;
            }
            else{
                // le joueur n'est pas entrain de faire des inputs liée au combats
                if(isAttacking || isCharging){
                    // On est dans une frame ou le joueur est entrain de faire l'attaque ou de charger (On peut peut etre enlever charger ici)
                    rb.velocity = rb.velocity.y * Vector3.up;
                    if(isAttacking){
                        var hit = attackHitBox.TouchEnemy(direction); 
                        // On regarde si l'épée touche quelque chose
                        if(hit.collider != null){
                            EnemyController ec = hit.collider.gameObject.GetComponent<EnemyController>();// On récupère un script de l'enemi pour qu'il prennent les dégats
                            float power = (1 + chargeTimer.Time/chargeTimer.MaxTime)*0.5f; // Calcul a la louche pour que plus on charge plus l'enemi part loin
                            ec.TakeHit(power, direction);
                        }
                    }
                }
                else{
                    //finalement si rien de tous ce qui précède est vrai on peu bouger le joueur

                    //mvt latéraux type : vitesse constante 
                    float latVelocity = axesInput.x * speed;
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
                        dF = Vector3.up * jumpSpeed * dt;
                        rb.AddForce(dF, ForceMode2D.Impulse);
                    }
                }
            }
            // On met à jour le sprite du personnage en fonction des inputs
            gfx.UpdateGfx(isGrounded, rb.velocity.x , attackLaunch,isCharging,chargeTimer, ref isAttacking);
            // je passe isAttacking en ref pour le changer si l'annimation est fini, il y a probablement mieu
        } */



        public void CustomUpdate2(){
        
            //récupère les inputs
            Vector2 axesInput = InputHandler.GetAxesInput();
            bool spacePressed = InputHandler.KeyPressed(Action.First); // charge l'attaque
            bool spaceRelease = InputHandler.KeyRelease(Action.First); // lance l'attaque
            bool spaceDown = InputHandler.KeyDown(Action.First);
            
            // récupère des information

            bool isGrounded = groundChecker.CheckGroundContact();

            
            if(spacePressed || spaceRelease || spaceDown){
                bool attackLaunch = false;
                Attack attack = character.GetAction(Action.First);
                bool attackCanCharge = attack.Settings.canCharge;
                if(!attackCanCharge){
                    if(!state.isAttacking){
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
                    Debug.Log("enter");
                    StartCoroutine(LaunchAttack(Action.First));
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

        public void SetCharacter(Character character){
            this.character = character;
            gfx.SetCharacter(character);
        }
    
        IEnumerator LaunchAttack(Action action){
            state.isAttacking = true;
            Attack attack = character.GetAction(action);
            gfx.LaunchAction(action);
            yield return gfx.Ready();
            while (gfx.IsInAttackAnimation())
            {
                yield return null;
            } 
            if(attack.Settings.attackType == AttackType.Distance){
                Vector3 dir = direction? Vector3.right : Vector3.left;
                Vector3 relPos = attack.Settings.bulletRelativePosition;
                Vector3 position = transform.position + dir*relPos.x+Vector3.up*relPos.y;
                var go = Instantiate(attack.prefabBullet, position, Quaternion.identity);
                var bullet = go.GetComponent<BulletScript>();
                bullet.Launch(direction, 3f);
            }
            else if(attack.Settings.attackType == AttackType.Contact){
                var hit = attackHitBox.TouchEnemy(direction); 
                // On regarde si l'épée touche quelque chose
                if(hit.collider != null){
                    EnemyController ec = hit.collider.gameObject.GetComponent<EnemyController>();// On récupère un script de l'enemi pour qu'il prennent les dégats
                    float power = (1 + actionTimer.Time/actionTimer.MaxTime) * 0.5f * attack.Settings.power; // Calcul a la louche pour que plus on charge plus l'enemi part loin
                    ec.TakeHit(power, direction);
                }
            }
            yield return new WaitForSeconds(0.4f);
            state.isAttacking = false;
            yield return null;
        }
    
    }

    public struct CharacterState{
        public bool isAttacking;
        public bool isCharging;
        public Action action;
    }
}


