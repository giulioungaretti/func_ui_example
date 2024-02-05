namespace DataExchange.Drivers

open System
open System.IO.Ports

type IDriver =
    abstract member Name: string
    abstract member Status: string
    abstract member Connected: bool
    abstract member ConnectAsync: unit -> System.Threading.Tasks.Task
    abstract member Disconnect: unit -> unit

type Driver(name: string) =
    member val Error: Option<Exception> = None with get, set
    member val Connected = false with get, set
    member val Status = "Initalized" with get, set
    member val Name = name

    /// <summary>
    ///   Sets the status to "Errored with: {Error.Message}"
    /// </summary>
    member this.ErrorStatus() =
        this.Status <-
            sprintf
                "Errored with: %s"
                (this.Error
                 |> Option.map (fun e -> e.Message)
                 |> Option.defaultValue "Unknown error")


type SerialComDriver(name: string, baudRate: int, portName: string, address: int, readTimeout: int) =
    inherit Driver(name)

    let mutable serialPort =
        new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One)
    // let mutable connected = false
    // let mutable error = None
    member this.SerialPort = serialPort
    member this.Address = address
    member this.ReadTimeout = readTimeout

    member this.ConnectAsync() =
        async {
            serialPort.Open()
            this.Connected <- true
            this.Status <- "Connected"
        }
        |> Async.Catch
        |> Async.Ignore

    member this.Disconnect() =
        try
            serialPort.Close()
            this.Connected <- false
        with e ->
            this.Error <- Some e
            this.ErrorStatus()

type ExampleTool(baudRate: int, portName: string, address: int, readTimeout: int) =
    inherit SerialComDriver("HT61", baudRate, portName, address, readTimeout)
    new() = ExampleTool(9600, "COM4", 1, 10)

    interface IDriver with
        member this.ConnectAsync() = failwith "todo"
        member this.Connected = failwith "todo"
        member this.Disconnect() = failwith "todo"
        member this.Name = this.Name
        member this.Status = this.Status
