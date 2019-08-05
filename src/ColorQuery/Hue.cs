using clipr;

namespace ColorQuery
{
    [StaticEnumeration]
    public sealed class Hue
    {
        public string HueText { get; private set; }

        public static readonly Hue Red = new Hue{HueText="red"};
        public static readonly Hue Orange = new Hue{HueText="orange"};
        public static readonly Hue Yellow = new Hue{HueText="yellow"};
        public static readonly Hue Green = new Hue{HueText="green"};
        public static readonly Hue Aqua = new Hue{HueText="aqua"};
        public static readonly Hue Blue = new Hue{HueText="blue"};
        public static readonly Hue Violet = new Hue{HueText="violet"};
        public static readonly Hue Fuchsia = new Hue{HueText="fuchsia"};
    }
}