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
        private string _filterText;
        private string _sortProperty;
        private Person _selectedUser;

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

        public RelayCommand AddCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand DeleteCommand { get; }

        public PersonViewModel()
        {
            _repository = new PersonRepository();
            LoadUsersAsync();
            AddCommand = new RelayCommand(AddUser);
            EditCommand = new RelayCommand(EditUser, () => SelectedUser != null);
            DeleteCommand = new RelayCommand(DeleteUser, () => SelectedUser != null);
        }

        private async void LoadUsersAsync()
        {
            try
            {
                var users = await _repository.LoadUsersAsync();
                _users = new ObservableCollection<Person>(users);
                Users = new ObservableCollection<Person>(_users);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                _users = new ObservableCollection<Person>();
                Users = new ObservableCollection<Person>();
            }
        }

        private async void SaveUsers()
        {
            try
            {
                await _repository.SaveUsersAsync(_users.ToList());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ApplyFilter()
        {
            if (string.IsNullOrEmpty(FilterText))
            {
                Users = new ObservableCollection<Person>(_users);
            }
            else
            {
                var filtered = _users.Where(p => p.Name.Contains(FilterText, StringComparison.OrdinalIgnoreCase) ||
                                                 p.Surname.Contains(FilterText, StringComparison.OrdinalIgnoreCase));
                Users = new ObservableCollection<Person>(filtered);
            }
            ApplySort();
        }

        private void ApplySort()
        {
            if (string.IsNullOrEmpty(SortProperty)) return;
            IEnumerable<Person> sorted;
            switch (SortProperty)
            {
                case "Name":
                    sorted = Users.OrderBy(p => p.Name);
                    break;
                case "Surname":
                    sorted = Users.OrderBy(p => p.Surname);
                    break;
                case "Email":
                    sorted = Users.OrderBy(p => p.Email);
                    break;
                case "BirthDate":
                    sorted = Users.OrderBy(p => p.BirthDate);
                    break;
                default:
                    return;
            }
            Users = new ObservableCollection<Person>(sorted);
        }

        private void AddUser()
        {
            var dialog = new EditPersonWindow();
            dialog.PersonSaved += person =>
            {
                _users.Add(person);
                SaveUsers();
                ApplyFilter();
            };
            dialog.ShowDialog();
        }

        private void EditUser()
        {
            if (SelectedUser == null) return;
            var dialog = new EditPersonWindow(SelectedUser);
            dialog.PersonSaved += person =>
            {
                int index = _users.IndexOf(SelectedUser);
                _users[index] = person;
                SaveUsers();
                ApplyFilter();
            };
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
