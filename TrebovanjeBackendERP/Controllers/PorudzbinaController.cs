using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using System.Net;
using TrebovanjeBackendERP.Repositories;
using TrebovanjeBackendERP.Entities;
using Microsoft.AspNetCore.Authorization;
using Stripe.Checkout;
using Stripe;
using System.IO;

namespace TrebovanjeBackendERP.Controllers
{
   
    [ApiController]
    [Route("api/porudzbina")]
    [Produces("application/json", "application/xml")]
    public class PorudzbinaController : ControllerBase
    {
        private readonly IPorudzbinaRepository porudzbinaRepository;
        private readonly IStavkaPorudzbineRepository stavkaPorRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;

        private readonly string WebhookSecret = "whsec_OurSigningSecret";

        public PorudzbinaController(IPorudzbinaRepository porudzbinaRepository, IStavkaPorudzbineRepository stavkaPorRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            this.porudzbinaRepository = porudzbinaRepository;
            this.stavkaPorRepository = stavkaPorRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
           
        }

        [HttpPost("stripePayment")]
        public void Processing([FromBody] float amount)
        {
            Dictionary<string, string> Metadata = new Dictionary<string, string>();
     
            var options = new ChargeCreateOptions
            {
                Amount = (long)amount*100,
                Currency = "EUR",
                Description = "Uplata za porudzbinu",
                Source = "tok_visa",
               // ReceiptEmail = stripeEmail,
                Metadata = Metadata
            };
            var service = new ChargeService();
            Charge charge = service.Create(options);
            
        }

        [HttpPost]
        public IActionResult ChargeChange()
        {
            var json = new StreamReader(HttpContext.Request.Body).ReadToEnd();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], WebhookSecret, throwOnApiVersionMismatch: true);
                Charge charge = (Charge)stripeEvent.Data.Object;
                switch (charge.Status)
                {
                    case "succeeded":
                      /*  //This is an example of what to do after a charge is successful
                        charge.Metadata.TryGetValue("Product", out string Product);
                        charge.Metadata.TryGetValue("Quantity", out string Quantity);
                        Database.ReduceStock(Product, Quantity);*/
                      
                        //ovde treba smanjiti kol na zalihama i checkirati da je porudzbina isplacine i eventualno brisati je 

                        break;
                    case "failed":
                        //Code to execute on a failed charge
                        break;
                }
            }
            catch (Exception e)
            {
               // e.Ship(HttpContext);
                return BadRequest();
            }
            return Ok();
        }

        [HttpGet]
       // [Authorize]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<List<Porudzbina>> GetPorudzbine()
        {
           

            List<Porudzbina> porudzbine = porudzbinaRepository.GetPorudzbine();

            foreach(Porudzbina por in porudzbine)
            {
                por.Iznos = stavkaPorRepository.GetIznosPorudzbineByPorudzbinaId(por.PorudzbinaId);
            }

            if (porudzbine.Count == 0 || porudzbine == null)
            {
                return NoContent();
            }
           
            return Ok(porudzbine);
        }

        [HttpGet("neisplacene")]
        [Authorize(Roles = "Admin")]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<List<Porudzbina>> GetNeisplacenePorudzbine()
        {


            List<Porudzbina> porudzbine = porudzbinaRepository.GetNeisplacenePorudzbine();
            if (porudzbine.Count == 0 || porudzbine == null)
            {
                return NoContent();
            }

            return Ok(porudzbine);
        }


        [HttpGet("{porudzbinaId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Porudzbina> GetPorudzbina(int porudzbinaId)
        {

            Porudzbina por = porudzbinaRepository.GetPorudzbinaById(porudzbinaId);
            if (por == null)
            {
                return NotFound();
            }
   
            return Ok(por);
        }


        
        [HttpPost]
      //  [Authorize]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Porudzbina> CreatePorudzbina([FromBody] Porudzbina porudzbina)
        {

            try
            {

                Porudzbina createdPorudzbina = porudzbinaRepository.CreatePorudzbina(porudzbina);

                string location = linkGenerator.GetPathByAction("GetPorudzbina", "Porudzbina", new { porudzbinaId = porudzbina.PorudzbinaId });

                return Created(location, createdPorudzbina);
            }
            catch
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError, "Create porudzbina Error");

            }

        }

       
        [HttpDelete("{PorudzbinaId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeletePorudzbina(int porudzbinaId)
        {
           
            try
            {
                Porudzbina por = porudzbinaRepository.GetPorudzbinaById(porudzbinaId);
                if (por== null)
                {
                    return NotFound();
                }
               porudzbinaRepository.DeletePorudzbina(porudzbinaId);

                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete porudzbina Error");
            }
        }

        
        [HttpPut]
        [Authorize]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Porudzbina> UpdatePorudzbina(Porudzbina porudzbina)
        {
           
            try
            {

                var oldPor = porudzbinaRepository.GetPorudzbinaById(porudzbina.PorudzbinaId);


                if (oldPor== null)
                {
                    return NotFound();
                }


                oldPor.Datum = porudzbina.Datum;
                oldPor.DistributerId = porudzbina.DistributerId;
                oldPor.Isplacena = porudzbina.Isplacena;
           


                porudzbinaRepository.SaveChanges();

                return Ok(oldPor);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update porudzbina error");
            }
        }

       
    }
}
