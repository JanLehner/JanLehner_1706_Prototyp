using JanLehner_Backend_Prototyp_1706.Models;
using Microsoft.AspNetCore.Mvc;

namespace JanLehner_Backend_Prototyp_1706.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        [HttpPost]
        public string PayForACar(string numberPlate)
        {
            try
            {
                string result = CarsModel.PayForACar(numberPlate);
                return result;
            }
            catch
            {
                return "Kein Eintrag gefunden";
            }
        }
    }
}
