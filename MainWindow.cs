using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Forms;
using System.DirectoryServices;
using System.Windows.Media.Imaging;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Diagnostics;
using System.CodeDom;

namespace MyWpfApp
{
    public class MainWindow : Window
    {
        public void SwitchPage(System.Windows.Controls.UserControl newPage)
        {
            this.Content = newPage;
        }

        public MainWindow()
        {
            this.Title = "Game Installer";
            this.Width = 600;
            this.Height = 500;
            this.ResizeMode = ResizeMode.NoResize;

            var welcomePage = this.WelcomePage(this);
            this.Content = welcomePage;

            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }
        public System.Windows.Controls.UserControl WelcomePage(MainWindow mainWindow)
        {
            System.Windows.Controls.UserControl newPage = new System.Windows.Controls.UserControl();

            Grid grid = PageLayout.CreateBasicGrid();

            TextBlock header_Text = PageLayout.CreateText("Welcome to Game Installer", 20, 2, 4, "Roboto");
            PageLayout.PlaceInLayout_Text(grid, header_Text, 0, 2);

            TextBlock message_Text = PageLayout.CreateText("Thank You for installing the Game! If You liked it please consider recommending it to your friends if You have any!", 12, 2, 4, "Roboto");
            PageLayout.PlaceInLayout_Text(grid, message_Text, 1, 2);

            System.Windows.Controls.Button cancel_button = PageLayout.CreateButton_Basic("Cancel");
            PageLayout.PlaceInLayout_Button(grid, cancel_button, 5, 4);
            cancel_button.Click += (sender, e) => this.Close();

            System.Windows.Controls.Button next_button = PageLayout.CreateButton_Basic("Next");
            PageLayout.PlaceInLayout_Button(grid, next_button, 5, 5);
            next_button.Click += (sender, e) => SwitchPage(SecondPage(mainWindow));

            System.Windows.Controls.Image image = PageLayout.CreateImage("logo", 6, 2);
            PageLayout.PlaceInLayout_Image(grid, image, 0, 0);
            image.Stretch = Stretch.Fill;

            newPage.Content = grid;

            return newPage;
        }
        public System.Windows.Controls.UserControl SecondPage(MainWindow mainWindow)
        {
            System.Windows.Controls.UserControl newPage = new System.Windows.Controls.UserControl();

            Grid grid = PageLayout.CreateBasicGrid();
            string path = "";

            System.Windows.Controls.Image image = PageLayout.CreateImage("logo", 6, 2);
            PageLayout.PlaceInLayout_Image(grid, image, 0, 0);
            image.Stretch = Stretch.Fill;

            TextBlock header_Text = PageLayout.CreateText("Choose a folder to install the game", 20, 2, 4, "Roboto");
            PageLayout.PlaceInLayout_Text(grid, header_Text, 0, 2);

            System.Windows.Controls.Button cancel_button = PageLayout.CreateButton_Basic("Cancel");
            PageLayout.PlaceInLayout_Button(grid, cancel_button, 5, 4);
            cancel_button.Click += (sender, e) => this.Close();

            TextBlock pathIndicator = PageLayout.CreateText("Path: ", 12, 2, 4, "Roboto");
            PageLayout.PlaceInLayout_Text(grid, pathIndicator, 2, 2);

            System.Windows.Controls.Button choosePath = PageLayout.CreateButton_Custom("Choose Folder", 100, 25, 1, 2);
            PageLayout.PlaceInLayout_Button(grid, choosePath, 5, 2);
            choosePath.Click += (sender, e) => path = ChooseFolder(sender, e, pathIndicator);

            System.Windows.Controls.Button next_button = PageLayout.CreateButton_Basic("Install");
            PageLayout.PlaceInLayout_Button(grid, next_button, 5, 5);
            next_button.Click += (sender, e) => SwitchPage(ThirdPage(path, mainWindow));

            newPage.Content = grid;

            return newPage;
        }
        public static System.Windows.Controls.UserControl ThirdPage(string filePath, MainWindow mainWindow)
        {
            System.Windows.Controls.UserControl newPage = new System.Windows.Controls.UserControl();

            Grid grid = PageLayout.CreateBasicGrid();

            System.Windows.Controls.Image image = PageLayout.CreateImage("logo", 6, 2);
            PageLayout.PlaceInLayout_Image(grid, image, 0, 0);
            image.Stretch = Stretch.Fill;

            TextBlock header_Text = PageLayout.CreateText("Please wait while the game is being installed", 20, 2, 4, "Roboto");
            PageLayout.PlaceInLayout_Text(grid, header_Text, 0, 2);

            TextBlock progress_Text = PageLayout.CreateText("",12, 2, 4, "Roboto");
            PageLayout.PlaceInLayout_Text(grid, progress_Text, 1, 2);

            TextBlock status = PageLayout.CreateText("Status: DOWNLOADING GAME FILES", 12, 2, 4, "Roboto");
            PageLayout.PlaceInLayout_Text(grid, status, 2, 2);

            System.Windows.Controls.ProgressBar progressBar = PageLayout.CreateProgressBar();
            PageLayout.PlaceInLayout_ProgressBar(grid, progressBar, 3, 2);

            newPage.Loaded += async (sender, e) =>
            {
                try
                {
                    await DownloadFileAsync("https://drive.google.com/drive/folders/1UJc65Zvd8BaGcu6TK8lbr_zO5t9YnTo2?usp=drive_link", @filePath + "\\dummy.zip", progressBar,progress_Text);
                    // await DownloadFileAsync("https://www.learningcontainer.com/wp-content/uploads/2020/05/sample-large-zip-file.zip", @filePath + "\\dummy.zip", progressBar,progress_Text);
                    UnzipFile(@filePath + "\\dummy.zip", filePath, progressBar, status, mainWindow);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Filepath: " + filePath + "\n Outside Error:  " + ex.Message);
                }
            };

            newPage.Content = grid;
            return newPage;
        }
        public static System.Windows.Controls.UserControl FourthPage(string filePath)
        {
            System.Windows.Controls.UserControl newPage = new System.Windows.Controls.UserControl();
            Grid grid = PageLayout.CreateBasicGrid();

            System.Windows.Controls.Image image = PageLayout.CreateImage("logo", 6, 2);
            PageLayout.PlaceInLayout_Image(grid, image, 0, 0);
            image.Stretch = Stretch.Fill;

            TextBlock header_Text = PageLayout.CreateText("Installation Complete", 20, 2, 4, "Roboto");
            PageLayout.PlaceInLayout_Text(grid, header_Text, 0, 2);

            TextBlock message_Text = PageLayout.CreateText("Thank You for installing the Game! If You liked it please consider recommending it to your friends if You have any!", 12, 2, 4, "Roboto");
            PageLayout.PlaceInLayout_Text(grid, message_Text, 1, 2);

            System.Windows.Controls.Button openFolder_Btn = PageLayout.CreateButton_Custom("Open Folder", 85, 25, 1, 1);
            PageLayout.PlaceInLayout_Button(grid, openFolder_Btn, 5, 4);
            openFolder_Btn.Click += (sender, e) => OpenFolder(filePath);

            // System.Windows.Controls.Button next_button = PageLayout.CreateButton_Basic("Show");
            // PageLayout.PlaceInLayout_Button(grid, next_button, 5, 3);
            // next_button.Click += (sender, e) => System.Windows.MessageBox.Show("Filepath: " + $"{filePath}\"");

            System.Windows.Controls.Button quit_Btn = PageLayout.CreateButton_Basic("Quit");
            PageLayout.PlaceInLayout_Button(grid, quit_Btn, 5, 5);
            quit_Btn.Click += (sender, e) => System.Windows.Application.Current.Shutdown();

            newPage.Content = grid;
            
            return newPage;
        }

        static string ChooseFolder(object sender, RoutedEventArgs e, TextBlock text)
        {
            FolderBrowserDialog folderPick = new();
            folderPick.Description = "Choose installation directory";
            folderPick.UseDescriptionForTitle = true;
            folderPick.ShowNewFolderButton = true;

            DialogResult result = folderPick.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                text.Text = "Selected Folder \n \n" + folderPick.SelectedPath;
                return folderPick.SelectedPath;
            }
            else
            {
                System.Windows.MessageBox.Show("Something went wrong", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return "null";
            }
        }

        // Click Event Callbacks
        void ClickHandle(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Button Clicked", "Message", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, System.Windows.MessageBoxOptions.DefaultDesktopOnly);
        }

        public static async Task DownloadFileAsync(string url, string filePath, System.Windows.Controls.ProgressBar progressBar, TextBlock progress_Text)
        {
            try
            {
                var handler = new HttpClientHandler
                {
                    AllowAutoRedirect = true
                };

                handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

                using HttpClient client = new(handler);
                client.DefaultRequestHeaders.AcceptEncoding.Clear();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0");

                using HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                var totalBytes = response.Content.Headers.ContentLength ?? -1;
                var canReportProgress = totalBytes != -1;

                using Stream contentStream = await response.Content.ReadAsStreamAsync();
                using FileStream fileStream = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None);

                byte[] buffer = new byte[8192];
                long totalRead = 0;
                int bytesRead;

                var dispatcher = progressBar.Dispatcher;

                while ((bytesRead = await contentStream.ReadAsync(buffer)) > 0)
                {
                    await fileStream.WriteAsync(buffer, 0, bytesRead);
                    totalRead += bytesRead;
                    progress_Text.Text = "Progress " + totalRead / 1024 / 1024 + "MB / " + totalBytes / 1024 / 1024 + "MB";

                    if (canReportProgress)
                    {
                        double progress = (double)totalRead / totalBytes * 100;
                        await dispatcher.InvokeAsync(() =>
                        {
                            progressBar.Value = progress;
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Inside Error: " + ex.Message);
            }
        }

        public static async void UnzipFile(string filePath, string destinationPath, System.Windows.Controls.ProgressBar progressBar, TextBlock statusText, MainWindow mainWindow)
        {
            progressBar.Value = 0;
            statusText.Text = "Status: UNZIPPING GAME FILES";
            try
            {
                await Task.Run(() =>
                {
                    ZipFile.ExtractToDirectory(filePath, destinationPath);
                });

                progressBar.Dispatcher.Invoke(() =>
                {
                    progressBar.Value = 100;
                    mainWindow.SwitchPage(FourthPage(filePath));
                    System.IO.File.Delete(filePath);
                });
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Inside Error: " + ex.Message);
            }
        }
        public static void OpenFolder(string folderPath)
        {
            string? path = System.IO.Path.GetDirectoryName(folderPath);

            if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
            {
                try
                {
                    Process.Start(new ProcessStartInfo()
                    {
                        FileName = path,
                        UseShellExecute = true,
                        Verb = "open"
                    });
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Inside Error: " + ex.Message);
                }
            }
        }

    }
    
}
