using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core.Flux.Domain;
using InfluxDB.Client.Writes;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace EasyIotSharp.Core.Repositories.Influxdb
{
    public class InfluxdbRepositoryBase<TEntity, TPrimaryKey> : IInfluxdbRepositoryBase<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>, new()
    {
        //public IInfluxDBClient Client;
        //public string Bucket;
        //public string Org;
        //public string _measurementName;

        //public InfluxdbRepositoryBase(IInfluxdbDatabaseProvider databaseProvider)
        //{
        //    Client = databaseProvider.Client;
        //    _bucket = databaseProvider.Bucket;
        //    _org = org ?? throw new ArgumentNullException(nameof(org));

        //    // 获取测量名称，可以使用自定义属性或类名
        //    var measurementAttr = typeof(TEntity).GetCustomAttribute<MeasurementAttribute>();
        //    _measurementName = measurementAttr?.Name ?? typeof(TEntity).Name;
        //}

        //public async Task<bool> WriteAsync(TEntity entity, WritePrecision precision = WritePrecision.Ns)
        //{
        //    try
        //    {
        //        var point = ConvertToPointData(entity, precision);
        //        using var writeApi = _client.GetWriteApi();
        //        writeApi.WritePoint(point, _bucket, _org);
        //        return await Task.FromResult(true);
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //public async Task<bool> WriteManyAsync(IEnumerable<TEntity> entities, WritePrecision precision = WritePrecision.Ns)
        //{
        //    try
        //    {
        //        var points = entities.Select(e => ConvertToPointData(e, precision)).ToList();
        //        using var writeApi = _client.GetWriteApi();
        //        writeApi.WritePoints(points, _bucket, _org);
        //        return await Task.FromResult(true);
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //public async Task<List<TEntity>> QueryByTimeRangeAsync(DateTime start, DateTime stop)
        //{
        //    var query = $@"from(bucket: ""{_bucket}"")
        //    |> range(start: {start.ToUniversalTime():o}, stop: {stop.ToUniversalTime():o})
        //    |> filter(fn: (r) => r._measurement == ""{_measurementName}"")";

        //    return await QueryByFluxAsync(query);
        //}

        //public async Task<List<TEntity>> QueryByFluxAsync(string fluxQuery)
        //{
        //    var tables = await _client.GetQueryApi().QueryAsync(fluxQuery, _org);
        //    var result = new List<TEntity>();

        //    foreach (var table in tables)
        //    {
        //        foreach (var record in table.Records)
        //        {
        //            var entity = ConvertToEntity(record);
        //            if (entity != null)
        //            {
        //                result.Add(entity);
        //            }
        //        }
        //    }

        //    return result;
        //}

        //public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, (DateTime Start, DateTime End)? timeRange = null)
        //{
        //    var query = BuildBaseQuery(timeRange);
        //    // 这里简化处理，实际应用中需要将表达式转换为Flux查询
        //    var entities = await QueryByFluxAsync(query);
        //    return entities.FirstOrDefault(predicate.Compile());
        //}

        //public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, (DateTime Start, DateTime End)? timeRange = null)
        //{
        //    var query = BuildBaseQuery(timeRange);
        //    var entities = await QueryByFluxAsync(query);
        //    return entities.Where(predicate.Compile()).ToList();
        //}

        //public async Task<(long TotalCount, List<TEntity> Items)> GetPagedListAsync(
        //    Expression<Func<TEntity, bool>> predicate,
        //    int pageIndex,
        //    int pageSize,
        //    (DateTime Start, DateTime End)? timeRange = null)
        //{
        //    var query = BuildBaseQuery(timeRange);
        //    var allEntities = await QueryByFluxAsync(query);
        //    var filtered = allEntities.Where(predicate.Compile()).ToList();

        //    var totalCount = filtered.Count;
        //    var items = filtered
        //        .Skip((pageIndex - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToList();

        //    return (totalCount, items);
        //}

        //public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, (DateTime Start, DateTime End)? timeRange = null)
        //{
        //    var query = BuildBaseQuery(timeRange);
        //    var entities = await QueryByFluxAsync(query);
        //    return entities.Any(predicate.Compile());
        //}

        //public async Task<long> CountAsync((DateTime Start, DateTime End)? timeRange = null)
        //{
        //    string start = timeRange?.Start.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ") ?? "0";
        //    string stop = timeRange?.End.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ") ?? "now()";

        //    var query = $@"from(bucket: ""{_bucket}"")
        //|> range(start: {start}, stop: {stop})
        //|> filter(fn: (r) => r._measurement == ""{_measurementName}"")
        //|> count()";

        //    var tables = await _client.GetQueryApi().QueryAsync(query, _org);
        //    return tables.SelectMany(t => t.Records).FirstOrDefault()?.GetValue() as long? ?? 0;
        //}

        //public async Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate, (DateTime Start, DateTime End)? timeRange = null)
        //{
        //    var query = BuildBaseQuery(timeRange);
        //    var entities = await QueryByFluxAsync(query);
        //    return entities.Count(predicate.Compile());
        //}

        //public async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate, (DateTime Start, DateTime End)? timeRange = null)
        //{
        //    try
        //    {
        //        // InfluxDB 的删除操作通常通过删除整个bucket或measurement来实现
        //        // 或者通过predicate构建删除条件
        //        // 这里简化处理，实际应用中需要更复杂的实现
        //        var query = BuildDeleteQuery(predicate, timeRange);
        //        await _client.GetDeleteApi().Delete(timeRange?.Start ?? DateTime.MinValue,
        //                                          timeRange?.End ?? DateTime.MaxValue,
        //                                          predicate.ToString(),
        //                                          _bucket,
        //                                          _org);
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //private PointData ConvertToPointData(TEntity entity, WritePrecision precision)
        //{
        //    var point = PointData.Measurement(_measurementName);

        //    // 设置时间戳（假设实体有Time属性）
        //    var timeProp = typeof(TEntity).GetProperty("Time");
        //    if (timeProp != null && timeProp.PropertyType == typeof(DateTime))
        //    {
        //        var timeValue = (DateTime)timeProp.GetValue(entity);
        //        point = point.Timestamp(timeValue, precision);
        //    }

        //    // 设置标签（Tag）
        //    var tagProperties = typeof(TEntity).GetProperties()
        //        .Where(p => p.GetCustomAttribute<TagAttribute>() != null);

        //    foreach (var prop in tagProperties)
        //    {
        //        var value = prop.GetValue(entity)?.ToString();
        //        if (!string.IsNullOrEmpty(value))
        //        {
        //            point = point.Tag(prop.Name, value);
        //        }
        //    }

        //    // 设置字段（Field）
        //    var fieldProperties = typeof(TEntity).GetProperties()
        //        .Where(p => p.GetCustomAttribute<FieldAttribute>() != null ||
        //                    (p.GetCustomAttribute<TagAttribute>() == null && p.Name != "Time"));

        //    foreach (var prop in fieldProperties)
        //    {
        //        var value = prop.GetValue(entity);
        //        if (value != null)
        //        {
        //            point = point.Field(prop.Name, value);
        //        }
        //    }

        //    return point;
        //}

        //private TEntity ConvertToEntity(FluxRecord record)
        //{
        //    var entity = new TEntity();
        //    var properties = typeof(TEntity).GetProperties();

        //    foreach (var prop in properties)
        //    {
        //        try
        //        {
        //            var value = record.GetValueByKey(prop.Name);
        //            if (value != null)
        //            {
        //                if (prop.PropertyType == typeof(DateTime) && value is DateTimeOffset offset)
        //                {
        //                    prop.SetValue(entity, offset.UtcDateTime);
        //                }
        //                else
        //                {
        //                    prop.SetValue(entity, Convert.ChangeType(value, prop.PropertyType));
        //                }
        //            }
        //        }
        //        catch
        //        {
        //            // 忽略转换错误
        //        }
        //    }

        //    return entity;
        //}

        //private string BuildBaseQuery((DateTime Start, DateTime End)? timeRange)
        //{
        //    string start = timeRange?.Start.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ") ?? "0";
        //    string stop = timeRange?.End.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ") ?? "now()";

        //    return $@"from(bucket: ""{_bucket}"")
        //|> range(start: {start}, stop: {stop})
        //|> filter(fn: (r) => r._measurement == ""{_measurementName}"")";
        //}

        //private string BuildDeleteQuery(Expression<Func<TEntity, bool>> predicate, (DateTime Start, DateTime End)? timeRange)
        //{
        //    // 这里简化处理，实际应用中需要将表达式转换为Flux删除条件
        //    return $@"_measurement=""{_measurementName}""";
        //}
    }

    // 辅助属性
    [AttributeUsage(AttributeTargets.Class)]
    public class MeasurementAttribute : Attribute
    {
        public string Name { get; }

        public MeasurementAttribute(string name)
        {
            Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class TagAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class FieldAttribute : Attribute
    {
    }

    public interface IEntity<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }
    }
}