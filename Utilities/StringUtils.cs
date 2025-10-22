using System.ComponentModel;
using System.Text;

namespace ERP_API.Utilities
{
    public static class StringUtils
    {
        public static void AddPaging(StringBuilder query, int pageIndex, int pageSize)
        {
            if (pageSize > 0)
            {
                int offset = (pageIndex - 1) * pageSize;
                query.AppendFormat(" LIMIT {0} OFFSET {1} ", pageSize, offset);
            }
        }

        public static string GetEnumDescription(Enum value)
        {
            try
            {
                var fi = value.GetType().GetField(value.ToString());
                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                return attributes.Length > 0 ? attributes[0].Description : value.ToString();
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

    }
}
