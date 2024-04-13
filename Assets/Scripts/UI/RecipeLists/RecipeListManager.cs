using System;
using System.Collections;
using System.Collections.Generic;
using Cocktailor;
using Cocktailor.Utility;
using UnityEngine;
using VoxelBusters.CoreLibrary.Editor.NativePlugins.Build.Android;

public class RecipeListManager : MonoBehaviour
{
    [SerializeField] private Selection[] selections = new Selection[40];
    [SerializeField] private RectTransform contents;

    private List<int> recipeFound;
    
    public void Start()
    {
        PlayerData.OnuserMemorizedStatehange += UpdateMarker;
    }

    public void SetupSelections(List<int> recipeFound, Action<int> action)
    {
        this.recipeFound = recipeFound;
        
        if (recipeFound.Count == 0)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);

        for (var i = 0; i < recipeFound.Count; i++) selections[i].Init(recipeFound[i], i + 1, action);
        for (var i = recipeFound.Count; i < selections.Length; i++) selections[i].gameObject.SetActive(false);

        UpdateSizeDelta(recipeFound.Count);
    }

    private void UpdateSizeDelta(int maxIdx)
    {
        var lastSelectionRect = selections[maxIdx - 1].GetComponent<RectTransform>();
        contents.sizeDelta = new Vector2(contents.sizeDelta.x, lastSelectionRect.anchoredPosition.y * -1 + 550);
    }

    private void UpdateMarker(int idx, MemorizedState state)
    {
        if(!recipeFound.Contains(idx)) return;
        selections[recipeFound.IndexOf(idx)].UpdateIcon(state);
    }
}
