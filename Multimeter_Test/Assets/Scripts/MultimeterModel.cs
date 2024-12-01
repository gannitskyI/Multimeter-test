using UnityEngine;

public class MultimeterModel
{
    public const float Resistance = 1000f;
    public const float Power = 400f;

    public enum Mode
    {
        Off,
        Resistance,
        Current,
        VoltageDC,
        VoltageAC
    }

    public Mode CurrentMode { get; private set; }

    public MultimeterModel()
    {
        CurrentMode = Mode.Off;
    }

    public float Calculate()
    {
        switch (CurrentMode)
        {
            case Mode.Resistance:
                return Resistance;
            case Mode.Current:
                return Mathf.Sqrt(Power / Resistance);
            case Mode.VoltageDC:
                return Mathf.Sqrt(Power * Resistance);
            case Mode.VoltageAC:
                return 0.01f;
            case Mode.Off:
            default:
                return 0;
        }
    }

    public void SwitchMode()
    {
        CurrentMode = (Mode)(((int)CurrentMode + 1) % 5);
    }
}
