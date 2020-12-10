using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraDetector : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _vcam1;
    [SerializeField] GameObject _changePoint2;
    [SerializeField] float _change1;
    [SerializeField] float _change2;

    enum State
    {
        first, second, third
    }

    State _state = State.first;
    Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _change1 = _vcam1.transform.position.x;
        _change2 = _changePoint2.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        SetState();
        _anim.SetInteger("state", (int)_state);
    }

    private void SetState()
    {
        if (transform.position.x < _change1)
        {
            _state = State.first;
        }
        else if (transform.position.x >= _change1 && transform.position.x < _change2)
        {
            _state = State.second;
        }
        else
        {
            _state = State.third;
        }
    }
}
