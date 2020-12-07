using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class MoveCamera : MonoBehaviour
{
    [SerializeField] Transform camera;

    [SerializeField] Transform rightPath;
    [SerializeField] Transform leftPath;

    public NamePlayerOnCar namePlayer;

    Transform car;
    Vector3 defaultPosition;
    Vector3 defaultRotation;
    Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        defaultPosition = transform.position;
        defaultRotation = camera.localEulerAngles;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (car != null)
        {
            transform.position = car.position - offset;
            camera.LookAt(car);
        }
    }
    public void SetCar(Transform currentCar)
    {
        car = currentCar;
        offset = car.position - transform.position;
        camera.LookAt(car);
        Transform pathAnim = Random.Range(0, 100) > 50 ? rightPath : leftPath;
        Vector3[] path = new Vector3[pathAnim.childCount];
        for (int i = 0; i < path.Length; i++)
        {
            path[i] = pathAnim.GetChild(i).localPosition;
        }
        camera.DOLocalPath(path, Random.Range(2, 4), PathType.CatmullRom).SetLoops(-1, LoopType.Yoyo);
    }
    public void MoveStartPosition()
    {
        car = null;
        DOTween.Kill(camera);
        transform.DOMove(defaultPosition, 1).OnComplete(() => { namePlayer.ActivateName(); });
        camera.DOLocalMove(Vector3.zero, 1);
        camera.DORotate(defaultRotation, 1);
    }
}
