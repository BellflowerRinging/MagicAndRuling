using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapMenuContorl : ActionList
{
    public void Show(Vector3 pos)
    {
        Show();
        transform.position = pos;
    }

}
