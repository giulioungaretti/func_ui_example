namespace CounterApp

open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.FuncUI.Elmish
open Avalonia.Themes.Fluent
open Avalonia.FuncUI.Hosts
open Elmish


type MainWindow() as this =
    inherit HostWindow()

    do
        base.Title <- "Counter Example"

        Elmish.Program.mkProgram Counter.init Counter.update Counter.view
        |> Program.withSubscription Counter.subscribe
        |> Program.withHost this
        |> Program.withConsoleTrace
        |> Program.runWithAvaloniaSyncDispatch ()
//|> Program.run


type App() =
    inherit Application()

    override this.Initialize() = this.Styles.Add(FluentTheme())
    // this.RequestedThemeVariant <- Styling.ThemeVariant.Dark

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime -> desktopLifetime.MainWindow <- MainWindow()
        | _ -> ()

module Program =

    [<EntryPoint>]
    let main (args: string[]) =
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)
