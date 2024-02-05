namespace MyApp.ViewModels

open ReactiveElmish
open ReactiveElmish.Avalonia

type CounterViewModel(root: CompositionRoot) =
    inherit ReactiveElmishViewModel()