using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using CosmosDb.Manager;
using System;
using Microsoft.Extensions.Logging;

namespace FunctionApp.Azure.CosmosDb
{
    public class CosmosDbOrderApi
    {
        private readonly OrderManager _orderManager;

        public CosmosDbOrderApi(OrderManager OrderManager)
        {
            _orderManager = OrderManager;
        }

        [FunctionName("CreateDatabaseAsync")]
        public async Task<IActionResult> CreateDatabaseAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            try
            {
                await _orderManager.CreateDatabaseAsync().ConfigureAwait(false);
                return new OkObjectResult("database_create_sucess");
            }
            catch
            {
                return new BadRequestObjectResult("database_create_fail");
            }
        }

        [FunctionName("CreateOrdersConatinerAsync")]
        public async Task<IActionResult> CreateOrdersConatinerAsync(
           [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger logger)
        {
            try
            {
                await _orderManager.CreateConatinerAsync().ConfigureAwait(false);
                return new OkObjectResult("container_create_sucess");
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return new BadRequestObjectResult("container_create_fail");
            }
        }

        [FunctionName("CreateOrderAsync")]
        public async Task<IActionResult> CreateOrderAsync(
           [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
        {
            try
            {
                string reuestBody = string.Empty;
                using (StreamReader streamReader = new StreamReader(req.Body))
                {
                    reuestBody = await streamReader.ReadToEndAsync();
                }
                var order = JsonConvert.DeserializeObject<Order>(reuestBody);
                await _orderManager.CreateAsync(order).ConfigureAwait(false);
                return new OkObjectResult("order_create_sucess");
            }
            catch
            {
                return new BadRequestObjectResult("order_create_fail");
            }
        }

        [FunctionName("UpdateAsync")]
        public async Task<IActionResult> UpdateAsync(
          [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
        {
            try
            {
                string reuestBody = string.Empty;
                using (StreamReader streamReader = new StreamReader(req.Body))
                {
                    reuestBody = await streamReader.ReadToEndAsync();
                }
                var order = JsonConvert.DeserializeObject<Order>(reuestBody);
                await _orderManager.UpdateAsync(order, order.Id).ConfigureAwait(false);
                return new OkObjectResult("order_update_sucess");
            }
            catch
            {
                return new BadRequestObjectResult("order_update_fail");
            }
        }

        [FunctionName("GetItems")]
        public IActionResult GetItems(
         [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
        {
            try
            {
                 var orders = _orderManager.GetItems();
                return new OkObjectResult(orders);
            }
            catch
            {
                return new BadRequestObjectResult("order_getitems_fail");
            }
        }

        [FunctionName("Get")]
        public IActionResult Get(
         [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
        {
            try
            {
                string id = "4ff32abc-ae33-4df1-81a7-8a8148cd9878";
                var order = _orderManager.Get(id);
                return new OkObjectResult(order);
            }
            catch
            {
                return new BadRequestObjectResult("order_get_fail");
            }
        }

        [FunctionName("DeleteAsync")]
        public async Task<IActionResult> DeleteAsync(
         [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
        {
            try
            {
                string id = "4ff32abc-ae33-4df1-81a7-8a8148cd9878";
                var order = await _orderManager.DeleteAsync(id);
                return new OkObjectResult(order);
            }
            catch(Exception ex)
            {
                return new BadRequestObjectResult("order_delete_fail");
            }
        }
    }
}
