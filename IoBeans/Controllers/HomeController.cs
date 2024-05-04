using IoBeans.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace IoBeans.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IoBeansContext _dbcontext;

        public HomeController(ILogger<HomeController> logger, IoBeansContext dbcontext)
        {
            _logger = logger;
            _dbcontext = dbcontext;
        }

        //Gráfica de línea para temperatura a lo largo del tiempo
        public IActionResult Temperatura(DateTime? startDate, DateTime? endDate)
        {
            var query = from data in _dbcontext.SensorData
                        where data.Temperature != null && data.Humidity != null && data.SoilMoisture != null && data.Timestamp != null
                        select data;

            if (startDate != null)
            {
                query = query.Where(data => data.Timestamp >= startDate);
            }

            if (endDate != null)
            {
                query = query.Where(data => data.Timestamp <= endDate);
            }

            var sensorData = query.Select(data => new
            {
                Timestamp = data.Timestamp,
                Temperature = data.Temperature,
                Humidity = data.Humidity,
                SoilMoisture = data.SoilMoisture
            }).ToList();


            return StatusCode(StatusCodes.Status200OK, sensorData);
        }

        public IActionResult sensoresTemperatura()
        {

            List<SensorDatum> Lista = (from tbSensor in _dbcontext.SensorData
                                       group tbSensor by tbSensor.Temperature into grupo
                                       orderby grupo.Count() descending
                                       select new SensorDatum
                                       {
                                           Temperature = grupo.Key,
                                           SensorId = grupo.Count(),
                                       }).Take(4).ToList();

            return StatusCode(StatusCodes.Status200OK, Lista);
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
