using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusObject : MonoBehaviour
{
    float _minVelocity = 0.5f;
    public bool _waiting = false;

    Rigidbody2D _rigidbody2d;


    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _waiting = _rigidbody2d.velocity.magnitude >= _minVelocity;
    }
}
