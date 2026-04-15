using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "DialogueSO", menuName = "Scriptable Objects/DialogueSO")]
public class DialogueSO : ScriptableObject
{
    [SerializeField] public List<LineaTexto> dialogo;
}
