using Avalonia.Controls;
using Avalonia.Controls.PanAndZoom;
using Avalonia.Input;
using CommunityToolkit.Mvvm.Input;
using TelltaleTextureTool.ViewModels;
using System;

namespace TelltaleTextureTool.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();

        DataContext = new MainViewModel();

        var viewModel = (MainViewModel)DataContext;
        viewModel.ResetPanAndZoomCommand = new RelayCommand(ResetPanAndZoom);

        DataContextChanged += OnDataContextChanged;
        viewModel.ResetPanAndZoomCommand.Execute(null);
        ResetPanAndZoom();
    }

    private void ResetPanAndZoom()
    {
        // Assuming zoomBorder is the name of your ZoomBorder control
        ZoomBorder1.ResetMatrix();
    }

    private void ZoomBorder_KeyDown(object? sender, KeyEventArgs e)
    {
        var zoomBorder = this.DataContext as ZoomBorder;

        switch (e.Key)
        {
            case Key.F:
                zoomBorder?.Fill();
                break;
            case Key.U:
                zoomBorder?.Uniform();
                break;
            case Key.R:
                zoomBorder?.ResetMatrix();
                break;
            case Key.T:
                zoomBorder?.ToggleStretchMode();
                zoomBorder?.AutoFit();
                break;
        }
    }

    private void OnDataContextChanged(object sender, EventArgs e)
    {
        if (DataContext is MainViewModel viewModel)
        {

        }
    }


    private void Binding_1(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
    }

    private void PreviewImageCommand_1(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
    }

    private void Binding(object? sender, Avalonia.Controls.Primitives.RangeBaseValueChangedEventArgs e)
    {
    }

    private void PreviewImageCommand_1(object? sender, Avalonia.Controls.Primitives.RangeBaseValueChangedEventArgs e)
    {
    }

    private void PreviewImageCommand(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }
}