namespace DeciderApp.Classes
{
    public static class Extensions
    {
        public static List<T> ShuffleMe<T>(this IList<T> list)
        {
            Random random = new Random();
            int n = list.Count;
            var res = list.ToList();

            for (int i = res.Count - 1; i > 1; i--)
            {
                int rnd = random.Next(i + 1);

                T value = res[rnd];
                res[rnd] = res[i];
                res[i] = value;
            }

            return res;
        }
    }
}
