﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Npgsql;

class Profile
{
    public Guid UserId { get; set; }
    public byte[] PropertyNames { get; set; } // Store as byte[]
    public byte[] PropertyValuesString { get; set; } // Store as byte[]
    public byte[] PropertyValuesBinary { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
}

class Program
{
    static void Main()
    {
        string mssqlConnectionString = "Data Source=DESKTOP-O7DMBQ5;Initial Catalog=aspnetdb;Integrated Security=True";
        string pgConnectionString = "Server=178.128.123.190;Port=5432;Database=postgres;User Id=gpadmin;Password=;";

        var profiles = new List<Profile>();
        using (SqlConnection mssqlConnection = new SqlConnection(mssqlConnectionString))
        {
            mssqlConnection.Open();
            string query = @"SELECT    userid as userid,
                            CAST(CAST(propertynames AS NVARCHAR(MAX)) AS VARBINARY(MAX)) as propertynames,
                            CAST(CAST(propertyvaluesstring AS NVARCHAR(MAX)) AS VARBINARY(MAX)) as propertyvaluesstring,
                            propertyvaluesbinary as propertyvaluesbinary,
                            lastupdateddate as lastupdateddate
                            FROM  aspnet_Profile;";
            using (SqlCommand cmd = new SqlCommand(query, mssqlConnection))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    profiles.Add(new Profile
                    {
                        UserId = reader.GetGuid(0),
                        PropertyNames = (byte[])reader["propertynames"],
                        PropertyValuesString = (byte[])reader["propertyvaluesstring"],
                        PropertyValuesBinary = (byte[])reader["propertyvaluesbinary"],
                        LastUpdatedDate = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4)
                    });
                }



            }
        }
    
        using (NpgsqlConnection pgConnection = new NpgsqlConnection(pgConnectionString))
        {
            pgConnection.Open();

            using (var writer = pgConnection.BeginBinaryImport(@"COPY aspnet_profiles " +
                "(userid, propertynames, propertyvaluesstring, propertyvaluesbinary, lastupdateddate)" +
                " FROM STDIN BINARY"))
            {
                foreach (var profile in profiles)
                {
                    writer.StartRow();
                    writer.Write(profile.UserId);
                    writer.Write(profile.PropertyNames);
                    writer.Write(profile.PropertyValuesString);
                    writer.Write(profile.PropertyValuesBinary);
                    writer.Write(profile.LastUpdatedDate);
                }

                writer.Complete();
            }
        }

        Console.WriteLine("Bulk data transfer complete.");
    }
}
