using UnityEngine;

public class ExampleScript : MonoBehaviour
{
    [LayerSelectorAttribute] // Apply the LayerSelectorAttribute here
    [SerializeField]
    public int selectedLayer;

    // Other properties and methods...
}
