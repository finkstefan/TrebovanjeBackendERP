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
    [Route("api/korisnik")]
    [Produces("application/json", "application/xml")]
    public class KorisnikController : ControllerBase
    {
        private readonly IKorisnikRepository korisnikRepository;
        private readonly IAdminRepository adminRepository;
        private readonly IDistributerRepository distributerRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;



        public KorisnikController(IKorisnikRepository korisnikRepository,IAdminRepository adminRepository,IDistributerRepository distributerRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            this.korisnikRepository = korisnikRepository;
            this.adminRepository = adminRepository;
            this.distributerRepository = distributerRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
           
        }

        
        [HttpGet("admins")]
        [HttpHead]
        [Authorize(Roles ="Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<List<Admin>> GetAdmins()
        {
           

            List<Admin> admins = adminRepository.GetAdmins();
            if (admins.Count == 0 || admins == null)
            {
                return NoContent();
            }

            foreach(Admin admin in admins)
            {
                admin.Korisnik = korisnikRepository.GetKorisnikById(admin.KorisnikId);
            }
           
            return Ok(admins);
        }

        [HttpGet]
        [HttpHead]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<List<Korisnik>> GetKorisniks()
        {


            List<Korisnik> korisniks = korisnikRepository.GetKorisnici();
            if (korisniks.Count == 0 || korisniks == null)
            {
                return NoContent();
            }


            return Ok(korisniks);
        }

        [HttpGet("distributers")]
        [HttpHead]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<List<Distributer>> GetDistributers()
        {


            List<Distributer> distributers = distributerRepository.GetDistributers();
            if (distributers.Count == 0 || distributers == null)
            {
                return NoContent();
            }

            foreach (Distributer distrib in distributers)
            {
                distrib.Korisnik = korisnikRepository.GetKorisnikById(distrib.KorisnikId);
            }

            return Ok(distributers);
        }

        [HttpGet("distributers/{naziv}")]
        [HttpHead]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<Distributer> GetDistributerByNaziv(string naziv)
        {


            Distributer distributer = distributerRepository.GetDistributerByNaziv(naziv);
            if (distributer == null)
            {
                return NoContent();
            }

                distributer.Korisnik = korisnikRepository.GetKorisnikById(distributer.KorisnikId);
            

            return Ok(distributer);
        }

        [HttpGet("distributers/{pib}")]
        [HttpHead]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<Distributer> GetDistributersByPib(string pib)
        {


            Distributer distributer = distributerRepository.GetDistributerByPib(pib);
            if (distributer == null)
            {
                return NoContent();
            }

            distributer.Korisnik = korisnikRepository.GetKorisnikById(distributer.KorisnikId);


            return Ok(distributer);
        }



        [HttpGet("{korisnikId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Korisnik> GetKorisnik(int korisnikId)
        {

            Korisnik kor = korisnikRepository.GetKorisnikById(korisnikId);
            if (kor == null)
            {
                return NotFound();
            }
   
            return Ok(kor);
        }

       
        [HttpGet("/admin/{korisnikId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Admin")]
        public ActionResult<Admin> GetAdmin(int korisnikId)
        {

            Admin admin = adminRepository.GetAdminById(korisnikId);
            if (admin == null)
            {
                return NotFound();
            }

            return Ok(admin);
        }


        [HttpGet("distributer/{korisnikId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Admin")]
        public ActionResult<Distributer> GetDistributer(int korisnikId)
        {

            Distributer distrib = distributerRepository.GetDistributerById(korisnikId);
            if (distrib == null)
            {
                return NotFound();
            }

            return Ok(distrib);
        }

     
        [HttpPost("admin")]
        [Authorize(Roles = "Admin")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Admin> CreateAdmin([FromBody] Admin admin)
        {

            try
            {

                Admin createdAdmin = adminRepository.CreateAdmin(admin);

                string location = linkGenerator.GetPathByAction("GetAdmin", "Admin", new { KorisnikId = admin.KorisnikId });

                return Created(location, createdAdmin);
            }
            catch
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError, "Create admin Error");

            }

        }

        [HttpPost("distributer")]
        [Authorize(Roles = "Admin")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Distributer> CreateDistributer([FromBody] Distributer distributer)
        {

            try
            {

                Distributer createdDistributer= distributerRepository.CreateDistributer(distributer);

                string location = linkGenerator.GetPathByAction("GetDistributer", "Distributer", new { KorisnikId = distributer.KorisnikId });

                return Created(location, createdDistributer);
            }
            catch
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Create admin Error");

            }

        }

        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Korisnik> CreateKorisnik([FromBody] Korisnik korisnik)
        {

            try
            {

                Korisnik createdKorisnik = korisnikRepository.CreateKorisnik(korisnik);

                string location = linkGenerator.GetPathByAction("GetKorisnik", "Korisnik", new { KorisnikId = korisnik.KorisnikId });

                return Created(location, createdKorisnik);
            }
            catch
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Create korisnik Error");

            }

        }

        
        [HttpDelete("admin/{korisnikId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteAdmin(int korisnikId)
        {
           
            try
            {
                Admin admin = adminRepository.GetAdminById(korisnikId);
                if (admin == null)
                {
                    return NotFound();
                }
                adminRepository.DeleteAdmin(korisnikId);
                korisnikRepository.DeleteKorisnik(korisnikId);

                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete admin/korisnik Error");
            }
        }

   
        [HttpDelete("distributer/{korisnikId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteDistributer(int korisnikId)
        {

            try
            {
                Distributer distrib = distributerRepository.GetDistributerById(korisnikId);
                if (distrib == null)
                {
                    return NotFound();
                }
                distributerRepository.DeleteDistributer(korisnikId);
                korisnikRepository.DeleteKorisnik(korisnikId);

                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete distributer/korisnik Error");
            }
        }


       
        [HttpPut("distributer")]
        [Consumes("application/json")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Distributer> UpdateDistributer(Distributer distrib)
        {
           
            try
            {

                var oldDistrib = distributerRepository.GetDistributerById(distrib.KorisnikId);


                if (oldDistrib== null)
                {
                    return NotFound();
                }


                oldDistrib.NaCrnojListi = distrib.NaCrnojListi;
                oldDistrib.Pib = distrib.Pib;
                oldDistrib.LokacijaId = distrib.LokacijaId;
           


                distributerRepository.SaveChanges();

                return Ok(oldDistrib);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update distributer error");
            }
        }

        
        [HttpPut("admin")]
        [Consumes("application/json")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Distributer> UpdateAdmin(Admin admin)
        {

            try
            {

                var oldAdmin = adminRepository.GetAdminById(admin.KorisnikId);


                if (oldAdmin == null)
                {
                    return NotFound();
                }


                oldAdmin.Ime = admin.Ime;
                oldAdmin.Prezime = admin.Prezime;



                adminRepository.SaveChanges();

                return Ok(oldAdmin);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update admin error");
            }
        }

       
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Korisnik> UpdateKorisnik(Korisnik korisnik)
        {

            try
            {

                var oldKorisnik = korisnikRepository.GetKorisnikById(korisnik.KorisnikId);


                if (oldKorisnik == null)
                {
                    return NotFound();
                }


                oldKorisnik.BrojTelefona = korisnik.BrojTelefona;
                oldKorisnik.Email = korisnik.Email;
                oldKorisnik.KorisnickoIme = korisnik.KorisnickoIme;
                oldKorisnik.Lozinka = korisnik.Lozinka;
                oldKorisnik.BrojTelefona = korisnik.BrojTelefona;



                korisnikRepository.SaveChanges();

                return Ok(oldKorisnik);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update korisnik error");
            }
        }


    }
}
