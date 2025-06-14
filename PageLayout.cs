using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

public static class PageLayout
{
    public static int pageWidth = 600;
    public static int pageHeight = 500;

    // Creation methods
    public static TextBlock CreateText(string text, int fontSize, int rowSpan, int columnSpan, string fontFamily)
    {
        TextBlock newText = new();
        newText.Text = text;
        newText.FontSize = fontSize;
        newText.Margin = new(20, 20, 20, 20);
        newText.TextWrapping = System.Windows.TextWrapping.Wrap;
        newText.FontFamily = new(fontFamily);

        Grid.SetColumnSpan(newText, columnSpan);
        Grid.SetRowSpan(newText, rowSpan);

        return newText;
    }
    public static System.Windows.Controls.Button CreateButton_Basic(string text)
    {
        System.Windows.Controls.Button newButton = new();
        newButton.Content = text;
        newButton.FontFamily = new("Roboto");
        newButton.Width = 70;
        newButton.Height = 25;

        Grid.SetColumnSpan(newButton, 1);
        Grid.SetRowSpan(newButton, 1);

        return newButton;
    }
    public static System.Windows.Controls.Button CreateButton_Custom(string text,int width, int height, int rowSpan, int columnSpan)
    {
        System.Windows.Controls.Button newButton = new();
        newButton.Content = text;
        newButton.FontFamily = new("Roboto");

        newButton.Width = width;
        newButton.Height = height;

        Grid.SetColumnSpan(newButton, columnSpan);
        Grid.SetRowSpan(newButton, rowSpan);

        return newButton;
    }
    public static System.Windows.Controls.ProgressBar CreateProgressBar()
    {
        System.Windows.Controls.ProgressBar progressBar = new();

        progressBar.Background = System.Windows.Media.Brushes.Gray;
        progressBar.Foreground = System.Windows.Media.Brushes.Blue;

        progressBar.Maximum = 100;
        progressBar.Minimum = 0;

        progressBar.Height = 20;

        progressBar.Margin = new Thickness(20, 20, 20, 20);

        progressBar.Value = 28;

        Grid.SetColumnSpan(progressBar, 4);
        Grid.SetRowSpan(progressBar, 1);

        return progressBar;
    }
    public static System.Windows.Controls.Image CreateImage(string imageName, int rowSpan, int columnSpan)
    {
        System.Windows.Controls.Image image = new();

        string source = System.IO.Path.GetFullPath("images/" + imageName + ".PNG");
        
        try
        {
            image.Source = new BitmapImage(new Uri(source));
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show("Error!" + ex.Message);
        }

        Grid.SetColumnSpan(image, columnSpan);
        Grid.SetRowSpan(image, rowSpan);

        return image;
    }

    // Placement methods
    public static void PlaceInLayout_Text(Grid grid, TextBlock textBlock, int row, int column)
    {
        Grid.SetRow(textBlock, row);
        Grid.SetColumn(textBlock, column);
        grid.Children.Add(textBlock);
    }
    public static void PlaceInLayout_Button(Grid grid, System.Windows.Controls.Button button, int row, int column)
    {
        Grid.SetRow(button, row);
        Grid.SetColumn(button, column);
        grid.Children.Add(button);
    }
    public static void PlaceInLayout_Image(Grid grid, System.Windows.Controls.Image image, int row, int column)
    {
        Grid.SetRow(image, row);
        Grid.SetColumn(image, column);
        grid.Children.Add(image);
    }
    public static void PlaceInLayout_ProgressBar(Grid grid, System.Windows.Controls.ProgressBar progressBar, int row, int column)
    {
        Grid.SetRow(progressBar, row);
        Grid.SetColumn(progressBar, column);
        grid.Children.Add(progressBar);
    }

    public static Grid CreateBasicGrid()
    {
        Grid newGrid = new();

        for (int i = 0; i < 6; i++)
        {
            RowDefinition row = new();
            ColumnDefinition column = new();

            newGrid.RowDefinitions.Add(row);
            newGrid.ColumnDefinitions.Add(column);
        }
        return newGrid;
    }
}