using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using MySql.Data.MySqlClient;

namespace ImageUploadDemo.Adapter
{
    public static class MariaHelper
    {
        public static IEnumerable<T> Query<T>(string connectionString, CommandType commandType, string commandText, MySqlParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString));
            if (string.IsNullOrEmpty(commandText)) throw new ArgumentNullException(nameof(commandText));
            
            IEnumerable<T> objs;
            using (MySqlConnection cnn = new MySqlConnection(connectionString))
            {
                DynamicParameters parameters = MariaHelper.ParseParameters(commandParameters);
                objs = cnn.Query<T>(commandText, (object) parameters, commandType: new CommandType?(commandType));
                MariaHelper.SetOutputParameterValue(commandParameters, parameters);
            }

            return objs;
        }

        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params MySqlParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString));
            if (string.IsNullOrEmpty(commandText)) throw new ArgumentNullException(nameof(commandText));
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand(commandText, connection))
                {
                    mySqlCommand.CommandType = commandType;
                    if (commandParameters != null)
                    {
                        foreach (MySqlParameter commandParameter in commandParameters)
                        {
                            if (commandParameter != null)
                            {
                                if ((commandParameter.Direction == ParameterDirection.Input || commandParameter.Direction == ParameterDirection.InputOutput) && commandParameter.Value == null)
                                    commandParameter.Value = (object) DBNull.Value;
                                mySqlCommand.Parameters.Add(commandParameter);
                            }
                        }
                    }

                    return mySqlCommand.ExecuteNonQuery();
                }
            }
        }

        private static DynamicParameters ParseParameters(MySqlParameter[] commandParameters)
        {
            DynamicParameters dynamicParameters = (DynamicParameters) null;
            if (commandParameters != null)
            {
                dynamicParameters = new DynamicParameters();
                foreach (MySqlParameter commandParameter in commandParameters)
                {
                    if (commandParameter != null)
                    {
                        if ((commandParameter.Direction == ParameterDirection.Input || commandParameter.Direction == ParameterDirection.InputOutput) && commandParameter.Value == null)
                            commandParameter.Value = (object) DBNull.Value;
                        dynamicParameters.Add(commandParameter.ParameterName, commandParameter.Value, new DbType?(commandParameter.DbType), new ParameterDirection?(commandParameter.Direction));
                    }
                }
            }

            return dynamicParameters;
        }

        private static void SetOutputParameterValue(MySqlParameter[] parameters, DynamicParameters dynamicParameters)
        {
            if (parameters == null) return;
            
            foreach (MySqlParameter parameter in parameters)
            {
                if (parameter.Direction != ParameterDirection.Input)
                    parameter.Value = dynamicParameters.Get<object>(parameter.ParameterName);
            }
        }
    }
}