using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCar : MonoBehaviour
{
    [SerializeField] Transform wheels;
    Rigidbody rbCar;
    FixedJoint[] jointWheels;
    // public Vector3 com;
    private void Start()
    {
        rbCar = transform.GetComponent<Rigidbody>();
        //  rbCar.centerOfMass = com;
        jointWheels = new FixedJoint[wheels.childCount];
        for (int i = 0; i < jointWheels.Length; i++)
        {
            jointWheels[i] = wheels.GetChild(i).GetComponent<FixedJoint>();
            jointWheels[i].breakForce = Mathf.Infinity;
        }

    }
    public void AddForceCar(float countEnergy)
    {
        rbCar.isKinematic = false;
        rbCar.transform.SetParent(null);
        rbCar.AddForce(rbCar.transform.position + transform.forward * countEnergy, ForceMode.Impulse);
        rbCar.AddTorque(Vector3.right * (countEnergy * Random.Range(0.05f, 0.075f)), ForceMode.Impulse);
        Invoke("ActivateFixedJoint", 0.5f);
    }
    public bool IsEndFly()
    {
        return rbCar.velocity.magnitude < 0.05f;
    }
    void ActivateFixedJoint()
    {
        for (int i = 0; i < jointWheels.Length; i++)
        {
            jointWheels[i].breakForce = rbCar.mass;
        }

    }

}
