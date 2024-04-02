using System;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using VitrineProdutos.Core.Data;
using VitrineProdutos.Infra.Repository;

namespace VitrineProdutosWeb.Controllers.Produto
{
    public class ProdutoController : Controller
    {
        private readonly ProdutoRepository _produtoRepository;
        private VITRINE_PRODUTOS_DBEntities1 db = new VITRINE_PRODUTOS_DBEntities1();

        public ProdutoController(ProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<ActionResult> Index()
        {
            var produtos = await _produtoRepository.GetAllAsync();
            return View(produtos);
        }

        public async Task<ActionResult> Details(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_produto tb_produto = await _produtoRepository.GetByIdAsync(id);
            if (tb_produto == null)
            {
                return HttpNotFound();
            }
            return View(tb_produto);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            [Bind(Include = "id,nome,descricao,valor,urlImagem,dataHoraCadastro,linkAfiliado,percentualComissao,valorComissao")] 
            tb_produto tb_produto)
        {
            if (ModelState.IsValid)
            {
                tb_produto.dataHoraCadastro = DateTime.Now;
                tb_produto.valorComissao = tb_produto.valor * (tb_produto.percentualComissao / 10);
                db.tb_produto.Add(tb_produto);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tb_produto);
        }

        public async Task<ActionResult> Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_produto tb_produto = await _produtoRepository.GetByIdAsync(id);
            if (tb_produto == null)
            {
                return HttpNotFound();
            }
            return View(tb_produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            [Bind(Include = "id,nome,descricao,valor,urlImagem,dataHoraCadastro,linkAfiliado,percentualComissao,valorComissao")] 
            tb_produto tb_produto)
        {
            if (ModelState.IsValid)
            {
                tb_produto.dataHoraCadastro = DateTime.Now;

                db.Entry(tb_produto).State = EntityState.Modified;
                
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tb_produto);
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_produto tb_produto = await _produtoRepository.GetByIdAsync(id);
            if (tb_produto == null)
            {
                return HttpNotFound();
            }
            return View(tb_produto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            tb_produto tb_Produto = await db.tb_produto.FindAsync(id);
            db.tb_produto.Remove(tb_Produto);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
