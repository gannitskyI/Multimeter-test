using UnityEngine;

public class MultimeterController : MonoBehaviour
{
    [SerializeField] private MultimeterView view;

    private MultimeterModel model;
    private bool isKnobActive = false;
    private float knobRotationSpeed = 2f;

    private void Start()
    {
        model = new MultimeterModel();
    }

    private void Update()
    {
        HandleKnobHover();
        HandleMouseScroll();
        UpdateDisplay();
    }

    private void HandleKnobHover()
    {
        if (IsCursorOverKnob() && !isKnobActive)
        {
            isKnobActive = true;
            view.HighlightKnob(true);
        }
        else if (!IsCursorOverKnob() && isKnobActive)
        {
            isKnobActive = false;
            view.HighlightKnob(false);
        }
    }

    private bool IsCursorOverKnob()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        return Physics.Raycast(ray, out hit) && hit.collider.gameObject == view.Knob;
    }

    private void HandleMouseScroll()
    {
        if (isKnobActive && Input.mouseScrollDelta.y != 0)
        {
            model.SwitchMode();
            RotateKnobToMode();
        }
    }

    private void UpdateDisplay()
    {
        if (model != null && view != null)
        {
            float value = model.Calculate();
            view.UpdateDisplay(value, model.CurrentMode);
        }
    }

    private void RotateKnobToMode()
    {
        int modeIndex = (int)model.CurrentMode;
        float targetAngle = view.GetKnobAngle(modeIndex);
        view.SetKnobPosition(targetAngle, knobRotationSpeed);
    }
}
