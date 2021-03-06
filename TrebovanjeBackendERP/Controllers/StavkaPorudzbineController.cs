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
    [Route("api/stavkaPorudzbine")]
    [Produces("application/json", "application/xml")]
    public class StavkaPorudzbineController : ControllerBase
    {
        private readonly IStavkaPorudzbineRepository stavkaPorRepository;
        public readonly IProizvodRepository proizvodRepository;
        public readonly IPorudzbinaRepository porudzbinaRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;



        public StavkaPorudzbineController(IPorudzbinaRepository porudzbinaRepository, IStavkaPorudzbineRepository stavkaPorRepository,IProizvodRepository proizvodRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            this.stavkaPorRepository = stavkaPorRepository;
            this.proizvodRepository = proizvodRepository;
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
        public ActionResult<List<StavkaPorudzbine>> GetStavkePorudzbine()
        {
           

            List<StavkaPorudzbine> stavke = stavkaPorRepository.GetStavkePorudzbine();
            if (stavke.Count == 0 || stavke == null)
            {
                return NoContent();
            }
           
            return Ok(stavke);
        }

        [HttpGet("stavkeByPorudzbinaId/{porId}")]
        [Authorize]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<List<StavkaPorudzbine>> GetStavkePorudzbineByPorudzbinaId(int porId)
        {


            List<StavkaPorudzbine> stavke = stavkaPorRepository.GetStavkeByPorudzbinaId(porId);

            foreach(StavkaPorudzbine s in stavke)
            {
                s.Proizvod = proizvodRepository.GetProizvodById(s.ProizvodId);
            }

            if (stavke.Count == 0 || stavke == null)
            {
                return NoContent();
            }

            return Ok(stavke);
        }

        [HttpGet("iznosPorudzbine/{porId}")]
        [Authorize]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<float> GetIznosPorudzbineByPorudzbinaId(int porId)
        {


            float iznos = stavkaPorRepository.GetIznosPorudzbineByPorudzbinaId(porId);

            if (iznos == 0)
            {
                return NoContent();
            }

            return Ok(iznos);
        }


        [HttpGet("{stavkaPorId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<StavkaPorudzbine> GetStavkaPorudzbine(int stavkaPorId)
        {

            StavkaPorudzbine stavka = stavkaPorRepository.GetStavkaPorudzbineById(stavkaPorId);
            if (stavka == null)
            {
                return NotFound();
            }
   
            return Ok(stavka);
        }


        
        [HttpPost]
        [Authorize]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StavkaPorudzbine> CreateStavkaPorudzbine([FromBody] StavkaPorudzbine stavka)
        {

            try
            {
                int dostupnaKol = proizvodRepository.GetDostupnaKolicina(stavka.ProizvodId);

                if(stavka.Kolicina <= dostupnaKol)
                {
                    StavkaPorudzbine createdStavka = stavkaPorRepository.CreateStavkaPorudzbine(stavka);

                    Proizvod proizv = proizvodRepository.GetProizvodById(stavka.ProizvodId);

                    proizv.DostupnaKolicina -= stavka.Kolicina;

                    proizvodRepository.UpdateProizvod(proizv);

                  //  Porudzbina por = porudzbinaRepository.GetPorudzbinaById(stavka.PorudzbinaId);

                  //  float iznos = proizv.Cena * stavka.Kolicina;

                   // por.Iznos += iznos;

                 //   porudzbinaRepository.UpdatePorudzbina(por);

                    string location = linkGenerator.GetPathByAction("GetStavkaPorudzbine", "StavkaPorudzbine", new { stavkaPorId = stavka.StavkaPorudzbineId });

                    return Created(location, createdStavka);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Ordered quantity error");
                }

              
            }
            catch
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError, "Create stavka porudzbine Error");

            }

        }

        
        [HttpDelete("{stavkaPorId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteStavkaPorudzbine(int stavkaPorId)
        {
           
            try
            {
                StavkaPorudzbine stavka = stavkaPorRepository.GetStavkaPorudzbineById(stavkaPorId);
                if (stavka == null)
                {
                    return NotFound();
                }
                stavkaPorRepository.DeleteStavkaPorudzbine(stavkaPorId);

                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete stavka porudzbine Error");
            }
        }

        
        [HttpPut]
        [Authorize]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StavkaPorudzbine> UpdateStavkaPorudzbine(StavkaPorudzbine stavka)
        {
           
            try
            {

                var oldStavka = stavkaPorRepository.GetStavkaPorudzbineById(stavka.StavkaPorudzbineId);


                if (oldStavka == null)
                {
                    return NotFound();
                }


                oldStavka.Kolicina = stavka.Kolicina;
                oldStavka.PorudzbinaId = stavka.PorudzbinaId;
                oldStavka.ProizvodId = stavka.ProizvodId;
          
                stavkaPorRepository.SaveChanges();

                return Ok(oldStavka);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update stavka porudzbine error");
            }
        }

       
    }
}
