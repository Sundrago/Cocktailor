using System;
using Cocktailor.Utility;
using Features.RecipeViewer;
using UnityEngine;
using UnityEngine.Serialization;

public class RecipeMarkerController : MonoBehaviour
{
    [SerializeField] private SfxManager sfxManager;
    [SerializeField] private Animator oMarkAnimator;
    [SerializeField] private Animator xMarkAnimator;
    [FormerlySerializedAs("mainControl")] [SerializeField] private RecipeViewerManager recipeViewerManager;
    
    private int currentCocktailIndex;
    private MemorizedState currentState;
    private static readonly int StateHidden = Animator.StringToHash("StateHidden");
    private static readonly int TriggerHide = Animator.StringToHash("TriggerHide");
    private static readonly int TriggerShow = Animator.StringToHash("TriggerShow");
    private static readonly int StateShown = Animator.StringToHash("StateShown");

    public void InitializeMarker(int index)
    {
        currentCocktailIndex = index;
        PlayerData.OnuserMemorizedStatehange += UpdateMarkerUI;
        SetupInitial();
    }

    private void SetupInitial()
    {
        var state = PlayerData.GetUserState(currentCocktailIndex);
        oMarkAnimator.SetTrigger(state == MemorizedState.Yes ? StateShown : StateHidden);
        xMarkAnimator.SetTrigger(state == MemorizedState.No ? StateShown : StateHidden);
    }
    private void UpdateMarkerUI(int index, MemorizedState newState)
    {
        if(index != currentCocktailIndex) return;
        Debug.Log(newState.ToString());
        switch (newState)
        {
            case MemorizedState.Yes:
                SetYesState();
                break;
            case MemorizedState.No:
                SetNoState();
                break;
            case MemorizedState.Undefined:
                SetUndefinedState();
                break;
        }

        currentState = newState;
    }

    private void SetYesState()
    {
        sfxManager.PlaySfx(6);
        oMarkAnimator.SetTrigger(TriggerShow);
        xMarkAnimator.SetTrigger(currentState == MemorizedState.No ? TriggerHide : StateHidden); 
    }

    private void SetNoState()
    {
        sfxManager.PlaySfx(6);
        oMarkAnimator.SetTrigger(currentState == MemorizedState.Yes ? TriggerHide : StateHidden);
        xMarkAnimator.SetTrigger(TriggerShow);
    }

    private void SetUndefinedState()
    {
        sfxManager.PlaySfx(1);
        if (currentState == MemorizedState.Yes)
        {
            oMarkAnimator.SetTrigger(TriggerHide);
            xMarkAnimator.SetTrigger(StateHidden);
        }
        else if(currentState == MemorizedState.No)
        {
            oMarkAnimator.SetTrigger(StateHidden);
            xMarkAnimator.SetTrigger(TriggerHide);
        }
        else
        {
            oMarkAnimator.SetTrigger(StateHidden);
            xMarkAnimator.SetTrigger(StateHidden);
        }
    }

    private void OnDestroy() {
        PlayerData.OnuserMemorizedStatehange -= UpdateMarkerUI;
    }
}