using System.Reflection;
using Radiotech.Data;

namespace Radiotech.Ui;

public static class UiTemplates
{
    public static Grid HeaderGrid(string[] labels)
    {
        var grid = new Grid
        {
            Padding = new Thickness(10),
            // Padding = 10,
            ColumnSpacing = 10,
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = 20 }
            }
        };
        for (var i = 0; i < labels.Length; i++)
        {
            if (i != 0) grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            var labelObj = new Label { Text = labels[i], FontAttributes = FontAttributes.Bold};
            grid.Add(labelObj, i);
        }
        return grid;
    }
}