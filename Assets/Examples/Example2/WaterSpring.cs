using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Example2 {
    public class WaterSpring : MonoBehaviour
    {
        private float velocity = 0;
        private float force = 0;
        // current height
        private float height = 0f;
        // normal height
        private float target_height = 0f;
        public void WaveSpringUpdate(float springStiffness, float dampening) { 
            height = transform.localPosition.y;
            // maximum extension
            var x = height - target_height;
            var loss = -dampening * velocity;

            force = - springStiffness * x + loss;
            velocity += force;
            var y = transform.localPosition.y;  
            transform.localPosition = new Vector3(transform.localPosition.x, y+velocity, transform.localPosition.z);
    
        }
        // For Example purposes
        private float start_height = 2f;
        private void OnEnable() {
             transform.localPosition = new Vector3(transform.localPosition.x, 2, transform.localPosition.z);
        }
    }
}