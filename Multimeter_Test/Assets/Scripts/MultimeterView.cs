using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class MultimeterView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private TextMeshProUGUI windowText;
    [SerializeField] private GameObject knob;

    private Color highlightColor = new Color(1f, 1f, 0f, 0.8f);
    private Color originalColor;
    private float[] knobAngles = { 0f, 160f, 45f, 335f, 250f };

    private Dictionary<MultimeterModel.Mode, string> modeDisplayText = new Dictionary<MultimeterModel.Mode, string>
    {
        { MultimeterModel.Mode.VoltageDC, "V {0}" },
        { MultimeterModel.Mode.VoltageAC, "~ {0}" },
        { MultimeterModel.Mode.Current, "A {0}" },
        { MultimeterModel.Mode.Resistance, "Ω {0}" },
        { MultimeterModel.Mode.Off, "0" }
    };

    private void Start()
    {
        originalColor = knob.GetComponent<Renderer>().material.color;
    }

    public GameObject Knob => knob; 

    public void UpdateDisplay(float value, MultimeterModel.Mode currentMode)
    {
        string valueWithUnit = currentMode != MultimeterModel.Mode.Off
            ? string.Format(modeDisplayText[currentMode], FormatValueForMode(value, currentMode))
            : "0";

        displayText.text = valueWithUnit;
 
        string vText = "V 0";
        string aText = "A 0";
        string omegaText = "Ω 0";
        string tildeText = "~ 0";

        if (currentMode == MultimeterModel.Mode.VoltageDC)
            vText = string.Format("V {0:F2}", value);
        else if (currentMode == MultimeterModel.Mode.VoltageAC)
            tildeText = string.Format("~ {0:F2}", value);
        else if (currentMode == MultimeterModel.Mode.Current)
            aText = string.Format("A {0:F2}", value);
        else if (currentMode == MultimeterModel.Mode.Resistance)
            omegaText = string.Format("Ω {0}", value.ToString("0")); 

        windowText.text = $"{vText}\n{aText}\n{tildeText}\n{omegaText}";
    }

    private string FormatValueForMode(float value, MultimeterModel.Mode mode)
    {
        if (mode == MultimeterModel.Mode.Resistance)
        {
            return value % 1 == 0 ? value.ToString("0") : value.ToString("F2");
        }
        return value.ToString("F2");
    }

    public void HighlightKnob(bool isActive)
    {
        if (isActive)
        {
            Color currentColor = knob.GetComponent<Renderer>().material.color;
            knob.GetComponent<Renderer>().material.color = Color.Lerp(currentColor, highlightColor, 0.5f);
        }
        else
        {
            knob.GetComponent<Renderer>().material.color = originalColor;
        }
    }

    public void SetKnobPosition(float angle, float rotationSpeed)
    {
        StopAllCoroutines();
        StartCoroutine(SmoothRotateKnob(angle, rotationSpeed));
    }

    public float GetKnobAngle(int modeIndex)
    {
        return knobAngles[modeIndex];
    }

    private IEnumerator SmoothRotateKnob(float targetAngle, float rotationSpeed)
    {
        Quaternion initialRotation = knob.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            knob.transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, elapsedTime);
            elapsedTime += Time.deltaTime * rotationSpeed;
            yield return null;
        }
        knob.transform.rotation = targetRotation;
    }
}
