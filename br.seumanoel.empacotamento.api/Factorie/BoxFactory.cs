using br.seumanoel.empacotamento.api.Interfaces;
using br.seumanoel.empacotamento.api.Models;
using br.seumanoel.empacotamento.api.Models.Enums;

namespace br.seumanoel.empacotamento.api.Factorie
{
    /// <summary>
    /// Factory for creating boxes of different types.
    /// </summary>
    public class BoxFactory : IBoxFactory
    {
        public Box CreateBox(BoxType boxType)
        {
            return boxType switch
            {
                BoxType.Box_30x40x80 => new Box(30, 40, 80, "Box 30x40x80"),
                BoxType.Box_80x50x40 => new Box(80, 50, 40, "Box 80x50x40"),
                BoxType.Box_50x80x60 => new Box(50, 80, 60, "Box 50x80x60"),
                _ => throw new ArgumentException("Invalid box type")
            };
        }

        /// <summary>
        /// Returns a list of all available boxes.
        /// </summary>
        /// <returns></returns>
        public List<Box> GetAllBoxes()
        {
            return new List<Box>
            {
                CreateBox(BoxType.Box_30x40x80),
                CreateBox(BoxType.Box_80x50x40),
                CreateBox(BoxType.Box_50x80x60)
            };
        }
    }
}
