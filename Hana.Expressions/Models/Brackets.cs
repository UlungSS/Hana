namespace Hana.Expressions.Models;


public class Brackets(char open, char close)
{
    public char Open { get; } = open;
    public char Close { get; } = close;
    public static Brackets Angle => new('<', '>');
    public static Brackets Square => new('[', ']');
}