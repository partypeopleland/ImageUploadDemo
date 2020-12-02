using System.Collections.Generic;
using System.Data;
using System.Linq;
using ImageUploadDemo.Common;
using ImageUploadDemo.Models;
using MySql.Data.MySqlClient;

namespace ImageUploadDemo.Adapter
{
    public class MariaAdapter
    {
        public void Create(ZipEntity entity)
        {
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Id", MySqlDbType.Int32) {Direction = ParameterDirection.Output},
                new MySqlParameter("@my_data", MySqlDbType.LongBlob) {Value = entity.Data},
                new MySqlParameter("@my_before", MySqlDbType.Double) {Value = entity.Before},
                new MySqlParameter("@my_after", MySqlDbType.Double) {Value = entity.After},
            };

            MariaHelper.ExecuteNonQuery(
                DBConfig.ConnStr,
                CommandType.StoredProcedure,
                "usp_myimage_create",
                parameters);
        }

        public List<ZipEntity> GetAll()
        {
            return MariaHelper.Query<ZipEntity>(DBConfig.ConnStr,
                CommandType.StoredProcedure,
                "usp_myimage_getall",
                null).ToList();
        }

        public ZipEntity Find(int id)
        {
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@my_id", MySqlDbType.Int32) {Value = id},
            };

            return MariaHelper.Query<ZipEntity>(
                DBConfig.ConnStr,
                CommandType.StoredProcedure,
                "usp_myimage_get",
                parameters).FirstOrDefault();
        }
    }
}