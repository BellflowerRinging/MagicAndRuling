using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class TableInputFieldCell : TableCell
{
    public InputField InputField;
    public Image Image;
    public string Value
    {
        get { return InputField.text; }
        set { InputField.text = value; }
    }

    void Awake()
    {
        InputField = GetComponent<InputField>();
        Image = GetComponent<Image>();
    }
    void Start()
    {
    }

    public void SetOnEndEditAction(System.Action<TableCell> Action)
    {
        /*InputField.onEndEdit.AddListener((tvalue) =>
        {
            User user = UserManager.GetUserByLoginName(((TableInputFieldCell)Row.Cells[2]).Value);
            if (tvalue != user.m_UserName)
            {
                user.m_UserName = tvalue;
                UserManager.SaveAll();
            }
        });*/
        if (Action == null) return;
        InputField.onEndEdit.RemoveAllListeners();
        InputField.onEndEdit.AddListener((value) => Action(this));
    }

    public void SetRacastTarget(bool bo)
    {
        Image.raycastTarget = bo;
        InputField.textComponent.raycastTarget = bo;
    }
}