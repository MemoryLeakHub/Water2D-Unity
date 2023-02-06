using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Example2 {
    public class WaterShapeController : MonoBehaviour
    {
        // Slowing the movement over time
        [SerializeField]
        private float dampening = 0.03f;
        // How stiff should our spring be constnat
        [SerializeField]
        private float spring_stiffness = 0.1f;
        [SerializeField]
        private List<WaterSpring> springs = new();
        void FixedUpdate()
        {
            foreach(WaterSpring waterSpringComponent in springs) {
                waterSpringComponent.WaveSpringUpdate(spring_stiffness, dampening);
            }
        }
    }
}