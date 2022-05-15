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
    [Route("api/porudzbina")]
    [Produces("application/json", "application/xml")]
    public class PorudzbinaController : ControllerBase
    {
        private readonly IPorudzbinaRepository porudzbinaRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;



        public PorudzbinaController(IPorudzbinaRepository porudzbinaRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            this.porudzbinaRepository = porudzbinaRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
           
        }


        [HttpGet]
        [Authorize]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<List<Porudzbina>> GetPorudzbine()
        {
           

            List<Porudzbina> porudzbine = porudzbinaRepository.GetPorudzbine();
            if (porudzbine.Count == 0 || porudzbine == null)
            {
                return NoContent();
            }
           
            return Ok(porudzbine);
        }

        [HttpGet]
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


        [HttpGet("{PorudzbinaId}")]
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
        [Authorize]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Porudzbina> CreatePorudzbina([FromBody] Porudzbina porudzbina)
        {

            try
            {

                Porudzbina createdPorudzbina = porudzbinaRepository.CreatePorudzbina(porudzbina);

                string location = linkGenerator.GetPathByAction("GetPorudzbina", "Porudzbina", new { PorudzbinaId = porudzbina.PorudzbinaId });

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
