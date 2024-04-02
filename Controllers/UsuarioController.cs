using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using VitrineProdutos.Core.Data;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace VitrineProdutosWeb.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly VITRINE_PRODUTOS_DBEntities _db;

        public UsuarioController()
        {
            _db = new VITRINE_PRODUTOS_DBEntities();
        }

        public async Task<ActionResult> Index()
        {
            return View(await _db.tb_usuario.ToListAsync());
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_usuario tb_usuario = await _db.tb_usuario.FindAsync(id);
            if (tb_usuario == null)
            {
                return HttpNotFound();
            }
            return View(tb_usuario);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Usuario,Senha,nomeExibicao")] tb_usuario tb_usuario)
        {
            if (ModelState.IsValid)
            {
                _db.tb_usuario.Add(tb_usuario);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tb_usuario);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_usuario tb_usuario = await _db.tb_usuario.FindAsync(id);
            if (tb_usuario == null)
            {
                return HttpNotFound();
            }
            return View(tb_usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Usuario,Senha,nomeExibicao")] tb_usuario tb_usuario)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(tb_usuario).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tb_usuario);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_usuario tb_usuario = await _db.tb_usuario.FindAsync(id);
            if (tb_usuario == null)
            {
                return HttpNotFound();
            }
            return View(tb_usuario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            tb_usuario tb_usuario = await _db.tb_usuario.FindAsync(id);
            _db.tb_usuario.Remove(tb_usuario);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Login(string usuario, string senha)
        {
            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(senha))
            {
                return Json(new { success = false });
            }

            string senhaMD5;
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(senha));
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sb.Append(data[i].ToString("x2"));
                }
                senhaMD5 = sb.ToString();
            }

            var usuarioAutenticado = _db.tb_usuario.FirstOrDefault(u => u.Usuario == usuario && u.Senha == senhaMD5);

            if (usuarioAutenticado != null)
            {
                TempData["UsuarioLogado"] = usuarioAutenticado.nomeExibicao;
                Session["UsuarioAutenticado"] = true;
                Session["NomeUsuario"] = usuarioAutenticado.nomeExibicao;
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        public ActionResult Logout()
        {
            TempData["UsuarioLogado"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}
