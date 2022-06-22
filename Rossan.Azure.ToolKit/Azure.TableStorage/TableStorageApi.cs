using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Rossan.Azure.TableStarage;
using Azure.Data.Model.AzureTable;
using System.Collections.Generic;

namespace Azure.TableStorage
{
    public class TableStorageApi
    {
        private readonly ITableStoreRepository _tableStoreRepository;

        public TableStorageApi(ITableStoreRepository tableStoreRepository)
        {
            _tableStoreRepository = tableStoreRepository;
        }

        [FunctionName("CreateAsync")]
        public async Task<IActionResult> CreateAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
        {
            try
            {
                string requestBody = string.Empty;
                using (StreamReader streamReader = new StreamReader(req.Body))
                {
                    requestBody = await streamReader.ReadToEndAsync();
                }

                var order = JsonConvert.DeserializeObject<Order>(requestBody);

                await _tableStoreRepository.CreateAsync<Order>(order, "Orders").ConfigureAwait(false);
                return new OkObjectResult(order);
            }
            catch(Exception ex)
            {
                log.LogError($"{ex.Message}");
                return new BadRequestObjectResult("error_in_create");
            }
        }

        [FunctionName("CreateEntityGroupAsync")]
        public async Task<IActionResult> CreateEntityGroupAsync(
           [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
        {
            try
            {
                string requestBody = string.Empty;
                using (StreamReader streamReader = new StreamReader(req.Body))
                {
                    requestBody = await streamReader.ReadToEndAsync();
                }

                var orders = JsonConvert.DeserializeObject<List<Order>>(requestBody);

                await _tableStoreRepository.CreateEntityGroupAsync<Order>(orders, "Orders").ConfigureAwait(false);
                return new OkObjectResult(orders);
            }
            catch (Exception ex)
            {
                log.LogError($"{ex.Message}");
                return new BadRequestObjectResult("error_in_createentitygroup"); ;
            }
        }

        [FunctionName("GetItemAsync")]
        public async Task<IActionResult> GetItemAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
        {
            try
            {
                string requestBody = string.Empty;
                using (StreamReader streamReader = new StreamReader(req.Body))
                {
                    requestBody = await streamReader.ReadToEndAsync();
                }

                var order = JsonConvert.DeserializeObject<Order>(requestBody);

                var result = await _tableStoreRepository.GetItemAsync<Order>(order.PartitionKey, order.RowKey, "Orders").ConfigureAwait(false);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                log.LogError($"{ex.Message}");
                return new BadRequestObjectResult("error_in_create");
            }
        }

        [FunctionName("GetAsync")]
        public async Task<IActionResult> GetAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
        {
            try
            {
                string requestBody = string.Empty;
                using (StreamReader streamReader = new StreamReader(req.Body))
                {
                    requestBody = await streamReader.ReadToEndAsync();
                }

                var order = JsonConvert.DeserializeObject<Order>(requestBody);

                var orders = await _tableStoreRepository.GetAsync<Order>(partitionKey: order.PartitionKey, pageSize: 1, tableName: "Orders").ConfigureAwait(false);
                return new OkObjectResult(orders);
            }
            catch (Exception ex)
            {
                log.LogError($"{ex.Message}");
                return new BadRequestObjectResult("error_in_create");
            }
        }

        [FunctionName("UpdateAsync")]
        public async Task<IActionResult> UpdateAsync(
           [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
        {
            try
            {
                string requestBody = string.Empty;
                using (StreamReader streamReader = new StreamReader(req.Body))
                {
                    requestBody = await streamReader.ReadToEndAsync();
                }

                var order = JsonConvert.DeserializeObject<Order>(requestBody);

                await _tableStoreRepository.UpdateAsync<Order>(order, tableName: "Orders").ConfigureAwait(false);
                return new OkObjectResult(order);
            }
            catch (Exception ex)
            {
                log.LogError($"{ex.Message}");
                return new BadRequestObjectResult("error_in_create");
            }
        }

        [FunctionName("UpdateEntityGroupAsync")]
        public async Task<IActionResult> UpdateEntityGroupAsync(
          [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
        {
            try
            {
                string requestBody = string.Empty;
                using (StreamReader streamReader = new StreamReader(req.Body))
                {
                    requestBody = await streamReader.ReadToEndAsync();
                }

                var orders = JsonConvert.DeserializeObject<List<Order>>(requestBody);

                await _tableStoreRepository.UpdateEntityGroupAsync<Order>(orders, tableName: "Orders").ConfigureAwait(false);
                return new OkObjectResult(orders);
            }
            catch (Exception ex)
            {
                log.LogError($"{ex.Message}");
                return new BadRequestObjectResult("error_in_create");
            }
        }

        [FunctionName("DeleteAsync")]
        public async Task<IActionResult> DeleteAsync(
          [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
        {
            try
            {
                string requestBody = string.Empty;
                using (StreamReader streamReader = new StreamReader(req.Body))
                {
                    requestBody = await streamReader.ReadToEndAsync();
                }

                var order = JsonConvert.DeserializeObject<Order>(requestBody);

                await _tableStoreRepository.DeleteAsync<Order>(order.PartitionKey, order.RowKey, tableName: "Orders").ConfigureAwait(false);
                return new OkObjectResult(order);
            }
            catch (Exception ex)
            {
                log.LogError($"{ex.Message}");
                return new BadRequestObjectResult("error_in_create");
            }
        }

        [FunctionName("DeleteEntityGroupAsync")]
        public async Task<IActionResult> DeleteEntityGroupAsync(
          [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
        {
            try
            {
                string requestBody = string.Empty;
                using (StreamReader streamReader = new StreamReader(req.Body))
                {
                    requestBody = await streamReader.ReadToEndAsync();
                }

                var orders = JsonConvert.DeserializeObject<List<Order>>(requestBody);

                await _tableStoreRepository.DeleteEntityGroupAsync<Order>(orders, tableName: "Orders").ConfigureAwait(false);
                return new OkObjectResult(orders);
            }
            catch (Exception ex)
            {
                log.LogError($"{ex.Message}");
                return new BadRequestObjectResult("error_in_create");
            }
        }
    }
}
