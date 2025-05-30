﻿using System;
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
using System.Windows.Shapes;
using KMA.Krachylo.Lab2.Models;
using KMA.Krachylo.Lab2.ViewModels;

namespace KMA.Krachylo.Lab2.Views
{
    /// <summary>
    /// Interaction logic for EditPersonWindow.xaml
    /// </summary>
    public partial class EditPersonWindow : Window
    {
        public event Action<Person> PersonSaved;

        public EditPersonWindow(Person? person = null)
        {
            InitializeComponent();
            DataContext = new EditPersonViewModel(person);
        }
    }
}
