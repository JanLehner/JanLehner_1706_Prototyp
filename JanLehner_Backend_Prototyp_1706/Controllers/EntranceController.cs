using JanLehner_Backend_Prototyp_1706.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace JanLehner_Backend_Prototyp_1706.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntranceController : ControllerBase
    {
        [HttpPost]
        public async Task<string> EnterACar(string imagePath)
        {
            try
            {
                string result = await CarsModel.EnterACar(imagePath);
                return result;
            }
            catch
            {
                return "Schranke geschlossen halten";
            }
        }
    }
}