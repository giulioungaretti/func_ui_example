namespace MyApp

open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Markup.Xaml
open Avalonia.Controls

type App() =
    inherit Application()

    override this.Initialize() = AvaloniaXamlLoader.Load(this)

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktop ->
            let appRoot = AppCompositionRoot()
            desktop.MainWindow <- appRoot.GetView<ViewModels.MainWindowViewModel>() :?> Window
        | _ -> ()

        base.OnFrameworkInitializationCompleted()
