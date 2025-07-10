using BNG;
using DG.Tweening;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class Menu : MonoBehaviour, IMenu
{
    public Menus MenuId { get => menuId; }
    [SerializeField] Menus menuId;

   

    void Start()
    {
       
    }



    public void Hide()
    {
        
    }

    public void Show()
    {
        
    }

}

public interface IMenu
{
    void Show();
    void Hide();
}

