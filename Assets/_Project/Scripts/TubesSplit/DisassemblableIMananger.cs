using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class DisassemblableIMananger : MonoSinglton<DisassemblableIMananger>
{

    public List<DisassemblableItem> items;
    public int NextItemOrderTotake = 1;
    public AudioSource audioSource;
    public LocalizedAudioClip localizedClip;
    [SerializeField] AudioClip succesAudio;
    public bool CanCheck = false;
    // Start is called before the first frame update
    void Start()
    {
       


        
    }

    // Update is called once per frame
    void Update()
    {
        
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

            item.OnTakeitemSucces();
            if (NextItemOrderTotake > items.Count + 1) return;
        }
        else
        {

            item.OnFailureItem();
            PlayAudio();
            //StartCoroutine(ShowSafteyWarning());
            //wrongsource.Play();
            //warningText.StringReference = SafteyWaring;

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
   
}
