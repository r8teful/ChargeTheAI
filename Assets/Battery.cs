using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battery : MonoBehaviour {
    [SerializeField] private SpriteRenderer rectangle;
    [SerializeField] private Text text;
    [SerializeField] private topCollision top;
    private float deathTime;

    void Update() {
        deathTime = 4 - top.UntillDeath();
        //Debug.Log(top.UntillDeath());
        rectangle.color = new Color(0 +  (1/deathTime), 1 - (1 / deathTime), 0,0.5f);
        if (deathTime >=0)text.text = deathTime.ToString();
    }
}
