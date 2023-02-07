using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Example10 {
    [ExecuteAlways]
    public class WaterShapeController : MonoBehaviour
    {
        [SerializeField]
        private GameObject box;
        [SerializeField]
        private GameObject wavePointPref;
        //////////////////
        private int CorsnersCount = 2;
        [SerializeField]
        private SpriteShapeController spriteShapeController;
        [SerializeField]
        [Range(1, 100)]
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
                //////////////
                waterSpline.SetTangentMode(index, ShapeTangentMode.Continuous);

            }
            // loop through all the wave points
            // plus the both top left and right corners
            CreateSprings(waterSpline);
            
        }
        private void CreateSprings(Spline waterSpline) { 
            springs = new();
            
            for (int i = 0; i <= WavesCount+1; i++) {
                int index = i + 1; 

                Smoothen(waterSpline, index);

                GameObject wavePoint = Instantiate(wavePointPref, wavePoints.transform, false);
                wavePoint.transform.localPosition = waterSpline.GetPosition(index);

                WaterSpring waterSpring = wavePoint.GetComponent<WaterSpring>();
                waterSpring.Init(spriteShapeController);
                springs.Add(waterSpring);
            }
        }
        
        private void Smoothen(Spline waterSpline, int index)
        {
            Vector3 position = waterSpline.GetPosition(index);
            Vector3 positionPrev = position;
            Vector3 positionNext = position;
            if (index > 1) {
                positionPrev = waterSpline.GetPosition(index-1);
            }
            if (index - 1 <= WavesCount) {
                positionNext = waterSpline.GetPosition(index+1);
            }

            Vector3 forward = gameObject.transform.forward;

            float scale = Mathf.Min((positionNext - position).magnitude, (positionPrev - position).magnitude) * 0.33f;

            Vector3 leftTangent = (positionPrev - position).normalized * scale;
            Vector3 rightTangent = (positionNext - position).normalized * scale;

            SplineUtility.CalculateTangents(position, positionPrev, positionNext, forward, scale, out rightTangent, out leftTangent);
            
            waterSpline.SetLeftTangent(index, leftTangent);
            waterSpline.SetRightTangent(index, rightTangent);
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

        // Examples
        private Vector3 boxStartPosition = new Vector3(1.25f, 9f, 0f);
        // On Enable for example purposes
        void OnEnable() { 
            box.transform.position = boxStartPosition;
            StartCoroutine(CreateWaves());
            foreach(WaterSpring waterSpringComponent in springs) {
                waterSpringComponent.Init(spriteShapeController);
            }
        }
        void OnValidate() {
            if (!gameObject.active) {
                return;
            }
            // Clean waterpoints 
            StartCoroutine(CreateWaves());
        }
        IEnumerator CreateWaves() {
            foreach (Transform child in wavePoints.transform) {
                StartCoroutine(Destroy(child.gameObject));
            }
            yield return null;
            SetWaves();
            yield return null;
        }
        IEnumerator Destroy(GameObject go) {
            yield return null;
            DestroyImmediate(go);
        }
    }
}