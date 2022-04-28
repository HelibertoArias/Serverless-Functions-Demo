using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace FunctionAppDemo01
{
    public static class CompanyApi
    {

        static List<Company> companies = new();

        [FunctionName("CreateCompany")]
        public static async Task<IActionResult> CreateCompany(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "companies")]
            HttpRequest req, ILogger log)
        {
            log.LogInformation("Creating a company");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var data = JsonConvert.DeserializeObject<CompanyCreateDto>(requestBody);

            var company = new Company() { Name = data.CompanyName };
            companies.Add(company);

            return new OkObjectResult(company);
        }


        [FunctionName("GetCompanies")]
        public static async Task<IActionResult> GetCompanies(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "companies")]
             HttpRequest req, ILogger log)
        {
            log.LogInformation("Getting all companies");

            await Task.FromResult(string.Empty);

            return new OkObjectResult(companies.Where(x => x.IsDeleted == false));
        }

        [FunctionName("GetCompanyById")]
        public static async Task<IActionResult> GetCompanyById(
           [HttpTrigger(AuthorizationLevel.Function, "get", Route = "companies/{id}")]
             HttpRequest req, ILogger log, string id)
        {
            log.LogInformation($"Get by id company {id}");

            var company = companies.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);

            await Task.FromResult(string.Empty);

            return company == null ? new NotFoundResult() : new OkObjectResult(company);
        }


        [FunctionName("UpdateCompany")]
        public static async Task<IActionResult> UpdateCompany(
          [HttpTrigger(AuthorizationLevel.Function, "put", Route = "companies/{id}")]
             HttpRequest req, ILogger log, string id)
        {
            log.LogInformation($"Updating company {id}");

            var company = companies.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);

            await Task.FromResult(string.Empty);

            if (company == null)
            {
                return new NotFoundResult();
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var companyUpdate = JsonConvert.DeserializeObject<CompanyUpdateDto>(requestBody);

            company.Name = companyUpdate.CompanyName;

            return new OkObjectResult(company);
        }


        [FunctionName("DeleteCompany")]
        public static async Task<IActionResult> DeleteCompany(
         [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "companies/{id}")]
             HttpRequest req, ILogger log, string id)
        {
            log.LogInformation($"Deleting company {id}");

            var company = companies.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);

            await Task.FromResult(string.Empty);

            if (company == null)
            {
                return new NotFoundResult();
            }

            companies.Remove(company);

            return new OkResult();
        }
    }
}
