namespace MyApp.ViewModels

open ReactiveElmish
open ReactiveElmish.Avalonia

// this view model tracks only the main window state
type MainWindowViewModel(root: CompositionRoot) =
    inherit ReactiveElmishViewModel()

    member this.ContentView =
        // root.GetView<CounterViewModel>()
        this.BindOnChanged(App.app, (fun m -> m.View), (fun m -> root.GetView<ContentViewModel>()))
    //         app,
    //         _.View,
    //         fun m ->
    //             match m.View with
    //             | TodoListView -> root.GetView<TodoListViewModel>()
    //             | CounterView -> root.GetView<CounterViewModel>()
    //             | AboutView -> root.GetView<AboutViewModel>()
    //             | ChartView -> root.GetView<ChartViewModel>()
    //             | FilePickerView -> root.GetView<FilePickerViewModel>()
    //     )

    // member this.ShowTodoList() = app.Dispatch(SetView TodoListView)
    // member this.ShowChart() = app.Dispatch(SetView ChartView)
    // member this.ShowCounter() = app.Dispatch(SetView CounterView)
    // member this.ShowAbout() = app.Dispatch(SetView AboutView)
    // member this.ShowFilePicker() = app.Dispatch(SetView FilePickerView)
    static member DesignVM = new MainWindowViewModel(Design.stub)

