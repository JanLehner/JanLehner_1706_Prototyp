using JanLehner_Backend_Prototyp_1706.Models;
using Microsoft.AspNetCore.Mvc;

namespace JanLehner_Backend_Prototyp_1706.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExitController : ControllerBase
    {
        [HttpPost]
        public async Task<string> ExitACar(string imagePath)
        {
            try
            {
                string result = await CarsModel.ExitACar(imagePath);
                return result;
            }
            catch
            {
                return "Schranke geschlossen halten";
            }
        }
    }
}
