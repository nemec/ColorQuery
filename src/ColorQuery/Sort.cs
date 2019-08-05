namespace ColorQuery
{
    [clipr.StaticEnumeration]
    public sealed class Sort
    {
        public static readonly Sort Default = new Sort{SortKeyword=""};
        public static readonly Sort New = new Sort{SortKeyword="new"};
        public static readonly Sort Top = new Sort{SortKeyword="top"};
        public static readonly Sort Random = new Sort{SortKeyword="random"};

        public string SortKeyword { get; private set; }
    }
}