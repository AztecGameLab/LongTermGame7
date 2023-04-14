using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Yarn.Unity;

public class ContinueButton : MonoBehaviour, IPointerClickHandler
{
    public LineView lineView;

    private void Start()
    {
        lineView = GetComponent<LineView>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            lineView.OnContinueClicked();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        lineView.OnContinueClicked();
    }
}
