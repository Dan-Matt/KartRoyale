namespace Assets.Scripts.Utilities
{
    public static class StringUtility
    {
        public static string GetNumberWithSuffix(int no)
        {
            return no + ((no % 10 == 1 && no != 11) ? "st"
                : (no % 10 == 2 && no != 12) ? "nd"
                    : (no % 10 == 3 && no != 13) ? "rd"
                        : "th");
        }
    }
}
