using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager
{
    private int _order = 10;

    private Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    private UI_Scene _sceneUI = null;

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject("@UI_Root");
            return root;
        }
    }

    public void SetCanvas(GameObject go, bool sort = true, bool world = false)
    {
        if (world)
        {
            Set3DCanvas(go);
            return;
        }

        Canvas canvas = go.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        //canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    private void Set3DCanvas(GameObject go)
    {
        Canvas canvas = go.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;
    }

    public T Show3DWorldUI<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.RM.Instantiate($"UI/3D/{name}");
        T world3D = go.GetOrAddComponent<T>();

        if (parent == null)
            parent = Root.transform;
        go.transform.SetParent(parent);

        Set3DCanvas(go);

        return world3D;
    }

    public T ShowItemUI<T>(Transform transform, string name = null) where T : UI_Item
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.RM.Instantiate($"UI/Item/{name}");
        T item = go.GetOrAddComponent<T>();

        go.transform.SetParent(transform);

        return item;
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.RM.Instantiate($"UI/Scene/{name}");
        T scene = go.GetOrAddComponent<T>();
        _sceneUI = scene;

        go.transform.SetParent(Root.transform);

        return scene;
    }

    public T ShowPopupUI<T>(string name = null, bool isStack = true) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.RM.Instantiate($"UI/Popup/{name}");
        T popup = go.GetOrAddComponent<T>();
        if (isStack)
            _popupStack.Push(popup);

        go.transform.SetParent(Root.transform);

        return popup;
    }

    public void ClosePopupUI(UI_Popup popup)
    {
        if (_popupStack.Count == 0)
            return;

        if (_popupStack.Peek() != popup)
        {
            Debug.Log("Failed to Close Popup.");
            return;
        }

        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;

        UI_Popup popup = _popupStack.Pop();

        Object.Destroy(popup.gameObject);
        _order--;
    }

    public void CloseAllPopup()
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }

    public void Clear()
    {
        CloseAllPopup();
        _sceneUI = null;
    }
}
