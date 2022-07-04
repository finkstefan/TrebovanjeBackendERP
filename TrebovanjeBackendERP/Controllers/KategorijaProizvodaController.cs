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
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace TrebovanjeBackendERP.Controllers
{
   
    [ApiController]
    [Route("api/kategorija")]
    [Produces("application/json", "application/xml")]
    public class KategorijaProizvodumController : ControllerBase
    {
        private readonly IKategorijaProizvodaRepository kategorijaRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;



        public KategorijaProizvodumController(IKategorijaProizvodaRepository kategorijaRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            this.kategorijaRepository = kategorijaRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
           
        }

        
        [HttpGet]
        [HttpHead]
      //  [Authorize(Roles ="Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<List<KategorijaProizvodum>> GetKategorije()
        {
           

            List<KategorijaProizvodum> kategorije = kategorijaRepository.GetKategorije();
            if (kategorije.Count == 0 || kategorije == null)
            {
                return NoContent();
            }
            
           
            return Ok(kategorije);
        }


        [HttpGet("{kategorijaId}")]
       // [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<KategorijaProizvodum> GetKategorija(int kategorijaId)
        {

            KategorijaProizvodum kat = kategorijaRepository.GetKategorijaById(kategorijaId);
            if (kat == null)
            {
                return NotFound();
            }
   
            return Ok(kat);
        }

       

     
        [HttpPost]
     //   [Authorize(Roles = "Admin")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<KategorijaProizvodum> CreateKategorija([FromBody] KategorijaProizvodum kategorija)
        {

            try
            {

                KategorijaProizvodum createdKategorija = kategorijaRepository.CreateKategorija(kategorija);

                string location = linkGenerator.GetPathByAction("GetKategorija", "KategorijaProizvodum", new { KategorijaId = kategorija.KategorijaId });

                return Created(location, createdKategorija);
            }
            catch
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError, "Create kategorija Error");

            }

        }

        
        [HttpDelete("{kategorijaId}")]
     //   [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteKategorija(int kategorijaId)
        {
           
            try
            {
                KategorijaProizvodum kat = kategorijaRepository.GetKategorijaById(kategorijaId);
                if (kat == null)
                {
                    return NotFound();
                }
                kategorijaRepository.DeleteKategorija(kategorijaId);

                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete admin/korisnik Error");
            }
        }

   


       
        [HttpPut]
        [Consumes("application/json")]
       // [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<KategorijaProizvodum> UpdateKategorija(KategorijaProizvodum kategorija)
        {
           
            try
            {

                var oldKat = kategorijaRepository.GetKategorijaById(kategorija.KategorijaId);


                if (oldKat== null)
                {
                    return NotFound();
                }


                oldKat.NazivKategorije = kategorija.NazivKategorije;
              

                kategorijaRepository.SaveChanges();

                return Ok(oldKat);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update kategorija error");
            }
        }

        
      

    }
}
