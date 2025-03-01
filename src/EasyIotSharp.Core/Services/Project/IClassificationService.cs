using EasyIotSharp.Core.Dto.Project;
using EasyIotSharp.Core.Dto.Project.Params;
using System.Threading.Tasks;
using UPrime.Services.Dto;

namespace EasyIotSharp.Core.Services.Project
{
    public interface IClassificationService
    {
        /// <summary>
        /// 通过id获取一条项目分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ClassificationDto> GetClassification(string id);

        /// <summary>
        /// 根据条件分页查询项目分类列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ClassificationDto>> QueryClassification(QueryClassificationInput input);

        /// <summary>
        /// 添加一条项目分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task InsertClassification(InsertClassificationInput input);

        /// <summary>
        /// 通过id修改一条项目分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateClassification(UpdateClassificationInput input);

        /// <summary>
        /// 通过id删除一条项目分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteClassification(DeleteClassificationInput input);
    }
}
