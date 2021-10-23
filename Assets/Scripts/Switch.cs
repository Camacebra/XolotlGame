using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Switch : MonoBehaviour
{
    [SerializeField] private UnityEvent activate;
    [SerializeField] private GameObject prompt;

    private PlayerActions player;
    private int index = 0;
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
        activate.Invoke();
        index = 1 - index;
        Helpers.AudioManager.instance.PlayClip("switch" + index.ToString());
        
    }

    public void EnablePrompt()
    {
        prompt.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == player.name)
        {
            prompt.SetActive(false);
        }
    }

}