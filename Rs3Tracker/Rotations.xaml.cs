using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Rs3Tracker
{
    public partial class Rotations
    {
        public ObservableCollection<Ability> Abilities { get; set; }
        private List<Rotation> RotationList = new List<Rotation>();

        public Rotations()
        {
            InitializeComponent();
            if (File.Exists(".\\Rotations.json"))
            {
                RotationList = JsonConvert.DeserializeObject<List<Rotation>>(File.ReadAllText(".\\Rotations.json"));
                foreach (Rotation rotation in RotationList)
                {
                    RotationBox.Items.Add(rotation.name);
                }
            }
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = ".\\"; //TODO fix
            ofd.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
            ofd.FilterIndex = 2;
            ofd.RestoreDirectory = true;
            try
            {
                ofd.ShowDialog();

                var filePath = ofd.FileName;

                var rotationToAdd = JsonConvert.DeserializeObject<Rotation>(File.ReadAllText(filePath));
                RotationList.Add(rotationToAdd);
                RotationBox.Items.Add(rotationToAdd.name);
                File.WriteAllText(".\\Rotations.json", JsonConvert.SerializeObject(RotationList));

            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
        }

        private void RotationBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var rotation in RotationList)
            {
                if (rotation.name == RotationBox.SelectedItem.ToString())
                {
                    Abilities = new ObservableCollection<Ability>(rotation.abilities);
                }
            }
            AbilityListView.DataContext = this;
            AbilityListView.ItemsSource = Abilities;
        }
    }
}

