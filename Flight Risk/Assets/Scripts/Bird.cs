using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] float _distance = 0;
    [SerializeField] float _launchForce = 1000f;
    [SerializeField] float _maxDragDistance = 3.5f;
    [SerializeField] float _boost = 0.2f;

    [SerializeField] GameObject _rightLimit;
    [SerializeField] GameObject _upperLimit;
    float _xLimit;
    float _yLimit;

    Rigidbody2D _rigidbody2d;
    SpriteRenderer _spriteRenderer;
    Animator _anim;

    enum State {
        flying, notFlying
    }
    State _state = State.notFlying;

    Vector2 _startPosition;
    bool _isResetting = false;
    bool _shot = false;
    bool _isBoosting = false;

    public bool IsDragging { get; private set; }

    void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = _rigidbody2d.position;
        _rigidbody2d.isKinematic = true;

        _xLimit = _rightLimit.transform.position.x;
        _yLimit = _upperLimit.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (_shot && !_isResetting)
            _isBoosting = ListenForBoost();

        if ((transform.position.x > _xLimit || transform.position.y > _yLimit) && !_isResetting)
            StartCoroutine(ResetAfterDelay());

        _anim.SetInteger("state", (int)_state);
    }

    void FixedUpdate()
    {
        if (_isBoosting)
        {
            _rigidbody2d.velocity = new Vector2(
                _rigidbody2d.velocity.x,
                _rigidbody2d.velocity.y + _boost);
        }
    }

    bool ListenForBoost()
    {
        if (Input.GetMouseButton(0))
            return true;

        return false;
    }

    void OnMouseDown()
    {
        if (_shot)
            return;

        _spriteRenderer.color = Color.red;
        IsDragging = true;
    }

    void OnMouseUp()
    {
        if (_shot)
            return;

        Vector2 currentPosition = _rigidbody2d.position;
        Vector2 direction = _startPosition - currentPosition;
        direction.Normalize();

        _rigidbody2d.isKinematic = false;
        _rigidbody2d.AddForce(direction * (_distance*_launchForce));

        _spriteRenderer.color = Color.white;

        IsDragging = false;
        _shot = true;
        _state = State.flying;
    }

    void OnMouseDrag()
    {
        if (_shot)
            return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 desiredPosition = mousePosition;

        _distance = Vector2.Distance(desiredPosition, _startPosition);
        if (_distance > _maxDragDistance)
        {
            Vector2 direction = desiredPosition - _startPosition;
            direction.Normalize();
            desiredPosition = _startPosition + (direction * _maxDragDistance);
        }

        if (desiredPosition.x > _startPosition.x)
            desiredPosition.x = _startPosition.x;

        _rigidbody2d.position = desiredPosition;
        _distance = Vector2.Distance(desiredPosition, _startPosition);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_isResetting)
            StartCoroutine(ResetAfterDelay());
    }

    IEnumerator ResetAfterDelay()
    {
        _isResetting = true;
        _state = State.notFlying;
        _isBoosting = false;

        yield return new WaitForSeconds(2);

        _rigidbody2d.position = _startPosition;
        _rigidbody2d.isKinematic = true;
        _rigidbody2d.velocity = Vector2.zero;
        _shot = false;

        yield return null;
        _isResetting = false;
    }
}
