#if UNITY_EDITOR


using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class EnumTable
{
    [FormerlySerializedAs("allCharecters")]
    [TableList(IsReadOnly = true, AlwaysExpanded = true), ShowInInspector]
    private readonly List<EnumWrapper> allCharacters;


    public EnumTable(List<string> characters)
    {
        //this.allCharacters = characters.Select(x => new EnumWrapper(x)).ToList();
    }
    private class EnumWrapper
    {
        //
    }
}
#endif
