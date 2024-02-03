namespace App

open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.FuncUI.Elmish
open Avalonia.Themes.Fluent
open Avalonia.FuncUI.Hosts
open Elmish


type Window() as this =
    inherit HostWindow()

    do
        base.Title <- "Func UI Example"

        Elmish.Program.mkProgram MainWindow.init MainWindow.update MainWindow.view
        |> Program.withSubscription MainWindow.subscribe
        |> Program.withHost this
        |> Program.withConsoleTrace
        |> Program.runWithAvaloniaSyncDispatch ()


type App() =
    inherit Application()

    override this.Initialize() = this.Styles.Add(FluentTheme())
    // TODO: configure based on os preference
    // this.RequestedThemeVariant <- Styling.ThemeVariant.Dark

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime -> desktopLifetime.MainWindow <- Window()
        | _ -> ()

module Program =

    [<EntryPoint>]
    let main (args: string[]) =
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)
