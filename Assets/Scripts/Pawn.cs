using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    [SerializeField] private bool isDragging = false;
    [SerializeField] private int rightSpot = 0;
    private Vector3 curPosition;
    [SerializeField] private Slot newSlot;
    [SerializeField] private Slot curSlot;
    private bool player;

    [SerializeField] private Color colorSlotBase = new Color(237, 201, 201, 0.68f);
    [SerializeField] private Color colorSlotRight = new Color(95, 152, 94, 49);

    public bool GetPlayer()
    {
        return player;
    }
    public void SetPlayer(bool p)
    {
        this. player = p;
    }
    public void SetSlot(Slot s)
    {
        curSlot = s;
    }

    public void OnMouseDownFake()
    {
        curPosition = this.transform.position;
        isDragging = true;
        ShowAvaibleSlots();
    }

    public bool FindAvaibleSlots() {
        bool ans = false;
        foreach (var i in Game.Instance.Slots.GetComponentsInChildren<Slot>())
        {
            if (IsRightSlot(i) != 0)
            {
                ans = true;
            }
        }
        return ans;
    }

    public void ShowAvaibleSlots()
    {/// returns 1 if found any
        
        foreach (var i in Game.Instance.Slots.GetComponentsInChildren<Slot>())
        {
            if (IsRightSlot(i) != 0)
            {
                i.GetComponentInChildren<SpriteRenderer>().color = new Color(0.706f, 0.961f, 0.757f, 0.25f);
                
            }
        }
    }

    public void OnMouseUp()
    {
        foreach (var i in Game.Instance.Slots.GetComponentsInChildren<Slot>())
        {
            i.GetComponentInChildren<SpriteRenderer>().color = new Color(0.961f, 0.808f, 0.706f, 0f);
        }

        if (rightSpot != 0)
        {

            curSlot.RemovePawn(this);
            newSlot.AddPawn(this);

            if (rightSpot == 1)
                Game.Instance.cube1 = 0;
            else if (rightSpot == 2)
                Game.Instance.cube2 = 0;
            else if (rightSpot == 3)
            {
                Game.Instance.cube1 = 0;
                Game.Instance.cube2 = 0;
            }
            
        } else
        {
            transform.position = curPosition;
        }
        isDragging = false;
        rightSpot = 0;
        if ((Game.Instance.cube1 == 0) && (Game.Instance.cube2 == 0))
            Game.Instance.EndTurn();
        else Game.Instance.AutoEndTurn();
    }

    void Update()
    {
        if (this.isDragging)
        {
            Vector3 cursorCoords= Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -1));
            cursorCoords.z = -1;
            transform.position = cursorCoords;
        }
    }   

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Slot"))
        {
            rightSpot = 0;
        }
    }

    public void Touch()
    {
        this.OnMouseDownFake();
        this.OnMouseUp();
    }

    public float GetPlayerCoeff()
        ///1 = 1; 0 = -1
    {
        if (this.player)
            return 1f;
        else 
            return -1f;
    }

    private int IsRightSlot(Slot slotCheck)
        ///s1 - start position
    {
        //если слот занят другим игроком
        if (slotCheck.GetUpPawn() != null && slotCheck.GetUpPawn().player != player)
            return 0;
        //если это тот же слот
        if (this.curSlot.GetSlotNumber() == slotCheck.GetSlotNumber())
            return 0;

        if (slotCheck.GetSlotNumber() == this.curSlot.GetSlotNumber() + Game.Instance.cube1 * GetPlayerCoeff())
            return 1;
        if (slotCheck.GetSlotNumber() == this.curSlot.GetSlotNumber() + Game.Instance.cube2 * GetPlayerCoeff())
            return 2;
        if (slotCheck.GetSlotNumber() == this.curSlot.GetSlotNumber() + Game.Instance.cube1 * GetPlayerCoeff() + Game.Instance.cube2 * GetPlayerCoeff())
            return 3;
        return 0;
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Slot"))
        {
            if (
                (collision.gameObject.GetComponent<Slot>() != curSlot) 
                && //либо пустой слот либо слот со своими пешками
                (collision.gameObject.GetComponent<Slot>().GetUpPawn() == null) 
                || (collision.gameObject.GetComponent<Slot>().GetUpPawn().player == player))
            {
                Debug.Log("Color = " + collision.gameObject.GetComponentInChildren<SpriteRenderer>().color);
                rightSpot = IsRightSlot(collision.gameObject.GetComponent<Slot>());
                newSlot = collision.gameObject.GetComponent<Slot>();
            }
        }
    }
}
