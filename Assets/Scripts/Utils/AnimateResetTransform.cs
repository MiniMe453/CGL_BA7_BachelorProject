using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateResetTransform : MonoBehaviour
{
    public Transform transformToReset;
    public float resetSpeed;
    IEnumerator m_rotationResetCoroutine;
    
    public void ResetRotation()
    {
        if(m_rotationResetCoroutine != null)
            return;

        m_rotationResetCoroutine = animationRotationReset(transform.rotation);
        StartCoroutine(m_rotationResetCoroutine);

        Debug.LogError(Quaternion.Angle(transform.rotation, Quaternion.identity));
    }

    IEnumerator animationRotationReset(Quaternion originalRot)
    {
        float timer = 0;

        while(Mathf.Abs(Quaternion.Angle(originalRot, Quaternion.identity)) > 0.1f)
        {
            timer += Time.deltaTime;
            Quaternion.Lerp(originalRot, Quaternion.identity,timer);
            yield return null;
        } 
    }
}
