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

namespace TrebovanjeBackendERP.Controllers
{
   
    [ApiController]
    [Route("api/proizvod")]
    [Produces("application/json", "application/xml")]
    public class ProizvodController : ControllerBase
    {
        private readonly IProizvodRepository proizvodRepository;
        private readonly IKategorijaProizvodaRepository kategorijaRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;



        public ProizvodController(IProizvodRepository proizvodRepository, IKategorijaProizvodaRepository kategorijaRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            this.proizvodRepository = proizvodRepository;
            this.kategorijaRepository = kategorijaRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
           
        }


        [HttpGet]
      //  [Authorize]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<List<Proizvod>> GetProizvodi()
        {
           

            List<Proizvod> proizvodi = proizvodRepository.GetProizvodi();
            if (proizvodi.Count == 0 || proizvodi == null)
            {
                return NoContent();
            }
            foreach (Proizvod pr in proizvodi)
            {
                pr.KategorijaNaziv = kategorijaRepository.GetKategorijaById(pr.KategorijaId).NazivKategorije;
            }


            return Ok(proizvodi);
        }

        [HttpGet("byPorudzbina/{porudzbinaId}")]
        [Authorize]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<List<Proizvod>> GetProizvodiByPorudzbina(int porudzbinaId)
        {


            List<Proizvod> proizvodi = proizvodRepository.GetProizvodsByPorudzbina(porudzbinaId);
           

            if (proizvodi.Count == 0 || proizvodi == null)
            {
                return NoContent();
            }

            foreach (Proizvod pr in proizvodi)
            {
                pr.KategorijaNaziv = kategorijaRepository.GetKategorijaById(pr.KategorijaId).NazivKategorije;
            }

            return Ok(proizvodi);
        }

        [HttpGet("byNazivAndKategorijaSorted/{naziv}/{kategorijaId}/{asc}")]
       // [Authorize]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<List<Proizvod>> GetProizvodiByNazivAndKategorijaSorted(string naziv,int kategorija,int asc)
        {


            List<Proizvod> proizvodi = proizvodRepository.GetProizvodsByNazivAndKategorijaSorted(naziv,kategorija,asc);


            if (proizvodi.Count == 0 || proizvodi == null)
            {
                return NoContent();
            }

            foreach (Proizvod pr in proizvodi)
            {
                pr.KategorijaNaziv = kategorijaRepository.GetKategorijaById(pr.KategorijaId).NazivKategorije;
            }

            return Ok(proizvodi);
        }


        [HttpGet("{proizvodId}")]
      //  [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Proizvod> GetProizvod(int proizvodId)
        {

            Proizvod pr = proizvodRepository.GetProizvodById(proizvodId);
            if (pr == null)
            {
                return NotFound();
            }
   
            return Ok(pr);
        }




        [HttpGet("byCena/{cena}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<Proizvod>> GetProizvodByCena(int cena)
        {

            List<Proizvod> pr = proizvodRepository.GetProizvodsByCena(cena);
            if (pr == null)
            {
                return NotFound();
            }

            return Ok(pr);
        }

        [HttpGet("byKategorija/{kategorija}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<Proizvod>> GetProizvodsByKategorija(int kategorija)
        {

            List<Proizvod> pr = proizvodRepository.GetProizvodsByKategorija(kategorija);
            if (pr == null)
            {
                return NotFound();
            }

            return Ok(pr);
        }

        [HttpGet("byNaziv/{naziv}")]
       //plaky [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<Proizvod>> GetProizvodsByNaziv(string naziv)
        {

            List<Proizvod> pr = proizvodRepository.GetProizvodsByNaziv(naziv);
            if (pr == null)
            {
                return NotFound();
            }

            return Ok(pr);
        }
      

    

    [HttpPost]
       // [Authorize(Roles = "Admin")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Proizvod> CreateProizvod([FromBody] Proizvod proizvod)
        {

            try
            {

                Proizvod createdProizvod = proizvodRepository.CreateProizvod(proizvod);

                string location = linkGenerator.GetPathByAction("GetProizvod", "Proizvod", new { proizvodId = proizvod.ProizvodId });

                return Created(location, createdProizvod);
            }
            catch
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError, "Create proizvod Error");

            }

        }

        
        [HttpDelete("{ProizvodId}")]
     //   [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteProizvod(int proizvodId)
        {
           
         //   try
            {
                Proizvod proizvod = proizvodRepository.GetProizvodById(proizvodId);
                if (proizvod == null)
                {
                    return NotFound();
                }
                proizvodRepository.DeleteProizvod(proizvodId);

                return NoContent();
            }
        //    catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete proizvod Error");
            }
        }

        
        [HttpPut]
      //  [Authorize(Roles = "Admin")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Proizvod> UpdateProizvod(Proizvod proizvod)
        {
           
           // try
            {

                var oldPr = proizvodRepository.GetProizvodById(proizvod.ProizvodId);


                if (oldPr== null)
                {
                    return NotFound();
                }


                oldPr.KategorijaId = proizvod.KategorijaId;
                oldPr.Dostupan = proizvod.Dostupan;
                oldPr.Cena = proizvod.Cena;
                oldPr.AdminId = proizvod.AdminId;
                oldPr.DostupnaKolicina = proizvod.DostupnaKolicina;


                proizvodRepository.SaveChanges();

                return Ok(oldPr);
            }
         //   catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update proizvod error");
            }
        }

       
    }
}
