using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("UI")]
    [SerializeField] private UIManager uiManager;
    
    [Header("Init")]
    [SerializeField] private float breadCreateYPosition;
    [SerializeField] private Transform breadCreatePosition;
    
    [Header("Prefab")]
    [SerializeField] private GameObject breadPrefab;

    private GameObject _bread;
    private int _maxLevel;
    private bool _isDragging;
    private int _gameScore;
    private List<GameObject> _destroyBreads = new List<GameObject>();
    
    public GameObject BreadPrefab => breadPrefab;
    public int MaxLevel => _maxLevel;

    private void OnEnable()
    {
        Bread.levelUpEvent += LevelUp;
        Bread.gameOverEvent += GetAllBread;
    }

    private void OnDisable()
    {
        Bread.levelUpEvent -= LevelUp;
        Bread.gameOverEvent -= GetAllBread;
    }

    private void Start()
    {
        StartCoroutine(NewBread(1, Vector3.zero, 0f));
        _maxLevel = 0;
        _gameScore = 0;
    }

    void Update()
    {
        if (_bread == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            _isDragging = true;
            SetBreadPosition();
        }

        if (_isDragging && Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
            _bread.GetComponent<Bread>().DropBread();
            _bread = null;
            int level = Random.Range(1, _maxLevel + 1);
            StartCoroutine(NewBread(level, Vector3.zero, 0.5f));
        }

        if (_isDragging)
        {
            SetBreadPosition();
        }
    }

    void SetBreadPosition()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 
            Input.mousePosition.y, -Camera.main.transform.position.z));
            
        float breadRadius = _bread.GetComponent<CircleCollider2D>().radius;
        if (Input.mousePosition.x - breadRadius > Screen.width / 2 - 482 && Input.mousePosition.x + breadRadius < Screen.width / 2 + 482)
            _bread.transform.position = new Vector3(point.x, breadCreateYPosition, point.z);
    }

    IEnumerator NewBread(int level, Vector3 position, float creatTime)
    {
        yield return new WaitForSeconds(creatTime);
        
        AudioManager.Instance.PlaySFX(SoundName.Create);
        var bread = Instantiate(breadPrefab, breadCreatePosition);
        bread.transform.position = new Vector3(position.x, breadCreateYPosition, position.z);
        bread.gameObject.GetComponent<Bread>().SetLevel(level);
        
        yield return new WaitForSeconds(0.5f);
        
        _bread = bread;

        yield return null;
    }

    public void LevelUp(int level)
    {
        if (level < 6 && _maxLevel < 5)
            _maxLevel = level;

        _gameScore += level * 4;
        uiManager.SetGameScore(_gameScore);
    }

    private void GetAllBread()
    {
        for (int i = 0; i < breadCreatePosition.childCount; i++)
        {
            if (breadCreatePosition.GetChild(i).gameObject.activeSelf)
                _destroyBreads.Add(breadCreatePosition.GetChild(i).gameObject);
        }

        _destroyBreads.OrderBy(x => x.transform.position.y);

        StartCoroutine(nameof(DestroyBread));
    }

    IEnumerator DestroyBread()
    {
        for (int i = 0; i < _destroyBreads.Count; i++)
        {
            Destroy(_destroyBreads[i]);
            yield return new WaitForSeconds(0.5f);
        }
    }
    
    
}
