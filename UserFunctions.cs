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
    public static class UserFunctions
    {
        static string SqlConnectionString ="Server=tcp:medicinesystem.database.windows.net,1433;Initial Catalog=MedicineSystem;Persist Security Info=False;User ID=shital;Password=Fujitsu@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        
        //[FunctionName("RegisterUser")]
        //public static async Task<IActionResult> RegisterUser(
        //     [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "RegisterUser")] HttpRequest req, ILogger log)
        //{
        //    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //    var input = JsonConvert.DeserializeObject<CreateModel>(requestBody);
        //    //string SqlConnectionString ="Server=tcp:medicinesystem.database.windows.net,1433;Initial Catalog=MedicineSystem;Persist Security Info=False;User ID=shital;Password=Fujitsu@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        //   // string SqlConnectionString ="Server=tcp:medicinesystem.database.windows.net,1433;Initial Catalog=MedicineSystem;Persist Security Info=False;User ID=shital;Password=Fujitsu@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        //    SqlConnection connection = new SqlConnection(SqlConnectionString);
        //    try
        //    {
        //        using (connection)
        //        {
        //            connection.Open();
        //            //if (String.IsNullOrEmpty(input.Name))
        //            //{
        //                var query = $"INSERT INTO [UserTable] (Name,Count,Location,ProviderType, StoreName,BatchID) VALUES('{input.Name}', '{input.Count}' , '{input.Location}' , '{input.ProviderType}' , '{input.StoreName}' , '{input.BatchID}')";
        //                SqlCommand command = new SqlCommand(query, connection);
        //                command.ExecuteNonQuery();
        //            //}
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        log.LogError(e.ToString());
        //        return new BadRequestResult();
        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }
        //    return new OkResult();
        //}

        [FunctionName("GetUserByNamePass")]
        public static async Task<IActionResult> GetUserByNamePass(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user")] HttpRequest req, ILogger log, string name, string password)
        {
            //string SqlConnectionString ="Server=tcp:medicinesystem.database.windows.net,1433;Initial Catalog=MedicineSystem;Persist Security Info=False;User ID=shital;Password=Fujitsu@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            List<UserModel> Usermodel = new List<UserModel>();
            SqlConnection connection = new SqlConnection(SqlConnectionString);
            try
            {
                using (connection)
                {
                    connection.Open();
                    var query = @"Select * from UserTable Where name = '@name' and Password= '@Password'";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@Password", password);
                    var reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        UserModel task = new UserModel()
                        {
                            
                            Name = reader["Name"].ToString(),                           
                            Password = reader["Location"].ToString()                           
                        };
                        Usermodel.Add(task);
                    }
                }
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
            }
            finally
            {
                connection.Close();
            }
            if (Usermodel.Count > 0)
            {
                return new OkObjectResult(Usermodel);
            }
            else
            {
                return new NotFoundResult();
            }
            
        }

        //[FunctionName("GetMedicineByName")]
        //public static IActionResult GetMedicineByName(
        //[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "medicine/{name}")] HttpRequest req, ILogger log, string name)
        //{
        //   // string SqlConnectionString ="Server=tcp:medicinesystem.database.windows.net,1433;Initial Catalog=MedicineSystem;Persist Security Info=False;User ID=shital;Password=Fujitsu@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        //    DataTable dt = new DataTable();
        //    SqlConnection connection = new SqlConnection(SqlConnectionString);
        //    try
        //    {
        //        using (connection)
        //        {
        //            connection.Open();
        //            var query = @"Select * from MedicineTable Where name like '%@name%'";
        //            SqlCommand command = new SqlCommand(query, connection);
        //            command.Parameters.AddWithValue("@name", name);
        //            SqlDataAdapter da = new SqlDataAdapter(command);
        //            da.Fill(dt);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        log.LogError(e.ToString());
        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }
        //    if (dt.Rows.Count == 0)
        //    {
        //        return new NotFoundResult();
        //    }
        //    return new OkObjectResult(dt);
        //}

        //[FunctionName("UpdateMedicine")]
        //public static async Task<IActionResult> UpdateMedicine(
        //[HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "medicine/{id}")] HttpRequest req, ILogger log, int id)
        //{
        //    //string SqlConnectionString ="Server=tcp:medicinesystem.database.windows.net,1433;Initial Catalog=MedicineSystem;Persist Security Info=False;User ID=shital;Password=Fujitsu@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        //    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //    var input = JsonConvert.DeserializeObject<UpdateModel>(requestBody);
        //    SqlConnection connection = new SqlConnection(SqlConnectionString);
        //    try
        //    {
        //        using (connection)
        //        {
        //            connection.Open();
        //            var query = @"Update MedicineTable Set Name = @Name , Count = @Count, Location=@Location, ProviderType=@ProviderType, StoreName=@StoreName, BatchID=@BatchID  Where Id = @Id";
        //            SqlCommand command = new SqlCommand(query, connection);
        //            command.Parameters.AddWithValue("@Name", input.Name);
        //            command.Parameters.AddWithValue("@Count", input.Count);
        //            command.Parameters.AddWithValue("@Location", input.Location);                    
        //            command.Parameters.AddWithValue("@ProviderType", input.ProviderType);
        //            command.Parameters.AddWithValue("@StoreName", input.StoreName);
        //            command.Parameters.AddWithValue("@BatchID", input.BatchID);
        //            command.ExecuteNonQuery();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        log.LogError(e.ToString());
        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }
        //    return new OkResult();
        //}
    }
}
