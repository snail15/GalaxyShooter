using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleShot : MonoBehaviour
{
    private Component[] _lasers;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _lasers = GetComponentsInChildren<Laser>();
        if(_lasers.Length == 0) Destroy(this.gameObject);
    }
}
