using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;

public class Signals : ISignals
{
    public IReadOnlyReactiveProperty<EnterStateSignal> EnterStateSignal => _enterStateSignal;
    private readonly ReactiveProperty<EnterStateSignal> _enterStateSignal = new ReactiveProperty<EnterStateSignal>() { };

    public async void NotifyEnterStateSignal(EnterStateSignal signal)
    {
        await Task.Yield();
        _enterStateSignal.SetValueAndForceNotify(signal);
    }

   
}
