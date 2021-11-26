using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public Transform antHolder;

    public TextMeshProUGUI antText;
    public TextMeshProUGUI dirtText;

    public float numberOfAnts = 0;
    public float numberOfDirt = 0;

    public void addAnt()
    {
        numberOfAnts += 1;
        antText.text = "Ants: " + numberOfAnts;
    }

    public void addDirt()
    {
        numberOfDirt += 1;
        dirtText.text = "Dirt: " + numberOfDirt;
    }
}
