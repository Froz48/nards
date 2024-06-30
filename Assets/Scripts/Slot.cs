using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    private List<Pawn> pawns = new List<Pawn>();
    private bool flag = false;
    private bool growUp = true;
    [SerializeField] private int slotNumber;

    private void OnMouseUp()
    {
        if (flag)
        {
            GetUpPawn().OnMouseUp();
        }
        flag = false;
        Game.Instance.SetDiceTexts();

    }

    private void OnMouseDown()
    {

        if ((GetUpPawn() != null) && (Game.Instance.turn == GetUpPawn().GetPlayer()))
        {
            GetUpPawn().OnMouseDownFake();
            flag = true;
        }
    }

    public void GetSlotNumber(int n)
    {
        slotNumber = n;
    }

    public int GetSlotNumber()
    {
        return slotNumber;
    }

    public void SetGrowUp(bool b)
    {
        this.growUp = b;
    }

    public float GetGrowCoeff()
    {
        if (this.growUp)
            return 1f;
        else
            return -1f;
    }

    public void AddPawn(Pawn p)
    {
        Vector3 vtmp= this.transform.position;
        pawns.Add(p);
        p.SetSlot(this);
        vtmp.y += //подвинуть относительно цетра слота
            GetGrowCoeff()*-1f*this.transform.localScale.y/2f
            + GetGrowCoeff()*this.GetPawnCount()* p.transform.localScale.y / 2; 
        vtmp.z = 1;
        p.transform.position = vtmp;
    }

    public void RemovePawn(Pawn p)
    {
        if (pawns.Contains(p))
            pawns.Remove(p);
    }

    public int GetPawnCount()
    {
        return pawns.Count;
    }

    public Pawn GetUpPawn()
    {
        if (pawns.Count != 0)
        {
            return pawns[pawns.Count - 1];
        }
        else return null;
    }
}
