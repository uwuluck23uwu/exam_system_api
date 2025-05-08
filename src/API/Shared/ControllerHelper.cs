using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using EXAM_SYSTEM_API.Application.Shared.Responses;

namespace EXAM_SYSTEM_API.API.Shared;

/// <summary>
/// ��Ǫ���
/// </summary>
public class ControllerHelper
{
    private readonly IHostEnvironment _hostEnvironment;
    private readonly EncryptJsonResponse _encrypt;

    /// <summary>
    /// constructor
    /// </summary>
    public ControllerHelper(IHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment;
        _encrypt = new EncryptJsonResponse();
    }

    /// <summary>
    /// ���¡��ҹ
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="action"></param>
    /// <returns></returns>
    public async Task<IActionResult> HandleRequest<T>(Func<Task<T>> action) where T : ResponseBase
    {
        try
        {
            var response = await action();
            return CreateResponse(response);
        }
        catch (Exception e)
        {
            return new BadRequestObjectResult(new ResponseErrorMessages(400, false, e.Message));
        }
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="response"></param>
    /// <returns></returns>
    public IActionResult CreateResponse<T>(T response) where T : ResponseBase
    {
        if (_hostEnvironment.IsDevelopment() || true)
        {
            return new ObjectResult(response) { StatusCode = response.StatusCode };
        }

        return new ObjectResult(_encrypt.EncryptJson(response, "Vm14U1ExWXhVblJXYkZwT1ZsWmFWVlpyVmtaUFVUMDk="))
        {
            StatusCode = response.StatusCode
        };
    }
}
