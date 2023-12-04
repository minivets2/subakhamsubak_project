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
    private bool _isDragging;
    private bool _canControl;
    
    public GameObject BreadPrefab => breadPrefab;

    private void OnEnable()
    {
        Bread.dropEvent += CanControl;
        Bread.mergeEvent += MergeBread;
    }

    private void OnDisable()
    {
        Bread.dropEvent -= CanControl;
        Bread.mergeEvent -= MergeBread;
    }

    private void Start()
    {
        StartCoroutine(NewBread(1, Vector3.zero, 0f));
        _canControl = true;
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
            StartCoroutine(NewBread(2, Vector3.zero, 0.5f));
        }

        if (_isDragging)
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 
                Input.mousePosition.y, -Camera.main.transform.position.z));

            int breadRadius = (int)_bread.GetComponent<CircleCollider2D>().radius;
            if (Input.mousePosition.x - breadRadius > 0 && Input.mousePosition.x + breadRadius < Screen.width)
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

    public void MergeBread(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    private void CanControl()
    {
        _canControl = true;
    }
}
