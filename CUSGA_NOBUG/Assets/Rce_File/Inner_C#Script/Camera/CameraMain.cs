using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraMain : MonoBehaviour
{ 
    public Transform PlayerTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var transform1 = transform;
        var position = PlayerTransform.position;
        transform1.position = new Vector3(position.x, position.y, transform1.position.z);
    }
}
