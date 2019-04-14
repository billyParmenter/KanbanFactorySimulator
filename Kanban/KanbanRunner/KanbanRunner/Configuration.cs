using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanRunner
{
    /*
     * Gets data from the config table
     */
    class Configuration
    {
        private readonly SqlConnection conn;

        public Configuration(SqlConnection conn)
        {
            this.conn = conn;
        }

        /*
         * DESCRIPTION:
         *      Gets a double from the config table
         * PARAMETERS:
         *      key - The entry's name/key
         *      result - The out-value to store the result in
         * RETURNS:
         *      true if the key was found and is a valid double, false otherwise
         */
        public bool GetDouble(string key, out double result)
        {
            if (!GetString(key, out string value))
            {
                result = 0;
                return false;
            }

            return double.TryParse(value, out result);
        }

        /*
         * DESCRIPTION:
         *      Gets an int from the config table
         * PARAMETERS:
         *      key - The entry's name/key
         *      result - The out-value to store the result in
         * RETURNS:
         *      true if the key was found and is a valid int, false otherwise
         */
        public bool GetInteger(string key, out int result)
        {
            if(!GetString(key, out string value))
            {
                result = 0;
                return false;
            }

            return int.TryParse(value, out result);
        }

        /*
         * DESCRIPTION:
         *      Gets a value from the config table
         * PARAMETERS:
         *      key - The entry's name/key
         *      result - The out-value to store the result in
         * RETURNS:
         *      true if the key was found (is always valid, values stored as strings)
         */
        public bool GetString(string key, out string result)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT [Value] FROM CONFIG WHERE [Key] = @key", conn))
            {
                SqlParameter param = new SqlParameter("key", System.Data.SqlDbType.VarChar, key.Length)
                {
                    Value = key
                };
                cmd.Parameters.Add(param);

                result = cmd.ExecuteScalar().ToString();
                return result != null;
            }
        }
        
    }
}
