using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PressureGUI : MonoBehaviour
{
    public Text label;
    [FormerlySerializedAs("PressureController")] public PressureController pressureController;

    public void LateUpdate()
    {
        label.text = pressureController.CurrentMass.ToString(CultureInfo.CurrentCulture);
        label.color = pressureController.CurrentMass >= pressureController.threshold ? Color.green : Color.red;
    }
}
