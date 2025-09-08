using Microsoft.AspNetCore.Components;
using TomAndJerry.Services;
using TomAndJerry.Model;

namespace TomAndJerry.Component;

public partial class Thumbnail
{
    [Parameter] public Video VideoModel { get; set; } = new();

    public void GoToPage()
    {
        Nav.NavigateTo($"playmedia/{VideoModel.Id}");
    }
    
    private string GetCleanTitle()
    {
        return string.Join(" ", VideoModel.Description.Split(".").Where(x => x != "mkv").Select(x => x));
    }
    
    private string GetRandomViews()
    {
        var random = new Random();
        var views = random.Next(100, 10000);
        if (views >= 1000)
        {
            return $"{views / 1000:F1}K";
        }
        return views.ToString();
    }
    
    private string GetRandomTimeAgo()
    {
        var random = new Random();
        var days = random.Next(1, 365);
        if (days < 7)
            return $"{days} days ago";
        else if (days < 30)
            return $"{days / 7} weeks ago";
        else if (days < 365)
            return $"{days / 30} months ago";
        else
            return $"{days / 365} years ago";
    }
}
