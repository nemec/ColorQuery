using System.Collections.Generic;
using clipr;

namespace ColorQuery
{
    public class Options
    {
        [Verb]
        public SearchVerb Search { get; set; }

        [Verb]
        public ColorVerb Color { get; set; }
    }

    public class ColorVerb
    {
        [PositionalArgument(0, Description="Colourlovers palette ID, found via a search or their website.")]
        public int Id { get; set; }
    }

    public class SearchVerb
    {
        public SearchVerb()
        {
            SortType = Sort.Default;
        }

        [NamedArgument('s', "sort",
                        Description="Choose how results are sorted. Defaults to unspecified order. Options: new, top, random")]
        public Sort SortType { get; set; }

        [NamedArgument("hue", Action=ParseAction.Append,
                        Description="Look for palettes matching similar hues. Options: red, orange, yellow, green, aqua, blue, violet, fuchsia")]
        public List<Hue> Hues { get; set; }

        [NamedArgument("hex", Action=ParseAction.Append,
                        Description="Find palettes that contain this color valuel. Up to five 6-character hex codes (e.g. '0xF60A23'. 0x prefix optional)")]
        public List<string> HexCodes { get; set; }

        [NamedArgument('k', "keyword", Action=ParseAction.Append,
                        Description="Filter palettes by keyword")]
        public List<string> Keywords { get; set; }
    }
}