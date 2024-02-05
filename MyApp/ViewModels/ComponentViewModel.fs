namespace MyApp.ViewModels

open DataExchange.Drivers
open ReactiveElmish
open ReactiveElmish.Avalonia
open Elmish

module Tool=    
    type Dummytool(name, status) =
        new() = Dummytool("ToolName", "Connected")
        
        member val Connected = status;
        member val Name = name;

        interface IDriver with
            member this.ConnectAsync() = 
                task {
                    let interval = System.Random().Next(5000)
                    do! System.Threading.Tasks.Task.Delay(interval)
                }
            member this.Connected = failwith "todo"
            member this.Disconnect() = failwith "todo"
            member this.Name = this.Name
            member this.Status = this.Connected

    type Status = 
        | Connecting 
        | Errored 
        | NotConnected
        | Connected

    type Model = 
        {
            Driver: IDriver
            Connected: bool
            Status: Status
        }

    type Msg = 
        | Connect
        | Connected
        | Disconnect

    let init (driver:IDriver) () = 
        { 
            Driver=driver
            Connected=false
            Status=NotConnected
        }

    let update (msg: Msg) (model: Model) = 
        match msg with
        | Connected -> 
            {model with Status=Status.Connected}
        | Connect -> 
            {model with Status=Status.Connecting}
        | Disconnect -> 
            {model with Status=Status.NotConnected}

open Tool

type ComponentViewModel(driver: IDriver) =
    inherit ReactiveElmishViewModel()

    let local = 
        Program.mkAvaloniaSimple (init driver) update
            |> Program.withErrorHandler (fun (_, ex) -> printfn $"Error: {ex.Message}")
            |> Program.withConsoleTrace
            |> Program.mkStore

    member this.Driver = driver
    member this.Name = driver.Name
    member this.Status = this.Bind (local, _.Status)

    member this.EanbleConnectButton = this.Bind (local,fun s -> s.Status <> Connecting)

    member this.Connect () =
        task {
            local.Dispatch(Connect)
            do! this.Driver.ConnectAsync ()
            local.Dispatch(Connected)
        }

    static member DesignVM = new ComponentViewModel(Dummytool())
        

