using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum BreadType
{
    Bread1,
    Bread2,
    Bread3,
    Bread4,
    Bread5,
    Bread6,
    Bread7,
    Bread8,
    Bread9,
    Bread10,
    Bread11,
}

public class Bread : MonoBehaviour
{
    [SerializeField] private BreadType breadType;
    
    private bool _hasDroped = false;
    private Rigidbody2D _rigidbody2D;
    
    public BreadType BreadType => breadType;

    public delegate void DropEvent();
    public static DropEvent dropEvent;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.bodyType = RigidbodyType2D.Static;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!_hasDroped)
        {
            _hasDroped = true;
            dropEvent?.Invoke();
        }
    }

    public void DropBread()
    {
        _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
    }
}
