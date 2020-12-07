using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ButtonsIHUD : MonoBehaviour
{
    public InventoryMovement IM;
	public void Button_Click()
	{
        IM.Clicked(Input.mousePosition.x, Input.mousePosition.y);
	}
}
