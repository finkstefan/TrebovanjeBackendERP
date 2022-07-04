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

namespace TrebovanjeBackendERP.Controllers
{
   
    [ApiController]
    [Route("api/lokacija")]
    [Produces("application/json", "application/xml")]
    public class LokacijaController : ControllerBase
    {
        private readonly ILokacijaRepository lokacijaRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;



        public LokacijaController(ILokacijaRepository lokacijaRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            this.lokacijaRepository = lokacijaRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
           
        }



        [HttpGet]
        [HttpHead]
      //  [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<List<Lokacija>> GetLokacije()
        {
           

            List<Lokacija> lokacije = lokacijaRepository.GetLokacije();
            if (lokacije.Count == 0 || lokacije == null)
            {
                return NoContent();
            }
           
            return Ok(lokacije);
        }

     
        [HttpGet("{LokacijaId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Lokacija> GetLokacija(int lokacijaId)
        {

            Lokacija lok = lokacijaRepository.GetLokacijaById(lokacijaId);
            if (lok == null)
            {
                return NotFound();
            }
   
            return Ok(lok);
        }


        
        [HttpPost]
        [Consumes("application/json")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Lokacija> CreateLokacija([FromBody] Lokacija lokacija)
        {

            try
            {

                Lokacija createdLokacija = lokacijaRepository.CreateLokacija(lokacija);

                string location = linkGenerator.GetPathByAction("GetLokacija", "Lokacija", new { lokacijaId = lokacija.LokacijaId });

                return Created(location, createdLokacija);
            }
            catch
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError, "Create lokacija Error");

            }

        }

        
        [HttpDelete("{LokacijaId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteLokacija(int lokacijaId)
        {
           
            try
            {
                Lokacija lokacija = lokacijaRepository.GetLokacijaById(lokacijaId);
                if (lokacija == null)
                {
                    return NotFound();
                }
                lokacijaRepository.DeleteLokacija(lokacijaId);

                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete lokacija Error");
            }
        }

        
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Lokacija> UpdateLokacija(Lokacija lok)
        {
           
            try
            {

                var oldLok = lokacijaRepository.GetLokacijaById(lok.LokacijaId);


                if (oldLok== null)
                {
                    return NotFound();
                }


                oldLok.LokacijaId = lok.LokacijaId;
                oldLok.Adresa = lok.Adresa;
                oldLok.Drzava = lok.Drzava;
                oldLok.Grad = lok.Grad;
           


                lokacijaRepository.SaveChanges();

                return Ok(oldLok);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update lokacija error");
            }
        }

       
    }
}
