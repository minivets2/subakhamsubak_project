using System;
using System.Collections;

using UnityEngine;

public class Bread : MonoBehaviour
{
    [SerializeField] private int level;
    
    private bool _isDrop = false;
    private bool _isMerge = false;
    private Rigidbody2D _rigidbody2D;
    private CircleCollider2D _circleCollider2D;
    private Animator _animator;
    
    public int Level => level;
    public bool IsMerge => _isMerge;

    public delegate void DropEvent();
    public static DropEvent dropEvent;
    
    public delegate void LevelUpEvent(int level);
    public static LevelUpEvent levelUpEvent;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _rigidbody2D.bodyType = RigidbodyType2D.Static;
        _animator = GetComponent<Animator>();
    }

    public void SetLevel(int level)
    {
        this.level = level;
        string stateName = "Level" + level;
        _animator.Play(stateName, -1, 0f);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!_isDrop)
        {
            _isDrop = true;
            dropEvent?.Invoke();
        }
        
        if ((col.gameObject.GetComponent<Bread>()))
        {
            Bread other = col.gameObject.GetComponent<Bread>();
            
            if (other.Level == level && _isMerge == false &&
                !other.IsMerge && level < 11)
            {
                float meX = transform.position.x;
                float meY = transform.position.y;
                float otherX = other.transform.position.x;
                float otherY = other.transform.position.y;

                if (meY < otherY || (meY == otherY && meX > otherY))
                {
                    other.Hide(transform.position);
                    LevelUp();
                }
            }
        }
    }

    public void Hide(Vector3 targetPos)
    {
        _isMerge = true;
        
        _rigidbody2D.simulated = false;
        _circleCollider2D.enabled = false;

        StartCoroutine(HideRoutine(targetPos));
    }

    IEnumerator HideRoutine(Vector3 targetPos)
    {
        int frameCount = 0;

        while (frameCount < 20)
        {
            frameCount++;
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.5f);
            yield return null;
        }

        _isMerge = false;
        gameObject.SetActive(false);
    }

    private void LevelUp()
    {
        _isMerge = true;
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.angularVelocity = 0;

        StartCoroutine(LevelUpRoutine());
    }

    IEnumerator LevelUpRoutine()
    {
        yield return new WaitForSeconds(0.2f);

        int nextLevel = level + 1;
        string stateName = "Level" + nextLevel;
        _animator.Play(stateName, -1, 0f);

        yield return new WaitForSeconds(0.3f);
        
        level++;
        levelUpEvent?.Invoke(level);

        _isMerge = false;
    }

    public void DropBread()
    {
        _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
    }
}
