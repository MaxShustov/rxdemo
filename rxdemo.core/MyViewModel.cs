using ReactiveUI;
using System.Reactive.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System;
using System.Reactive;
using DynamicData;
using System.Collections.ObjectModel;

namespace rxdemo.core
{
    public class ChildViewModel: ReactiveObject
    {
        public long Key { get; }

        private string _editableField;
        public string EditableField
        {
            get => _editableField;
            set => this.RaiseAndSetIfChanged(ref _editableField, value);
        }
    }

    public class MyViewModel : ReactiveObject
    {
        private readonly SourceList<ChildViewModel> _childViewModelsList;
        private readonly SourceCache<ChildViewModel, long> _childViewModelsCache;

        private readonly ReadOnlyObservableCollection<ChildViewModel> _items;
        public ReadOnlyObservableCollection<ChildViewModel> Items => _items;

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
        private ObservableAsPropertyHelper<string> _fullName;
        public string FullName => _fullName.Value;

        private ObservableAsPropertyHelper<IEnumerable<string>> _searchResults;
        public IEnumerable<string> SearchResults => _searchResults.Value;

        public ReactiveCommand<string, IEnumerable<string>> Search { get; }
        public ReactiveCommand<Unit,Unit> Cancel { get; }

        public MyViewModel()
        {
            _childViewModelsList = new SourceList<ChildViewModel>();

            var subscription = _childViewModelsList
                .Connect()
                .Transform(x => x)
                .Filter(x => x is null)
                .Bind(out _items)
                .Subscribe();

            _fullName = this.WhenAnyValue(vm => vm.FirstName)
                .CombineLatest(this.WhenAnyValue(vm => vm.LastName),
                    (firstName, lastName) => $"{firstName} {lastName}")
                .ToProperty(this, vm => vm.FullName);

            Search = ReactiveCommand.Create<string, IEnumerable<string>>(str => Enumerable.Repeat(str, 5));

            _searchResults = Search
                .Where(x => x != null && x.Any())
                .ToProperty(this, vm => vm.SearchResults);

            var errorHandling = Search
                .ThrownExceptions
                .Subscribe(ex =>
                {
                    Debug.WriteLine($"Error message: {ex.Message}");
                });
        }
    }



}
