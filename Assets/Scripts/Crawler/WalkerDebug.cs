using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WalkerDebug : MonoBehaviour
{
    [SerializeField] IKFootSolver _leftFoot;
    [SerializeField] IKFootSolver _rightFoot;
    [SerializeField] Text _leftOldPosition;
    [SerializeField] Text _leftCurrentPosition;
    [SerializeField] Text _leftNewPosition;
    [SerializeField] Text _rightOldPosition;
    [SerializeField] Text _rightCurrentPosition;
    [SerializeField] Text _rightNewPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChangeText();
    }

    void ChangeText()
    {
        _leftOldPosition.text = "Left Old Pos: " + _leftFoot.OldPosition.ToString();
        _leftCurrentPosition.text = "Left Current Pos: " + _leftFoot.CurrentPosition.ToString();
        _leftNewPosition.text = "Left New Pos: " + _leftFoot.NewPosition.ToString();

        _rightOldPosition.text = "Right Old Pos: " + _rightFoot.OldPosition.ToString();
        _rightCurrentPosition.text = "Right Current Pos: " + _rightFoot.CurrentPosition.ToString();
        _rightNewPosition.text = "Right New Pos: " + _rightFoot.NewPosition.ToString();
    }
}
