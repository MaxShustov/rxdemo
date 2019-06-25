using ReactiveUI;
using System.Reactive.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System;
using System.Threading.Tasks;

namespace rxdemo.core
{
    public class MyViewModel : ReactiveObject
    {
        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set => this.RaiseAndSetIfChanged(ref _firstName, value);
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set => this.RaiseAndSetIfChanged(ref _lastName, value);
        }

        private readonly ObservableAsPropertyHelper<string> _fullName;
        public string FullName => _fullName.Value;

        private readonly ObservableAsPropertyHelper<IEnumerable<string>> _searchResults;
        public IEnumerable<string> SearchResults => _searchResults.Value;

        public ReactiveCommand<string, IEnumerable<string>> Search { get; }

        private readonly ObservableAsPropertyHelper<bool> _isSearching;
        public bool IsSearching => _isSearching.Value;

        public MyViewModel()
        {
            Search = ReactiveCommand.CreateFromTask<string, IEnumerable<string>>(OnSearchAsync);

            _fullName = this.WhenAnyValue(vm => vm.FirstName)
                .CombineLatest(this.WhenAnyValue(vm => vm.LastName),
                    (firstName, lastName) => $"{firstName} {lastName}")
                .ToProperty(this, vm => vm.FullName);

            _isSearching = Search
                .IsExecuting
                .ToProperty(this, vm => vm.IsSearching);

            _searchResults = Search
                .Where(x => x != null && x.Any())
                .StartWith(Enumerable.Empty<string>())
                .ToProperty(this, vm => vm.SearchResults);

            var errorHandling = Search
                .ThrownExceptions
                .Subscribe(ex =>
                {
                    Debug.WriteLine($"Error message: {ex.Message}");
                });
        }

        private async Task<IEnumerable<string>> OnSearchAsync(string name)
        {
            await Task.Delay(TimeSpan.FromSeconds(3));

            return Enumerable.Repeat(name, 5);
        }

    }
}
