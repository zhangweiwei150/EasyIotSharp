using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace EasyIotSharp.Core.Validator
{
    /// <summary>
    /// 验证MongoObjectId是否合法
    /// </summary>
    public class IsMongoObjectIdAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is null)
            {
                ErrorMessage = "Id不能为空";
                return false;
            }
            string id = value as string;
            ObjectId objId;
            if (ObjectId.TryParse(id, out objId))
            {
                return true;
            }
            ErrorMessage = "Mongodb ObjectId格式有误";
            return false;
        }
    }
}