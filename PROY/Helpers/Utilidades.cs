using System.Security.Cryptography;
using System.Text;

namespace PROY.Helpers
{
    public class Utilidades
    {

        public static string Encriptar(string cad)
        {
            StringBuilder sb = new StringBuilder();

            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result=hash.ComputeHash(enc.GetBytes(cad));

                foreach (byte b in result)
                    sb.Append(b.ToString("x2"));
            }
            
            return sb.ToString();
        }

        public static string Exito(string cad) {
            return cad;
        }
    }
}
