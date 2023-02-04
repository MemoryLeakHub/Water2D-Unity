using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class WaveSpring : MonoBehaviour
{
    public static SpriteShapeController spriteShapeController = null;
    private int waveIndex = 0;
    public void Init(SpriteShapeController ssc) { 

        var index = transform.GetSiblingIndex();
        waveIndex = index+1;
        
        spriteShapeController = ssc;
    }

    void Update() { 
        // if (spriteShapeController != null) {
        //     Spline waterSpline = spriteShapeController.spline;
        //     Vector3 wavePosition = waterSpline.GetPosition(waveIndex);
        //     waterSpline.SetPosition(waveIndex, new Vector3(wavePosition.x, transform.localPosition.y, wavePosition.z));
        // }
    }
}
