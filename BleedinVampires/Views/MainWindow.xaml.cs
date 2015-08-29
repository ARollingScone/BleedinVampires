using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using BleedinVampires.Control;

namespace BleedinVampires.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string[] possibleResolutionsWidth;
        public string[] possibleResolutionsHeight;

        public MainWindow()
        {
            InitializeComponent();

            possibleResolutionsWidth = new string[] { "1920", "1680", "1366", "1280","1024", "800", "640" };
            possibleResolutionsHeight = new string[] { "1080", "1024", "800", "768", "600", "480" };

            foreach (var s in possibleResolutionsWidth)
            {
                ComboBoxItem box = new ComboBoxItem();
                box.Content = s;
                ViewSettings.ComboResolutionW.Items.Add(box);
            }
            ViewSettings.ComboResolutionW.SelectedIndex = 0;

            foreach (var s in possibleResolutionsHeight)
            {
                ComboBoxItem box = new ComboBoxItem();
                box.Content = s;
                ViewSettings.ComboResolutionH.Items.Add(box);
            }
            ViewSettings.ComboResolutionH.SelectedIndex = 0;
        }

        private void ui_ButtonSinglePlayer_Click(object sender, RoutedEventArgs e)
        {
            //SFML.Graphics.RenderWindow renderwindow = new SFML.Graphics.RenderWindow(new SFML.Window.VideoMode(800, 600), "Bleedin' Vampires");

            GameWindowController c = new GameWindowController();
            c.startRendering();
        }
    }
}
