using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using KMA.Krachylo.Lab2.Models;
using System.Windows;

namespace KMA.Krachylo.Lab2.ViewModels
{
    public class EditPersonViewModel : INotifyPropertyChanged
    {
        private string _name;
        private string _surname;
        private string _email;
        private DateTime _birthDate;
        private string _mode;

        public Person Person { get; private set; }

        public string Name
        {
            get => _name;
            set 
            { 
                _name = value;
                OnPropertyChanged();
                SaveCommand.NotifyCanExecuteChanged();
            }
        }

        public string Surname
        {
            get => _surname;
            set 
            {
                _surname = value;
                OnPropertyChanged();
                SaveCommand.NotifyCanExecuteChanged();
            }
        }

        public string Email
        {
            get => _email;
            set 
            { 
                _email = value;
                OnPropertyChanged();
                SaveCommand.NotifyCanExecuteChanged();
            }
        }

        public DateTime BirthDate
        {
            get => _birthDate;
            set 
            {
                _birthDate = value; 
                OnPropertyChanged();
                SaveCommand.NotifyCanExecuteChanged();
            }
        }

        public string Mode
        {
            get => _mode;
            set 
            {
                _mode = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand SaveCommand { get; }

        public EditPersonViewModel(Person? person)
        {
            SaveCommand = new RelayCommand(Save, CanSave);
            if (person != null)
            {
                Mode = "Edit";
                Name = person.Name;
                Surname = person.Surname;
                Email = person.Email;
                BirthDate = person.BirthDate;
                Person = person;
            }
            else
            {
                Mode = "Add";
                BirthDate = DateTime.Now;
            }
        }

        public EditPersonViewModel() : this(null) { }

        private void Save()
        {
            try
            {
                Person = new Person(Name, Surname, Email, BirthDate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(Surname) &&
                   !string.IsNullOrWhiteSpace(Email) &&
                   BirthDate <= DateTime.Now;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
