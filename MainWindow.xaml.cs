using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DDNetTracker;

public partial class MainWindow : Window
{
    private readonly DDNetService _apiService = new();
    private List<PlayerDisplayModel> _allPlayers = new();
    private PeriodicTimer? _timer;

    public MainWindow()
    {
        InitializeComponent();
        Loaded += MainWindow_Loaded;
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        await RefreshDataAsync();
        _ = StartAutoRefreshAsync();
    }

    private async Task RefreshDataAsync()
    {
        StatusText.Text = "Загрузка данных из DDNet...";
        _allPlayers = await _apiService.GetOnlinePlayersAsync();

        ApplyFilter();

        StatusText.Text = $"Обновлено: {DateTime.Now:HH:mm:ss}";
        CountText.Text = $"Игроков онлайн: {_allPlayers.Count}";
    }

    private async Task StartAutoRefreshAsync()
    {
        // Автообновление каждые 60 секунд
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(60));
        while (await _timer.WaitForNextTickAsync())
        {
            await RefreshDataAsync();
        }
    }

    private void ApplyFilter()
    {
        var query = SearchBox.Text.Trim().ToLower();

        if (string.IsNullOrWhiteSpace(query))
        {
            PlayersDataGrid.ItemsSource = _allPlayers;
        }
        else
        {
            var filtered = _allPlayers.Where(p =>
                p.Name.ToLower().Contains(query) ||
                p.Clan.ToLower().Contains(query)
            ).ToList();

            PlayersDataGrid.ItemsSource = filtered;
        }
    }

    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        ApplyFilter();
    }
}