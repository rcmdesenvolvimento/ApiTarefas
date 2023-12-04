using System.Text.Json.Serialization;

namespace ApiTarefas.Models
{
    public class Tarefa
    {
        //[JsonIgnore]
        public int Id { get; private set; }
        public string? Nome { get; set; }
        public bool isConcluida { get; set; }
    }
}
