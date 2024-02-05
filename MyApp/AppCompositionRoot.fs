namespace MyApp

open ReactiveElmish.Avalonia
open MyApp.ViewModels
open MyApp.Views

type AppCompositionRoot() =
    inherit CompositionRoot()

    let mainView = MainWindow()

    override _.RegisterServices services = base.RegisterServices(services) // Auto-registers view models

    override _.RegisterViews() =
        Map [ VM.Key<MainWindowViewModel>(), View.Singleton(mainView)
              VM.Key<ContentViewModel>(), View.Singleton<ContentView>()
               ]
