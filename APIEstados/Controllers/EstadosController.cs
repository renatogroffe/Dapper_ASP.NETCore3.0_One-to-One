using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Dapper;
using APIEstados.Models;

namespace APIEstados.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EstadosController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Estado> Get(
            [FromServices]IConfiguration config)
        {
            using (SqlConnection conexao = new SqlConnection(
                config.GetConnectionString("DadosGeograficos")))
            {
                return conexao.Query<Estado, Regiao, Estado>(
                    "SELECT * " +
                    "FROM dbo.Estados E " +
                    "INNER JOIN dbo.Regioes R ON R.IdRegiao = E.IdRegiao " +
                    "ORDER BY E.NomeEstado",
                    map: (estado, regiao) =>
                    {
                        estado.DadosRegiao = regiao;
                        return estado;
                    },
                    splitOn: "SiglaEstado,IdRegiao");
            }
        }
    }
}