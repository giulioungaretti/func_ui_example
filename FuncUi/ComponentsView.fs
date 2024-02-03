namespace App

open DataExchange.Drivers
open Avalonia.FuncUI.DSL
open Avalonia.Controls
open System
open Avalonia

module ComponentsView =
    open Avalonia.Layout
    open Avalonia.Media

    type Driver = { loading: bool; driver: IDriver }

    let createDriver (driver: IDriver) : Driver = { loading = false; driver = driver }

    // create an effect that runs the connection and dispatcing togheter in  the thread pool
    let mkeffect (driver: IDriver) (tag: 'msg) =

        let effectFn dispatch =
            async {
                do! Async.AwaitTask(driver.ConnectAsync())
                dispatch (tag)
            }
            |> Async.Start

            // do nothing IDispose, we rely on the mkSub below to dispose of the subscription
            { new IDisposable with
                member _.Dispose() = () }

        effectFn


    // create a sub that is on only if the dirver is in loading mode
    let mkSub (tag: 'a -> 'msg) (i: 'a) (driver: Driver) =
        [ if driver.loading then
              [ driver.driver.Name ], mkeffect driver.driver (tag i) ]

    let view state _ =
        Border.create
            [ Border.cornerRadius 5
              Border.padding 5
              Border.margin 5
              Border.borderThickness 1
              Border.borderBrush "black"

              Border.child (
                  StackPanel.create
                      [ StackPanel.orientation Orientation.Vertical
                        StackPanel.horizontalAlignment Layout.HorizontalAlignment.Left
                        StackPanel.children
                            [
                              // name
                              TextBlock.create
                                  [ TextBlock.text state.driver.Name
                                    TextBlock.horizontalAlignment HorizontalAlignment.Left
                                    TextBlock.verticalAlignment VerticalAlignment.Center ]
                              SelectableTextBlock.create
                                  [ TextBlock.text state.driver.Status
                                    TextBlock.horizontalAlignment HorizontalAlignment.Left
                                    TextBlock.verticalAlignment VerticalAlignment.Center
                                    TextBlock.textWrapping TextWrapping.Wrap ] ] ]
              ) ]
