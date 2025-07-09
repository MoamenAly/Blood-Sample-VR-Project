using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] List<Menu> menuList;
    public void Show(Menus menuId) {
        for (int i = 0; i < menuList.Count; i++) {
            if (menuList[i].MenuId == menuId)
            {
               menuList[i].gameObject.SetActive(true); 
            }            
        }
    }
    public void ShowAndHideOthors(Menus menuId)
    {
        for (int i = 0; i < menuList.Count; i++)
        {
            if (menuList[i].MenuId == menuId)
            {
                menuList[i].gameObject.SetActive(true);
            }
            else
            {
                menuList[i].gameObject.SetActive(false);
            }
        }
    }
    public void Hide(Menus menuId)
    {
        for (int i = 0; i < menuList.Count; i++)
        {
            if (menuList[i].MenuId == menuId)          
            {
                menuList[i].gameObject.SetActive(false);
            }
        }
    } 
}
