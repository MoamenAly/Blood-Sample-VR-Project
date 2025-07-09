using UnityEngine;

//public class DropdownListAttribute : PropertyAttribute
//{
//    public string[] List;

//    public DropdownListAttribute(string[] list)
//    {
//        List = list;
//    }
//}

public class DropdownListAttribute : PropertyAttribute
{
    public string ListName;

    public DropdownListAttribute(string listName)
    {
        ListName = listName;
    }
}
