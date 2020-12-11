using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]

public class Monster : FocusObject
{
    [SerializeField] Sprite _deadSprite;
    [SerializeField] ParticleSystem _particleSystem;
    [SerializeField] float _hitTolerance = 5f;

    bool _hasDied = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (ShouldDieFromCollision(collision))
            StartCoroutine(Die());
    }

    bool ShouldDieFromCollision(Collision2D collision)
    {
        if (_hasDied)
            return false;

        if (collision.contacts[0].normal.y < -0.5)
            return true;

        if (collision.contacts[0].relativeVelocity.magnitude > _hitTolerance)
            return true;

        return false;
    }

    IEnumerator Die()
    {
        _hasDied = true;
        GetComponent<SpriteRenderer>().sprite = _deadSprite;
        _particleSystem.Play();
        yield return new WaitForSeconds(1);
        _waiting = false;
        gameObject.SetActive(false);
    }
}
