namespace MyApp.ViewModels

open DataExchange.Drivers
open ReactiveElmish

        
type Dummytool(name, status) =
    new() = Dummytool("ToolName", "Connected")
    
    member val Connected = status;
    member val Name = name;

    interface IDriver with
        member this.ConnectAsync() = failwith "todo"
        member this.Connected = failwith "todo"
        member this.Disconnect() = failwith "todo"
        member this.Name = this.Name
        member this.Status = this.Connected

type ComponentViewModel(driver: IDriver) =
        inherit ReactiveElmishViewModel()
       
        member this.Driver = driver 
        static member DesignVM = new ComponentViewModel(Dummytool())
        

