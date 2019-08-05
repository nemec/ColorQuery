using System;
using clipr;
using clipr.Validation;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace ColorQuery
{
    class Program
    {
        private static void PrintPalettes(List<Palette> ps)
        {
            var maxIdLen = ps.Max(p => p.Id.ToString().Length);
            var maxColorCount = ps.Max(p => p.Colors.Count);
            foreach(var p in ps)
            {
                PrintPalette(p, maxIdLen, maxColorCount);
            }
        }

        private static void PrintPalette(Palette p)
        {
            PrintPalette(p, p.Id.ToString().Length, p.Colors.Count);
            foreach(var color in p.Colors)
            {
                PrintColor(color);
                Console.Write($"| {color.ToUpperInvariant()} | rgb(");
                var first = true;
                foreach(var c in ParseColor(color))
                {
                    if(first) first = false;
                    else Console.Write(',');
                    Console.Write(c);
                }
                Console.WriteLine(')');
            }
        }

        private static void PrintPalette(Palette p, int maxIdLen, int maxColorCount)
        {
            var idlen = p.Id.ToString().Length;

            Console.Write($"{new string(' ', maxIdLen - idlen)}{p.Id} |");
            Console.Write(new string(' ', maxColorCount - p.Colors.Count));
            foreach(var col in p.Colors)
            {
                PrintColor(col);
            }
            Console.WriteLine($"| {p.Title} by {p.Username}");
        }

        private static void PrintColor(string color)
        {
            Console.Write("\u001B[48;2");
            foreach(var hex in ParseColor(color))
            {
                Console.Write(";");
                Console.Write(hex);
            }
            Console.Write("m \u001B[0m");
        }

        private static int[] ParseColor(string color)
        {
            var idx = 0;
            var colors = new int[3];
            if(color.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                idx = 2;
            }
            for(var i = 0; i < 3; i++,idx+=2)
            {
                colors[i] = int.Parse(color.Substring(idx, 2), 
                            System.Globalization.NumberStyles.HexNumber);
            }
            return colors;
        }

        static async Task Main(string[] args)
        {
            var settings = new ParserSettings<Options>();
            var parser = new CliParser<Options>();
            var validator = new BasicParseValidator<Options>();
            parser.Validator = validator;
            validator.AddRule(o => {
                if(o.Search == null || o.Search.HexCodes == null) {
                    return null;
                }
                foreach(var code in o.Search.HexCodes) {
                    if(!Regex.IsMatch(code, "(0x)?[0-9A-Fa-f]{6}"))
                    {
                        return new ValidationFailure(
                            nameof(o.Search.HexCodes),
                            $"HexCodes input '{code}' is not a valid six-character hex code.");
                    }
                }
                return null;
            });
            validator.AddRule(o => {
                if(o?.Search?.HexCodes != null && o.Search.HexCodes.Count > 5)
                {
                    return new ValidationFailure(
                        nameof(o.Search.HexCodes),
                        $"The service supports up to 5 hex codes. You provided {o.Search.HexCodes.Count}"
                    );
                }
                return null;
            });

            var opts = new Options();
            parser.Parse(args, opts);

            var api = new Api();
            if(opts.Color != null)
            {
                try
                {
                    var pal = await api.GetPalette(opts.Color.Id);
                    Program.PrintPalette(pal);
                }
                catch(ArgumentException e)
                {
                    Console.Error.WriteLine(e.Message);
                    return;
                }
            }
            if(opts.Search != null)
            {
                try
                {
                    var pals = await api.SearchPalettes(opts.Search);
                    if(pals.Count == 0)
                    {
                        Console.Error.WriteLine("No palettes found for search criteria.");
                        return;
                    }
                    Program.PrintPalettes(pals);
                }
                catch(ArgumentException e)
                {
                    Console.Error.WriteLine(e.Message);
                    return;
                }
            }
        }
    }
}
