using System.Windows;
using System.Windows.Controls;

namespace calc_laba2
{
    public partial class MainWindow : Window
    {
        private bool _columnAdded = false; // Флаг для отслеживания добавления пятой колонки

        public MainWindow()
        {
            InitializeComponent();
        }

        // Обработчик изменения размера окна
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Проверка, если ширина окна >= 560 и колонка еще не добавлена
            if (this.ActualWidth >= 560 && !_columnAdded)
            {
                // Изменяем ширину существующих колонок на пропорции для лучшего распределения
                MainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                MainGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
                MainGrid.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);
                MainGrid.ColumnDefinitions[3].Width = new GridLength(1, GridUnitType.Star);

                // Добавляем пятую колонку с шириной 2 части
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.2, GridUnitType.Star) });

                // Добавляем текстовый блок в новую колонку
                TextBlock messageTextBlock = new TextBlock
                {
                    Text = "История вычислений!",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 16,
                    Foreground = System.Windows.Media.Brushes.Red
                };

                // Указываем, что этот элемент будет находиться в пятой колонке
                Grid.SetColumn(messageTextBlock, 4); // Пятая колонка имеет индекс 4
                MainGrid.Children.Add(messageTextBlock);

                // Устанавливаем флаг, что колонка была добавлена
                _columnAdded = true;
            }
            // Проверка, если ширина окна меньше 560 и колонка была добавлена
            else if (this.ActualWidth < 560 && _columnAdded)
            {
                // Удаляем пятую колонку
                MainGrid.ColumnDefinitions.RemoveAt(4);

                // Удаляем все элементы, находящиеся в пятой колонке
                for (int i = MainGrid.Children.Count - 1; i >= 0; i--)
                {
                    UIElement element = MainGrid.Children[i];
                    if (Grid.GetColumn(element) == 4)
                    {
                        MainGrid.Children.Remove(element);
                    }
                }

                // Возвращаем ширину первых 4 колонок к исходному состоянию (равномерное распределение)
                MainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                MainGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
                MainGrid.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);
                MainGrid.ColumnDefinitions[3].Width = new GridLength(1, GridUnitType.Star);

                // Сбрасываем флаг добавления колонкиа
                _columnAdded = false;
            }

            // Обновляем текст с информацией о текущей ширине окна
            WidthTextBlock.Text = $"Ширина окна: {this.ActualWidth:F2}";
        }
    }
}
