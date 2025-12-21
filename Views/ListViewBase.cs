using System.Collections.ObjectModel;
using Radiotech.Ui;

namespace Radiotech.Views;

public abstract class ListViewBase<T> : ContentPage where T : class
{
    protected readonly CollectionView CollectionView;
    // protected readonly ObservableCollection<T> Items;
    // protected readonly string[] Headers;
    // protected readonly Func<Grid> ItemTemplateFactory;
    protected ListViewBase(
        string title, 
        string[] headers, 
        ObservableCollection<T> items, 
        Func<Grid> itemTemplate)
    {
        // Headers = headers;
        // Items = items;
        // ItemTemplateFactory = itemTemplate;

        CollectionView = new CollectionView
        {
            SelectionMode = SelectionMode.Single,
            ItemsSource = items,
            ItemTemplate = new DataTemplate(itemTemplate)
        };
        CollectionView.SelectionChanged += OnSelectionChanged;

        var addButton = new Button { Text = $"Добавить {title}" };
        addButton.Clicked += OnAddClicked;

        Content = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Spacing = 12,
                Padding = 4,
                Children =
                {
                    UiTemplates.HeaderGrid(headers),
                    CollectionView,
                    addButton
                }
            }
        };
    }

    protected abstract Task ShowEditForm(T item);
    protected abstract Task ShowAddForm();

    private async void OnAddClicked(object sender, EventArgs e) => await ShowAddForm();

    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not T item)
        {
            CollectionView.SelectedItem = null;
            return;
        }
        CollectionView.SelectedItem = null;

        string action = await Shell.Current.DisplayActionSheetAsync(
            $"Действие над элементом", "Отмена", null, "Изменить", "Удалить");

        if (action == "Изменить")
            await ShowEditForm(item);
        else if (action == "Удалить")
        {
            bool confirm = await DisplayAlertAsync("Подтверждение", "Удалить?", "Да", "Нет");
            if (confirm) OnDelete(item);
        }
    }

    protected abstract void OnDelete(T item);
}