using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dices : MonoBehaviour
{
    private void OnMouseDown()
    {
        Game.Instance.AutoEndTurn();
    }
}
