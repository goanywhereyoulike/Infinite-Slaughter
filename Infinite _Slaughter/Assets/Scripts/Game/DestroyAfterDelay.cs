using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    public float delay = 1.0f;

    public void OnEnable()
    {
        StartCoroutine(DelayDestroy());
    }
    private IEnumerator DelayDestroy()
    {

        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);

    }
}
