using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Example3 {
    public class WaterShapeController : MonoBehaviour
    {
        // How much to spread to the other springs
        public float spread = 0.006f;
        // Slowing the movement over time
        [SerializeField]
        private float dampening = 0.03f;
        // How stiff should our spring be constnat
        [SerializeField]
        private float springStiffness = 0.1f;
        [SerializeField]
        private List<WaterSpring> springs = new();
        void FixedUpdate()
        {
            foreach(WaterSpring waterSpringComponent in springs) {
                waterSpringComponent.WaveSpringUpdate(springStiffness, dampening);
            }

            UpdateSprings();
        }
        private void UpdateSprings() { 
            int count = springs.Count;
            float[] left_deltas = new float[count];
            float[] right_deltas = new float[count];

            for(int i = 0; i < count; i++) {
                if (i > 0) {
                    left_deltas[i] = spread * (springs[i].height - springs[i-1].height);
                    springs[i-1].velocity += left_deltas[i];
                }
                if (i < springs.Count - 1) {
                    right_deltas[i] = spread * (springs[i].height - springs[i+1].height);
                    springs[i+1].velocity += right_deltas[i];
                }
            }
        }

        private void Splash(int index, float speed) { 
            if (index >= 0 && index < springs.Count) {
                springs[index].velocity += speed;
            }
        }

        // On Enable for example purposes
        void OnEnable() { 
            foreach(WaterSpring waterSpringComponent in springs) {
                waterSpringComponent.Init();
            }
            Splash(2, 1);
        }
    }
}