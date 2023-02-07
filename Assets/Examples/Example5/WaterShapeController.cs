using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Example5 {
    public class WaterShapeController : MonoBehaviour
    {
        private int CorsnersCount = 2;
        [SerializeField]
        private SpriteShapeController spriteShapeController;
        [SerializeField]
        private int WavesCount = 6;
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
        }
        // On Enable for example purposes
        void OnEnable() { 
            SetWaves();
        }
    }
}