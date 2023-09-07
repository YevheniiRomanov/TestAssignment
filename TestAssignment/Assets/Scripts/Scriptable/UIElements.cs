using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

    [CreateAssetMenu(fileName = "UIElements", menuName = "UIManager", order = 0)]
    public class UIElements : ScriptableObject
    {
        [SerializeField]
        public List<UIElement> UIElementsList;
 
        [System.Serializable]
   
        [IncludeInSettings(true)]
        public class UIElement{
            [SerializeField]
            public UIType Type;
       
            [SerializeField]
            public GameObject Obj;
   
        }
    }

public enum UIType
{
    SessionSelector,
    Loading,
    Damage,
    EndGame
}