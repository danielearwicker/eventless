# eventless

*Functional Reactive Programming for .NET and WPF/XAML*

# Installation

    Install-Package Eventless

# Documentation

The assembly, `Eventless.dll`, is fully documented for Visual Studio auto-completion.

[Browse the documentation](http://earwicker.com/eventless/)

# Quick Start

All the facilities are in the `Eventless` namespace:

```csharp
using Eventless;
```

You can create a model that mixes mutable data and computed (immutable) data like this:

```csharp
public class Person
{
    public IMutable<string> FirstName { get; } = Mutable.From(string.Empty);
    public IMutable<string> LastName { get; } = Mutable.From(string.Empty);
    
    public IImmutable<string> FullName { get; }

    public Person()
    {
        FullName = Computed.From(() => $"{FirstName.Value} {LastName.Value}");
    }
}
```

This can then be used as the view model of a simple window (note that you have to bind to the `Value` property in the binding expressions).

```xaml
<Window x:Class="WpfApplication1.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:WpfApplication1"
        mc:Ignorable="d"
        Title="MainWindow" Height="350" Width="525">
    
    <Window.DataContext>
        <local:Person/>
    </Window.DataContext>
    
    <Grid Margin="8">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="Auto"/>
            <ColumnDefinition Width="*"/>
        </Grid.ColumnDefinitions>
        
        <Label Grid.Row="0" Grid.Column="0">First name</Label>
        <TextBox Grid.Row="0" Grid.Column="1" 
                 Text="{Binding FirstName.Value, UpdateSourceTrigger=PropertyChanged}"/>
           
        <Label Grid.Row="1" Grid.Column="0">Last name</Label>
        <TextBox Grid.Row="1" Grid.Column="1" 
                 Text="{Binding LastName.Value, UpdateSourceTrigger=PropertyChanged}"/>

        <Label Grid.Row="2" Grid.Column="0">Full name</Label>
        <Label Grid.Row="2" Grid.Column="1" 
               Content="{Binding FullName.Value}"/>
    </Grid>
</Window>
```

# Compatibility

At the moment `Eventless` is built for old-school WPF as a .NET 4.5 assembly. But I'm interested in seeing how it fits with Universal Windows Platform. Anyone with experience of that platform is encouraged to investigate and send me a pull request!

# MIT License

https://opensource.org/licenses/MIT

Copyright (c) 2013-2016 Daniel Earwicker <dan@earwicker.com>
