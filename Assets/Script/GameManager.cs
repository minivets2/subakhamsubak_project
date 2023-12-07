using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
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
    [SerializeField] private ParticleSystem destroyParticle; 

    private GameObject _bread;
    private int _maxLevel;
    private bool _isDragging;
    private bool _isGameOver;
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
        StarGame();
    }

    void Update()
    {
        if (_bread == null || _isGameOver) return;

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
    
    public void StarGame()
    {
        foreach (Transform child in breadCreatePosition)
        {
            Destroy(child.gameObject) ;
        }
        
        _maxLevel = 0;
        _gameScore = 0;
        _isGameOver = false;
        _isDragging = false;
        uiManager.InitUI();
        
        StartCoroutine(NewBread(1, Vector3.zero, 0.5f));
    }

    void SetBreadPosition()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 
            Input.mousePosition.y, -Camera.main.transform.position.z));
            
        float breadRadius = _bread.GetComponent<CircleCollider2D>().radius;
        
        if (Input.mousePosition.x - breadRadius > 63 && Input.mousePosition.x + breadRadius < Screen.width - 63)
            _bread.transform.position = new Vector3(point.x, breadCreateYPosition, point.z);
    }

    IEnumerator NewBread(int level, Vector3 position, float creatTime)
    {
        yield return new WaitForSeconds(creatTime);
        
        AudioManager.Instance.PlaySFX(SoundName.Create);
        var bread = Instantiate(breadPrefab, breadCreatePosition);
        bread.transform.position = new Vector3(position.x, breadCreateYPosition, position.z);
        bread.gameObject.GetComponent<Bread>().SetLevel(level);
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
        if (!_isGameOver) _isGameOver = true;
        else return;
        
        _destroyBreads.Clear();

        for (int i = 0; i < breadCreatePosition.childCount; i++)
        {
            if (breadCreatePosition.GetChild(i).gameObject.activeSelf)
                _destroyBreads.Add(breadCreatePosition.GetChild(i).gameObject);
        }

        _destroyBreads.OrderBy(x => x.transform.position.y);
        _destroyBreads.Reverse();
        StartCoroutine(nameof(DestroyBread));
    }

    IEnumerator DestroyBread()
    {
        for (int i = 0; i < _destroyBreads.Count; i++)
        {
            var particle = Instantiate(destroyParticle, breadCreatePosition);
            particle.gameObject.transform.localPosition = _destroyBreads[i].transform.localPosition;
            Destroy(_destroyBreads[i]);
            yield return new WaitForSeconds(0.15f);
        }

        yield return new WaitForSeconds(1f);
        
        uiManager.ShowGameOverPopup();
    }
    
    
}
