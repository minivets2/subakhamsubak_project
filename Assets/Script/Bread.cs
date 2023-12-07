using System;
using System.Collections;

using UnityEngine;

public class Bread : MonoBehaviour
{
    [SerializeField] private int level;
    
    private bool _isDrop = false;
    private bool _isMerge = false;
    private bool _isExplode;

    private float _explodeTime;
    
    private Rigidbody2D _rigidbody2D;
    private CircleCollider2D _circleCollider2D;
    private Animator _animator;
    
    public int Level => level;
    public bool IsMerge => _isMerge;
    public bool IsDrop => _isDrop;

    public delegate void LevelUpEvent(int level);
    public static LevelUpEvent levelUpEvent;
    
    public delegate void GameOverEvent();
    public static GameOverEvent gameOverEvent;
    
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (_isExplode)
        {
            _explodeTime += Time.deltaTime;
            
            if (_explodeTime > 3)
            {
                gameOverEvent?.Invoke();
                _isExplode = false;
            }
        }
    }

    public void SetLevel(int level)
    {
        this.level = level;
        string stateName = "Level" + level;
        _animator.Play(stateName, -1, 0f);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!_isDrop && (col.gameObject.CompareTag($"Floor") || col.gameObject.CompareTag("Bread")))
        {
            _isDrop = true;
        }
        
        if (col.gameObject.CompareTag($"Bread"))
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

        HideRoutine(targetPos);
    }

    void HideRoutine(Vector3 targetPos)
    {
        int frameCount = 0;

        while (frameCount < 10)
        {
            frameCount++;
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.5f);
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
        _rigidbody2D.gravityScale = 2f;
    }

    public void Explode(bool value)
    {
        _isExplode = value;
        _animator.SetBool("isExplode", value);
    }
}
