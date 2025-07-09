using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IIteractableItem : IItem
{
    bool _IsIntractiobEnabled();
    void _EnableInteraction();
    void _DisableInteraction();
}
