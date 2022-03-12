namespace TradingJournal.Application.Common.Extensions;

public static class NumberFormatExtension
{
    // extension to change the format of all decimals at one place
    public static string ToFormatedString(this decimal number) => $"{number:N}";
}