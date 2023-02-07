using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Example8 {
    public class WaterSpring : MonoBehaviour
    {
        private int waveIndex = 0;
        [SerializeField]
        private static SpriteShapeController spriteShapeController = null;
        /////////////////
        [System.NonSerialized]
        public float velocity = 0;
        private float force = 0;
        // current height
        [System.NonSerialized]
        public float height = 0f;
        // normal height
        private float target_height = 0f;
        public void Init(SpriteShapeController ssc) { 
            var index = transform.GetSiblingIndex();
            waveIndex = index+1;
            spriteShapeController = ssc;

            velocity = 0;
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
        public void WavePointUpdate() { 
            if (spriteShapeController != null) {
                Spline waterSpline = spriteShapeController.spline;
                Vector3 wavePosition = waterSpline.GetPosition(waveIndex);
                waterSpline.SetPosition(waveIndex, new Vector3(wavePosition.x, transform.localPosition.y, wavePosition.z));
            }
        }
    }
}