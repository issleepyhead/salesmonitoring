Public Interface IObservablePanel

    ''' <summary>
    ''' Register the observer to the observable.
    ''' </summary>
    ''' <param name="o">An IObserverPanel to register in the observable.</param>
    Sub RegisterObserver(o As IObserverPanel)

    ''' <summary>
    ''' Notifies all the observes that are listed in observable.
    ''' </summary>
    Sub NotifyObserver()

End Interface
