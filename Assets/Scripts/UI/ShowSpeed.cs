using TMPro;

public class ShowSpeed : OnBehaviour
{
    private HandlerSpeed _handlerSpeed;
    private TMP_Text _text;
    protected override void OnInitialize()
    {
        _text = GetComponent<TMP_Text>();
        _handlerSpeed = ServiceLocator.Instance.Get<HandlerSpeed>();
    }

    protected override void OnLateUpdate()
    {
        _text.text = _handlerSpeed.CurrentSpeed.ToString("F0") + "KM";
    }
}
