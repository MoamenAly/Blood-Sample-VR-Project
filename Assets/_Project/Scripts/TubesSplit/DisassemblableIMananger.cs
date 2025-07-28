using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

public class DisassemblableIMananger : MonoSinglton<DisassemblableIMananger>
{

    public List<DisassemblableItem> items;
    
    public List<DisassemblableItem> items_vactNeedle;
    public List<DisassemblableItem> items_ButterNeedle;

    public int NextItemOrderTotake = 1;
    public AudioSource audioSource;
    public LocalizedAudioClip localizedClip;
    [SerializeField] AudioClip succesAudio;
    public bool CanCheck = false;


    [SerializeField] private UnityEvent2 OnAllSucess;
    
    public ChooseNeedleType chooseNeedleType;
    
    public void SetAvailable(bool state)
    {
       CanCheck = state;
    }
    public void Subscribe(DisassemblableItem item)
    {
        items.Add(item);
    }

    public void CheckDisassemble(DisassemblableItem item)
    {
        if (!CanCheck) return;
        if (NextItemOrderTotake == item.Order)
        {
            print("Takee it"); /// sunscribe to ui manager
            audioSource.clip = succesAudio;
            audioSource.Play();
            IncreasIndex();
            item.OnTakeitemSucces();
            OnAllSucces();
            if (NextItemOrderTotake > items.Count + 1) return;
        }
        else
        {

            item.OnFailureItem();
            PlayAudio();


        }
    }

    [Button]
    public void AssignListOfItemNeedleTybes()
    {
        if (chooseNeedleType.needleType == NeedleType.Vacutainer)
        {
            items = items_vactNeedle;
        }
        else if (chooseNeedleType.needleType == NeedleType.Butterfly)
        {
            items = items_ButterNeedle;
        }
    }
    private void IncreasIndex()
    {
        if(NextItemOrderTotake <= items.Count)
        {
            NextItemOrderTotake++;
        }
    }


    public void PlayAudio()
    {
       localizedClip.LoadAssetAsync().Completed += handle =>
        {
            if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
            {
                AudioClip clip = handle.Result;
                audioSource.clip = clip;
                audioSource.Play();
            }
            else
            {
                Debug.LogError($"Failed to load localized audio");
            }
        };
    }

    public void OnAllSucces()
    {
        if(NextItemOrderTotake > items.Count)
             OnAllSucess?.Invoke();
    }
   
}
