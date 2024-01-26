module EffectThread

open Elmish
open DataExchange.Drivers
open System

let mkeffect (tag: 'msg) : Effect<'msg> =
    let effectFn dispatch = dispatch (tag)
    effectFn

let mkeffectA a (tag: 'a -> 'msg) : Effect<'msg> =
    let effectFn dispatch = dispatch (tag a)
    effectFn




module ToggleTimer =
    type Model = { current: IDriver; enabled: bool }

    type Msg =
        | Done
        | Toggle of enabled: bool

    let init () dirver =
        { current = dirver; enabled = false }, []

    let update msg model =
        match msg with
        | Done -> model, []
        | Toggle enabled -> { model with enabled = enabled }, []


    let mkeffect (driver: IDriver) (tag: 'msg) =
        Async.AwaitTask(driver.ConnectAsync()) |> Async.RunSynchronously

        let effectFn dispatch =
            dispatch (tag)

            { new IDisposable with
                member _.Dispose() = () }

        effectFn

    /// do nothing IDispose

    let mkSub model =
        [ if model.enabled then
              [ model.current.Name ], mkeffect model.current Done ]
