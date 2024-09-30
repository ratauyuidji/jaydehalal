using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructManager : MonoBehaviour
{
    [SerializeField] private GameObject instructPanel;
    [SerializeField] private GameObject player;

    private void Start()
    {
        if (instructPanel.activeSelf)
        {
            player.SetActive(false);
        }
    }
    public void TurnOnInstructionPanel()
    {
        instructPanel.SetActive(true);
        player.SetActive(false);
    }
    public void TurnOffInstructionPanel()
    {
        instructPanel.SetActive(false);
        player.SetActive(true);
    }
}
