using System.Text;
using Toypad;
using Color = System.Drawing.Color;

namespace ToypadCli;

internal static class Program
{
    private static readonly IToypad Toypad = global::Toypad.Toypad.CreateToypad();

    private static void Main()
    {
        Console.WriteLine("Toypad.Cli");
        Console.OutputEncoding = Encoding.UTF8;

        Toypad.SetColor(Pad.All, Color.DarkRed);
        
        Toypad.TagAdded += ToypadOnTagAdded;
        Toypad.TagRemoved += ToypadOnTagRemoved;

        Display();
        Console.ReadLine();
    }

    private static void ToypadOnTagRemoved(object? sender, Tag tag)
    {
        if (sender is not IToypad) return;
        Console.WriteLine();
        Console.WriteLine($"Removed from {tag.Pad}");
        Toypad.SetColor(tag.Pad, Color.DarkBlue);
        Display();
    }

    private static void ToypadOnTagAdded(object? sender, Tag tag)
    {
        if (sender is not IToypad) return;
        Console.WriteLine();
        Console.WriteLine($"Set to {tag.Pad}");
        Toypad.SetColor(tag.Pad, Color.Yellow);
        Display();
    }

    private static void Display()
    {
        Console.WriteLine();
        Task.Delay(200).Wait();
        
        var center = Toypad.Tags.Count(t => t.Pad == Pad.Center);
        Console.WriteLine($"     {center}");
        
        var left = Toypad.Tags.Count(t => t.Pad == Pad.Left);
        Console.Write($" {left} ");

        Console.ForegroundColor = ClosestConsoleColor(Toypad.LeftColor);
        Console.Write("\u2599 ");
        Console.ResetColor();
        
        Console.ForegroundColor = ClosestConsoleColor(Toypad.CenterColor);
        Console.Write("\u25cf ");
        Console.ResetColor();
        
        Console.ForegroundColor = ClosestConsoleColor(Toypad.RightColor);
        Console.Write("\u259f ");
        Console.ResetColor();
        
        var right = Toypad.Tags.Count(t => t.Pad == Pad.Right);
        Console.WriteLine($"{right}");
    }
      
    private static ConsoleColor ClosestConsoleColor(Color color)
    {
        ConsoleColor ret = 0;
        double rr = color.R, gg = color.G, bb = color.B, delta = double.MaxValue;

        foreach (ConsoleColor cc in Enum.GetValues(typeof(ConsoleColor)))
        {
            var n = Enum.GetName(typeof(ConsoleColor), cc) ?? "White";
            var c = Color.FromName(n == "DarkYellow" ? "Orange" : n); // bug fix
            var t = Math.Pow(c.R - rr, 2.0) + Math.Pow(c.G - gg, 2.0) + Math.Pow(c.B - bb, 2.0);
            if (t == 0.0)
                return cc;
            if (t < delta)
            {
                delta = t;
                ret = cc;
            }
        }
        return ret;
    }
    
}