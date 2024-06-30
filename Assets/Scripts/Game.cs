using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public static Game Instance { get; set; }
    [SerializeField] GameObject pawn;
    [SerializeField] GameObject slot;
    [SerializeField] public GameObject Slots;
    [SerializeField] public int cube1;
    [SerializeField] public int cube2;
    public bool turn = true;
    ///1 - верх | 0 - низ

    const int N = 10;
    Vector3 startPos1 = new Vector3(122f, 42f, 0f);
    readonly private float difXSlot = -21.5f;
    readonly private float difBigYSlot = -85f;
    readonly private float difBigXSlot = -136f;

    [SerializeField] private Slot startSlot1;
    [SerializeField] private Slot startSlot0;
    [SerializeField] private GameObject diceText11;
    [SerializeField] private GameObject diceText12;
    [SerializeField] private GameObject diceText21;
    [SerializeField] private GameObject diceText22;
    
    public void AutoEndTurn() {
        ///завершает ход если нет возможных ходов
        foreach (var i in Game.Instance.Slots.GetComponentsInChildren<Slot>())
        {
            if (i.GetUpPawn() != null &&
                i.GetUpPawn().GetPlayer() == turn
                && i.GetUpPawn().FindAvaibleSlots())
            {
                Debug.Log("Found avaible turn from " + i.GetSlotNumber());
                return;
            }
        }
        Debug.Log("Timer started");
        StartCoroutine(PauseEndTurn(2.0f));
    }

    public void SetDiceTexts()
    {
        if (turn) {
            diceText11.GetComponent<Text>().text = cube1.ToString();
            diceText12.GetComponent<Text>().text = cube2.ToString();
            diceText21.GetComponent<Text>().text = "";
            diceText22.GetComponent<Text>().text = "";
        } else
        {
            diceText21.GetComponent<Text>().text = cube1.ToString();
            diceText22.GetComponent<Text>().text = cube2.ToString();
            diceText11.GetComponent<Text>().text = "";
            diceText12.GetComponent<Text>().text = "";
        }
    }

    public void EndTurn()
    {
        turn = !turn;
        Roll();
        SetDiceTexts();
        AutoEndTurn();
    }

    void SpawnSlots(Vector3 startPos, float xdif, bool growup, int startSlotNumber)
    {
        for (int i = startSlotNumber; i<startSlotNumber+6; i++)
        {
            GameObject g = Instantiate(slot, startPos, Quaternion.identity);
            startPos.x += xdif;
            g.GetComponent<Slot>().GetSlotNumber(i);
            g.GetComponent<Slot>().SetGrowUp(growup);

            if (i == 0) startSlot1 = g.GetComponent<Slot>();
            g.transform.SetParent(Slots.transform);
        }
    }

    void SpawnPawns(int count, Slot itsSlot, bool player)
    {
        for (int i = 0; i<count; i++)
        {
            GameObject g = Instantiate(pawn, new Vector3(), Quaternion.identity);
            itsSlot.AddPawn(g.GetComponent<Pawn>());//
            g.GetComponent<Pawn>().SetPlayer(player);
            if (player)
                g.GetComponentInChildren<SpriteRenderer>().color = Color.white;
            else
                g.GetComponentInChildren<SpriteRenderer>().color = Color.black;
        }
    }

    void Start()
    {
        Instance = this;
        SpawnSlots(startPos1,/* +  vector3 (0        0    0)*/ difXSlot, false, 0);
        SpawnSlots(startPos1 + new Vector3(difBigXSlot,  0,   0), difXSlot, false, 6);
        SpawnSlots(startPos1 + new Vector3(   0,    difBigYSlot, 0), difXSlot, true, 12);
        SpawnSlots(startPos1 + new Vector3(difBigXSlot, difBigYSlot, 0), difXSlot, true, 18);

        foreach (var i in FindObjectsOfType<Slot>())
        {
            if (i.GetSlotNumber() == 0)
                startSlot1 = i;
            if (i.GetSlotNumber() == 23)
                startSlot0 = i;
        }
        SpawnPawns(N, startSlot1, true);
        SpawnPawns(N, startSlot0, false);
        SetDiceTexts();
    }

    IEnumerator PauseEndTurn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        EndTurn();
    }

    public void Roll()
    {
        cube1 = Random.Range(1, 7);
        cube2 = Random.Range(1, 7);
        
    }
}
