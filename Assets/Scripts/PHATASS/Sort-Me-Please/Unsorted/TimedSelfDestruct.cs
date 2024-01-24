using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSelfDestruct : MonoBehaviour
{
    [SerializeField]
    [Tooltip("GameObject will self destruct after given time")]
    private float lifeTime = 5f;

    private void Update ()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f)
        { UnityEngine.Object.Destroy(this.gameObject); }
    }
}
