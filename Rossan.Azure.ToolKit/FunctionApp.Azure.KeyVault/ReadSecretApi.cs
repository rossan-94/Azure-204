using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Rossan.Azure.KeyVault;

namespace FunctionApp.Azure.KeyVault
{
    public class ReadSecretApi
    {

        private readonly IKeyVaultRepository _keyVaultRepository;

        public ReadSecretApi(IKeyVaultRepository keyVaultRepository)
        {
            _keyVaultRepository = keyVaultRepository;
        }

        [FunctionName("ReadSecretAsync")]
        public async Task<IActionResult> ReadSecretAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            try
            {

                string secretName = req.Query["secretname"];

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                secretName = secretName ?? data?.secretName;
                var secret = await _keyVaultRepository.GetSecretAsync(secretName);

                return new OkObjectResult(secret.Value);
            }
            catch(Exception ex)
            {
                log.LogError(ex.Message);
                return new BadRequestObjectResult("get_secret_fail");
            }
        }
    }
}
