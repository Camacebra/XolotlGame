using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Switch : MonoBehaviour
{
    [SerializeField] private GameObject prompt;
    [SerializeField] private UnityEvent enable, disable;

    private PlayerActions player;
    private bool State { get; set; }
    void Start()
    {
        State = false;
        player = GameObject.Find("Player").GetComponent<PlayerActions>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Activate()
    {
        State = !State;
        if (State)
            enable.Invoke();
        else
            disable.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.name == player.name && !player.HasItem)
        {
            prompt.SetActive(true);
            player.ChangeAction(4);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        prompt.SetActive(false);
    }

}