using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private float breadCreateYPosition;
    [SerializeField] private Transform breadCreatePosition;
    
    [Header("Prefab")]
    [SerializeField] private GameObject breadPrefab;

    private GameObject _bread;
    private int _maxLevel;
    private bool _isDragging;
    private bool _canControl;
    
    public GameObject BreadPrefab => breadPrefab;
    public int MaxLevel => _maxLevel;

    private void OnEnable()
    {
        Bread.dropEvent += CanControl;
        Bread.levelUpEvent += LevelUp;
    }

    private void OnDisable()
    {
        Bread.dropEvent -= CanControl;
        Bread.levelUpEvent -= LevelUp;
    }

    private void Start()
    {
        StartCoroutine(NewBread(1, Vector3.zero, 0f));
        _canControl = true;
        _maxLevel = 0;
    }

    void Update()
    {
        if (!_canControl || _bread == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            _isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
            _bread.GetComponent<Bread>().DropBread();
            _bread = null;
            int level = Random.Range(1, _maxLevel + 1);
            StartCoroutine(NewBread(level, Vector3.zero, 0.5f));
        }

        if (_isDragging)
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 
                Input.mousePosition.y, -Camera.main.transform.position.z));
            
            float breadRadius = _bread.GetComponent<CircleCollider2D>().radius;
            if (Input.mousePosition.x - breadRadius > Screen.width / 2 - 540 && Input.mousePosition.x + breadRadius < Screen.width / 2 + 540)
                _bread.transform.position = new Vector3(point.x, breadCreateYPosition, point.z);
        }
    }

    IEnumerator NewBread(int level, Vector3 position, float creatTime)
    {
        yield return new WaitForSeconds(creatTime);
        
        var bread = Instantiate(breadPrefab, breadCreatePosition);
        bread.transform.position = new Vector3(position.x, breadCreateYPosition, position.z);
        bread.gameObject.GetComponent<Bread>().SetLevel(level);
        _bread = bread;

        yield return null;
    }

    public void LevelUp(int level)
    {
        _maxLevel = level;
    }

    private void CanControl()
    {
        _canControl = true;
    }
}
