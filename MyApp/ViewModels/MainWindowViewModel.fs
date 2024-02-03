namespace MyApp.ViewModels

open ReactiveElmish

type MainViewModel() =
    inherit ReactiveElmishViewModel()

    member this.Version = "v1.0"

    static member DesignVM = new MainViewModel()
