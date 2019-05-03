namespace IIOT_OPC.Shared.Extensions
{
    using System.Text;
    public static class StringExtension
    {
        public static string GetValueOrDefault(this string s, string defaultValue)
        {
            return s ?? defaultValue;
        }

        public static string ApplyFilters(this string query, string pattern, object filters)
        {
            var filterNames = filters?.GetType()?.GetProperties();
            if (filterNames == null)
                return query.Replace(pattern, "");

            StringBuilder sb = new StringBuilder("WHERE ");
            int i = 0;
            do
            {
                if (filterNames[i].GetValue(filters) == null)
                {
                    sb.Append($"({filterNames[i].Name} is NULL)");
                }
                else
                {
                    sb.Append($"({filterNames[i].Name} = @{filterNames[i].Name})");
                }

                if (++i > filterNames.Length - 1) break;
                sb.Append(" AND ");
            } while (true);

            return query.Replace(pattern, sb.ToString());
        }
    }
}