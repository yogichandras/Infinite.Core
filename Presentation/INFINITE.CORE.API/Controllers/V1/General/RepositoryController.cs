using Microsoft.AspNetCore.Mvc;
using INFINITE.CORE.Shared.Attributes;
using INFINITE.CORE.Core.Repository.Query;
using INFINITE.CORE.Core.Request;
using INFINITE.CORE.Core.Repository.Command;

namespace INFINITE.CORE.API.Controllers
{
    public partial class RepositoryController : BaseController<RepositoryController>
    {
        [HttpPost(template: "upload")]
        public async Task<IActionResult> Upload([FromBody] RepositoryRequest request)
        {
            var upload_request = _mapper.Map<UploadRepositoryRequest>(request);
            upload_request.Inputer = Inputer;
            return Wrapper(await _mediator.Send(upload_request));
        }

        [HttpDelete(template: "delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Wrapper(await _mediator.Send(new DeleteRepositoryRequest() { Id = id, Inputer = Inputer }));
        }

        [HttpGet(template: "download/{id}")]
        public async Task<IActionResult> Download(Guid id)
        {
            return Wrapper(await _mediator.Send(new DownloadRepositoryRequest() { Id = id, Inputer = Inputer }));
        }
        [HttpPost(template: "list")]
        public async Task<IActionResult> List([FromBody] ListRequest request)
        {
            var list_request = _mapper.Map<GetRepositoryListRequest>(request);
            return Wrapper(await _mediator.Send(list_request));
        }
    }
}
