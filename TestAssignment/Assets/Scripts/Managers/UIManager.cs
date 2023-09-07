using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] UIElements _uiElementsList;
    [SerializeField] Transform _contant;

    Dictionary<UIType, BaseView> _openPopUps = new();

    public void OpenPopUp(UIType type)
    {
        var prefab = _uiElementsList.UIElementsList.FirstOrDefault(x => x.Type == type)?.Obj;
        if (prefab == null)
            return;

        if (_openPopUps.ContainsKey(type))
            ClosePopUp(type);
        var obj = Instantiate(prefab, _contant);
        var view = obj.GetComponent<BaseView>();
        view.Init();
        _openPopUps.Add(type, view);
    }

    public void ClosePopUp(UIType type)
    {
        var view = _openPopUps[type];
        _openPopUps.Remove(type);
        Destroy(view.gameObject);
    }
}