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
        private string _name = string.Empty;
        private string _surname = string.Empty;
        private string _email = string.Empty;
        private DateTime? _birthDate;
        private bool _isProcessing;
        private string _mode;
        private AsyncRelayCommand _saveCommand;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event Action<Person> PersonSaved;

        public string Mode
        {
            get => _mode;
            private set
            {
                _mode = value;
                OnPropertyChanged();
            }
        }

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

        public DateTime? BirthDate
        {
            get => _birthDate;
            set
            {
                _birthDate = value;
                OnPropertyChanged();
                SaveCommand.NotifyCanExecuteChanged();
            }
        }

        public bool IsProcessing
        {
            get => _isProcessing;
            private set
            {
                _isProcessing = value;
                OnPropertyChanged();
                SaveCommand.NotifyCanExecuteChanged();
            }
        }

        public Visibility ProcessingVisibility => IsProcessing ? Visibility.Visible : Visibility.Collapsed;
        public bool InputsEnabled => !IsProcessing;

        public bool CanSave => !string.IsNullOrWhiteSpace(Name) &&
                               !string.IsNullOrWhiteSpace(Surname) &&
                               !string.IsNullOrWhiteSpace(Email) &&
                               BirthDate.HasValue &&
                               !IsProcessing;

        public AsyncRelayCommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new AsyncRelayCommand(SaveAsync, () => CanSave);
                }
                return _saveCommand;
            }
        }

        public EditPersonViewModel(Person? person = null)
        {
            if (person != null)
            {
                Name = person.Name;
                Surname = person.Surname;
                Email = person.Email;
                BirthDate = person.BirthDate;
                Mode = "Edit";
            }
            else
            {
                BirthDate = DateTime.Now.AddYears(-18).Date;
                Mode = "Add";
            }
        }

        private async Task SaveAsync()
        {
            IsProcessing = true;
            try
            {
                var person = await Task.Run(() =>
                {
                    if (!BirthDate.HasValue)
                        throw new ArgumentException("Birth date is required");

                    return new Person(Name, Surname, Email, BirthDate.Value);
                });

                PersonSaved?.Invoke(person);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsProcessing = false;
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (propertyName == nameof(IsProcessing))
            {
                OnPropertyChanged(nameof(ProcessingVisibility));
                OnPropertyChanged(nameof(InputsEnabled));
            }
        }
    }
}
