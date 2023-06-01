using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObject : MonoBehaviour
{
    public float NormalDamageMin = 1f;
    public float NormalDamageMax = 5f;
    public float StrongDamageMin = 2f;
    public float StrongDamageMax = 8f;
    public float CriticalDamageMin = 3f;
    public float CriticalDamageMax = 10f;
    public bool CausePushBackDamageVelocity = false;
    public float PushBackDamageVelocityMultiplier = 1f;
}
