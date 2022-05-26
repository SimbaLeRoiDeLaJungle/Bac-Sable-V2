using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Simba{
    // pour que la cam suive le joueur
    public class CameraScript : MonoBehaviour
    {
        [SerializeField] private Transform player;
        private Camera cam;
        private Vector3 velocity;
        [SerializeField] private float dampTime;
        void Start()
        {
            cam = GetComponent<Camera>();
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 point = cam.WorldToViewportPoint(player.position);
            Vector3 delta = player.position - cam.ViewportToWorldPoint(new Vector3(.5f, .3f, point.z)); 
            Vector3 destination = transform.position + delta;
            destination = new Vector3(destination.x,destination.y,-12f);
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime); 
        }
    }
}