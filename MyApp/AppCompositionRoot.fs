namespace MyApp

open ReactiveElmish.Avalonia
open Microsoft.Extensions.DependencyInjection
open MyApp.ViewModels
open MyApp.Views

type AppCompositionRoot() =
    inherit CompositionRoot()

    let mainView = MainWindow()

    override this.RegisterServices services = base.RegisterServices(services) // Auto-registers view models
    // Add any additional services

    override this.RegisterViews() =
        Map [ VM.Key<MainViewModel>(), View.Singleton(mainView) ]
