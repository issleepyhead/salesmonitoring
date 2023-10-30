Public Interface IObservablePanel
    Sub RegisterObserver(o As IObserverPanel)
    Sub NotifyObserver()

End Interface
