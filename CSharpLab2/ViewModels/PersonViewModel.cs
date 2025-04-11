using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using KMA.Krachylo.Lab2.Exceptions;
using KMA.Krachylo.Lab2.Models;
using KMA.Krachylo.Lab2.Repository;
using KMA.Krachylo.Lab2.Views;

namespace KMA.Krachylo.Lab2.ViewModels
{
    class PersonViewModel : INotifyPropertyChanged
    {
        private readonly PersonRepository _repository;
        private ObservableCollection<Person> _users;
        private ObservableCollection<Person> _filteredUsers;
        private string _filterText = string.Empty;
        private string _sortProperty = string.Empty;
        private Person _selectedUser;
        private bool _isLoading = false;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Person> Users
        {
            get => _filteredUsers;
            set
            {
                _filteredUsers = value;
                OnPropertyChanged();
            }
        }

        public string FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value;
                ApplyFilter();
                OnPropertyChanged();
            }
        }

        public string SortProperty
        {
            get => _sortProperty;
            set
            {
                _sortProperty = value;
                ApplySort();
                OnPropertyChanged();
            }
        }

        public Person SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
                EditCommand.NotifyCanExecuteChanged();
                DeleteCommand.NotifyCanExecuteChanged();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public Visibility IsLoadingVisible
        { 
            get => _isLoading ? Visibility.Visible : Visibility.Hidden; 
        }

        public RelayCommand AddCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand DeleteCommand { get; }

        public PersonViewModel()
        {
            IsLoading = false;
            _repository = new PersonRepository();
            _users = new ObservableCollection<Person>();
            _filteredUsers = new ObservableCollection<Person>();

            AddCommand = new RelayCommand(AddUser);
            EditCommand = new RelayCommand(EditUser, () => SelectedUser != null);
            DeleteCommand = new RelayCommand(DeleteUser, () => SelectedUser != null);

            Task.Run(async () => await LoadUsersAsync());
        }

        private async Task LoadUsersAsync()
        {
            try
            {
                IsLoading = true;
                var users = await _repository.LoadUsersAsync();
                _users = new ObservableCollection<Person>(users);
                ApplyFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                _users = new ObservableCollection<Person>();
                Users = new ObservableCollection<Person>();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async void SaveUsers()
        {
            try
            {
                IsLoading = true;
                await _repository.SaveUsersAsync(_users.ToList());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving users: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ApplyFilter()
        {
            IEnumerable<Person> filtered = _users;

            if (!string.IsNullOrEmpty(FilterText))
            {
                filtered = _users.Where(p =>
                    p.Name.Contains(FilterText, StringComparison.OrdinalIgnoreCase) ||
                    p.Surname.Contains(FilterText, StringComparison.OrdinalIgnoreCase) ||
                    p.Email.Contains(FilterText, StringComparison.OrdinalIgnoreCase) ||
                    p.SunSign.Contains(FilterText, StringComparison.OrdinalIgnoreCase) ||
                    p.ChineseSign.Contains(FilterText, StringComparison.OrdinalIgnoreCase));
            }

            Users = new ObservableCollection<Person>(filtered);
            ApplySort();
        }

        private void ApplySort()
        {
            if (string.IsNullOrEmpty(SortProperty)) return;

            IEnumerable<Person> sorted;

            Func<Person, IComparable> keySelector = SortProperty switch
            {
                "Name" => p => p.Name,
                "Surname" => p => p.Surname,
                "Email" => p => p.Email,
                "BirthDate" => p => p.BirthDate,
                "SunSign" => p => p.SunSign,
                "ChineseSign" => p => p.ChineseSign,
                "IsAdult" => p => p.IsAdult,
                "IsBirthday" => p => p.IsBirthday,
                _ => p => p.Surname
            };

            sorted = Users.OrderBy(keySelector);

            Users = new ObservableCollection<Person>(sorted);
        }

        private void AddUser()
        {
            var dialog = new EditPersonWindow();
            var viewModel = dialog.DataContext as EditPersonViewModel;

            if (viewModel != null)
            {
                viewModel.PersonSaved += person =>
                {
                    _users.Add(person);
                    SaveUsers();
                    ApplyFilter();
                    dialog.Close();
                };
            }

            dialog.ShowDialog();
        }

        private void EditUser()
        {
            if (SelectedUser == null) return;

            var dialog = new EditPersonWindow(SelectedUser);
            var viewModel = dialog.DataContext as EditPersonViewModel;

            if (viewModel != null)
            {
                viewModel.PersonSaved += person =>
                {
                    int index = _users.IndexOf(SelectedUser);
                    _users[index] = person;
                    SaveUsers();
                    ApplyFilter();
                    dialog.Close();
                };
            }
            dialog.ShowDialog();
        }

        private void DeleteUser()
        {
            if (SelectedUser == null) return;
            if (MessageBox.Show("Sure you wanna delete this user?", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _users.Remove(SelectedUser);
                SaveUsers();
                ApplyFilter();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
