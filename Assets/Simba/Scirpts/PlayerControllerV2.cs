using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Simba{
    // Un autres système de déplacement peut etre mieu pour du rétro
    public class PlayerControllerV2 : MonoBehaviour
    {
        Rigidbody2D rb;
        [SerializeField] float speed;
        [SerializeField] float jumpSpeed;
        GroundChecker groundChecker;
        bool isAttacking;
        bool isCharging;
        [SerializeField] GfxUpdater gfx;
        Timer chargeTimer;
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
            chargeTimer = new Timer(2,TimerMode.LOCK);
        }

        void FixedUpdate() {
            float dt = Time.fixedDeltaTime; // temps entre 2 fixedUpdate

            //récupère les inputs
            Vector2 axesInput = InputHandler.GetAxesInput();
            bool spacePressed = InputHandler.KeyPressed(KeyCode.Space); // charge l'attaque
            bool spaceRelease = InputHandler.KeyRelease(KeyCode.Space); // lance l'attaque
            bool spaceDown = InputHandler.KeyDown(KeyCode.Space);

            bool isGrounded = groundChecker.CheckGroundContact();

            bool attackLaunch = false;
            if(spacePressed){
                isCharging = true;
                chargeTimer.Reset();
                rb.velocity = rb.velocity.y * Vector3.up;
            }
            else if(spaceDown){
                bool maxCharge = chargeTimer.Update(Time.fixedDeltaTime);
            }
            else if(spaceRelease){
                isAttacking = true;
                isCharging = false;
                attackLaunch = true;
                rb.velocity = rb.velocity.y * Vector3.up;
            }
            else{
                if(isAttacking || isCharging){
                    rb.velocity = rb.velocity.y * Vector3.up;
                    if(isAttacking){
                        var hit = attackHitBox.TouchEnemy(direction);
                        if(hit.collider != null){
                            EnemyController ec = hit.collider.gameObject.GetComponent<EnemyController>();
                            float power = (1 + chargeTimer.Time/chargeTimer.MaxTime)*0.5f;
                            ec.TakeHit(power, direction);
                        }
                    }
                }
                else{

                    //mvt latéraux
                    float latVelocity = axesInput.x * speed;
                    rb.velocity = latVelocity * Vector3.right + rb.velocity.y * Vector3.up;
                    //set la variable direction
                    if(axesInput.x > 0){
                        direction = true;
                    }
                    else if(axesInput.x < 0){
                        direction = false;
                    }
                    //saut 
                    Vector3 dF = Vector3.zero;
                    if(axesInput.y > 0 && isGrounded){
                        dF = Vector3.up * jumpSpeed * dt;
                    }
                    rb.AddForce(dF, ForceMode2D.Impulse);
                }
            } 
            // On met à jour le sprite du personnage en fonction des inputs
            gfx.UpdateGfx(isGrounded, rb.velocity.x , attackLaunch,isCharging,chargeTimer, ref isAttacking);
            // je passe isAttacking en ref pour le changer si l'annimation est fini, il y a probablement mieu
        }
    }
}