using UnityEngine;


public class UIScriptCreator : MonoBehaviour
{
    public string moduleName = "";
    public bool isListItem = false;
    public bool isMarked = false;

    public void Mark(bool isMark)
    {
        isMarked = isMark;
    }

    public void SetModuleName(string name)
    {
        moduleName = name;
    }

    public string GetModuleName()
    {
        return moduleName;
    }

    public void MarkAsItem()
    {
        isListItem = true;
    }

    public bool IsItem()
    {
        return isListItem;
    }
}

