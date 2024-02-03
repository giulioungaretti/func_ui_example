namespace App

open Avalonia.FuncUI.DSL
open DataExchange.Drivers
open ComponentsView
open Avalonia.Controls
open Avalonia.Layout
open Elmish

module MainWindow =

    type State =
        { connecting: bool
          components: List<Driver> }

    let init () =
        ({ connecting = false
           components = [ createDriver (new ExampleTool()); createDriver (new ExampleTool()) ] },
         Cmd.none)

    type Msg =
        | Uppityy of int
        | Connect
        | OnConnect
        | Connected
    // we collect a bunch of task and perform them, and when all done  send a message back
    // the taks mutate state of the objecst
    let toAsync (d: Driver) =
        Async.AwaitTask(d.driver.ConnectAsync())

    let connectAll (components: List<Driver>) =
        async {
            let! _ = List.map (toAsync) components |> Async.Parallel
            ()
        }

    let update (msg: Msg) (state: State) : (State * Cmd<Msg>) =
        match msg with
        | Uppityy i ->
            let newState =
                { state with
                    components = List.mapi (fun j c -> if j = i then { c with loading = false } else c) state.components }

            match List.filter (fun c -> c.loading) newState.components with
            | [] -> newState, Cmd.ofMsg Connected
            | _ -> newState, Cmd.none
        | Connect -> ({ state with connecting = true }, Cmd.ofMsg (OnConnect))
        | OnConnect ->
            ({ state with
                components = List.map (fun c -> { c with loading = true }) state.components },
             Cmd.none)
        | Connected -> ({ state with connecting = false }, Cmd.none)


    let subscribe (model: State) : (SubId * Subscribe<Msg>) list =
        List.mapi (mkSub Uppityy) model.components |> Sub.batch


    let viewCounter (state: State) (dispatch) =
        DockPanel.create
            [ DockPanel.children
                  [
                    // bottom
                    Border.create
                        [ Border.cornerRadius 5
                          Border.padding 5
                          Border.margin 5
                          Border.borderThickness 1
                          Border.dock Dock.Bottom
                          //Border.background "red"
                          Border.child (
                              if state.connecting then
                                  Button.create
                                      [ Button.content "..connecting.."
                                        Button.isEnabled false
                                        Button.horizontalAlignment HorizontalAlignment.Center ]
                              else
                                  Button.create
                                      [ Button.onClick (fun _ -> dispatch Connect)
                                        Button.content "Connect"
                                        Button.horizontalAlignment HorizontalAlignment.Center ]
                          ) ]
                    // rest
                    Border.create
                        [ Border.cornerRadius 5
                          Border.padding 5
                          Border.margin 5
                          Border.borderThickness 1
                          //Border.background "yellow"
                          Border.child (
                              StackPanel.create
                                  [ StackPanel.orientation Orientation.Vertical
                                    StackPanel.children (List.map (fun diver -> view diver dispatch) state.components) ]
                          ) ] ] ]

    let view (state: State) (dispatch) =
        TabControl.create
            [ TabControl.tabStripPlacement Dock.Left
              TabControl.viewItems
                  [ TabItem.create [ TabItem.header "Counter"; TabItem.content (viewCounter state dispatch) ] ] ]
