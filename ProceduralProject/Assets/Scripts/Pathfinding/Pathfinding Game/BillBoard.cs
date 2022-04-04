using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    public Camera cam;

    private void Start()
    {
        cam = OverheadCam.instance.cam;
    }
    private void LateUpdate()
    {
        transform.LookAt(transform.position + cam.transform.forward);
    }
}
