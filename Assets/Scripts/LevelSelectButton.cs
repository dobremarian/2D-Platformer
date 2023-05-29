using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    [SerializeField] int listIndex;
    [SerializeField] string levelToLoad;
    [SerializeField] GameObject selectBorder;
    [SerializeField] Sprite levelPreviewImage;

    bool isSelected = false;

    LevelSelectMenuUI lsmUI;

    bool IsSelected
    {
        get { return isSelected; }
        set { isSelected = value; }
    }

    public int ListIndex
    {
        get { return listIndex; }
    }

    
    void Awake()
    {
        lsmUI = FindObjectOfType<LevelSelectMenuUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectButton()
    {
        selectBorder.SetActive(true);
        lsmUI.SetPreviewImage(levelPreviewImage);
        lsmUI.SetLevelToLoad(levelToLoad);
    }

    public void DeselectButton()
    {
        selectBorder.SetActive(false);
    }
}
