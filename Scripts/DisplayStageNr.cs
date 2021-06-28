using UnityEngine.UI;
using UnityEngine;

public class DisplayStageNr : MonoBehaviour
{
    public Text stageNr;

    void Update()
    {
        stageNr.text = "Poziom " + FindObjectOfType<Collision>().newStageNr;
    }
}
