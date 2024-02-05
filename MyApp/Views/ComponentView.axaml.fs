namespace MyApp.Views

open Avalonia
open Avalonia.Controls
open Avalonia.Markup.Xaml

type ComponentView () as this = 
    inherit UserControl ()

    do this.InitializeComponent()

    member private this.InitializeComponent() =
        AvaloniaXamlLoader.Load(this)
