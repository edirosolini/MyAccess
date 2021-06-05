// <copyright file="SqlServerDataBase.cs" company="El Roso">
// Copyright (c) El Roso. All rights reserved.
// </copyright>

namespace MyAccess.Providers
{
    using System;
    using System.Data.SqlClient;
    using System.IO;
    using System.Reflection;
    using MyAccess.Domains.Providers;

    public class SqlServerDataBase : ISqlServerDataBase
    {
        public SqlConnection GetSqlConnection() => new (Environment.GetEnvironmentVariable("CONNECTION_STRING"));

        public string GetQuery(string nameFile)
        {
            var assembly = Assembly.GetExecutingAssembly();
            Type type = typeof(SqlServerDataBase);
            var resourceName = $@"{type.Namespace}.SQL.{nameFile}.sql";
            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            using StreamReader reader = new (stream);
            return reader.ReadToEnd();
        }
    }
}
