using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    private Transform camera;
    private Transform self;
    void Start()
    {
        self = GetComponent<Transform>();
        camera = Camera.main.transform;
    }
    private void Update()
    {
        self.LookAt(camera);
        self.localRotation = self.localRotation * Quaternion.Euler(0, 180, 0);
    }

}
