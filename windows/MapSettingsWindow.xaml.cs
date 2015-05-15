using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PSim
{
    /// <summary>
    /// Interaction logic for MapSettingsWindow.xaml
    /// </summary>
    public partial class MapSettingsWindow : Window
    {
        public MapSettingsWindow()
        {
            InitializeComponent();
            this.Loaded += MapSettingsWindow_Loaded;
        }

        void MapSettingsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //ext.
        }
        public void RefreshShits()
        {
            refreshing = true;
            bigParkingSettingsGrid.Visibility = ext.ParkingType == ParkingType.BigParking ? Visibility.Visible : Visibility.Hidden;
            paralelParkingSettingsGrid.Visibility = ext.ParkingType == ParkingType.LateralParking ? Visibility.Visible : Visibility.Hidden;

            car1paCheckBox.IsChecked = ext.busyParkingPlaces[0, 0];
            car2paCheckBox.IsChecked = ext.busyParkingPlaces[0, 1];
            car3paCheckBox.IsChecked = ext.busyParkingPlaces[0, 2];
            car4paCheckBox.IsChecked = ext.busyParkingPlaces[0, 3];
            car5paCheckBox.IsChecked = ext.busyParkingPlaces[0, 4];
            car6paCheckBox.IsChecked = ext.busyParkingPlaces[0, 5];

            car1ppaCheckBox.IsChecked = ext.busyParkingPlaces[1, 0];
            car2ppaCheckBox.IsChecked = ext.busyParkingPlaces[1, 1];
            car3ppaCheckBox.IsChecked = ext.busyParkingPlaces[1, 2];
            car4ppaCheckBox.IsChecked = ext.busyParkingPlaces[1, 3];
            
            this.Title = ext.ParkingType == ParkingType.BigParking ? "Big Parking" : "Parallel Parking";
            refreshing = false;
        }
        bool refreshing = false;
        private void car1paCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (refreshing) return;

            ext.busyParkingPlaces[0, 0] = car1paCheckBox.IsChecked.Value;
            ext.busyParkingPlaces[0, 1] = car2paCheckBox.IsChecked.Value;
            ext.busyParkingPlaces[0, 2] = car3paCheckBox.IsChecked.Value;
            ext.busyParkingPlaces[0, 3] = car4paCheckBox.IsChecked.Value;
            ext.busyParkingPlaces[0, 4] = car5paCheckBox.IsChecked.Value;
            ext.busyParkingPlaces[0, 5] = car6paCheckBox.IsChecked.Value;

            ext.busyParkingPlaces[1, 0] = car1ppaCheckBox.IsChecked.Value;
            ext.busyParkingPlaces[1, 1] = car2ppaCheckBox.IsChecked.Value;
            ext.busyParkingPlaces[1, 2] = car3ppaCheckBox.IsChecked.Value;
            ext.busyParkingPlaces[1, 3] = car4ppaCheckBox.IsChecked.Value;

            ext.MapWindow.RefreshCozBusyParkingPlacesChanged();
        }
    }
}
