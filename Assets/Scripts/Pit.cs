using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : MonoBehaviour
{
    enum PitState {
        Grow,
        Hold,
        Shrink,
    };
    PitState state = PitState.Grow;

    [SerializeField]
    float GrowTo = 1f;
    [SerializeField]
    float ShrinkTo = .1f;
    [SerializeField]
    float Rate = 20f;
    float holdTimer = 1f;

    // Start is called before the first frame update
    void Start()
    {
        int index = (int)Globals.UpgradeTypes.Pit * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Pit] - 1;
        holdTimer = Globals.UpgradeLevelAttackTimes[index];
        GrowTo = GrowTo * Globals.UpgradeLevelAttackSizes[index];
    }

    // Update is called once per frame
    void Update ()
    {
        if (state == PitState.Grow)
        {
            float newScale = Mathf.Min(GrowTo, this.transform.localScale.x + Time.deltaTime * Rate);
            this.transform.localScale = new Vector3(newScale, newScale, newScale);
            if (newScale == GrowTo)
            {
                state =  PitState.Hold;
            }
        }
        else if (state == PitState.Hold)
        {
            holdTimer -= Time.deltaTime;
            if (holdTimer <= 0)
            {
                state =  PitState.Shrink;
            }
        }
        else if (state == PitState.Shrink)
        {
            float newScale = Mathf.Max(ShrinkTo, this.transform.localScale.x - Time.deltaTime * Rate);
            this.transform.localScale = new Vector3(newScale, newScale, newScale);
            if (newScale == ShrinkTo)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
