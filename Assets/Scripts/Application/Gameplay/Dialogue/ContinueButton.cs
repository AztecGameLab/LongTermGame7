using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class ContinueButton : MonoBehaviour
{
    public LineView lineView;
    // Start is called before the first frame update
    void Start()
    {
        lineView = GetComponent<LineView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            lineView.OnContinueClicked();
        }
    }
}
