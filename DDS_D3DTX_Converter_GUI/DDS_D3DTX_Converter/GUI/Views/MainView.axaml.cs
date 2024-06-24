using Avalonia.Controls;
using Avalonia.Controls.PanAndZoom;
using Avalonia.Input;
using CommunityToolkit.Mvvm.Input;
using DDS_D3DTX_Converter.ViewModels;
using System;
using System.Diagnostics;

namespace DDS_D3DTX_Converter.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();

        DataContext = new MainViewModel();

        var viewModel = (MainViewModel)DataContext;
        viewModel.ResetPanAndZoomCommand = new RelayCommand(ResetPanAndZoom);

        DataContextChanged += OnDataContextChanged;
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
}