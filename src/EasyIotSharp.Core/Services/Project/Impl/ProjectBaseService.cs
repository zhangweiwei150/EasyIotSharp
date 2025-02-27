using EasyIotSharp.Core.Dto.Project.Params;
using EasyIotSharp.Core.Dto.Project;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UPrime.Services.Dto;
using EasyIotSharp.Core.Repositories.Project;
using EasyIotSharp.Core.Dto.Tenant;
using UPrime.AutoMapper;
using EasyIotSharp.Core.Dto.TenantAccount.Params;
using EasyIotSharp.Core.Domain.Proejct;

namespace EasyIotSharp.Core.Services.Project.Impl
{
    public class ProjectBaseService:ServiceBase, IProjectBaseService
    {
        private readonly IProjectBaseRepository _projectBaseRepository;

        public ProjectBaseService(IProjectBaseRepository projectBaseRepository)
        {
            _projectBaseRepository = projectBaseRepository;
        }

        public async Task<ProjectBaseDto> GetProjectBase(string id)
        {
            var info = await _projectBaseRepository.FirstOrDefaultAsync(x => x.Id == id && x.IsDelete == false);
            return info.MapTo<ProjectBaseDto>();
        }

        public async Task<PagedResultDto<ProjectBaseDto>> QueryProjectBase(QueryProjectBaseInput input)
        {

        }

        public async Task InsertProjectBase(InsertProjectBaseInput input)
        {
            var info = await _projectBaseRepository.FirstOrDefaultAsync(x => x.TenantNumId == ContextUser.TenantNumId && x.Name == input.Name && x.IsDelete == false);
            if (info.IsNotNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "项目名称重复");
            }
            int numId = (await _projectBaseRepository.CountAsync()) + 1;
            var model = new ProjectBase();
            model.Id = Guid.NewGuid().ToString().Replace("-", "");
            model.TenantNumId = numId;
            model.Name = input.Name;
            model.Longitude = input.Longitude;
            model.latitude = input.latitude;
            model.State = 0;
            model.Address = input.Address;
            model.Remark=input.Remark;
            model.IsDelete = false;
            model.CreationTime = DateTime.Now;
            model.UpdatedAt = DateTime.Now;
            model.OperatorId = ContextUser.UserId;
            model.OperatorName = ContextUser.UserName;
            await _projectBaseRepository.InsertAsync(model);
        }
    }
}
