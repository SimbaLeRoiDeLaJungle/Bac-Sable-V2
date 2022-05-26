using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simba
{
    //MonoBehaviour == C'est une classe qui peut s'attacher aux "GameObjects" ici le gameObject est le Player, on dit aussi que PlayerController est un "component" de Player. 
    // Implémente le saut et les mouvement lattéraux avec une physique réaliste de la forme F = -m.g.Vector3.up
    public class PlayerController : MonoBehaviour
    {
        Rigidbody2D rb;
        [SerializeField] float speed;
        [SerializeField] float jumpSpeed;
        [SerializeField] float maxVelocity;
        GroundChecker groundChecker;
        // appeller au moment où l'objet s'initialise (avant le start)
        void Awake(){

        }
        // Start is called before the first frame update 
        void Start()
        {
            rb = GetComponent<Rigidbody2D>(); // méthode MonoBehaviour.GetComponent 
            if(rb == null){
                // il n'y a pas de RigidBody2D attaché a notre gameObject
                Debug.Log("error : RigidBody2D missing.");
            }
            groundChecker = GetComponent<GroundChecker>();
            if(groundChecker == null){
                Debug.Log("error : GroundChecker missing.");
            }
        }

        // Update is called once per frame (pour mettre à jour les informations)
        void Update()
        {
            
        }

        // Pour la physique
        void FixedUpdate() {
            float conversionFactor = 500f; // pour que l'on es pas besoin de mettre des chiffre trop grand en vitesse
            // limitateur de vitesse latérale (y a probalbment mieux)
            if( rb.velocity.x > maxVelocity){
                rb.velocity = new Vector3(maxVelocity, rb.velocity.y, 0);
            }
            else if(rb.velocity.x < -maxVelocity){
                rb.velocity = new Vector3(-maxVelocity, rb.velocity.y, 0);
            }

            float dt = Time.fixedDeltaTime; // temps entre 2 fixedUpdate
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            Vector3 dF = Vector3.zero;

            if(x>0){
                dF += Vector3.right * speed * dt * conversionFactor ; // dF = da = accleration(t + dt) - acceleration(t) = vitesse * dt (masse = 1)
            }
            else if(x<0){
                dF += Vector3.left * speed * dt * conversionFactor ;
            }
            if(y > 0){
                // ici c'est la classe que j'ai crée dans l'autre fichier pour checker si le joueur touche le sol ou pas
                bool isGrounded = groundChecker.CheckGroundContact();
                if(isGrounded){
                    dF += Vector3.up * jumpSpeed * dt *conversionFactor ;
                }
            }
            rb.AddForce(dF);
        }

    }


}