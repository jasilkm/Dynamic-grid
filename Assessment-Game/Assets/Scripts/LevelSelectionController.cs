using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelSelectionController : MonoBehaviour
{
    [SerializeField] private GameObject _levelPrefab;
    [SerializeField] private RectTransform _transform;
    [SerializeField] private Button _loadBtn;

    private int totalLevel = 10;
    // Start is called before the first frame update
    void Start()
    {
        CreateLevels();
    }

     void Awake()
    {
        _loadBtn.onClick.AddListener(LoadLevelFromPersistance);

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void LoadLevelFromPersistance()
    {
        GameEvents.RaiseOnLoadLevelFromPersistance();
        Hide();
    }

    public void CreateLevels()
    {
        for (int i = 0; i < totalLevel; i++)
        {
            GameObject level = Instantiate(_levelPrefab, _transform);
            LevelItem levelItem = level.GetComponent<LevelItem>();
            levelItem.UpdateLevelInfo(i+1);
        }

    }


    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }


}


