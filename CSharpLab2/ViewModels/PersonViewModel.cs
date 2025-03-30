using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using KMA.Krachylo.Lab2.Models;

namespace KMA.Krachylo.Lab2.ViewModels
{
    class PersonViewModel : INotifyPropertyChanged
    {
        private string _name = string.Empty;
        private string _surname = string.Empty;
        private string _email = string.Empty;
        private DateTime? _birthDate;
        private string _resultOutput = string.Empty;
        private bool _isProcessing;
        private RelayCommand _proceedCommand;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
                _proceedCommand?.NotifyCanExecuteChanged();
            }
        }

        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged();
                _proceedCommand?.NotifyCanExecuteChanged();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
                _proceedCommand?.NotifyCanExecuteChanged();
            }
        }

        public DateTime? BirthDate
        {
            get => _birthDate;
            set
            {
                _birthDate = value;
                OnPropertyChanged();
                _proceedCommand?.NotifyCanExecuteChanged();
            }
        }

        public string ResultOutput
        {
            get => _resultOutput;
            private set
            {
                _resultOutput = value;
                OnPropertyChanged();
            }
        }

        public bool IsProcessing
        {
            get => _isProcessing;
            private set
            {
                _isProcessing = value;
                OnPropertyChanged();
                _proceedCommand?.NotifyCanExecuteChanged();
            }
        }

        public Visibility ProcessingVisibility => IsProcessing ? Visibility.Visible : Visibility.Collapsed;

        public bool InputsEnabled => !IsProcessing;

        public bool CanProceed => !string.IsNullOrWhiteSpace(Name) &&
                                  !string.IsNullOrWhiteSpace(Surname) &&
                                  !string.IsNullOrWhiteSpace(Email) &&
                                  BirthDate.HasValue &&
                                  !IsProcessing;

        public RelayCommand ProceedCommand => _proceedCommand ??= new RelayCommand(
            async () => await ProceedAsync(),
            () => CanProceed);

        public PersonViewModel() { }

        private async Task ProceedAsync()
        {
            IsProcessing = true;
            try
            {
                var dateNow = DateTime.Now;
                if (BirthDate!.Value > dateNow)
                {
                    MessageBox.Show("Surely you can't be from the future, choose a valid date please.");
                    return;
                }
                if (dateNow.Year - BirthDate.Value.Year >= 135)
                {
                    MessageBox.Show("You should be in Guinness World Records book :) Enter your real birthday, please!!");
                    return;
                }

                await Task.Delay(5000);

                Person person = new Person(Name, Surname, Email, BirthDate.Value);

                string birthdayMessage = person.IsBirthday ? "Happy Birthday!" : "";
                ResultOutput = $"""
                    Name: {person.Name}
                    Surname: {person.Surname}
                    Email: {person.Email}
                    Birth Date: {person.BirthDate:yyyy-MM-dd}
                    Is Adult? {person.IsAdult}
                    Sun Sign: {person.SunSign}
                    Chinese Sign: {person.ChineseSign}
                    {birthdayMessage}
                    """;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
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
