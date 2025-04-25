using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionController : MonoBehaviour
{
    [SerializeField] private GameObject _levelPrefab;
    [SerializeField] private RectTransform _transform;

    private int totalLevel = 10;
    // Start is called before the first frame update
    void Start()
    {
        CreateLevels();
    }

    // Update is called once per frame
    void Update()
    {
        
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


