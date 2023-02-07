using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Example1 {
    public class WaterSpring : MonoBehaviour
    {
        private float velocity = 0;
        private float force = 0;
        // current height
        private float height = 0f;
        // normal height
        private float target_height = 0f;
        
        public void WaveSpringUpdate(float springStiffness) { 
            height = transform.localPosition.y;
            // maximum extension
            var x = height - target_height;

            force = - springStiffness * x;
            velocity += force;
            var y = transform.localPosition.y;  
            transform.localPosition = new Vector3(transform.localPosition.x, y+velocity, transform.localPosition.z);
    
        }
    }
}