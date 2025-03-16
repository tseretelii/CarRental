namespace CarRental
{
    public static class HelperServices
    {
        public static string GetFullException (this Exception ex)
        {
            var exs = new List<string>();

            while (ex != null)
            {
                exs.Add (ex.Message);

                ex = ex.InnerException;
            }

            return string.Join ("=>", exs);
        }
    }
}
