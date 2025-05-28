using br.seumanoel.empacotamento.api.Models;
using br.seumanoel.empacotamento.api.Models.Enums;

namespace br.seumanoel.empacotamento.api.Interfaces
{
    public interface IBoxFactory
    {
        Box CreateBox(BoxType boxType);
        List<Box> GetAllBoxes();

    }
}
