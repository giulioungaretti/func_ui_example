namespace MyApp.ViewModels

open ReactiveElmish
open Elmish
open System.Threading.Tasks

module ContentStore =
    type State =
        { Connecting: bool
          Components: List<ComponentViewModel> }

    let init () =
        ({ Connecting = false
           Components = [ 
             new ComponentViewModel(Tool.Dummytool())
             new ComponentViewModel(Tool.Dummytool())
            ] },
         Cmd.none)

    type Msg =
        | Connect
        | Connected

    let connectAll (components: List<ComponentViewModel>) =
        let tasks:List<Task> = List.map (fun (c: ComponentViewModel) -> c.Connect ()) components
        task {
            do!  Task.WhenAll(tasks)
        }

    let update (msg: Msg) (state: State) : (State * Cmd<Msg>) =
        match msg with
        | Connect -> ({ state with Connecting = true }, Cmd.none)
        | Connected -> ({ state with Connecting = false }, Cmd.none)


open ContentStore
open ReactiveElmish.Avalonia

type ContentViewModel() =
    inherit ReactiveElmishViewModel()
    let local = 
        Program.mkAvaloniaProgram init update
        |> Program.mkStore


    member this.Componentlist = this.BindList(local, _.Components)

    member this.ConnectIsEnabled = this.Bind(local, fun s -> not s.Connecting)
 
    member this.ConnectAll () = 
        task {
            local.Dispatch Connect
            do! connectAll local.Model.Components
            local.Dispatch Connected
        }
    static member DesignVM = new ContentViewModel()
