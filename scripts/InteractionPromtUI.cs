using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionPromtUI : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private GameObject UIPanel;
    [SerializeField] private TextMeshProUGUI promt;
    public bool IsDisplayed = false;

    // Start is called before the first frame update
    void Start()
    {
        UIPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetUp(string promtText)
    {
        promt.text = promtText;
        UIPanel.SetActive(true);
        IsDisplayed = true;
    }
    public void Close()
    {
        UIPanel.SetActive(false);
        IsDisplayed = false;
    }
}
