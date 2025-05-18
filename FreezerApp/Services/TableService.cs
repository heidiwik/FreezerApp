using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using FreezerApp.Models;

namespace FreezerApp.Services
{
    public class TableService
    {
        private static TableClient GetTableStorageClient(IConfiguration config, ILogger log)
        {
            try
            {
                var client = new TableServiceClient(config["Storage:ConnectionString"]);

                var tableName = "FreezerItems";

                return client.GetTableClient(tableName);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error connecting to Azure Table Storage");
                throw;
            }
        }

        public static async Task<List<FreezerItem>> GetFreezerItemsAsync(IConfiguration config, ILogger log)
        {
            var tableClient = GetTableStorageClient(config, log);
            var items = new List<FreezerItem>();

            await foreach (var entity in tableClient.QueryAsync<FreezerItemEntity>())
            {
                if (entity != null && entity.Name != null)
                {
                    items.Add(new FreezerItem
                    {
                        Id = Guid.Parse(entity.RowKey),
                        BoxId = entity.BoxId,
                        Name = entity.Name,
                        Quantity = entity.Quantity,
                        Location = entity.Location,
                        StoreDate = entity.StoreDate
                    });
                }
            }

            return items;
        }

        public static async Task AddFreezerItemAsync(FreezerItem item, IConfiguration config, ILogger log)
        {
            var tableClient = GetTableStorageClient(config, log);

            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var entity = new FreezerItemEntity
            {
                PartitionKey = "items",
                RowKey = Guid.NewGuid().ToString(),
                BoxId = item.BoxId,
                Name = item.Name,
                Quantity = item.Quantity,
                Location = item.Location,
                StoreDate = item.StoreDate.Kind == DateTimeKind.Utc
                    ? item.StoreDate
                    : DateTime.SpecifyKind(item.StoreDate.ToUniversalTime(), DateTimeKind.Utc)
            };

            await tableClient.AddEntityAsync(entity);
        }

        public static async Task UpdateFreezerItemAsync(FreezerItem item, IConfiguration config, ILogger log)
        {
            var tableClient = GetTableStorageClient(config, log);
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            var entity = new FreezerItemEntity
            {
                PartitionKey = "items",
                RowKey = item.Id.ToString(),
                BoxId = item.BoxId,
                Name = item.Name,
                Quantity = item.Quantity,
                Location = item.Location,
                StoreDate = item.StoreDate
            };
            await tableClient.UpdateEntityAsync(entity, entity.ETag);
        }

        public static async Task DeleteFreezerItemAsync(Guid id, IConfiguration config, ILogger log)
        {
            var tableClient = GetTableStorageClient(config, log);
            FreezerItemEntity entity = new FreezerItemEntity
            {
                PartitionKey = "items",
                RowKey = id.ToString(),
            };
            await tableClient.DeleteEntityAsync(entity.PartitionKey, entity.RowKey);
        }
    }
}

public class FreezerItemEntity : ITableEntity
{
    public required string PartitionKey { get; set; }
    public required string RowKey { get; set; }
    public int? BoxId { get; set; }
    public string? Name { get; set; }
    public int Quantity { get; set; }
    public string? Location { get; set; }
    public DateTime StoreDate { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}
