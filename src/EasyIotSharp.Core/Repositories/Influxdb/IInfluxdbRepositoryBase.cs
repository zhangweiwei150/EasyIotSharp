using InfluxData.Net.Common.Enums;
using InfluxData.Net.InfluxDb;
using InfluxData.Net.InfluxDb.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UPrime.Domain.Entities;

namespace EasyIotSharp.Core.Repositories.Influxdb
{
    public interface IInfluxdbRepositoryBase<TEntity, in TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>, new()
    {
        
    }
}
