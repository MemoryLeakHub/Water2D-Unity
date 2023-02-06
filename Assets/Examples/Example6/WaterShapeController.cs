using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Example6 {
    public class WaterShapeController : MonoBehaviour
    {
        [SerializeField]
        private GameObject wavePointPref;
        //////////////////
        private int CorsnersCount = 2;
        [SerializeField]
        private SpriteShapeController spriteShapeController;
        [SerializeField]
        private int WavesCount = 6;
        [SerializeField]
        private GameObject wavePoints;
        //////////////////
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
                waterSpringComponent.WavePointUpdate();
            }

            UpdateSprings();
        }
        private void SetWaves() { 
            Spline waterSpline = spriteShapeController.spline;
            int waterPointsCount = waterSpline.GetPointCount();

            // Remove middle points for the waves
            // Keep only the corners
            // Removing 1 point at a time we can remove only the 1st point
            // This means every time we remove 1st point the 2nd point becomes first
            for (int i = CorsnersCount; i < waterPointsCount - CorsnersCount; i++) {
                waterSpline.RemovePointAt(CorsnersCount);
            }

            Vector3 waterTopLeftCorner = waterSpline.GetPosition(1);
            Vector3 waterTopRightCorner = waterSpline.GetPosition(2);
            float waterWidth = waterTopRightCorner.x - waterTopLeftCorner.x;

            float spacingPerWave = waterWidth / (WavesCount+1);
            // Set new points for the waves
            for (int i = WavesCount; i > 0 ; i--) {
                int index = CorsnersCount;

                float xPosition = waterTopLeftCorner.x + (spacingPerWave*i);
                Vector3 wavePoint = new Vector3(xPosition, waterTopLeftCorner.y, waterTopLeftCorner.z);
                waterSpline.InsertPointAt(index, wavePoint);
                waterSpline.SetHeight(index, 0.1f);
                waterSpline.SetCorner(index, false);

            }

            // loop through all the wave points
            // plus the both top left and right corners
            CreateSprings(waterSpline);
            Splash(2, 1);
            
        }
        private void CreateSprings(Spline waterSpline) { 
            if (springs.Count > 0) {
                return;
            }

            springs = new();
            for (int i = 0; i <= WavesCount+1; i++) {
                int index = i + 1; 

                GameObject wavePoint = Instantiate(wavePointPref, wavePoints.transform, false);
                wavePoint.transform.localPosition = waterSpline.GetPosition(index);

                WaterSpring waterSpring = wavePoint.GetComponent<WaterSpring>();
                waterSpring.Init(spriteShapeController);
                springs.Add(waterSpring);
            }
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
                waterSpringComponent.Init(spriteShapeController);
            }
            SetWaves();
        }
    }
}