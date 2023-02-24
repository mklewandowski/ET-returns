using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombShockWave : MonoBehaviour
{
    enum ExplosionState {
        Grow,
        Shrink,
    };
    ExplosionState state = ExplosionState.Grow;

    [SerializeField]
    float GrowTo = 1f;
    [SerializeField]
    float ShrinkTo = .1f;
    [SerializeField]
    float Rate = 20f;

    // Update is called once per frame
    void Update ()
    {
        if (state == ExplosionState.Grow)
        {
            float newScale = Mathf.Min(GrowTo, this.transform.localScale.x + Time.deltaTime * Rate);
            this.transform.localScale = new Vector3(newScale, newScale, newScale);
            if (newScale == GrowTo)
            {
                state =  ExplosionState.Shrink;
            }
        }
        else if (state == ExplosionState.Shrink)
        {
            float newScale = Mathf.Max(ShrinkTo, this.transform.localScale.x - Time.deltaTime * Rate);
            this.transform.localScale = new Vector3(newScale, newScale, newScale);
            if (newScale == ShrinkTo)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void StartEffect()
    {
        state = ExplosionState.Grow;
    }
}
