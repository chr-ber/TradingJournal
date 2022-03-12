using MudBlazor;
using TradingJournal.Domain.Enums;

namespace TradingJournal.Client.Theme;

public class Theme
{
    private static MudTheme _mainTheme;

    public static MudTheme MainTheme
    {
        // create theme on first request
        get => _mainTheme ??= new MudTheme()
        {
            Palette = LightPalette,
            PaletteDark = DarkPalette,
            LayoutProperties = new LayoutProperties(),
        };
    }

    private static readonly Palette LightPalette = new()
    {
        Primary = "#3ccdb6",
        Black = "#110e2d",
        AppbarText = "#424242",
        AppbarBackground = "rgba(255,255,255,0.8)",
        DrawerBackground = "#ffffff",
        GrayLight = "#e8e8e8",
        GrayLighter = "#f9f9f9",
        TableHover = "rgba(127,207,194,0.20)",
        Success = "#3ccdb6",
        Warning = "#cda03c",
        Error = "#a1366c",
    };

    private static readonly Palette DarkPalette = new()
    {
        Primary = "#3ccdb6",
        Surface = "#1e1e2d",
        Background = "#1a1a27",
        BackgroundGrey = "#151521",
        AppbarText = "#92929f",
        AppbarBackground = "rgba(26,26,39,0.8)",
        DrawerBackground = "#1a1a27",
        ActionDefault = "#74718e",
        ActionDisabled = "#9999994d",
        ActionDisabledBackground = "#605f6d4d",
        TextPrimary = "#b2b0bf",
        TextSecondary = "#92929f",
        TextDisabled = "#ffffff33",
        DrawerIcon = "#92929f",
        DrawerText = "#92929f",
        GrayLight = "#2a2833",
        GrayLighter = "#1e1e2d",
        Info = "#4a86ff",
        Success = "#3ccdb6",
        Warning = "#cda03c",
        Error = "#a1366c",
        LinesDefault = "#33323e",
        TableLines = "#33323e",
        Divider = "#292838",
        OverlayLight = "#1e1e2d80",
        TableHover = "rgba(127,207,194,0.20)",
    };

    public static ChartOptions ReturnColorChartOptions() => new ChartOptions() { ChartPalette = new[] { "#3ccdb6", "#a1366c", "#cda03c" } };

    public static Color GetAccountStatusColor(TradingAccountStatus status)
    {
        return status switch
        {
            TradingAccountStatus.Enabled => Color.Success,
            TradingAccountStatus.Disabled => Color.Warning,
            _ => Color.Default,
        };
    }

    public static Color GetTradeDirectionColor(TradeDirection direction)
    {
        return direction switch
        {
            TradeDirection.Open => Color.Success,
            TradeDirection.Close => Color.Error,
            _ => Color.Default,
        };
    }

    public static Color GetReturnColor(decimal amount)
    {
        return amount switch
        {
            0 => Color.Default,
            > 0 => Color.Success,
            < 0 => Color.Error,
        };
    }

    public static Color GetTradeStatusColor(TradeStatus result)
    {
        return result switch
        {
            TradeStatus.Open => Color.Warning,
            TradeStatus.Breakeven => Color.Default,
            TradeStatus.Win => Color.Success,
            TradeStatus.Loss => Color.Error,
            _ => Color.Default,
        };
    }
}