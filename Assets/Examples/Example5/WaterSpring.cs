using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Example5 {
    public class WaterSpring : MonoBehaviour
    {
        [System.NonSerialized]
        public float velocity = 0;
        private float force = 0;
        // current height
        [System.NonSerialized]
        public float height = 0f;
        // normal height
        private float target_height = 0f;
        public void Init() { 
            velocity = 0;
            transform.localPosition = new Vector3(transform.localPosition.x, start_height, transform.localPosition.z);
            height = transform.localPosition.y;
            target_height = transform.localPosition.y;
        }
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
    }
}