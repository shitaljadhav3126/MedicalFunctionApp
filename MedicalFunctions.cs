using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MedicalSolution
{   
    public static class MedicalFunctions
    {
        
        [FunctionName("CreateMedicine")]
        public static async Task<IActionResult> CreateMedicine(
             [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "task")] HttpRequest req, ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<CreateModel>(requestBody);
            string SqlConnectionString ="Server=tcp:medicinesystem.database.windows.net,1433;Initial Catalog=MedicineSystem;Persist Security Info=False;User ID=shital;Password=Fujitsu@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            try
            {
                using (SqlConnection connection = new SqlConnection(SqlConnectionString))
                {
                    connection.Open();
                    if (String.IsNullOrEmpty(input.Name))
                    {
                        var query = $"INSERT INTO [MedicineTable] (Name,Count,Location,ProviderType) VALUES('{input.Name}', '{input.Count}' , '{input.Location}' , '{input.ProviderType}')";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                return new BadRequestResult();
            }
            return new OkResult();
        }

        [FunctionName("GetMedicine")]
        public static async Task<IActionResult> GetMedicine(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "task")] HttpRequest req, ILogger log)
        {
            string SqlConnectionString ="Server=tcp:medicinesystem.database.windows.net,1433;Initial Catalog=MedicineSystem;Persist Security Info=False;User ID=shital;Password=Fujitsu@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            List<MedicalModel> medialmodel = new List<MedicalModel>();
            try
            {
                using (SqlConnection connection = new SqlConnection(SqlConnectionString))
                {
                    connection.Open();
                    var query = @"Select * from MedicineTable";
                    SqlCommand command = new SqlCommand(query, connection);
                    var reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        MedicalModel task = new MedicalModel()
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            Count = (int)reader["Count"],
                            Location = reader["Location"].ToString(),
                            ProviderType = reader["ProviderType"].ToString()
                        };
                        medialmodel.Add(task);
                    }
                }
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
            }
            if (medialmodel.Count > 0)
            {
                return new OkObjectResult(medialmodel);
            }
            else
            {
                return new NotFoundResult();
            }
        }

        [FunctionName("GetMedicineByName")]
        public static IActionResult GetMedicineByName(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "medicine/{name}")] HttpRequest req, ILogger log, string name)
        {
            string SqlConnectionString ="Server=tcp:medicinesystem.database.windows.net,1433;Initial Catalog=MedicineSystem;Persist Security Info=False;User ID=shital;Password=Fujitsu@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(SqlConnectionString))
                {
                    connection.Open();
                    var query = @"Select * from MedicineTable Where name like '%@name%'";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@name", name);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dt);
                }
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
            }
            if (dt.Rows.Count == 0)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(dt);
        }

        [FunctionName("UpdateMedicine")]
        public static async Task<IActionResult> UpdateMedicine(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "task/{id}")] HttpRequest req, ILogger log, int id)
        {
            string SqlConnectionString ="Server=tcp:medicinesystem.database.windows.net,1433;Initial Catalog=MedicineSystem;Persist Security Info=False;User ID=shital;Password=Fujitsu@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<UpdateModel>(requestBody);
            try
            {
                using (SqlConnection connection = new SqlConnection(SqlConnectionString))
                {
                    connection.Open();
                    var query = @"Update TaskList Set Description = @Description , IsDone = @IsDone Where Id = @Id";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Name", input.Name);
                    command.Parameters.AddWithValue("@Count", input.Count);
                    command.Parameters.AddWithValue("@Location", input.Location);
                    command.Parameters.AddWithValue("@Count", input.Count);
                    command.Parameters.AddWithValue("@ProviderType", input.ProviderType);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
            }
            return new OkResult();
        }
    }
}