
using System.Reflection;

namespace Strzelecki_Baranowski.DuckApp.BL
{

    public class BLC
    {
        public BLC(string config_path) {
            Assembly DAO = Assembly.UnsafeLoadFrom(config_path);
            Type ti = DAO.GetType("DAO");
            var o = Activator.CreateInstance(ti, new object[] { "" });

        }
    }
}