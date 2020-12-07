using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentationCur : MonoBehaviour
{
    Quaternion defaultRootation;
    private void Start()
    {
        defaultRootation = transform.rotation;
    }
    public void RotateAnim()
    {
        transform.DOLocalRotate(Vector3.up * 359, 3f, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    }
    public void StopRotate()
    {
        DOTween.Kill(transform);
        transform.rotation = defaultRootation;
    }
}
