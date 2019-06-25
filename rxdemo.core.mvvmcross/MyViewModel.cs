using System;
using MvvmCross.ViewModels;
using System.Collections.Generic;
using MvvmCross.Commands;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;

namespace rxdemo.core.mvvmcross
{
    public class MyViewModel: MvxViewModel
    {
        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set
            {
                SetProperty(ref _firstName, value);
                UpdateFullName(FirstName, LastName);
            }
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set
            {
                SetProperty(ref _lastName, value);
                UpdateFullName(FirstName, LastName);
            }
        }

        private string _fullName;
        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }

        private IEnumerable<string> _searchResults;
        public IEnumerable<string> SearchResults
        {
            get => _searchResults;
            set => SetProperty(ref _searchResults, value);
        }

        private bool _isSearching;
        public bool IsSearching
        {
            get => _isSearching;
            set => SetProperty(ref _isSearching, value);
        }

        public MvxAsyncCommand<string> Search { get; }

        public MyViewModel()
        {
            Search = new MvxAsyncCommand<string>(OnSearchAsync);

            SearchResults = Enumerable.Empty<string>();
        }

        private void UpdateFullName(string firstName, string lastName)
        {
            FullName = $"{firstName} {lastName}";
        }

        private async Task OnSearchAsync(string name)
        {
            IsSearching = true;

            try
            {
                await Task.Delay(TimeSpan.FromSeconds(3));

                var searchResults = Enumerable.Repeat(name, 5);
                if (searchResults != null && searchResults.Any())
                {
                    SearchResults = searchResults;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Error message: {ex.Message}");
            }

            IsSearching = false;
        }
    }
}
