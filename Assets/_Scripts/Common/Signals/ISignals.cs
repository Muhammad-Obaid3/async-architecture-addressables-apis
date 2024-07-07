using UniRx;
public interface ISignals
{
    public IReadOnlyReactiveProperty<EnterStateSignal> EnterStateSignal { get; }

    public void NotifyEnterStateSignal(EnterStateSignal signal);
}
